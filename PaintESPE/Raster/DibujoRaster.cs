using System;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class DibujoRaster
    {
        public static void LineaBresenham(Bitmap bmp, Point p1, Point p2, Color color, int grosor = 1)
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

        public static void CirculoPuntoMedio(Bitmap bmp, Point centro, int radio, Color color, int grosor = 1)
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

        public static void ElipseBresenham(Bitmap bmp, int x0, int y0, int x1, int y1, Color color, int grosor = 1)
        {
            int cx = (x0 + x1) / 2;
            int cy = (y0 + y1) / 2;
            int rx = Math.Abs(x1 - x0) / 2;
            int ry = Math.Abs(y1 - y0) / 2;

            if (rx == 0 || ry == 0) return;

            int x = 0, y = ry;
            int rx2 = rx * rx;
            int ry2 = ry * ry;
            int p = ry2 - rx2 * ry + rx2 / 4;

            while (2 * ry2 * x <= 2 * rx2 * y)
            {
                DibujarPuntosSimetricosElipse(bmp, cx, cy, x, y, color, grosor);
                x++;
                if (p < 0)
                {
                    p += 2 * ry2 * x + ry2;
                }
                else
                {
                    y--;
                    p += 2 * ry2 * x - 2 * rx2 * y + ry2;
                }
            }

            int p2 = (int)(ry2 * (x + 0.5) * (x + 0.5) + rx2 * (y - 1) * (y - 1) - rx2 * ry2);

            while (y > 0)
            {
                DibujarPuntosSimetricosElipse(bmp, cx, cy, x, y, color, grosor);
                y--;
                if (p2 > 0)
                {
                    p2 -= 2 * rx2 * y + rx2;
                }
                else
                {
                    x++;
                    p2 += 2 * ry2 * x - 2 * rx2 * y + rx2;
                }
            }
        }

        public static void RellenarRectangulo(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int minX = Math.Max(0, Math.Min(x0, x1));
            int maxX = Math.Min(bmp.Width - 1, Math.Max(x0, x1));
            int minY = Math.Max(0, Math.Min(y0, y1));
            int maxY = Math.Min(bmp.Height - 1, Math.Max(y0, y1));

            for (int y = minY; y <= maxY; y++)
                for (int x = minX; x <= maxX; x++)
                    bmp.SetPixel(x, y, color);
        }

        public static void RellenarCirculo(Bitmap bmp, Point centro, int radio, Color color)
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
                    bmp.SetPixel(px, py, color);
            }
        }

        public static void RellenarElipse(Bitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int cx = (x0 + x1) / 2;
            int cy = (y0 + y1) / 2;
            double rx = Math.Abs(x1 - x0) / 2.0;
            double ry = Math.Abs(y1 - y0) / 2.0;

            if (rx < 1 || ry < 1) return;

            int yStart = Math.Max(0, cy - (int)Math.Ceiling(ry));
            int yEnd = Math.Min(bmp.Height - 1, cy + (int)Math.Ceiling(ry));

            for (int y = yStart; y <= yEnd; y++)
            {
                double dy = y - cy;
                double dx = rx * Math.Sqrt(Math.Max(0, 1 - (dy * dy) / (ry * ry)));
                int xStart = Math.Max(0, cx - (int)Math.Ceiling(dx));
                int xEnd = Math.Min(bmp.Width - 1, cx + (int)Math.Ceiling(dx));
                for (int x = xStart; x <= xEnd; x++)
                    bmp.SetPixel(x, y, color);
            }
        }

        private static void DibujarPuntosSimetricosCirculo(Bitmap bmp, Point c, int x, int y, Color color, int grosor)
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

        private static void DibujarPuntosSimetricosElipse(Bitmap bmp, int cx, int cy, int x, int y, Color color, int grosor)
        {
            DibujarPuntoConGrosor(bmp, cx + x, cy + y, color, grosor);
            DibujarPuntoConGrosor(bmp, cx - x, cy + y, color, grosor);
            DibujarPuntoConGrosor(bmp, cx + x, cy - y, color, grosor);
            DibujarPuntoConGrosor(bmp, cx - x, cy - y, color, grosor);
        }

        private static void DibujarPuntoConGrosor(Bitmap bmp, int x, int y, Color color, int grosor)
        {
            if (grosor <= 1)
            {
                if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                    bmp.SetPixel(x, y, color);
                return;
            }

            int offset = grosor / 2;
            for (int i = -offset; i <= offset; i++)
                for (int j = -offset; j <= offset; j++)
                {
                    int px = x + i;
                    int py = y + j;
                    if (px >= 0 && px < bmp.Width && py >= 0 && py < bmp.Height)
                        bmp.SetPixel(px, py, color);
                }
        }
    }
}
