using System.Drawing;
using System.Collections.Generic;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public abstract class Figura
    {
        public Color ColorLinea { get; set; } = Color.Black;
        public Color ColorRelleno { get; set; } = Color.Transparent;
        public int Grosor { get; set; } = 1;

        // Puntos base de la figura para renderizar y transformar
        public List<Point> Puntos { get; set; } = new List<Point>();

        public abstract void Dibujar(FastBitmap lienzo);

        public virtual void Trasladar(int dx, int dy)
        {
            for (int i = 0; i < Puntos.Count; i++)
            {
                Puntos[i] = Transformacion.Trasladar(Puntos[i], dx, dy);
            }
        }

        public virtual void Rotar(float angulo, Point centro)
        {
            for (int i = 0; i < Puntos.Count; i++)
            {
                Puntos[i] = Transformacion.Rotar(Puntos[i], angulo, centro);
            }
        }

        public virtual void Escalar(float factorX, float factorY, Point centro)
        {
            for (int i = 0; i < Puntos.Count; i++)
            {
                Puntos[i] = Transformacion.Escalar(Puntos[i], factorX, factorY, centro);
            }
        }
    }
}
