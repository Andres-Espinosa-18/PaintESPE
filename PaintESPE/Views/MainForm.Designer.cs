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
            this.btnElipse = new System.Windows.Forms.Button();
            this.btnTriangulo = new System.Windows.Forms.Button();
            this.btnPoligono = new System.Windows.Forms.Button();
            this.btnEstrella = new System.Windows.Forms.Button();
            this.btnRelleno = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnColor1 = new System.Windows.Forms.Button();
            this.btnColor2 = new System.Windows.Forms.Button();
            this.btnMasColores = new System.Windows.Forms.Button();
            this.lblGrosor = new System.Windows.Forms.Label();
            this.nudGrosor = new System.Windows.Forms.NumericUpDown();
            this.lblLados = new System.Windows.Forms.Label();
            this.nudLados = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLienzo)).BeginInit();
            this.panelHerramientas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrosor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLados)).BeginInit();
            this.SuspendLayout();
            //
            // panelHerramientas
            //
            this.panelHerramientas.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelHerramientas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHerramientas.Controls.Add(this.btnLapiz);
            this.panelHerramientas.Controls.Add(this.btnLinea);
            this.panelHerramientas.Controls.Add(this.btnRectangulo);
            this.panelHerramientas.Controls.Add(this.btnCirculo);
            this.panelHerramientas.Controls.Add(this.btnElipse);
            this.panelHerramientas.Controls.Add(this.btnTriangulo);
            this.panelHerramientas.Controls.Add(this.btnPoligono);
            this.panelHerramientas.Controls.Add(this.btnEstrella);
            this.panelHerramientas.Controls.Add(this.btnRelleno);
            this.panelHerramientas.Controls.Add(this.btnLimpiar);
            this.panelHerramientas.Controls.Add(this.btnColor1);
            this.panelHerramientas.Controls.Add(this.btnColor2);
            this.panelHerramientas.Controls.Add(this.btnMasColores);
            this.panelHerramientas.Controls.Add(this.lblGrosor);
            this.panelHerramientas.Controls.Add(this.nudGrosor);
            this.panelHerramientas.Controls.Add(this.lblLados);
            this.panelHerramientas.Controls.Add(this.nudLados);
            this.panelHerramientas.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHerramientas.Location = new System.Drawing.Point(0, 0);
            this.panelHerramientas.Name = "panelHerramientas";
            this.panelHerramientas.Size = new System.Drawing.Size(900, 78);
            this.panelHerramientas.TabIndex = 0;
            //
            // btnLapiz
            //
            this.btnLapiz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLapiz.FlatAppearance.BorderSize = 1;
            this.btnLapiz.Location = new System.Drawing.Point(8, 8);
            this.btnLapiz.Name = "btnLapiz";
            this.btnLapiz.Size = new System.Drawing.Size(55, 26);
            this.btnLapiz.TabIndex = 0;
            this.btnLapiz.Text = "Lápiz";
            this.btnLapiz.UseVisualStyleBackColor = true;
            this.btnLapiz.Click += new System.EventHandler(this.btnLapiz_Click);
            //
            // btnLinea
            //
            this.btnLinea.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLinea.FlatAppearance.BorderSize = 1;
            this.btnLinea.Location = new System.Drawing.Point(68, 8);
            this.btnLinea.Name = "btnLinea";
            this.btnLinea.Size = new System.Drawing.Size(55, 26);
            this.btnLinea.TabIndex = 1;
            this.btnLinea.Text = "Línea";
            this.btnLinea.UseVisualStyleBackColor = true;
            this.btnLinea.Click += new System.EventHandler(this.btnLinea_Click);
            //
            // btnRectangulo
            //
            this.btnRectangulo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRectangulo.FlatAppearance.BorderSize = 1;
            this.btnRectangulo.Location = new System.Drawing.Point(128, 8);
            this.btnRectangulo.Name = "btnRectangulo";
            this.btnRectangulo.Size = new System.Drawing.Size(60, 26);
            this.btnRectangulo.TabIndex = 2;
            this.btnRectangulo.Text = "Rect.";
            this.btnRectangulo.UseVisualStyleBackColor = true;
            this.btnRectangulo.Click += new System.EventHandler(this.btnRectangulo_Click);
            //
            // btnCirculo
            //
            this.btnCirculo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCirculo.FlatAppearance.BorderSize = 1;
            this.btnCirculo.Location = new System.Drawing.Point(193, 8);
            this.btnCirculo.Name = "btnCirculo";
            this.btnCirculo.Size = new System.Drawing.Size(55, 26);
            this.btnCirculo.TabIndex = 3;
            this.btnCirculo.Text = "Círculo";
            this.btnCirculo.UseVisualStyleBackColor = true;
            this.btnCirculo.Click += new System.EventHandler(this.btnCirculo_Click);
            //
            // btnElipse
            //
            this.btnElipse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnElipse.FlatAppearance.BorderSize = 1;
            this.btnElipse.Location = new System.Drawing.Point(253, 8);
            this.btnElipse.Name = "btnElipse";
            this.btnElipse.Size = new System.Drawing.Size(55, 26);
            this.btnElipse.TabIndex = 4;
            this.btnElipse.Text = "Elipse";
            this.btnElipse.UseVisualStyleBackColor = true;
            this.btnElipse.Click += new System.EventHandler(this.btnElipse_Click);
            //
            // btnTriangulo
            //
            this.btnTriangulo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTriangulo.FlatAppearance.BorderSize = 1;
            this.btnTriangulo.Location = new System.Drawing.Point(313, 8);
            this.btnTriangulo.Name = "btnTriangulo";
            this.btnTriangulo.Size = new System.Drawing.Size(60, 26);
            this.btnTriangulo.TabIndex = 5;
            this.btnTriangulo.Text = "Triáng.";
            this.btnTriangulo.UseVisualStyleBackColor = true;
            this.btnTriangulo.Click += new System.EventHandler(this.btnTriangulo_Click);
            //
            // btnPoligono
            //
            this.btnPoligono.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPoligono.FlatAppearance.BorderSize = 1;
            this.btnPoligono.Location = new System.Drawing.Point(378, 8);
            this.btnPoligono.Name = "btnPoligono";
            this.btnPoligono.Size = new System.Drawing.Size(60, 26);
            this.btnPoligono.TabIndex = 6;
            this.btnPoligono.Text = "Políg.";
            this.btnPoligono.UseVisualStyleBackColor = true;
            this.btnPoligono.Click += new System.EventHandler(this.btnPoligono_Click);
            //
            // btnEstrella
            //
            this.btnEstrella.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEstrella.FlatAppearance.BorderSize = 1;
            this.btnEstrella.Location = new System.Drawing.Point(443, 8);
            this.btnEstrella.Name = "btnEstrella";
            this.btnEstrella.Size = new System.Drawing.Size(60, 26);
            this.btnEstrella.TabIndex = 7;
            this.btnEstrella.Text = "Estrella";
            this.btnEstrella.UseVisualStyleBackColor = true;
            this.btnEstrella.Click += new System.EventHandler(this.btnEstrella_Click);
            //
            // btnRelleno
            //
            this.btnRelleno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelleno.FlatAppearance.BorderSize = 1;
            this.btnRelleno.Location = new System.Drawing.Point(520, 8);
            this.btnRelleno.Name = "btnRelleno";
            this.btnRelleno.Size = new System.Drawing.Size(60, 26);
            this.btnRelleno.TabIndex = 21;
            this.btnRelleno.Text = "Relleno";
            this.btnRelleno.UseVisualStyleBackColor = true;
            this.btnRelleno.Click += new System.EventHandler(this.btnRelleno_Click);
            //
            // btnLimpiar
            //
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.FlatAppearance.BorderSize = 1;
            this.btnLimpiar.Location = new System.Drawing.Point(700, 8);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(65, 26);
            this.btnLimpiar.TabIndex = 8;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            //
            // btnColor1
            //
            this.btnColor1.BackColor = System.Drawing.Color.Black;
            this.btnColor1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor1.FlatAppearance.BorderSize = 3;
            this.btnColor1.Location = new System.Drawing.Point(8, 38);
            this.btnColor1.Name = "btnColor1";
            this.btnColor1.Size = new System.Drawing.Size(28, 24);
            this.btnColor1.TabIndex = 9;
            this.btnColor1.UseVisualStyleBackColor = false;
            this.btnColor1.Click += new System.EventHandler(this.btnColor1_Click);
            //
            // btnColor2
            //
            this.btnColor2.BackColor = System.Drawing.Color.White;
            this.btnColor2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor2.FlatAppearance.BorderSize = 1;
            this.btnColor2.Location = new System.Drawing.Point(38, 38);
            this.btnColor2.Name = "btnColor2";
            this.btnColor2.Size = new System.Drawing.Size(28, 24);
            this.btnColor2.TabIndex = 10;
            this.btnColor2.UseVisualStyleBackColor = false;
            this.btnColor2.Click += new System.EventHandler(this.btnColor2_Click);
            //
            // btnMasColores
            //
            this.btnMasColores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMasColores.FlatAppearance.BorderSize = 1;
            this.btnMasColores.Location = new System.Drawing.Point(585, 42);
            this.btnMasColores.Name = "btnMasColores";
            this.btnMasColores.Size = new System.Drawing.Size(85, 24);
            this.btnMasColores.TabIndex = 20;
            this.btnMasColores.Text = "Más colores...";
            this.btnMasColores.UseVisualStyleBackColor = true;
            this.btnMasColores.Click += new System.EventHandler(this.btnMasColores_Click);
            //
            // lblGrosor
            //
            this.lblGrosor.AutoSize = true;
            this.lblGrosor.Location = new System.Drawing.Point(680, 47);
            this.lblGrosor.Name = "lblGrosor";
            this.lblGrosor.Size = new System.Drawing.Size(43, 13);
            this.lblGrosor.TabIndex = 11;
            this.lblGrosor.Text = "Grosor:";
            //
            // nudGrosor
            //
            this.nudGrosor.Location = new System.Drawing.Point(723, 44);
            this.nudGrosor.Name = "nudGrosor";
            this.nudGrosor.Size = new System.Drawing.Size(45, 20);
            this.nudGrosor.TabIndex = 12;
            this.nudGrosor.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudGrosor.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudGrosor.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudGrosor.ValueChanged += new System.EventHandler(this.nudGrosor_ValueChanged);
            //
            // lblLados
            //
            this.lblLados.AutoSize = true;
            this.lblLados.Location = new System.Drawing.Point(778, 47);
            this.lblLados.Name = "lblLados";
            this.lblLados.Size = new System.Drawing.Size(39, 13);
            this.lblLados.TabIndex = 13;
            this.lblLados.Text = "Lados:";
            //
            // nudLados
            //
            this.nudLados.Location = new System.Drawing.Point(815, 44);
            this.nudLados.Name = "nudLados";
            this.nudLados.Size = new System.Drawing.Size(45, 20);
            this.nudLados.TabIndex = 14;
            this.nudLados.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            this.nudLados.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudLados.Value = new decimal(new int[] { 5, 0, 0, 0 });
            this.nudLados.ValueChanged += new System.EventHandler(this.nudLados_ValueChanged);
            //
            // pictureBoxLienzo
            //
            this.pictureBoxLienzo.BackColor = System.Drawing.Color.White;
            this.pictureBoxLienzo.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pictureBoxLienzo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxLienzo.Location = new System.Drawing.Point(0, 78);
            this.pictureBoxLienzo.Name = "pictureBoxLienzo";
            this.pictureBoxLienzo.Size = new System.Drawing.Size(900, 472);
            this.pictureBoxLienzo.TabIndex = 1;
            this.pictureBoxLienzo.TabStop = false;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Controls.Add(this.pictureBoxLienzo);
            this.Controls.Add(this.panelHerramientas);
            this.Name = "MainForm";
            this.Text = "Paint ESPE - Arquitectura MVC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLienzo)).EndInit();
            this.panelHerramientas.ResumeLayout(false);
            this.panelHerramientas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrosor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLados)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHerramientas;
        private System.Windows.Forms.Button btnLapiz;
        private System.Windows.Forms.Button btnLinea;
        private System.Windows.Forms.Button btnRectangulo;
        private System.Windows.Forms.Button btnCirculo;
        private System.Windows.Forms.Button btnElipse;
        private System.Windows.Forms.Button btnTriangulo;
        private System.Windows.Forms.Button btnPoligono;
        private System.Windows.Forms.Button btnEstrella;
        private System.Windows.Forms.Button btnRelleno;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnColor1;
        private System.Windows.Forms.Button btnColor2;
        private System.Windows.Forms.Button btnMasColores;
        private System.Windows.Forms.Label lblGrosor;
        private System.Windows.Forms.NumericUpDown nudGrosor;
        private System.Windows.Forms.Label lblLados;
        private System.Windows.Forms.NumericUpDown nudLados;
        private System.Windows.Forms.PictureBox pictureBoxLienzo;
    }
}
