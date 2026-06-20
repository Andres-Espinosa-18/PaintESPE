namespace PaintESPE.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBoxLienzo = new System.Windows.Forms.PictureBox();
            this.panelHerramientas = new System.Windows.Forms.Panel();
            this.btnLapiz = new System.Windows.Forms.Button();
            this.btnLinea = new System.Windows.Forms.Button();
            this.btnRectangulo = new System.Windows.Forms.Button();
            this.btnCirculo = new System.Windows.Forms.Button();
            this.btnColorPrimario = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLienzo)).BeginInit();
            this.panelHerramientas.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHerramientas
            // 
            this.panelHerramientas.BackColor = System.Drawing.SystemColors.Control;
            this.panelHerramientas.Controls.Add(this.btnLapiz);
            this.panelHerramientas.Controls.Add(this.btnLinea);
            this.panelHerramientas.Controls.Add(this.btnRectangulo);
            this.panelHerramientas.Controls.Add(this.btnCirculo);
            this.panelHerramientas.Controls.Add(this.btnColorPrimario);
            this.panelHerramientas.Controls.Add(this.btnLimpiar);
            this.panelHerramientas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHerramientas.Location = new System.Drawing.Point(0, 0);
            this.panelHerramientas.Name = "panelHerramientas";
            this.panelHerramientas.Size = new System.Drawing.Size(800, 50);
            this.panelHerramientas.TabIndex = 0;
            // 
            // btnLapiz
            // 
            this.btnLapiz.Location = new System.Drawing.Point(12, 12);
            this.btnLapiz.Name = "btnLapiz";
            this.btnLapiz.Size = new System.Drawing.Size(75, 25);
            this.btnLapiz.TabIndex = 0;
            this.btnLapiz.Text = "Lápiz";
            this.btnLapiz.UseVisualStyleBackColor = true;
            this.btnLapiz.Click += new System.EventHandler(this.btnLapiz_Click);
            // 
            // btnLinea
            // 
            this.btnLinea.Location = new System.Drawing.Point(93, 12);
            this.btnLinea.Name = "btnLinea";
            this.btnLinea.Size = new System.Drawing.Size(75, 25);
            this.btnLinea.TabIndex = 1;
            this.btnLinea.Text = "Línea";
            this.btnLinea.UseVisualStyleBackColor = true;
            this.btnLinea.Click += new System.EventHandler(this.btnLinea_Click);
            // 
            // btnRectangulo
            // 
            this.btnRectangulo.Location = new System.Drawing.Point(174, 12);
            this.btnRectangulo.Name = "btnRectangulo";
            this.btnRectangulo.Size = new System.Drawing.Size(80, 25);
            this.btnRectangulo.TabIndex = 2;
            this.btnRectangulo.Text = "Rectángulo";
            this.btnRectangulo.UseVisualStyleBackColor = true;
            this.btnRectangulo.Click += new System.EventHandler(this.btnRectangulo_Click);
            // 
            // btnCirculo
            // 
            this.btnCirculo.Location = new System.Drawing.Point(260, 12);
            this.btnCirculo.Name = "btnCirculo";
            this.btnCirculo.Size = new System.Drawing.Size(75, 25);
            this.btnCirculo.TabIndex = 3;
            this.btnCirculo.Text = "Círculo";
            this.btnCirculo.UseVisualStyleBackColor = true;
            this.btnCirculo.Click += new System.EventHandler(this.btnCirculo_Click);
            // 
            // btnColorPrimario
            // 
            this.btnColorPrimario.BackColor = System.Drawing.Color.Black;
            this.btnColorPrimario.ForeColor = System.Drawing.Color.White;
            this.btnColorPrimario.Location = new System.Drawing.Point(341, 12);
            this.btnColorPrimario.Name = "btnColorPrimario";
            this.btnColorPrimario.Size = new System.Drawing.Size(75, 25);
            this.btnColorPrimario.TabIndex = 4;
            this.btnColorPrimario.Text = "Color";
            this.btnColorPrimario.UseVisualStyleBackColor = false;
            this.btnColorPrimario.Click += new System.EventHandler(this.btnColorPrimario_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(422, 12);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 25);
            this.btnLimpiar.TabIndex = 5;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // pictureBoxLienzo
            // 
            this.pictureBoxLienzo.BackColor = System.Drawing.Color.White;
            this.pictureBoxLienzo.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBoxLienzo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLienzo.Location = new System.Drawing.Point(0, 50);
            this.pictureBoxLienzo.Name = "pictureBoxLienzo";
            this.pictureBoxLienzo.Size = new System.Drawing.Size(800, 400);
            this.pictureBoxLienzo.TabIndex = 1;
            this.pictureBoxLienzo.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBoxLienzo);
            this.Controls.Add(this.panelHerramientas);
            this.Name = "MainForm";
            this.Text = "Paint ESPE - Arquitectura MVC Pura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLienzo)).EndInit();
            this.panelHerramientas.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHerramientas;
        private System.Windows.Forms.Button btnLapiz;
        private System.Windows.Forms.Button btnLinea;
        private System.Windows.Forms.Button btnRectangulo;
        private System.Windows.Forms.Button btnCirculo;
        private System.Windows.Forms.Button btnColorPrimario;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.PictureBox pictureBoxLienzo;
    }
}
