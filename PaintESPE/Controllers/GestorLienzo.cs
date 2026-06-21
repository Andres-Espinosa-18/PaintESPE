using System;
using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Models;
using PaintESPE.Raster;

namespace PaintESPE.Controllers
{
    public struct AccionRelleno
    {
        public Point PuntoInicial;
        public Color ColorRelleno;
    }

    public class GestorLienzo
    {
        private Bitmap _buffer;
        public List<Figura> Figuras { get; private set; }
        public List<AccionRelleno> AccionesRelleno { get; private set; }

        public GestorLienzo(int anchoInicial, int altoInicial)
        {
            Figuras = new List<Figura>();
            AccionesRelleno = new List<AccionRelleno>();
            ActualizarTamanio(anchoInicial, altoInicial);
        }

        public void ActualizarTamanio(int ancho, int alto)
        {
            if (ancho <= 0 || alto <= 0) return;

            Bitmap nuevoBuffer = new Bitmap(ancho, alto);

            using (Graphics g = Graphics.FromImage(nuevoBuffer))
            {
                g.Clear(Color.White);
            }

            if (_buffer != null)
            {
                _buffer.Dispose();
            }

            _buffer = nuevoBuffer;
        }

        public Bitmap Renderizar()
        {
            if (_buffer == null) return null;

            using (Graphics g = Graphics.FromImage(_buffer))
            {
                g.Clear(Color.White);
            }

            using (FastBitmap fb = new FastBitmap(_buffer))
            {
                fb.Bloquear();

                foreach (var figura in Figuras)
                {
                    figura.Dibujar(fb);
                }

                foreach (var accion in AccionesRelleno)
                {
                    if (accion.PuntoInicial.X >= 0 && accion.PuntoInicial.X < fb.Width &&
                        accion.PuntoInicial.Y >= 0 && accion.PuntoInicial.Y < fb.Height)
                    {
                        Color colorObjetivo = fb.GetPixelRapido(accion.PuntoInicial.X, accion.PuntoInicial.Y);
                        RellenoRaster.FloodFill(fb, accion.PuntoInicial, accion.ColorRelleno, colorObjetivo);
                    }
                }
            }

            return _buffer;
        }

        public void AgregarFigura(Figura f)
        {
            if (f != null)
            {
                Figuras.Add(f);
            }
        }

        public void AgregarRelleno(Point punto, Color colorRelleno)
        {
            AccionesRelleno.Add(new AccionRelleno { PuntoInicial = punto, ColorRelleno = colorRelleno });
        }

        public void LimpiarLienzo()
        {
            Figuras.Clear();
            AccionesRelleno.Clear();
        }
    }
}
