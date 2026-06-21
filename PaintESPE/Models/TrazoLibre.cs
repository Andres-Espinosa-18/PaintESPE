using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class TrazoLibre : Figura
    {
        public TrazoLibre(List<Point> vertices)
        {
            Puntos.AddRange(vertices);
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count < 2) return;

            for (int i = 0; i < Puntos.Count - 1; i++)
            {
                DibujoRaster.LineaBresenham(lienzo, Puntos[i], Puntos[i + 1], ColorLinea, Grosor);
            }
        }


    }
}
