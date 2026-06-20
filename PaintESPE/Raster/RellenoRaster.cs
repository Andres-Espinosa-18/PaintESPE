using System.Collections.Generic;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class RellenoRaster
    {
        // Algoritmo iterativo de Flood Fill basado en Queue
        public static void FloodFill(Bitmap bmp, Point puntoInicial, Color colorRelleno, Color colorFondo)
        {
            if (colorRelleno.ToArgb() == colorFondo.ToArgb()) return;

            int ancho = bmp.Width;
            int alto = bmp.Height;

            if (puntoInicial.X < 0 || puntoInicial.X >= ancho || puntoInicial.Y < 0 || puntoInicial.Y >= alto) return;

            Color colorActual = bmp.GetPixel(puntoInicial.X, puntoInicial.Y);
            if (colorActual.ToArgb() != colorFondo.ToArgb()) return;

            Queue<Point> cola = new Queue<Point>();
            cola.Enqueue(puntoInicial);

            while (cola.Count > 0)
            {
                Point p = cola.Dequeue();

                if (p.X < 0 || p.X >= ancho || p.Y < 0 || p.Y >= alto) continue;

                if (bmp.GetPixel(p.X, p.Y).ToArgb() == colorFondo.ToArgb())
                {
                    bmp.SetPixel(p.X, p.Y, colorRelleno);

                    cola.Enqueue(new Point(p.X + 1, p.Y));
                    cola.Enqueue(new Point(p.X - 1, p.Y));
                    cola.Enqueue(new Point(p.X, p.Y + 1));
                    cola.Enqueue(new Point(p.X, p.Y - 1));
                }
            }
        }
    }
}
