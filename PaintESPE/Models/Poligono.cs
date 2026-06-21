using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Poligono : Figura
    {
        public Poligono(List<Point> vertices)
        {
            Puntos.AddRange(vertices);
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count < 2) return;

            if (ColorRelleno != Color.Transparent && Puntos.Count >= 3)
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
