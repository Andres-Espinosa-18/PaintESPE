using System;
using System.Drawing;
using PaintESPE.Models;
using PaintESPE.Raster;

namespace PaintESPE.Controllers
{
    public enum HerramientaBasica
    {
        Ninguna,
        LapizLibre,
        LineaRecta,
        Rectangulo,
        Elipse,
        Triangulo,
        PoligonoRegular,
        Estrella,
        Curva,
        Relleno,
        Seleccion
    }

    public enum ManejadorActivo
    {
        Ninguno, Cuerpo, Rotacion,
        EscalaNO, EscalaN, EscalaNE,
        EscalaE, EscalaSE, EscalaS,
        EscalaSO, EscalaO
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

        private Figura _figuraSeleccionada;
        public Figura FiguraSeleccionada => _figuraSeleccionada;
        private ManejadorActivo _manejadorActivo = ManejadorActivo.Ninguno;
        private Point _puntoPrevio;
        private Rectangle _cajaOriginal;
        private Point _centroOriginal;
        private double _anguloPrevio;

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

            if (HerramientaActual == HerramientaBasica.Seleccion)
            {
                _manejadorActivo = ManejadorActivo.Ninguno;
                
                if (_figuraSeleccionada != null)
                {
                    Rectangle cajaBase = _figuraSeleccionada.ObtenerAABBBase();
                    _manejadorActivo = DeterminarManejador(_puntoInicio, _figuraSeleccionada, cajaBase);
                    
                    if (_manejadorActivo != ManejadorActivo.Ninguno)
                    {
                        _figuraSeleccionada.IniciarTransformacion();
                        _puntoPrevio = _puntoInicio;
                        _cajaOriginal = cajaBase;
                        _centroOriginal = _figuraSeleccionada.CentroGeometrico;
                        if (_manejadorActivo == ManejadorActivo.Rotacion)
                        {
                            _anguloPrevio = Math.Atan2(_puntoInicio.Y - _centroOriginal.Y, _puntoInicio.X - _centroOriginal.X) * 180 / Math.PI;
                        }
                        return;
                    }
                }

                _figuraSeleccionada = null;
                for (int i = _gestor.Figuras.Count - 1; i >= 0; i--)
                {
                    var fig = _gestor.Figuras[i];
                    Rectangle cajaBase = fig.ObtenerAABBBase();
                    Point clickInverso = Transformacion.Rotar(_puntoInicio, -fig.AnguloRotacion, fig.CentroGeometrico);
                    
                    if (cajaBase.Contains(clickInverso))
                    {
                        _figuraSeleccionada = fig;
                        _figuraSeleccionada.IniciarTransformacion();
                        _manejadorActivo = ManejadorActivo.Cuerpo;
                        _puntoPrevio = _puntoInicio;
                        _cajaOriginal = cajaBase;
                        _centroOriginal = fig.CentroGeometrico;
                        
                        _gestor.Figuras.RemoveAt(i);
                        _gestor.Figuras.Add(fig);
                        break;
                    }
                }
            }
            else if (HerramientaActual == HerramientaBasica.LapizLibre)
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
                {
                    using (PaintESPE.Raster.FastBitmap fb = new PaintESPE.Raster.FastBitmap(buffer))
                    {
                        fb.Bloquear();
                        _figuraTemporal.Dibujar(fb);
                    }
                }
                return buffer;
            }

            Point puntoActual = new Point(x, y);

