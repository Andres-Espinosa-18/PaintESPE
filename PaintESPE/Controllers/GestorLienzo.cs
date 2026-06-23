using System;
using System.Collections.Generic;
using System.Drawing;
using PaintESPE.Models;
using PaintESPE.Raster;

namespace PaintESPE.Controllers
{
    public class GestorLienzo
    {
        public Bitmap LienzoPrincipal { get; private set; }

        public GestorLienzo(int anchoInicial, int altoInicial)
        {
            ActualizarTamanio(anchoInicial, altoInicial);
        }

        public void ActualizarTamanio(int ancho, int alto)
        {
            if (ancho <= 0 || alto <= 0) return;

            Bitmap nuevoBuffer = new Bitmap(ancho, alto);

            using (Graphics g = Graphics.FromImage(nuevoBuffer))
            {
                g.Clear(Color.White);
                if (LienzoPrincipal != null)
                {
                    g.DrawImageUnscaled(LienzoPrincipal, 0, 0);
                }
            }

            if (LienzoPrincipal != null)
            {
                LienzoPrincipal.Dispose();
            }

            LienzoPrincipal = nuevoBuffer;
        }



        public void SellarFigura(Figura f)
        {
            if (f == null || LienzoPrincipal == null) return;

            using (FastBitmap fb = new FastBitmap(LienzoPrincipal))
            {
                fb.Bloquear();
                f.Dibujar(fb);
            }
            
            if (f is IDisposable disp)
            {
                disp.Dispose();
            }
        }

        public void AplicarRelleno(Point punto, Color colorRelleno)
        {
            if (LienzoPrincipal == null) return;
            if (punto.X < 0 || punto.X >= LienzoPrincipal.Width || punto.Y < 0 || punto.Y >= LienzoPrincipal.Height) return;

            using (FastBitmap fb = new FastBitmap(LienzoPrincipal))
            {
                fb.Bloquear();
                Color colorObjetivo = fb.GetPixelRapido(punto.X, punto.Y);
                if (colorObjetivo.ToArgb() != colorRelleno.ToArgb())
                {
                    RellenoRaster.FloodFill(fb, punto, colorRelleno, colorObjetivo);
                }
            }
        }

        public void LimpiarLienzo()
        {
            if (LienzoPrincipal == null) return;
            using (Graphics g = Graphics.FromImage(LienzoPrincipal))
            {
                g.Clear(Color.White);
            }
        }

        public void GuardarImagen(string ruta, System.Drawing.Imaging.ImageFormat formato)
        {
            try
            {
                if (LienzoPrincipal != null)
                {
                    LienzoPrincipal.Save(ruta, formato);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al guardar la imagen: " + ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void CargarImagenDesdeArchivo(string ruta)
        {
            if (System.IO.File.Exists(ruta))
            {
                int anchoOriginal = LienzoPrincipal != null ? Math.Max(LienzoPrincipal.Width, 1) : 800;
                int altoOriginal = LienzoPrincipal != null ? Math.Max(LienzoPrincipal.Height, 1) : 600;

                Bitmap nuevoLienzo = new Bitmap(anchoOriginal, altoOriginal, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (var fs = new System.IO.FileStream(ruta, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (var img = Image.FromStream(fs))
                    {
                        using (Graphics g = Graphics.FromImage(nuevoLienzo))
                        {
                            g.Clear(Color.White);
                            g.DrawImage(img, 0, 0);
                        }
                    }
                }

                if (LienzoPrincipal != null)
                {
                    LienzoPrincipal.Dispose();
                }

                LienzoPrincipal = nuevoLienzo;
            }
        }

    }
}
