using System;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Circulo : Figura
    {
        public Circulo(Point centro, Point borde)
        {
            Puntos.Add(centro);
            Puntos.Add(borde);
        }

        public override void Dibujar(Bitmap lienzo)
        {
            if (Puntos.Count < 2) return;

            int dx = Puntos[1].X - Puntos[0].X;
            int dy = Puntos[1].Y - Puntos[0].Y;
            int radio = (int)Math.Round(Math.Sqrt(dx * dx + dy * dy));

            if (ColorRelleno != Color.Transparent)
            {
                DibujoRaster.RellenarCirculo(lienzo, Puntos[0], radio, ColorRelleno);
            }

            DibujoRaster.CirculoPuntoMedio(lienzo, Puntos[0], radio, ColorLinea, Grosor);
        }
    }
}
