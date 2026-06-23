using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using PaintESPE.Controllers;

namespace PaintESPE.Views
{
    public partial class MainForm : Form
    {
        private GestorLienzo _gestorLienzo;
        private ControladorDibujo _controladorDibujo;
        private Bitmap _imagenRenderizada;
        private bool _colorActivoEsPrimario = true;
        private Cursor _cursorActual = Cursors.Default;
        private Point _posicionRaton;

        private static readonly Color[] ColoresPaleta = {
            Color.Black, Color.FromArgb(64, 64, 64), Color.Gray, Color.Silver,
            Color.White, Color.Maroon, Color.Red, Color.Orange,
            Color.Gold, Color.Yellow, Color.Lime, Color.Green,
            Color.Teal, Color.Cyan, Color.Blue, Color.Navy,
            Color.Purple, Color.FromArgb(255, 105, 180), Color.Brown, Color.Coral
        };

        public MainForm()
        {
            InitializeComponent();
            InicializarMVC();
            InicializarPaletaColores();
            ResaltarBotonHerramienta(_controladorDibujo.HerramientaActual);
            ActualizarIndicadorColorActivo();
        }

        private void InicializarMVC()
        {
            _gestorLienzo = new GestorLienzo(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
            _controladorDibujo = new ControladorDibujo(_gestorLienzo);

            typeof(PictureBox).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pictureBoxLienzo, new object[] { true });

            _controladorDibujo.ActualizarTamanioBuffer(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
            _imagenRenderizada = _gestorLienzo.LienzoPrincipal;

            pictureBoxLienzo.MouseDown += PictureBoxLienzo_MouseDown;
            pictureBoxLienzo.MouseMove += PictureBoxLienzo_MouseMove;
            pictureBoxLienzo.MouseUp += PictureBoxLienzo_MouseUp;
            pictureBoxLienzo.Paint += PictureBoxLienzo_Paint;
            pictureBoxLienzo.Resize += PictureBoxLienzo_Resize;
        }

        private void InicializarPaletaColores()
        {
            int size = 18;
            int gap = 3;
            int startX = 75;

            for (int i = 0; i < ColoresPaleta.Length; i++)
            {
                int x = startX + i * (size + gap);
                Color color = ColoresPaleta[i];
                var btn = new Button
                {
                    Size = new Size(size, size),
                    Location = new Point(x, 45),
                    BackColor = color,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 1 },
                    Tag = color,
                    Cursor = Cursors.Hand
                };

                btn.MouseClick += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (_colorActivoEsPrimario)
                        {
                            _controladorDibujo.ColorPrimario = color;
                            _controladorDibujo.ColorActivo = color;
                            btnColor1.BackColor = color;
                        }
                        else
                        {
                            _controladorDibujo.ColorSecundario = color;
                            _controladorDibujo.ColorActivo = color;
                            btnColor2.BackColor = color;
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        _controladorDibujo.ColorSecundario = color;
                        btnColor2.BackColor = color;
                    }
                };

                panelHerramientas.Controls.Add(btn);
            }
        }

