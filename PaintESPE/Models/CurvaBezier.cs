using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class CurvaBezier : Figura
    {
        public CurvaBezier(Point p0, Point p1, Point p2, Point p3)
        {
            Puntos.Add(p0);
            Puntos.Add(p1);
            Puntos.Add(p2);
            Puntos.Add(p3);
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count == 4)
            {
                DibujoRaster.CurvaBezierCubica(lienzo, Puntos[0], Puntos[1], Puntos[2], Puntos[3], ColorLinea, Grosor);
            }
        }


    }
}
