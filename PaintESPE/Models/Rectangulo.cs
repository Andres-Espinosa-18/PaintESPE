using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class Rectangulo : Figura
    {
        public Rectangulo(Point p1, Point p2)
        {
            // Se definen los 4 vértices para que mantenga la forma al rotar
            Puntos.Add(new Point(p1.X, p1.Y));
            Puntos.Add(new Point(p2.X, p1.Y));
            Puntos.Add(new Point(p2.X, p2.Y));
            Puntos.Add(new Point(p1.X, p2.Y));
        }

        public override void Dibujar(Bitmap lienzo)
        {
            if (Puntos.Count < 4) return;

            if (ColorRelleno != Color.Transparent)
            {
                // TODO: Implementar relleno (Scanline para polígonos o FloodFill tras dibujar borde)
                // Por ahora solo se deja la estructura preparada.
            }
            
            // Dibujar los 4 lados conectando los vértices
            for (int i = 0; i < Puntos.Count; i++)
            {
                Point inicio = Puntos[i];
                Point fin = Puntos[(i + 1) % Puntos.Count];
                DibujoRaster.LineaBresenham(lienzo, inicio, fin, ColorLinea, Grosor);
            }
        }
    }
}
