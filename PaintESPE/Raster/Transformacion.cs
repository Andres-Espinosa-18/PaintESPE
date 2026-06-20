using System;
using System.Drawing;

namespace PaintESPE.Raster
{
    public static class Transformacion
    {
        public static Point Trasladar(Point p, int dx, int dy)
        {
            return new Point(p.X + dx, p.Y + dy);
        }

        public static Point Rotar(Point p, float anguloGrados, Point centro)
        {
            double anguloRadianes = anguloGrados * Math.PI / 180.0;
            double cosTheta = Math.Cos(anguloRadianes);
            double sinTheta = Math.Sin(anguloRadianes);

            // Trasladar al origen relativo al centro de rotación
            int x = p.X - centro.X;
            int y = p.Y - centro.Y;

            // Matriz de rotación 2D
            int xNuevo = (int)Math.Round(x * cosTheta - y * sinTheta);
            int yNuevo = (int)Math.Round(x * sinTheta + y * cosTheta);

            // Trasladar de vuelta
            return new Point(xNuevo + centro.X, yNuevo + centro.Y);
        }

        public static Point Escalar(Point p, float factorX, float factorY, Point centro)
        {
            // Trasladar al origen relativo al centro
            int x = p.X - centro.X;
            int y = p.Y - centro.Y;

            // Matriz de escalamiento
            int xNuevo = (int)Math.Round(x * factorX);
            int yNuevo = (int)Math.Round(y * factorY);

            // Trasladar de vuelta
            return new Point(xNuevo + centro.X, yNuevo + centro.Y);
        }
    }
}
