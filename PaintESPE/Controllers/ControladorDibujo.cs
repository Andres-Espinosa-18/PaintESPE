using System;
using System.Drawing;
using PaintESPE.Models;
using PaintESPE.Raster;
using System.Windows.Forms;

namespace PaintESPE.Controllers
{
    public enum HerramientaBasica
    {
        Ninguna, LapizLibre, Borrador, LineaRecta, Rectangulo, Elipse,
        Triangulo, PoligonoRegular, Estrella, Curva, Relleno, Seleccion
    }

    public enum ManejadorActivo
    {
        Ninguno, Cuerpo, Rotacion, EscalaNO, EscalaN, EscalaNE,
        EscalaE, EscalaSE, EscalaS, EscalaSO, EscalaO
    }

    public class ControladorDibujo
    {
        public const int TAMANIO_BORRADOR_GRANDE = 30;
        private GestorLienzo _gestor;

        private HerramientaBasica _herramientaActual = HerramientaBasica.LapizLibre;
        public HerramientaBasica HerramientaActual
        {
            get => _herramientaActual;
            set
            {
                if (_herramientaActual != value)
                {
                    SellarFiguraActiva();
                    _herramientaActual = value;
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

        public Figura FiguraActiva { get; private set; }
        private TrazoLibre _trazoLibreTemporal;

        private int _estadoCurva = 0;
        private Point _p0, _p1, _p2, _p3;

        private ManejadorActivo _manejadorActivo = ManejadorActivo.Ninguno;
        private Point _puntoPrevio;
        private Rectangle _cajaOriginal;
        private Point _centroOriginal;
        private double _anguloPrevio;

        private Bitmap _bufferTrabajo;

        public void ActualizarTamanioBuffer(int width, int height)
        {
            if (width <= 0 || height <= 0) return;
            if (_bufferTrabajo != null)
            {
                _bufferTrabajo.Dispose();
            }
            _bufferTrabajo = new Bitmap(width, height);
        }

        public ControladorDibujo(GestorLienzo gestor)
        {
            _gestor = gestor;
        }

        public void EstablecerColorActivo(bool esPrimario)
        {
            ColorActivo = esPrimario ? ColorPrimario : ColorSecundario;
        }

        public void SellarFiguraActiva()
        {
            if (FiguraActiva != null)
            {
                _gestor.SellarFigura(FiguraActiva);
                FiguraActiva = null;
                _manejadorActivo = ManejadorActivo.Ninguno;
                _trazoLibreTemporal = null;
                _estadoCurva = 0;
            }
        }

        public void ProcesarMouseDown(int x, int y)
        {
            _puntoInicio = new Point(x, y);
            _estaDibujando = true;

            if (HerramientaActual == HerramientaBasica.Relleno)
            {
                SellarFiguraActiva();
                _gestor.AplicarRelleno(new Point(x, y), ColorActivo);
                _estaDibujando = false;
                return;
            }

            if (HerramientaActual == HerramientaBasica.Seleccion)
            {
                if (FiguraActiva != null)
                {
                    Rectangle cajaBase = FiguraActiva.ObtenerAABBBase();
                    _manejadorActivo = DeterminarManejador(_puntoInicio, FiguraActiva, cajaBase);
                    if (_manejadorActivo != ManejadorActivo.Ninguno)
                    {
                        FiguraActiva.IniciarTransformacion();
                        _puntoPrevio = _puntoInicio;
                        _cajaOriginal = cajaBase;
                        _centroOriginal = FiguraActiva.CentroGeometrico;
                        if (_manejadorActivo == ManejadorActivo.Rotacion)
                        {
                            _anguloPrevio = Math.Atan2(_puntoInicio.Y - _centroOriginal.Y, _puntoInicio.X - _centroOriginal.X) * 180 / Math.PI;
                        }
                        return;
                    }
                }
                SellarFiguraActiva();
                
                FiguraActiva = new Rectangulo(_puntoInicio, _puntoInicio) { ColorLinea = Color.DarkGray, ColorRelleno = Color.Transparent, Grosor = 1 };
                return;
            }

            if (FiguraActiva != null && _estadoCurva == 0)
            {
                Rectangle cajaBase = FiguraActiva.ObtenerAABBBase();
                _manejadorActivo = DeterminarManejador(_puntoInicio, FiguraActiva, cajaBase);
                if (_manejadorActivo != ManejadorActivo.Ninguno)
                {
                    FiguraActiva.IniciarTransformacion();
                    _puntoPrevio = _puntoInicio;
                    _cajaOriginal = cajaBase;
                    _centroOriginal = FiguraActiva.CentroGeometrico;
                    if (_manejadorActivo == ManejadorActivo.Rotacion)
                        _anguloPrevio = Math.Atan2(_puntoInicio.Y - _centroOriginal.Y, _puntoInicio.X - _centroOriginal.X) * 180 / Math.PI;
                    return;
                }
                else
                {
                    SellarFiguraActiva();
                }
            }

            switch (HerramientaActual)
            {
                case HerramientaBasica.LapizLibre:
                case HerramientaBasica.Borrador:
                    _puntoPrevio = _puntoInicio;
                    break;
                case HerramientaBasica.Curva:
                    if (_estadoCurva == 0)
                    {
                        _p0 = _puntoInicio; _p3 = _puntoInicio; _p1 = _puntoInicio; _p2 = _puntoInicio;
                        FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else if (_estadoCurva == 1)
                    {
                        _p1 = _puntoInicio;
                        FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else if (_estadoCurva == 2)
                    {
                        _p2 = _puntoInicio;
                        FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    break;
            }
        }

        public Bitmap ProcesarMouseMove(int x, int y, out Rectangle regionActualizacion)
        {
            regionActualizacion = Rectangle.Empty;
            Point puntoActual = new Point(x, y);

            if (HerramientaActual == HerramientaBasica.LapizLibre || HerramientaActual == HerramientaBasica.Borrador)
            {
                if (_estaDibujando)
                {
                    int grosorParaUsar = HerramientaActual == HerramientaBasica.Borrador ? TAMANIO_BORRADOR_GRANDE : GrosorActual;
                    Color colorTrazo = HerramientaActual == HerramientaBasica.Borrador ? Color.White : ColorActivo;
                    
                    using (PaintESPE.Raster.FastBitmap fb = new PaintESPE.Raster.FastBitmap(_gestor.LienzoPrincipal))
                    {
                        fb.Bloquear();
                        PaintESPE.Raster.DibujoRaster.LineaBresenham(fb, _puntoPrevio, puntoActual, colorTrazo, grosorParaUsar);
                    }
                    
                    int pad = grosorParaUsar + 2;
                    int minX = Math.Min(_puntoPrevio.X, puntoActual.X) - pad;
                    int minY = Math.Min(_puntoPrevio.Y, puntoActual.Y) - pad;
                    int maxX = Math.Max(_puntoPrevio.X, puntoActual.X) + pad;
                    int maxY = Math.Max(_puntoPrevio.Y, puntoActual.Y) + pad;
                    
                    regionActualizacion = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                    _puntoPrevio = puntoActual;
                }
                return null;
            }

            if (_bufferTrabajo == null || _gestor.LienzoPrincipal == null) return null;

            using (Graphics g = Graphics.FromImage(_bufferTrabajo))
            {
                g.DrawImage(_gestor.LienzoPrincipal, 0, 0);
            }
            Bitmap buffer = _bufferTrabajo;

            if (!_estaDibujando)
            {
                return buffer;
            }

            if (FiguraActiva != null && _manejadorActivo != ManejadorActivo.Ninguno)
            {
                int dx = puntoActual.X - _puntoInicio.X;
                int dy = puntoActual.Y - _puntoInicio.Y;

                if (_manejadorActivo == ManejadorActivo.Cuerpo)
                {
                    FiguraActiva.Mover(dx, dy);
                }
                else if (_manejadorActivo == ManejadorActivo.Rotacion)
                {
                    double anguloActual = Math.Atan2(puntoActual.Y - _centroOriginal.Y, puntoActual.X - _centroOriginal.X) * 180 / Math.PI;
                    double difAngulo = anguloActual - _anguloPrevio;
                    FiguraActiva.Rotar((float)difAngulo, _centroOriginal);
                }
                else
                {
                    float factorX = 1f;
                    float factorY = 1f;
                    
                    Point puntoActualInverso = Transformacion.Rotar(puntoActual, -FiguraActiva.AnguloRotacion, _centroOriginal);
                    Point puntoInicioInverso = Transformacion.Rotar(_puntoInicio, -FiguraActiva.AnguloRotacion, _centroOriginal);

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
                    
                    FiguraActiva.Escalar(factorX, factorY, _centroOriginal);
                }
            }
            else
            {
                switch (HerramientaActual)
                {
                    case HerramientaBasica.Curva:
                        if (_estadoCurva == 0)
                        {
                            _p3 = puntoActual;
                            _p1 = new Point(_p0.X + (_p3.X - _p0.X) / 3, _p0.Y + (_p3.Y - _p0.Y) / 3);
                            _p2 = new Point(_p0.X + 2 * (_p3.X - _p0.X) / 3, _p0.Y + 2 * (_p3.Y - _p0.Y) / 3);
                            FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                        }
                        else if (_estadoCurva == 1)
                        {
                            _p1 = puntoActual;
                            FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                        }
                        else if (_estadoCurva == 2)
                        {
                            _p2 = puntoActual;
                            FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                        }
                        break;
                    case HerramientaBasica.Seleccion:
                        FiguraActiva = new Rectangulo(_puntoInicio, puntoActual) { ColorLinea = Color.DarkGray, ColorRelleno = Color.Transparent, Grosor = 1 };
                        break;
                    case HerramientaBasica.LineaRecta:
                        FiguraActiva = new Linea(_puntoInicio, puntoActual) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                        break;
                    case HerramientaBasica.Rectangulo:
                        FiguraActiva = new Rectangulo(_puntoInicio, puntoActual) { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                        break;
                    case HerramientaBasica.Elipse:
                        FiguraActiva = new Elipse(_puntoInicio, puntoActual) { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                        break;
                    case HerramientaBasica.Triangulo:
                        FiguraActiva = new Triangulo(_puntoInicio, puntoActual) { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                        break;
                    case HerramientaBasica.PoligonoRegular:
                        FiguraActiva = new PoligonoRegular(_puntoInicio, puntoActual, NumeroLados) { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                        break;
                    case HerramientaBasica.Estrella:
                        FiguraActiva = new Estrella(_puntoInicio, puntoActual, NumeroLados) { ColorLinea = ColorActivo, ColorRelleno = Color.Transparent, Grosor = GrosorActual };
                        break;
                }
            }

            return buffer;
        }

        public void ProcesarMouseUp(int x, int y)
        {
            if (!_estaDibujando) return;
            _estaDibujando = false;

            if (_manejadorActivo != ManejadorActivo.Ninguno)
            {
                _manejadorActivo = ManejadorActivo.Ninguno;
                return;
            }

            Point puntoFinal = new Point(x, y);

            if (HerramientaActual == HerramientaBasica.Seleccion)
            {
                if (FiguraActiva is Rectangulo)
                {
                    Rectangle aabb = FiguraActiva.ObtenerAABBBase();
                    FiguraActiva = null; 

                    if (aabb.Width > 0 && aabb.Height > 0 && _gestor.LienzoPrincipal != null)
                    {
                        aabb.Intersect(new Rectangle(0, 0, _gestor.LienzoPrincipal.Width, _gestor.LienzoPrincipal.Height));

                        if (aabb.Width > 0 && aabb.Height > 0)
                        {
                            Bitmap originalLienzo = _gestor.LienzoPrincipal;
                            Bitmap capturado = new Bitmap(aabb.Width, aabb.Height);
                            using (Graphics g = Graphics.FromImage(capturado))
                            {
                                g.DrawImage(originalLienzo, new Rectangle(0, 0, aabb.Width, aabb.Height), aabb, GraphicsUnit.Pixel);
                            }

                            using (Graphics g = Graphics.FromImage(originalLienzo))
                            {
                                g.FillRectangle(Brushes.White, aabb);
                            }

                            FiguraActiva = new FiguraPixel(capturado, new Point(aabb.Left, aabb.Top));
                            AutoSeleccionar(FiguraActiva);
                        }
                    }
                }
                return;
            }

            if (HerramientaActual == HerramientaBasica.LapizLibre || HerramientaActual == HerramientaBasica.Borrador)
            {
                // Ya fue dibujado permanentemente en LienzoPrincipal
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
                        FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    }
                    else
                    {
                        FiguraActiva = null;
                    }
                }
                else if (_estadoCurva == 1)
                {
                    _p1 = puntoFinal;
                    _estadoCurva = 2;
                    FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                }
                else if (_estadoCurva == 2)
                {
                    _p2 = puntoFinal;
                    _estadoCurva = 0;
                    FiguraActiva = new CurvaBezier(_p0, _p1, _p2, _p3) { ColorLinea = ColorActivo, Grosor = GrosorActual };
                    AutoSeleccionar(FiguraActiva);
                }
            }
            else if (HerramientaActual != HerramientaBasica.Seleccion && HerramientaActual != HerramientaBasica.Relleno)
            {
                if (_puntoInicio != puntoFinal && FiguraActiva != null)
                {
                    AutoSeleccionar(FiguraActiva);
                }
                else
                {
                    FiguraActiva = null;
                }
            }
        }

        private void AutoSeleccionar(Figura figura)
        {
            if (figura == null) return;
            
            if (figura.CentroGeometrico == Point.Empty)
            {
                Rectangle baseCaja = figura.ObtenerAABBBase();
                figura.CentroGeometrico = new Point(baseCaja.Left + baseCaja.Width / 2, baseCaja.Top + baseCaja.Height / 2);
            }

            figura.IniciarTransformacion();
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

        public Cursor ObtenerCursor(Point puntoActual)
        {
            if (HerramientaActual == HerramientaBasica.Seleccion)
            {
                if (FiguraActiva != null && !(FiguraActiva is Rectangulo && _estaDibujando))
                {
                    Rectangle cajaBase = FiguraActiva.ObtenerAABBBase();
                    ManejadorActivo manejador = DeterminarManejador(puntoActual, FiguraActiva, cajaBase);
                    
                    switch (manejador)
                    {
                        case ManejadorActivo.Cuerpo: return Cursors.SizeAll;
                        case ManejadorActivo.EscalaNO:
                        case ManejadorActivo.EscalaSE: return Cursors.SizeNWSE;
                        case ManejadorActivo.EscalaNE:
                        case ManejadorActivo.EscalaSO: return Cursors.SizeNESW;
                        case ManejadorActivo.EscalaN:
                        case ManejadorActivo.EscalaS: return Cursors.SizeNS;
                        case ManejadorActivo.EscalaE:
                        case ManejadorActivo.EscalaO: return Cursors.SizeWE;
                        case ManejadorActivo.Rotacion: return Cursors.Hand;
                    }
                }
                return Cursors.Cross;
            }
            else if (FiguraActiva != null && _estadoCurva == 0)
            {
                Rectangle cajaBase = FiguraActiva.ObtenerAABBBase();
                ManejadorActivo manejador = DeterminarManejador(puntoActual, FiguraActiva, cajaBase);
                switch (manejador)
                {
                    case ManejadorActivo.Cuerpo: return Cursors.SizeAll;
                    case ManejadorActivo.EscalaNO:
                    case ManejadorActivo.EscalaSE: return Cursors.SizeNWSE;
                    case ManejadorActivo.EscalaNE:
                    case ManejadorActivo.EscalaSO: return Cursors.SizeNESW;
                    case ManejadorActivo.EscalaN:
                    case ManejadorActivo.EscalaS: return Cursors.SizeNS;
                    case ManejadorActivo.EscalaE:
                    case ManejadorActivo.EscalaO: return Cursors.SizeWE;
                    case ManejadorActivo.Rotacion: return Cursors.Hand;
                }
            }
            else if (HerramientaActual == HerramientaBasica.Relleno) return Cursors.Cross;
            else if (HerramientaActual == HerramientaBasica.Borrador) return Cursors.Default; // o un cursor personalizado
            
            return Cursors.Cross;
        }
    }
}
