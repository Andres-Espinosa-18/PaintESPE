using System;
using System.Collections.Generic;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class RellenoRaster
    {
        public static void FloodFill(FastBitmap bmp, Point puntoInicial, Color colorRelleno, Color colorFondo)
        {
            if (colorRelleno.ToArgb() == colorFondo.ToArgb()) return;

            int ancho = bmp.Width;
            int alto = bmp.Height;

            if (puntoInicial.X < 0 || puntoInicial.X >= ancho || puntoInicial.Y < 0 || puntoInicial.Y >= alto) return;

            Color colorActual = bmp.GetPixelRapido(puntoInicial.X, puntoInicial.Y);
            if (colorActual.ToArgb() != colorFondo.ToArgb()) return;

            Queue<Point> cola = new Queue<Point>();
            cola.Enqueue(puntoInicial);

            while (cola.Count > 0)
            {
                Point p = cola.Dequeue();

                if (p.X < 0 || p.X >= ancho || p.Y < 0 || p.Y >= alto) continue;

                if (bmp.GetPixelRapido(p.X, p.Y).ToArgb() == colorFondo.ToArgb())
                {
                    bmp.SetPixelRapido(p.X, p.Y, colorRelleno);

                    cola.Enqueue(new Point(p.X + 1, p.Y));
                    cola.Enqueue(new Point(p.X - 1, p.Y));
                    cola.Enqueue(new Point(p.X, p.Y + 1));
                    cola.Enqueue(new Point(p.X, p.Y - 1));
                }
            }
        }

        public static void RellenarPoligono(FastBitmap bmp, List<Point> vertices, Color color)
        {
            if (vertices.Count < 3) return;

            int yMin = int.MaxValue, yMax = int.MinValue;
            foreach (var p in vertices)
            {
                if (p.Y < yMin) yMin = p.Y;
                if (p.Y > yMax) yMax = p.Y;
            }

            yMin = Math.Max(0, yMin);
            yMax = Math.Min(bmp.Height - 1, yMax);

            for (int y = yMin; y <= yMax; y++)
            {
                List<float> interX = new List<float>();

                for (int i = 0; i < vertices.Count; i++)
                {
                    Point p1 = vertices[i];
                    Point p2 = vertices[(i + 1) % vertices.Count];

                    if (p1.Y == p2.Y) continue;

                    if ((p1.Y < y && p2.Y >= y) || (p2.Y < y && p1.Y >= y))
                    {
                        float x = p1.X + (float)(y - p1.Y) / (p2.Y - p1.Y) * (p2.X - p1.X);
                        interX.Add(x);
                    }
                }

                interX.Sort();

                for (int i = 0; i + 1 < interX.Count; i += 2)
                {
                    int xStart = (int)Math.Ceiling(interX[i]);
                    int xEnd = (int)Math.Floor(interX[i + 1]);

                    xStart = Math.Max(0, xStart);
                    xEnd = Math.Min(bmp.Width - 1, xEnd);

                    for (int x = xStart; x <= xEnd; x++)
                        bmp.SetPixelRapido(x, y, color);
                }
            }
        }
    }
}
