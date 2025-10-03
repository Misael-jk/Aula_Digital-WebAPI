namespace CapaPresentacion
{
    partial class Dashboard
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            btnElementos = new Button();
            groupBox1 = new GroupBox();
            btnDocentes = new Button();
            btnDevoluciones = new Button();
            btnPrestamos = new Button();
            btnCarritos = new Button();
            dataGridView1 = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnElementos
            // 
            btnElementos.Location = new Point(18, 34);
            btnElementos.Name = "btnElementos";
            btnElementos.Size = new Size(140, 141);
            btnElementos.TabIndex = 0;
            btnElementos.Text = "ELEMENTOS";
            btnElementos.TextAlign = ContentAlignment.BottomCenter;
            btnElementos.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDocentes);
            groupBox1.Controls.Add(btnDevoluciones);
            groupBox1.Controls.Add(btnPrestamos);
            groupBox1.Controls.Add(btnCarritos);
            groupBox1.Controls.Add(btnElementos);
            groupBox1.Location = new Point(17, 14);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(840, 211);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "DESTACADOS";
            // 
            // btnDocentes
            // 
            btnDocentes.Location = new Point(683, 34);
            btnDocentes.Name = "btnDocentes";
            btnDocentes.Size = new Size(140, 141);
            btnDocentes.TabIndex = 4;
            btnDocentes.Text = "DOCENTES";
            btnDocentes.TextAlign = ContentAlignment.BottomCenter;
            btnDocentes.UseVisualStyleBackColor = true;
            // 
            // btnDevoluciones
            // 
            btnDevoluciones.Location = new Point(519, 34);
            btnDevoluciones.Name = "btnDevoluciones";
            btnDevoluciones.Size = new Size(140, 141);
            btnDevoluciones.TabIndex = 3;
            btnDevoluciones.Text = "DEVOLUCIONES";
            btnDevoluciones.TextAlign = ContentAlignment.BottomCenter;
            btnDevoluciones.UseVisualStyleBackColor = true;
            // 
            // btnPrestamos
            // 
            btnPrestamos.Location = new Point(352, 34);
            btnPrestamos.Name = "btnPrestamos";
            btnPrestamos.Size = new Size(140, 141);
            btnPrestamos.TabIndex = 2;
            btnPrestamos.Text = "PRESTAMOS";
            btnPrestamos.TextAlign = ContentAlignment.BottomCenter;
            btnPrestamos.UseVisualStyleBackColor = true;
            // 
            // btnCarritos
            // 
            btnCarritos.Location = new Point(183, 34);
            btnCarritos.Name = "btnCarritos";
            btnCarritos.Size = new Size(140, 141);
            btnCarritos.TabIndex = 1;
            btnCarritos.Text = "CARRITOS";
            btnCarritos.TextAlign = ContentAlignment.BottomCenter;
            btnCarritos.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(129, 379);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(621, 197);
            dataGridView1.TabIndex = 7;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dataGridView1);
            Controls.Add(groupBox1);
            Name = "Dashboard";
            Size = new Size(1054, 671);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnElementos;
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private Button btnDocentes;
        private Button btnDevoluciones;
        private Button btnPrestamos;
        private Button btnCarritos;
    }
}
