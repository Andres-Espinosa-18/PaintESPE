using System;
using System.Drawing;
using PaintESPE.Raster;

namespace PaintESPE.Models
{
    public class FiguraPixel : Figura, IDisposable
    {
        public Bitmap BufferPixeles { get; set; }

        public FiguraPixel(Bitmap buffer, Point ubicacionInicial)
        {
            BufferPixeles = buffer;
            
            Puntos.Add(new Point(ubicacionInicial.X, ubicacionInicial.Y));
            Puntos.Add(new Point(ubicacionInicial.X + buffer.Width, ubicacionInicial.Y));
            Puntos.Add(new Point(ubicacionInicial.X + buffer.Width, ubicacionInicial.Y + buffer.Height));
            Puntos.Add(new Point(ubicacionInicial.X, ubicacionInicial.Y + buffer.Height));
        }

        public override void Dibujar(FastBitmap lienzo)
        {
            if (BufferPixeles == null) return;

            lienzo.Desbloquear();
            try
            {
                using (Graphics g = Graphics.FromImage(lienzo.Bitmap))
                {
                    PointF[] destPoints = new PointF[] {
                        new PointF(Puntos[0].X, Puntos[0].Y),
                        new PointF(Puntos[1].X, Puntos[1].Y),
                        new PointF(Puntos[3].X, Puntos[3].Y)
                    };
                    g.DrawImage(BufferPixeles, destPoints);
                }
            }
            finally
            {
                lienzo.Bloquear();
            }
        }

        public void Dispose()
        {
            if (BufferPixeles != null)
            {
                BufferPixeles.Dispose();
                BufferPixeles = null;
            }
        }
    }
}