            switch (HerramientaActual)
            {
                case HerramientaBasica.Seleccion:
                    if (_figuraSeleccionada != null && _manejadorActivo != ManejadorActivo.Ninguno)
                    {
                        int dx = puntoActual.X - _puntoInicio.X;
                        int dy = puntoActual.Y - _puntoInicio.Y;

                        if (_manejadorActivo == ManejadorActivo.Cuerpo)
                        {
                            _figuraSeleccionada.Mover(dx, dy);
                        }
                        else if (_manejadorActivo == ManejadorActivo.Rotacion)
                        {
                            double anguloActual = Math.Atan2(puntoActual.Y - _centroOriginal.Y, puntoActual.X - _centroOriginal.X) * 180 / Math.PI;
                            double difAngulo = anguloActual - _anguloPrevio;
                            _figuraSeleccionada.Rotar((float)difAngulo, _centroOriginal);
                        }
                        else
                        {
                            float factorX = 1f;
                            float factorY = 1f;
                            
                            Point puntoActualInverso = Transformacion.Rotar(puntoActual, -_figuraSeleccionada.AnguloRotacion, _centroOriginal);
                            Point puntoInicioInverso = Transformacion.Rotar(_puntoInicio, -_figuraSeleccionada.AnguloRotacion, _centroOriginal);

                            if (_manejadorActivo == ManejadorActivo.EscalaE || _manejadorActivo == ManejadorActivo.EscalaNE || _manejadorActivo == ManejadorActivo.EscalaSE || 
                                _manejadorActivo == ManejadorActivo.EscalaO || _manejadorActivo == ManejadorActivo.EscalaNO || _manejadorActivo == ManejadorActivo.EscalaSO)
                            {
                                float distPrev = Math.Abs(puntoInicioInverso.X - _centroOriginal.X);
                                float distAct = Math.Abs(puntoActualInverso.X - _centroOriginal.X);
                                if (distPrev > 0) factorX = distAct / distPrev;
                            }

                            if (_manejadorActivo == ManejadorActivo.EscalaS || _manejadorActivo == ManejadorActivo.EscalaSE || _manejadorActivo == ManejadorActivo.EscalaSO ||
                                _manejadorActivo == ManejadorActivo.EscalaN || _manejadorActivo == ManejadorActivo.EscalaNE || _manejadorActivo == ManejadorActivo.EscalaNO)
                            {
                                float distPrev = Math.Abs(puntoInicioInverso.Y - _centroOriginal.Y);
                                float distAct = Math.Abs(puntoActualInverso.Y - _centroOriginal.Y);
                                if (distPrev > 0) factorY = distAct / distPrev;
                            }
                            
                            _figuraSeleccionada.Escalar(factorX, factorY, _centroOriginal);
                        }
                    }
                    break;
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
            {
                using (PaintESPE.Raster.FastBitmap fb = new PaintESPE.Raster.FastBitmap(buffer))
                {
                    fb.Bloquear();
                    _figuraTemporal.Dibujar(fb);
                }
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
                    AutoSeleccionar(_trazoLibreTemporal);
                }
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
                    var curva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    _gestor.AgregarFigura(curva);
                    AutoSeleccionar(curva);
                    _figuraTemporal = null;
                }
                return;
            }
            else if (HerramientaActual != HerramientaBasica.Seleccion)
            {
                if (_puntoInicio != puntoFinal && _figuraTemporal != null)
                {
                    _gestor.AgregarFigura(_figuraTemporal);
                    AutoSeleccionar(_figuraTemporal);
                }
            }

            _figuraTemporal = null;
            _trazoLibreTemporal = null;
        }

        private void AutoSeleccionar(Figura figura)
        {
            HerramientaActual = HerramientaBasica.Seleccion;
            _figuraSeleccionada = figura;
            
            if (figura.CentroGeometrico == Point.Empty)
            {
                Rectangle baseCaja = figura.ObtenerAABBBase();
                figura.CentroGeometrico = new Point(baseCaja.Left + baseCaja.Width / 2, baseCaja.Top + baseCaja.Height / 2);
            }

            _figuraSeleccionada.IniciarTransformacion();
            _cajaOriginal = _figuraSeleccionada.ObtenerAABBBase();
            _centroOriginal = _figuraSeleccionada.CentroGeometrico;
            _manejadorActivo = ManejadorActivo.Ninguno;
        }

        private ManejadorActivo DeterminarManejador(Point click, Figura fig, Rectangle cajaBase)
        {
            Point clickInverso = Transformacion.Rotar(click, -fig.AnguloRotacion, fig.CentroGeometrico);
            int r = PaintESPE.Raster.DibujoSeleccion.TamañoManejador / 2;
            int r2 = 4;
            
            Point rotHandle = new Point(cajaBase.Left + cajaBase.Width / 2, cajaBase.Top - PaintESPE.Raster.DibujoSeleccion.DistanciaRotacion);
            if (new Rectangle(rotHandle.X - r2, rotHandle.Y - r2, r2 * 2, r2 * 2).Contains(clickInverso)) return ManejadorActivo.Rotacion;

            if (new Rectangle(cajaBase.Left - r, cajaBase.Top - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaNO;
            if (new Rectangle(cajaBase.Left + cajaBase.Width / 2 - r, cajaBase.Top - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaN;
            if (new Rectangle(cajaBase.Right - r, cajaBase.Top - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaNE;
            if (new Rectangle(cajaBase.Right - r, cajaBase.Top + cajaBase.Height / 2 - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaE;
            if (new Rectangle(cajaBase.Right - r, cajaBase.Bottom - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaSE;
            if (new Rectangle(cajaBase.Left + cajaBase.Width / 2 - r, cajaBase.Bottom - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaS;
            if (new Rectangle(cajaBase.Left - r, cajaBase.Bottom - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaSO;
            if (new Rectangle(cajaBase.Left - r, cajaBase.Top + cajaBase.Height / 2 - r, r * 2, r * 2).Contains(clickInverso)) return ManejadorActivo.EscalaO;

            if (cajaBase.Contains(clickInverso)) return ManejadorActivo.Cuerpo;

            return ManejadorActivo.Ninguno;
        }
    }
}
