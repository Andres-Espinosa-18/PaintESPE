using System;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class DibujoSeleccion
    {
        public const int TamañoManejador = 6;
        public const int DistanciaRotacion = 25;

        public static void Dibujar(FastBitmap bmp, Rectangle caja)
        {
            if (caja.Width == 0 && caja.Height == 0) return;

            // Dibujar bordes segmentados
            Color colorBorde = Color.DarkGray;
            DibujarLineaSegmentada(bmp, caja.Left, caja.Top, caja.Right, caja.Top, colorBorde);
            DibujarLineaSegmentada(bmp, caja.Right, caja.Top, caja.Right, caja.Bottom, colorBorde);
            DibujarLineaSegmentada(bmp, caja.Right, caja.Bottom, caja.Left, caja.Bottom, colorBorde);
            DibujarLineaSegmentada(bmp, caja.Left, caja.Bottom, caja.Left, caja.Top, colorBorde);

            // 8 Manejadores (Escala)
            Point[] handles = new Point[] {
                new Point(caja.Left, caja.Top),
                new Point(caja.Left + caja.Width / 2, caja.Top),
                new Point(caja.Right, caja.Top),
                new Point(caja.Right, caja.Top + caja.Height / 2),
                new Point(caja.Right, caja.Bottom),
                new Point(caja.Left + caja.Width / 2, caja.Bottom),
                new Point(caja.Left, caja.Bottom),
                new Point(caja.Left, caja.Top + caja.Height / 2)
            };

            foreach (var p in handles)
            {
                DibujarCuadrito(bmp, p);
            }

            // Manejador de rotación
            Point centroSuperior = new Point(caja.Left + caja.Width / 2, caja.Top);
            Point rotHandle = new Point(centroSuperior.X, centroSuperior.Y - DistanciaRotacion);
            
            // Línea que conecta el rectángulo con el manejador de rotación
            DibujarLineaSegmentada(bmp, centroSuperior.X, centroSuperior.Y, rotHandle.X, rotHandle.Y, colorBorde);
            
            // Círculo de rotación
            DibujoRaster.RellenarCirculo(bmp, rotHandle, 4, Color.White);
            DibujoRaster.CirculoPuntoMedio(bmp, rotHandle, 4, Color.Black);
        }

        private static void DibujarCuadrito(FastBitmap bmp, Point centro)
        {
            int r = TamañoManejador / 2;
            DibujoRaster.RellenarRectangulo(bmp, centro.X - r, centro.Y - r, centro.X + r, centro.Y + r, Color.White);
            DibujoRaster.LineaBresenham(bmp, new Point(centro.X - r, centro.Y - r), new Point(centro.X + r, centro.Y - r), Color.Black);
            DibujoRaster.LineaBresenham(bmp, new Point(centro.X + r, centro.Y - r), new Point(centro.X + r, centro.Y + r), Color.Black);
            DibujoRaster.LineaBresenham(bmp, new Point(centro.X + r, centro.Y + r), new Point(centro.X - r, centro.Y + r), Color.Black);
            DibujoRaster.LineaBresenham(bmp, new Point(centro.X - r, centro.Y + r), new Point(centro.X - r, centro.Y - r), Color.Black);
        }

        private static void DibujarLineaSegmentada(FastBitmap bmp, int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            int count = 0;

            while (true)
            {
                if ((count / 4) % 2 == 0)
                {
                    if (x0 >= 0 && x0 < bmp.Width && y0 >= 0 && y0 < bmp.Height)
                        bmp.SetPixelRapido(x0, y0, color);
                }
                count++;

                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }
    }
}
