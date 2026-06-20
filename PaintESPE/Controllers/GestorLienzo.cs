using System;
using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Models;

namespace PaintESPE.Controllers
{
    public class GestorLienzo
    {
        private Bitmap _buffer;
        public List<Figura> Figuras { get; private set; }

        public GestorLienzo(int anchoInicial, int altoInicial)
        {
            Figuras = new List<Figura>();
            ActualizarTamanio(anchoInicial, altoInicial);
        }

        public void ActualizarTamanio(int ancho, int alto)
        {
            if (ancho <= 0 || alto <= 0) return;

            // Regenerar el Bitmap al cambiar el tamaño de la ventana
            Bitmap nuevoBuffer = new Bitmap(ancho, alto);
            
            // Pintar de blanco inicialmente por defecto usando Graphics para mayor velocidad base
            using (Graphics g = Graphics.FromImage(nuevoBuffer))
            {
                g.Clear(Color.White);
            }

            if (_buffer != null)
            {
                _buffer.Dispose(); // Prevenir fugas de memoria
            }

            _buffer = nuevoBuffer;
        }

        public Bitmap Renderizar()
        {
            if (_buffer == null) return null;

            // Limpiar el lienzo antes de dibujar cada frame
            using (Graphics g = Graphics.FromImage(_buffer))
            {
                g.Clear(Color.White);
            }

            // Invocar el dibujado por software (matemático) de cada figura almacenada
            foreach (var figura in Figuras)
            {
                figura.Dibujar(_buffer);
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

        public void LimpiarLienzo()
        {
            Figuras.Clear();
        }
    }
}
