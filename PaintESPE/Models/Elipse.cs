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
            Rectangle aabb = ObtenerAABBBase();
            if (aabb.Width == 0 || aabb.Height == 0) return;

            int rx = aabb.Width / 2;
            int ry = aabb.Height / 2;

            Point centro = CentroGeometrico;
            if (centro == Point.Empty)
            {
                centro = new Point(aabb.Left + rx, aabb.Top + ry);
            }

            if (ColorRelleno != Color.Transparent)
            {
                DibujoRaster.RellenarElipseRotada(lienzo, centro, rx, ry, AnguloRotacion, ColorRelleno);
            }

            DibujoRaster.ElipseRotadaParametrica(lienzo, centro, rx, ry, AnguloRotacion, ColorLinea, Grosor);
        }


    }
}
