using System;
using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Estrella : Figura
    {
        public Estrella(Point centro, Point borde, int numPuntas)
        {
            double radioExterno = Math.Sqrt(Math.Pow(borde.X - centro.X, 2) + Math.Pow(borde.Y - centro.Y, 2));
            if (radioExterno < 1) radioExterno = 1;
            numPuntas = Math.Max(3, numPuntas);
            double radioInterno = radioExterno * 0.4;

            for (int i = 0; i < numPuntas * 2; i++)
            {
                double angle = -Math.PI / 2 + Math.PI * i / numPuntas;
                double r = (i % 2 == 0) ? radioExterno : radioInterno;
                int x = centro.X + (int)Math.Round(r * Math.Cos(angle));
                int y = centro.Y + (int)Math.Round(r * Math.Sin(angle));
                Puntos.Add(new Point(x, y));
            }
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (Puntos.Count < 4) return;

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
