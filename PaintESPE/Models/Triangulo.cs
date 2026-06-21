using System;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Triangulo : Figura
    {
        public Triangulo(Point p1, Point p2)
        {
            int minX = Math.Min(p1.X, p2.X);
            int maxX = Math.Max(p1.X, p2.X);
            int minY = Math.Min(p1.Y, p2.Y);
            int maxY = Math.Max(p1.Y, p2.Y);

            Puntos.Add(new Point(minX, maxY));
            Puntos.Add(new Point(maxX, maxY));
            Puntos.Add(new Point((minX + maxX) / 2, minY));
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count < 3) return;

            if (ColorRelleno != Color.Transparent)
            {
                RellenoRaster.RellenarPoligono(lienzo, Puntos, ColorRelleno);
            }

            for (int i = 0; i < Puntos.Count; i++)
            {
                Point inicio = Puntos[i];
                Point fin = Puntos[(i + 1) % Puntos.Count];
                DibujoRaster.LineaBresenham(lienzo, inicio, fin, ColorLinea, Grosor);
            }
        }
    }
}
