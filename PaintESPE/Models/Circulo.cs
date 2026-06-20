using System;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Circulo : Figura
    {
        // Puntos[0] = Centro, Puntos[1] = Punto en el borde 
        // Permite calcular el radio dinámicamente si el círculo se escala
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

            DibujoRaster.CirculoPuntoMedio(lienzo, Puntos[0], radio, ColorLinea, Grosor);

            if (ColorRelleno != Color.Transparent)
            {
                // Un relleno simple para el círculo podría iniciar desde el centro
                // RellenoRaster.FloodFill(lienzo, Puntos[0], ColorRelleno, Color.Transparent);
            }
        }
    }
}
