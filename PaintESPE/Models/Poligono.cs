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

        public override void Dibujar(Bitmap lienzo)
        {
            if (Puntos.Count < 2) return;

            if (ColorRelleno != Color.Transparent)
            {
                // TODO: Scanline algorithm para rellenar polígonos complejos
            }

            for (int i = 0; i < Puntos.Count; i++)
            {
                Point inicio = Puntos[i];
                // Conectar el último con el primero
                Point fin = Puntos[(i + 1) % Puntos.Count];
                DibujoRaster.LineaBresenham(lienzo, inicio, fin, ColorLinea, Grosor);
            }
        }
    }
}
