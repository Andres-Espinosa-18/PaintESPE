using System;
using System.Drawing;

namespace PaintESPE.Raster
{
    public unsafe static class DibujoRaster
    {
        public static void LineaBresenham(FastBitmap bmp, Point p1, Point p2, Color color, int grosor = 1)
        {
            int x0 = p1.X, y0 = p1.Y;
            int x1 = p2.X, y1 = p2.Y;

            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                DibujarPuntoConGrosor(bmp, x0, y0, color, grosor);
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        public static void CirculoPuntoMedio(FastBitmap bmp, Point centro, int radio, Color color, int grosor = 1)
        {
            int x = radio, y = 0;
            int err = 0;

            while (x >= y)
            {
                DibujarPuntosSimetricosCirculo(bmp, centro, x, y, color, grosor);

                if (err <= 0)
                {
                    y += 1;
                    err += 2 * y + 1;
                }

                if (err > 0)
                {
                    x -= 1;
                    err -= 2 * x + 1;
                }
            }
        }

        public static void ElipseRotadaParametrica(FastBitmap bmp, Point centro, int rx, int ry, float anguloGrados, Color color, int grosor = 1)
        {
            if (rx <= 0 || ry <= 0) return;

            float theta = (float)(anguloGrados * Math.PI / 180.0);
            float cosTheta = (float)Math.Cos(theta);
            float sinTheta = (float)Math.Sin(theta);

            // Calcular paso dinámico para mantener suavidad
            double perimetroAprox = Math.PI * (3 * (rx + ry) - Math.Sqrt((3 * rx + ry) * (rx + 3 * ry)));
            double paso = perimetroAprox > 0 ? Math.Max(0.005, 2.0 / perimetroAprox) : 0.02;

            Point? ultimoPunto = null;
            Point primerPunto = Point.Empty;

            for (double t = 0; t <= 2 * Math.PI; t += paso)
            {
                float cosT = (float)Math.Cos(t);
                float sinT = (float)Math.Sin(t);

                int x = (int)Math.Round(centro.X + rx * cosT * cosTheta - ry * sinT * sinTheta);
                int y = (int)Math.Round(centro.Y + rx * cosT * sinTheta + ry * sinT * cosTheta);
                
                Point pActual = new Point(x, y);

                if (ultimoPunto.HasValue)
                {
                    LineaBresenham(bmp, ultimoPunto.Value, pActual, color, grosor);
                }
                else
                {
                    primerPunto = pActual;
                }
                
                ultimoPunto = pActual;
            }
            
            if (ultimoPunto.HasValue && ultimoPunto.Value != primerPunto)
            {
                LineaBresenham(bmp, ultimoPunto.Value, primerPunto, color, grosor);
            }
        }

        public static void RellenarRectangulo(FastBitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int minX = Math.Max(0, Math.Min(x0, x1));
            int maxX = Math.Min(bmp.Width - 1, Math.Max(x0, x1));
            int minY = Math.Max(0, Math.Min(y0, y1));
            int maxY = Math.Min(bmp.Height - 1, Math.Max(y0, y1));

            for (int y = minY; y <= maxY; y++)
                for (int x = minX; x <= maxX; x++)
                    bmp.SetPixelRapido(x, y, color);
        }

        public static void RellenarCirculo(FastBitmap bmp, Point centro, int radio, Color color)
        {
            int cx = centro.X, cy = centro.Y;

            for (int y = -radio; y <= radio; y++)
            {
                int x = (int)Math.Round(Math.Sqrt(radio * radio - y * y));
                int xStart = Math.Max(0, cx - x);
                int xEnd = Math.Min(bmp.Width - 1, cx + x);
                int py = cy + y;
                if (py < 0 || py >= bmp.Height) continue;
                for (int px = xStart; px <= xEnd; px++)
                    bmp.SetPixelRapido(px, py, color);
            }
        }

        public static void RellenarElipseRotada(FastBitmap bmp, Point centro, int rx, int ry, float anguloGrados, Color color)
        {
            if (rx < 1 || ry < 1) return;

            // Encontrar la caja delimitadora (Bounding Box) de la elipse rotada
            float theta = (float)(anguloGrados * Math.PI / 180.0);
            float cosTheta = (float)Math.Cos(theta);
            float sinTheta = (float)Math.Sin(theta);

            int halfWidth = (int)Math.Ceiling(Math.Sqrt(rx * rx * cosTheta * cosTheta + ry * ry * sinTheta * sinTheta));
            int halfHeight = (int)Math.Ceiling(Math.Sqrt(rx * rx * sinTheta * sinTheta + ry * ry * cosTheta * cosTheta));

            int minX = Math.Max(0, centro.X - halfWidth);
            int maxX = Math.Min(bmp.Width - 1, centro.X + halfWidth);
            int minY = Math.Max(0, centro.Y - halfHeight);
            int maxY = Math.Min(bmp.Height - 1, centro.Y + halfHeight);

            double rx2 = rx * rx;
            double ry2 = ry * ry;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // Des-rotar el punto
                    double dx = x - centro.X;
                    double dy = y - centro.Y;
                    double unrotatedX = dx * cosTheta + dy * sinTheta;
                    double unrotatedY = -dx * sinTheta + dy * cosTheta;

                    // Ecuación de la elipse: (x^2 / rx^2) + (y^2 / ry^2) <= 1
                    if ((unrotatedX * unrotatedX) / rx2 + (unrotatedY * unrotatedY) / ry2 <= 1.0)
                    {
                        bmp.SetPixelRapido(x, y, color);
                    }
                }
            }
        }

