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
        public List<Point> PuntosOriginales { get; set; } = new List<Point>();

        public float AnguloRotacion { get; set; } = 0f;
        public float AnguloInicial { get; set; } = 0f;
        public Point CentroGeometrico { get; set; }
        public Point CentroOriginal { get; set; }

        public void IniciarTransformacion()
        {
            PuntosOriginales.Clear();
            foreach (var p in Puntos)
            {
                PuntosOriginales.Add(new Point(p.X, p.Y));
            }
            AnguloInicial = AnguloRotacion;
            CentroOriginal = CentroGeometrico;
        }

        public abstract void Dibujar(FastBitmap lienzo);

        public Rectangle ObtenerAABBBase()
        {
            if (Puntos.Count == 0) return Rectangle.Empty;

            int minX = int.MaxValue, minY = int.MaxValue;
            int maxX = int.MinValue, maxY = int.MinValue;

            foreach (var p in Puntos)
            {
                Point pBase = Transformacion.Rotar(p, -AnguloRotacion, CentroGeometrico);
                if (pBase.X < minX) minX = pBase.X;
                if (pBase.Y < minY) minY = pBase.Y;
                if (pBase.X > maxX) maxX = pBase.X;
                if (pBase.Y > maxY) maxY = pBase.Y;
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public Point[] ObtenerPuntosCaja()
        {
            Rectangle aabb = ObtenerAABBBase();
            if (aabb.IsEmpty) return new Point[4];

            Point[] esquinas = new Point[]
            {
                new Point(aabb.Left, aabb.Top),
                new Point(aabb.Right, aabb.Top),
                new Point(aabb.Right, aabb.Bottom),
                new Point(aabb.Left, aabb.Bottom)
            };

            for (int i = 0; i < 4; i++)
            {
                esquinas[i] = Transformacion.Rotar(esquinas[i], AnguloRotacion, CentroGeometrico);
            }

            return esquinas;
        }

        public virtual void Mover(int dx, int dy)
        {
            if (PuntosOriginales.Count != Puntos.Count) IniciarTransformacion();
            for (int i = 0; i < Puntos.Count; i++)
            {
                Puntos[i] = Transformacion.Trasladar(PuntosOriginales[i], dx, dy);
            }
            CentroGeometrico = Transformacion.Trasladar(CentroOriginal, dx, dy);
        }

        public virtual void Rotar(float difAngulo, Point centro)
        {
            if (PuntosOriginales.Count != Puntos.Count) IniciarTransformacion();
            AnguloRotacion = AnguloInicial + difAngulo;
            for (int i = 0; i < Puntos.Count; i++)
            {
                Puntos[i] = Transformacion.Rotar(PuntosOriginales[i], difAngulo, centro);
            }
        }

        public virtual void Escalar(float factorX, float factorY, Point centro)
        {
            if (PuntosOriginales.Count != Puntos.Count) IniciarTransformacion();
            for (int i = 0; i < Puntos.Count; i++)
            {
                Point pUnrotated = Transformacion.Rotar(PuntosOriginales[i], -AnguloRotacion, centro);
                Point pScaled = Transformacion.Escalar(pUnrotated, factorX, factorY, centro);
                Puntos[i] = Transformacion.Rotar(pScaled, AnguloRotacion, centro);
            }
        }
    }
}
