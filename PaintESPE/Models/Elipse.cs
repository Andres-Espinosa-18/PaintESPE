using System;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Elipse : Figura
    {
        public Elipse(Point p1, Point p2)
        {
            Puntos.Add(new Point(p1.X, p1.Y));
            Puntos.Add(new Point(p2.X, p1.Y));
            Puntos.Add(new Point(p2.X, p2.Y));
            Puntos.Add(new Point(p1.X, p2.Y));
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count < 4) return;

            int x0 = Math.Min(Puntos[0].X, Puntos[2].X);
            int y0 = Math.Min(Puntos[0].Y, Puntos[2].Y);
            int x1 = Math.Max(Puntos[0].X, Puntos[2].X);
            int y1 = Math.Max(Puntos[0].Y, Puntos[2].Y);

            if (ColorRelleno != Color.Transparent)
            {
                DibujoRaster.RellenarElipse(lienzo, x0, y0, x1, y1, ColorRelleno);
            }

            DibujoRaster.ElipseBresenham(lienzo, x0, y0, x1, y1, ColorLinea, Grosor);
        }
    }
}
