using System;
using System.Drawing;
using PaintESPE.Models;

namespace PaintESPE.Controllers
{
    public enum HerramientaBasica
    {
        Ninguna,
        LapizLibre,
        LineaRecta,
        Rectangulo,
        Circulo,
        Poligono
    }

    public class ControladorDibujo
    {
        private GestorLienzo _gestor;
        
        public HerramientaBasica HerramientaActual { get; set; } = HerramientaBasica.LapizLibre;
        public Color ColorPrimario { get; set; } = Color.Black;
        public Color ColorSecundario { get; set; } = Color.Transparent;
        public int GrosorActual { get; set; } = 1;

        private Point _puntoInicio;
        private bool _estaDibujando;
        
        // Mantiene la referencia temporal para la previsualización
        private Figura _figuraTemporal;
        private Poligono _trazoLibreTemporal;

        public ControladorDibujo(GestorLienzo gestor)
        {
            _gestor = gestor;
        }

        public void ProcesarMouseDown(int x, int y)
        {
            _puntoInicio = new Point(x, y);
            _estaDibujando = true;

            if (HerramientaActual == HerramientaBasica.LapizLibre)
            {
                _trazoLibreTemporal = new Poligono(new System.Collections.Generic.List<Point> { _puntoInicio })
                {
                    ColorLinea = ColorPrimario,
                    Grosor = GrosorActual
                };
                _figuraTemporal = _trazoLibreTemporal;
            }
        }

        public Bitmap ProcesarMouseMove(int x, int y)
        {
            // El Gestor limpia la pantalla y redibuja las figuras confirmadas
            Bitmap buffer = _gestor.Renderizar();
            
            if (!_estaDibujando || buffer == null) 
                return buffer;

            Point puntoActual = new Point(x, y);

            // Generamos la instancia temporal en tiempo real dependiendo de la herramienta
            switch (HerramientaActual)
            {
                case HerramientaBasica.LineaRecta:
                    _figuraTemporal = new Linea(_puntoInicio, puntoActual) 
                    { ColorLinea = ColorPrimario, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Rectangulo:
                    _figuraTemporal = new Rectangulo(_puntoInicio, puntoActual) 
                    { ColorLinea = ColorPrimario, ColorRelleno = ColorSecundario, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Circulo:
                    _figuraTemporal = new Circulo(_puntoInicio, puntoActual) 
                    { ColorLinea = ColorPrimario, ColorRelleno = ColorSecundario, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.LapizLibre:
                    if (_trazoLibreTemporal != null)
                    {
                        // Optimización básica para evitar duplicar puntos sin movimiento
                        Point ultimoPunto = _trazoLibreTemporal.Puntos[_trazoLibreTemporal.Puntos.Count - 1];
                        if (ultimoPunto != puntoActual)
                        {
                            _trazoLibreTemporal.Puntos.Add(puntoActual);
                        }
                    }
                    break;
                case HerramientaBasica.Poligono:
                    // Inicialmente previsualizamos una sola línea para el primer borde
                    _figuraTemporal = new Linea(_puntoInicio, puntoActual) 
                    { ColorLinea = ColorPrimario, Grosor = GrosorActual };
                    break;
            }

            // Dibujamos la figura en construcción por encima del buffer limpio (doble buffering en memoria).
            // Esto previene el parpadeo ya que la vista simplemente tomará el buffer resultante listo y lo pintará de golpe.
            if (_figuraTemporal != null)
            {
                _figuraTemporal.Dibujar(buffer);
            }

            return buffer;
        }

        public void ProcesarMouseUp(int x, int y)
        {
            if (!_estaDibujando) return;
            _estaDibujando = false;

            Point puntoFinal = new Point(x, y);

            if (HerramientaActual == HerramientaBasica.LapizLibre)
            {
                if (_trazoLibreTemporal != null && _trazoLibreTemporal.Puntos.Count > 1)
                {
                    _gestor.AgregarFigura(_trazoLibreTemporal);
                }
            }
            else
            {
                // Consolidamos la forma enviándola a la lista permanente de GestorLienzo
                // si realmente hubo un arrastre.
                if (_puntoInicio != puntoFinal && _figuraTemporal != null)
                {
                    _gestor.AgregarFigura(_figuraTemporal);
                }
            }

            // Reiniciamos temporalidades
            _figuraTemporal = null;
            _trazoLibreTemporal = null;
        }
    }
}