        private void PictureBoxLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _controladorDibujo.ProcesarMouseDown(e.X, e.Y);
                Bitmap nuevoBuffer = _controladorDibujo.ProcesarMouseMove(e.X, e.Y, out Rectangle dirtyRect);
                ActualizarRenderizado(nuevoBuffer, dirtyRect);
            }
        }

        private void PictureBoxLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            _posicionRaton = e.Location;

            Cursor nuevoCursor = _controladorDibujo.ObtenerCursor(e.Location);
            if (_cursorActual != nuevoCursor)
            {
                _cursorActual = nuevoCursor;
                pictureBoxLienzo.Cursor = _cursorActual;
            }

            if (e.Button == MouseButtons.Left)
            {
                Bitmap nuevoBuffer = _controladorDibujo.ProcesarMouseMove(e.X, e.Y, out Rectangle dirtyRect);
                ActualizarRenderizado(nuevoBuffer, dirtyRect);
            }
        }

        private void PictureBoxLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _controladorDibujo.ProcesarMouseUp(e.X, e.Y);
                Bitmap nuevoBuffer = _controladorDibujo.ProcesarMouseMove(e.X, e.Y, out Rectangle dirtyRect);
                ActualizarRenderizado(nuevoBuffer, dirtyRect);
            }
        }

        private void ActualizarRenderizado(Bitmap nuevoBuffer, Rectangle dirtyRect)
        {
            if (nuevoBuffer != null)
            {
                _imagenRenderizada = nuevoBuffer;
            }
            else
            {
                _imagenRenderizada = _gestorLienzo.LienzoPrincipal;
            }

            if (dirtyRect != Rectangle.Empty)
                pictureBoxLienzo.Invalidate(dirtyRect);
            else
                pictureBoxLienzo.Invalidate();
        }

        private void PictureBoxLienzo_Paint(object sender, PaintEventArgs e)
        {
            if (_imagenRenderizada != null)
                e.Graphics.DrawImageUnscaled(_imagenRenderizada, 0, 0);

            if (_controladorDibujo.FiguraActiva != null)
            {
                var fig = _controladorDibujo.FiguraActiva;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                if (fig is PaintESPE.Models.FiguraPixel figPixel && figPixel.BufferPixeles != null)
                {
                    PointF[] destPoints = new PointF[] {
                        new PointF(fig.Puntos[0].X, fig.Puntos[0].Y),
                        new PointF(fig.Puntos[1].X, fig.Puntos[1].Y),
                        new PointF(fig.Puntos[3].X, fig.Puntos[3].Y)
                    };
                    e.Graphics.DrawImage(figPixel.BufferPixeles, destPoints);
                }
                else
                {
                    using (Pen penFantasma = new Pen(fig.ColorLinea, fig.Grosor))
                    {
                        if (fig is PaintESPE.Models.Elipse)
                        {
                            Rectangle aabb = fig.ObtenerAABBBase();
                            int rx = aabb.Width / 2;
                            int ry = aabb.Height / 2;
                            Point centro = fig.CentroGeometrico;
                            if (centro == Point.Empty) centro = new Point(aabb.Left + rx, aabb.Top + ry);

                            e.Graphics.TranslateTransform(centro.X, centro.Y);
                            e.Graphics.RotateTransform(fig.AnguloRotacion);
                            e.Graphics.DrawEllipse(penFantasma, -rx, -ry, rx * 2, ry * 2);
                            e.Graphics.ResetTransform();
                        }
                        else if (fig is PaintESPE.Models.CurvaBezier && fig.Puntos.Count == 4)
                        {
                            e.Graphics.DrawBezier(penFantasma, fig.Puntos[0], fig.Puntos[1], fig.Puntos[2], fig.Puntos[3]);
                        }
                        else if (fig is PaintESPE.Models.Linea && fig.Puntos.Count == 2)
                        {
                            e.Graphics.DrawLine(penFantasma, fig.Puntos[0], fig.Puntos[1]);
                        }
                        else if (fig.Puntos.Count > 1)
                        {
                            e.Graphics.DrawPolygon(penFantasma, fig.Puntos.ToArray());
                        }
                    }
                }

                Point[] esquinas = fig.ObtenerPuntosCaja();
                Rectangle cajaBase = fig.ObtenerAABBBase();

                if (esquinas.Length == 4 && cajaBase.Width > 0 && cajaBase.Height > 0)
                {
                    using (Pen penBorde = new Pen(Color.DarkGray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    {
                        e.Graphics.DrawPolygon(penBorde, esquinas);
                    }

                    int r = PaintESPE.Raster.DibujoSeleccion.TamañoManejador / 2;
                    Point[] handlesLocales = new Point[] {
                        new Point(cajaBase.Left, cajaBase.Top),
                        new Point(cajaBase.Left + cajaBase.Width / 2, cajaBase.Top),
                        new Point(cajaBase.Right, cajaBase.Top),
                        new Point(cajaBase.Right, cajaBase.Top + cajaBase.Height / 2),
                        new Point(cajaBase.Right, cajaBase.Bottom),
                        new Point(cajaBase.Left + cajaBase.Width / 2, cajaBase.Bottom),
                        new Point(cajaBase.Left, cajaBase.Bottom),
                        new Point(cajaBase.Left, cajaBase.Top + cajaBase.Height / 2)
                    };

                    foreach (var pLocal in handlesLocales)
                    {
                        Point p = PaintESPE.Raster.Transformacion.Rotar(pLocal, fig.AnguloRotacion, fig.CentroGeometrico);
                        Rectangle rect = new Rectangle(p.X - r, p.Y - r, r * 2, r * 2);
                        e.Graphics.FillRectangle(Brushes.White, rect);
                        e.Graphics.DrawRectangle(Pens.Black, rect);
                    }

                    Point centroSuperiorLocal = new Point(cajaBase.Left + cajaBase.Width / 2, cajaBase.Top);
                    Point rotHandleLocal = new Point(centroSuperiorLocal.X, centroSuperiorLocal.Y - PaintESPE.Raster.DibujoSeleccion.DistanciaRotacion);
                    
                    Point centroSuperior = PaintESPE.Raster.Transformacion.Rotar(centroSuperiorLocal, fig.AnguloRotacion, fig.CentroGeometrico);
                    Point rotHandle = PaintESPE.Raster.Transformacion.Rotar(rotHandleLocal, fig.AnguloRotacion, fig.CentroGeometrico);

                    using (Pen penLinea = new Pen(Color.DarkGray, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
                    {
                        e.Graphics.DrawLine(penLinea, centroSuperior, rotHandle);
                    }
                    
                    int r2 = 4;
                    Rectangle rectRot = new Rectangle(rotHandle.X - r2, rotHandle.Y - r2, r2 * 2, r2 * 2);
                    e.Graphics.FillEllipse(Brushes.White, rectRot);
                    e.Graphics.DrawEllipse(Pens.Black, rectRot);
                }
            }

        }

        private void PictureBoxLienzo_Resize(object sender, EventArgs e)
        {
            if (pictureBoxLienzo.Width > 0 && pictureBoxLienzo.Height > 0)
            {
                _controladorDibujo.SellarFiguraActiva();
                _gestorLienzo.ActualizarTamanio(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
                _controladorDibujo.ActualizarTamanioBuffer(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
                _imagenRenderizada = _gestorLienzo.LienzoPrincipal;
                pictureBoxLienzo.Invalidate();
            }
        }

        private void SeleccionarHerramienta(HerramientaBasica herramienta)
        {
            _controladorDibujo.HerramientaActual = herramienta;
            ResaltarBotonHerramienta(herramienta);
        }

        private void ResaltarBotonHerramienta(HerramientaBasica herramienta)
        {
            Color highlight = Color.FromArgb(200, 210, 230);
            Color normal = SystemColors.Control;

            btnLapiz.BackColor = normal;
            btnBorrador.BackColor = normal;
            btnLinea.BackColor = normal;
            btnRectangulo.BackColor = normal;
            btnElipse.BackColor = normal;
            btnTriangulo.BackColor = normal;
            btnPoligono.BackColor = normal;
            btnEstrella.BackColor = normal;
            btnCurva.BackColor = normal;
            btnRelleno.BackColor = normal;
            btnSeleccion.BackColor = normal;

            switch (herramienta)
            {
                case HerramientaBasica.LapizLibre: btnLapiz.BackColor = highlight; break;
            case HerramientaBasica.Borrador: btnBorrador.BackColor = highlight; break;
                case HerramientaBasica.LineaRecta: btnLinea.BackColor = highlight; break;
                case HerramientaBasica.Rectangulo: btnRectangulo.BackColor = highlight; break;
                case HerramientaBasica.Elipse: btnElipse.BackColor = highlight; break;
                case HerramientaBasica.Triangulo: btnTriangulo.BackColor = highlight; break;
                case HerramientaBasica.PoligonoRegular: btnPoligono.BackColor = highlight; break;
                case HerramientaBasica.Estrella: btnEstrella.BackColor = highlight; break;
                case HerramientaBasica.Curva: btnCurva.BackColor = highlight; break;
                case HerramientaBasica.Relleno: btnRelleno.BackColor = highlight; break;
                case HerramientaBasica.Seleccion: btnSeleccion.BackColor = highlight; break;
            }
        }

        private void btnLapiz_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.LapizLibre);
        private void btnBorrador_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Borrador);
        private void btnLinea_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.LineaRecta);
        private void btnRectangulo_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Rectangulo);
        private void btnElipse_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Elipse);
        private void btnTriangulo_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Triangulo);
        private void btnPoligono_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.PoligonoRegular);
        private void btnEstrella_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Estrella);
        private void btnCurva_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Curva);
        private void btnRelleno_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Relleno);
        private void btnSeleccion_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Seleccion);

        private void ActualizarIndicadorColorActivo()
        {
            if (_colorActivoEsPrimario)
            {
                btnColor1.FlatAppearance.BorderSize = 3;
                btnColor2.FlatAppearance.BorderSize = 1;
            }
            else
            {
                btnColor1.FlatAppearance.BorderSize = 1;
                btnColor2.FlatAppearance.BorderSize = 3;
            }
        }

        private void btnColor1_Click(object sender, EventArgs e)
        {
            _colorActivoEsPrimario = true;
            _controladorDibujo.EstablecerColorActivo(true);
            ActualizarIndicadorColorActivo();
        }

        private void btnColor2_Click(object sender, EventArgs e)
        {
            _colorActivoEsPrimario = false;
            _controladorDibujo.EstablecerColorActivo(false);
            ActualizarIndicadorColorActivo();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            _controladorDibujo.SellarFiguraActiva();
            _gestorLienzo.LimpiarLienzo();
            _imagenRenderizada = _gestorLienzo.LienzoPrincipal;
            pictureBoxLienzo.Invalidate();
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Abrir Imagen de Proyecto";
                dialog.Filter = "Archivos de Imagen|*.png;*.jpg;*.jpeg;*.bmp|Todos los archivos|*.*";
                
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _controladorDibujo.SellarFiguraActiva();
                    _gestorLienzo.CargarImagenDesdeArchivo(dialog.FileName);
                    _controladorDibujo.ActualizarTamanioBuffer(_gestorLienzo.LienzoPrincipal.Width, _gestorLienzo.LienzoPrincipal.Height);
                    _imagenRenderizada = _gestorLienzo.LienzoPrincipal;
                    pictureBoxLienzo.Invalidate();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Guardar Dibujo";
                dialog.Filter = "Imagen PNG (*.png)|*.png|Imagen JPEG (*.jpg)|*.jpg|Mapa de bits (*.bmp)|*.bmp";
                dialog.DefaultExt = "png";
                dialog.AddExtension = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _controladorDibujo.SellarFiguraActiva();

                    System.Drawing.Imaging.ImageFormat formato = System.Drawing.Imaging.ImageFormat.Png;
                    if (dialog.FilterIndex == 2) formato = System.Drawing.Imaging.ImageFormat.Jpeg;
                    else if (dialog.FilterIndex == 3) formato = System.Drawing.Imaging.ImageFormat.Bmp;

                    _gestorLienzo.GuardarImagen(dialog.FileName, formato);
                    MessageBox.Show("¡Imagen guardada exitosamente!", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnMasColores_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                if (_colorActivoEsPrimario)
                {
                    dialog.Color = _controladorDibujo.ColorPrimario;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _controladorDibujo.ColorPrimario = dialog.Color;
                        _controladorDibujo.ColorActivo = dialog.Color;
                        btnColor1.BackColor = dialog.Color;
                    }
                }
                else
                {
                    dialog.Color = _controladorDibujo.ColorSecundario;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        _controladorDibujo.ColorSecundario = dialog.Color;
                        _controladorDibujo.ColorActivo = dialog.Color;
                        btnColor2.BackColor = dialog.Color;
                    }
                }
            }
        }

        private void nudGrosor_ValueChanged(object sender, EventArgs e)
        {
            _controladorDibujo.GrosorActual = (int)nudGrosor.Value;
        }

        private void nudLados_ValueChanged(object sender, EventArgs e)
        {
            _controladorDibujo.NumeroLados = (int)nudLados.Value;
        }
    }
}
