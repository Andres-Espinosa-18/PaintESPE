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

        public MainForm()
        {
            InitializeComponent();
            InicializarMVC();
        }

        private void InicializarMVC()
        {
            // 1. Inicialización de los Controladores
            _gestorLienzo = new GestorLienzo(pictureBoxLienzo.Width, pictureBoxLienzo.Height);
            _controladorDibujo = new ControladorDibujo(_gestorLienzo);

            // Activación manual del DoubleBuffering en el PictureBox usando Reflection para evitar Flicker
            typeof(PictureBox).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, pictureBoxLienzo, new object[] { true });

            _imagenRenderizada = _gestorLienzo.Renderizar();

            // 2. Suscripción de Eventos del Mouse
            pictureBoxLienzo.MouseDown += PictureBoxLienzo_MouseDown;
            pictureBoxLienzo.MouseMove += PictureBoxLienzo_MouseMove;
            pictureBoxLienzo.MouseUp += PictureBoxLienzo_MouseUp;
            
            // 3. Suscripción al Redibujado (Paint) y Cambio de Tamaño
            pictureBoxLienzo.Paint += PictureBoxLienzo_Paint;
            pictureBoxLienzo.Resize += PictureBoxLienzo_Resize;
        }

        // ==========================================
        // EVENTOS DEL MOUSE
        // ==========================================
        private void PictureBoxLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _controladorDibujo.ProcesarMouseDown(e.X, e.Y);
            }
        }

        private void PictureBoxLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Solicitamos previsualización y forzamos redibujo
                _imagenRenderizada = _controladorDibujo.ProcesarMouseMove(e.X, e.Y);
                pictureBoxLienzo.Invalidate();
            }
        }

        private void PictureBoxLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _controladorDibujo.ProcesarMouseUp(e.X, e.Y);
                
                // Consolidamos la figura y refrescamos
                _imagenRenderizada = _gestorLienzo.Renderizar();
                pictureBoxLienzo.Invalidate();
            }
        }

        // ==========================================
        // REDIBUJADO
        // ==========================================
        private void PictureBoxLienzo_Paint(object sender, PaintEventArgs e)
        {
            // Se dibuja el buffer maestro manejado por GestorLienzo en un solo golpe de procesamiento
            if (_imagenRenderizada != null)
            {
                e.Graphics.DrawImageUnscaled(_imagenRenderizada, 0, 0);
            }
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

        // ==========================================
        // EVENTOS DE BOTONES (CAMBIO DE ESTADO UI)
        // ==========================================
        private void btnLapiz_Click(object sender, EventArgs e)
        {
            _controladorDibujo.HerramientaActual = HerramientaBasica.LapizLibre;
        }

        private void btnLinea_Click(object sender, EventArgs e)
        {
            _controladorDibujo.HerramientaActual = HerramientaBasica.LineaRecta;
        }

        private void btnRectangulo_Click(object sender, EventArgs e)
        {
            _controladorDibujo.HerramientaActual = HerramientaBasica.Rectangulo;
        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            _controladorDibujo.HerramientaActual = HerramientaBasica.Circulo;
        }

        private void btnColorPrimario_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = _controladorDibujo.ColorPrimario;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _controladorDibujo.ColorPrimario = dialog.Color;
                    btnColorPrimario.BackColor = dialog.Color;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            _gestorLienzo.LimpiarLienzo();
            _imagenRenderizada = _gestorLienzo.Renderizar();
            pictureBoxLienzo.Invalidate();
        }
    }
}