        public static void CurvaBezierCubica(FastBitmap bmp, Point p0, Point p1, Point p2, Point p3, Color color, int grosor = 1)
        {
            Point ultimoPunto = p0;
            double paso = 0.01;

            for (double t = paso; t <= 1.0; t += paso)
            {
                double u = 1 - t;
                double tt = t * t;
                double uu = u * u;
                double uuu = uu * u;
                double ttt = tt * t;

                double x = uuu * p0.X + 3 * uu * t * p1.X + 3 * u * tt * p2.X + ttt * p3.X;
                double y = uuu * p0.Y + 3 * uu * t * p1.Y + 3 * u * tt * p2.Y + ttt * p3.Y;

                Point pActual = new Point((int)Math.Round(x), (int)Math.Round(y));
                LineaBresenham(bmp, ultimoPunto, pActual, color, grosor);
                ultimoPunto = pActual;
            }
            LineaBresenham(bmp, ultimoPunto, p3, color, grosor);
        }

        private static void DibujarPuntosSimetricosCirculo(FastBitmap bmp, Point c, int x, int y, Color color, int grosor)
        {
            DibujarPuntoConGrosor(bmp, c.X + x, c.Y + y, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X + y, c.Y + x, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X - y, c.Y + x, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X - x, c.Y + y, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X - x, c.Y - y, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X - y, c.Y - x, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X + y, c.Y - x, color, grosor);
            DibujarPuntoConGrosor(bmp, c.X + x, c.Y - y, color, grosor);
        }



        private static void DibujarPuntoConGrosor(FastBitmap bmp, int x, int y, Color color, int grosor)
        {
            if (grosor <= 1)
            {
                if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                {
                    byte* pixel = bmp.BasePointer + (y * bmp.Stride) + (x * 4);
                    pixel[0] = color.B;
                    pixel[1] = color.G;
                    pixel[2] = color.R;
                    pixel[3] = color.A;
                }
                return;
            }

            int offset = grosor / 2;
            int minX = Math.Max(0, x - offset);
            int maxX = Math.Min(bmp.Width - 1, x + offset);
            int minY = Math.Max(0, y - offset);
            int maxY = Math.Min(bmp.Height - 1, y + offset);

            int stride = bmp.Stride;
            byte* basePtr = bmp.BasePointer;
            byte b = color.B;
            byte g = color.G;
            byte r = color.R;
            byte a = color.A;

            for (int py = minY; py <= maxY; py++)
            {
                byte* row = basePtr + (py * stride) + (minX * 4);
                for (int px = minX; px <= maxX; px++)
                {
                    row[0] = b;
                    row[1] = g;
                    row[2] = r;
                    row[3] = a;
                    row += 4;
                }
            }
        }
    }
}
