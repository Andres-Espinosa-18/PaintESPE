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

            _imagenRenderizada = _gestorLienzo.Renderizar();

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
                if (_controladorDibujo.HerramientaActual == HerramientaBasica.Relleno)
                {
                    _imagenRenderizada = _controladorDibujo.ProcesarClickRelleno(e.X, e.Y, _controladorDibujo.ColorActivo);
                    pictureBoxLienzo.Invalidate();
                }
                else
                {
                    _controladorDibujo.ProcesarMouseDown(e.X, e.Y);
                }
            }
        }

        private void PictureBoxLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _imagenRenderizada = _controladorDibujo.ProcesarMouseMove(e.X, e.Y);
                pictureBoxLienzo.Invalidate();
            }
        }

        private void PictureBoxLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _controladorDibujo.ProcesarMouseUp(e.X, e.Y);
                _imagenRenderizada = _gestorLienzo.Renderizar();
                pictureBoxLienzo.Invalidate();
            }
        }

        private void PictureBoxLienzo_Paint(object sender, PaintEventArgs e)
        {
            if (_imagenRenderizada != null)
                e.Graphics.DrawImageUnscaled(_imagenRenderizada, 0, 0);
        }

        private void PictureBoxLienzo_Resize(object sender, EventArgs e)
        {
            if (pictureBoxLienzo.Width > 0 && pictureBoxLienzo.Height > 0)
            {
                _gestorLienzo.ActualizarTamanio(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
                _imagenRenderizada = _gestorLienzo.Renderizar();
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
            btnLinea.BackColor = normal;
            btnRectangulo.BackColor = normal;
            btnCirculo.BackColor = normal;
            btnElipse.BackColor = normal;
            btnTriangulo.BackColor = normal;
            btnPoligono.BackColor = normal;
            btnEstrella.BackColor = normal;
            btnRelleno.BackColor = normal;

            switch (herramienta)
            {
                case HerramientaBasica.LapizLibre: btnLapiz.BackColor = highlight; break;
                case HerramientaBasica.LineaRecta: btnLinea.BackColor = highlight; break;
                case HerramientaBasica.Rectangulo: btnRectangulo.BackColor = highlight; break;
                case HerramientaBasica.Circulo: btnCirculo.BackColor = highlight; break;
                case HerramientaBasica.Elipse: btnElipse.BackColor = highlight; break;
                case HerramientaBasica.Triangulo: btnTriangulo.BackColor = highlight; break;
                case HerramientaBasica.PoligonoRegular: btnPoligono.BackColor = highlight; break;
                case HerramientaBasica.Estrella: btnEstrella.BackColor = highlight; break;
                case HerramientaBasica.Relleno: btnRelleno.BackColor = highlight; break;
            }
        }

        private void btnLapiz_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.LapizLibre);
        private void btnLinea_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.LineaRecta);
        private void btnRectangulo_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Rectangulo);
        private void btnCirculo_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Circulo);
        private void btnElipse_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Elipse);
        private void btnTriangulo_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Triangulo);
        private void btnPoligono_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.PoligonoRegular);
        private void btnEstrella_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Estrella);
        private void btnRelleno_Click(object sender, EventArgs e) => SeleccionarHerramienta(HerramientaBasica.Relleno);

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
            _gestorLienzo.LimpiarLienzo();
            _imagenRenderizada = _gestorLienzo.Renderizar();
            pictureBoxLienzo.Invalidate();
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
