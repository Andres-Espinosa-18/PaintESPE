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
        Elipse,
        Triangulo,
        PoligonoRegular,
        Estrella,
        Curva,
        Relleno
    }

    public class ControladorDibujo
    {
        private GestorLienzo _gestor;

        private HerramientaBasica _herramientaActual = HerramientaBasica.LapizLibre;
        public HerramientaBasica HerramientaActual
        {
            get => _herramientaActual;
            set
            {
                _herramientaActual = value;
                if (_herramientaActual != HerramientaBasica.Curva)
                {
                    _estadoCurva = 0;
                    _figuraTemporal = null;
                }
            }
        }
        public Color ColorPrimario { get; set; } = Color.Black;
        public Color ColorSecundario { get; set; } = Color.White;
        public Color ColorActivo { get; set; } = Color.Black;
        public int GrosorActual { get; set; } = 1;
        public int NumeroLados { get; set; } = 5;

        private Point _puntoInicio;
        private bool _estaDibujando;

        private Figura _figuraTemporal;
        private TrazoLibre _trazoLibreTemporal;

        private int _estadoCurva = 0;
        private Point _p0, _p1, _p2, _p3;

        public ControladorDibujo(GestorLienzo gestor)
        {
            _gestor = gestor;
        }

        public void EstablecerColorActivo(bool esPrimario)
        {
            ColorActivo = esPrimario ? ColorPrimario : ColorSecundario;
        }

        public Bitmap ProcesarClickRelleno(int x, int y, Color colorRelleno)
        {
            Bitmap buffer = _gestor.Renderizar();
            if (buffer != null && x >= 0 && x < buffer.Width && y >= 0 && y < buffer.Height)
            {
                _gestor.AgregarRelleno(new Point(x, y), colorRelleno);
                return _gestor.Renderizar();
            }
            return buffer;
        }

        public void ProcesarMouseDown(int x, int y)
        {
            _puntoInicio = new Point(x, y);
            _estaDibujando = true;

            if (HerramientaActual == HerramientaBasica.LapizLibre)
            {
                _trazoLibreTemporal = new TrazoLibre(new System.Collections.Generic.List<Point> { _puntoInicio })
                {
                    ColorLinea = ColorActivo,
                    Grosor = GrosorActual
                };
                _figuraTemporal = _trazoLibreTemporal;
            }
            else if (HerramientaActual == HerramientaBasica.Curva)
            {
                if (_estadoCurva == 0)
                {
                    _p0 = _puntoInicio;
                    _p3 = _puntoInicio;
                    _p1 = _puntoInicio;
                    _p2 = _puntoInicio;
                    _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                }
                else if (_estadoCurva == 1)
                {
                    _p1 = _puntoInicio;
                    _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                }
                else if (_estadoCurva == 2)
                {
                    _p2 = _puntoInicio;
                    _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                }
            }
        }

        public Bitmap ProcesarMouseMove(int x, int y)
        {
            Bitmap buffer = _gestor.Renderizar();

            if (buffer == null) return null;

            if (!_estaDibujando)
            {
                if (_figuraTemporal != null)
                    _figuraTemporal.Dibujar(buffer);
                return buffer;
            }

            Point puntoActual = new Point(x, y);

            switch (HerramientaActual)
            {
                case HerramientaBasica.Curva:
                    if (_estadoCurva == 0)
                    {
                        _p3 = puntoActual;
                        _p1 = new Point(_p0.X + (_p3.X - _p0.X) / 3, _p0.Y + (_p3.Y - _p0.Y) / 3);
                        _p2 = new Point(_p0.X + 2 * (_p3.X - _p0.X) / 3, _p0.Y + 2 * (_p3.Y - _p0.Y) / 3);
                        _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else if (_estadoCurva == 1)
                    {
                        _p1 = puntoActual;
                        _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else if (_estadoCurva == 2)
                    {
                        _p2 = puntoActual;
                        _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    break;
                case HerramientaBasica.LineaRecta:
                    _figuraTemporal = new Linea(_puntoInicio, puntoActual)
                    { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Rectangulo:
                    _figuraTemporal = new Rectangulo(_puntoInicio, puntoActual)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Circulo:
                    _figuraTemporal = new Circulo(_puntoInicio, puntoActual)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Elipse:
                    _figuraTemporal = new Elipse(_puntoInicio, puntoActual)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Triangulo:
                    _figuraTemporal = new Triangulo(_puntoInicio, puntoActual)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.PoligonoRegular:
                    _figuraTemporal = new PoligonoRegular(_puntoInicio, puntoActual, NumeroLados)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.Estrella:
                    _figuraTemporal = new Estrella(_puntoInicio, puntoActual, NumeroLados)
                    { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                    break;
                case HerramientaBasica.LapizLibre:
                    if (_trazoLibreTemporal != null)
                    {
                        Point ultimoPunto = _trazoLibreTemporal.Puntos[_trazoLibreTemporal.Puntos.Count - 1];
                        if (ultimoPunto != puntoActual)
                            _trazoLibreTemporal.Puntos.Add(puntoActual);
                    }
                    break;
            }

            if (_figuraTemporal != null)
                _figuraTemporal.Dibujar(buffer);

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
                    _gestor.AgregarFigura(_trazoLibreTemporal);
            }
            else if (HerramientaActual == HerramientaBasica.Curva)
            {
                if (_estadoCurva == 0)
                {
                    _p3 = puntoFinal;
                    _p1 = new Point(_p0.X + (_p3.X - _p0.X) / 3, _p0.Y + (_p3.Y - _p0.Y) / 3);
                    _p2 = new Point(_p0.X + 2 * (_p3.X - _p0.X) / 3, _p0.Y + 2 * (_p3.Y - _p0.Y) / 3);
                    if (_p0 != _p3)
                    {
                        _estadoCurva = 1;
                        _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else
                    {
                        _figuraTemporal = null;
                    }
                }
                else if (_estadoCurva == 1)
                {
                    _p1 = puntoFinal;
                    _estadoCurva = 2;
                    _figuraTemporal = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                }
                else if (_estadoCurva == 2)
                {
                    _p2 = puntoFinal;
                    _estadoCurva = 0;
                    _gestor.AgregarFigura(new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual });
                    _figuraTemporal = null;
                }
                return;
            }
            else
            {
                if (_puntoInicio != puntoFinal && _figuraTemporal != null)
                    _gestor.AgregarFigura(_figuraTemporal);
            }

            _figuraTemporal = null;
            _trazoLibreTemporal = null;
        }
    }
}
