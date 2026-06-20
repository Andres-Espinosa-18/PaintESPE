using System;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class DibujoRaster
    {
        // Algoritmo de Bresenham para dibujar líneas
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

        // Algoritmo de Punto Medio para dibujar círculos
        public static void CirculoPuntoMedio(Bitmap bmp, Point centro, int radio, Color color, int grosor = 1)
        {
            int x = radio, y = 0;
            int err = 0;

            while (x >= y)
            {
                DibujarPuntosSimetricos(bmp, centro, x, y, color, grosor);
                
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

        private static void DibujarPuntosSimetricos(Bitmap bmp, Point c, int x, int y, Color color, int grosor)
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

        // Dibuja un "pixel" o una brocha gruesa manejando los límites del lienzo
        private static void DibujarPuntoConGrosor(Bitmap bmp, int x, int y, Color color, int grosor)
        {
            if (grosor <= 1)
            {
                if (x >= 0 && x < bmp.Width && y >= 0 && y < bmp.Height)
                {
                    bmp.SetPixel(x, y, color);
                }
                return;
            }

            int offset = grosor / 2;
            for (int i = -offset; i <= offset; i++)
            {
                for (int j = -offset; j <= offset; j++)
                {
                    int px = x + i;
                    int py = y + j;
                    if (px >= 0 && px < bmp.Width && py >= 0 && py < bmp.Height)
                    {
                        bmp.SetPixel(px, py, color);
                    }
                }
            }
        }
    }
}
