using System;
using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class PoligonoRegular : Figura
    {
        public PoligonoRegular(Point centro, Point borde, int numLados)
        {
            double radio = Math.Sqrt(Math.Pow(borde.X - centro.X, 2) + Math.Pow(borde.Y - centro.Y, 2));
            if (radio < 1) radio = 1;
            numLados = Math.Max(3, numLados);

            for (int i = 0; i < numLados; i++)
            {
                double angle = -Math.PI / 2 + 2 * Math.PI * i / numLados;
                int x = centro.X + (int)Math.Round(radio * Math.Cos(angle));
                int y = centro.Y + (int)Math.Round(radio * Math.Sin(angle));
                Puntos.Add(new Point(x, y));
            }
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
