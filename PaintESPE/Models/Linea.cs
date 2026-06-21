using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Linea : Figura
    {
        public Linea(Point inicio, Point fin)
        {
            Puntos.Add(inicio);
            Puntos.Add(fin);
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count >= 2)
            {
                DibujoRaster.LineaBresenham(lienzo, Puntos[0], Puntos[1], ColorLinea, Grosor);
            }
        }


    }
}
