namespace CapaPresentacion
{
    partial class FormPantallaCarga
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPantallaCarga));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlFondo = new Guna.UI2.WinForms.Guna2Panel();
            lblTitulo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            pgbCarga = new Guna.UI2.WinForms.Guna2ProgressBar();
            lblPorcentaje = new Guna.UI2.WinForms.Guna2HtmlLabel();
            tmrCarga = new System.Windows.Forms.Timer(components);
            pnlFondo.SuspendLayout();
            SuspendLayout();
            // 
            // pnlFondo
            // 
            pnlFondo.BackgroundImage = (Image)resources.GetObject("pnlFondo.BackgroundImage");
            pnlFondo.BackgroundImageLayout = ImageLayout.Zoom;
            pnlFondo.Controls.Add(lblPorcentaje);
            pnlFondo.Controls.Add(lblTitulo);
            pnlFondo.Controls.Add(pgbCarga);
            pnlFondo.CustomizableEdges = customizableEdges3;
            pnlFondo.Dock = DockStyle.Fill;
            pnlFondo.FillColor = Color.Transparent;
            pnlFondo.Location = new Point(0, 0);
            pnlFondo.Name = "pnlFondo";
            pnlFondo.ShadowDecoration.Color = Color.FromArgb(200, 200, 200);
            pnlFondo.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlFondo.ShadowDecoration.Depth = 10;
            pnlFondo.ShadowDecoration.Enabled = true;
            pnlFondo.Size = new Size(820, 520);
            pnlFondo.TabIndex = 0;
            // 
            // lblTitulo
            // 
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(75, 0, 130);
            lblTitulo.Location = new Point(0, 472);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(195, 32);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Cargando sistema...";
            // 
            // pgbCarga
            // 
            pgbCarga.AutoRoundedCorners = true;
            pgbCarga.BackColor = Color.Transparent;
            pgbCarga.BorderRadius = 4;
            pgbCarga.CustomizableEdges = customizableEdges1;
            pgbCarga.Dock = DockStyle.Bottom;
            pgbCarga.FillColor = Color.FromArgb(235, 235, 235);
            pgbCarga.Location = new Point(0, 510);
            pgbCarga.Name = "pgbCarga";
            pgbCarga.ProgressColor = Color.FromArgb(75, 0, 130);
            pgbCarga.ProgressColor2 = Color.FromArgb(240, 190, 50);
            pgbCarga.ShadowDecoration.Color = Color.FromArgb(120, 120, 120);
            pgbCarga.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pgbCarga.ShadowDecoration.Depth = 3;
            pgbCarga.ShadowDecoration.Enabled = true;
            pgbCarga.Size = new Size(820, 10);
            pgbCarga.TabIndex = 2;
            pgbCarga.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // lblPorcentaje
            // 
            lblPorcentaje.BackColor = Color.Transparent;
            lblPorcentaje.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblPorcentaje.ForeColor = Color.FromArgb(75, 0, 130);
            lblPorcentaje.Location = new Point(197, 472);
            lblPorcentaje.Name = "lblPorcentaje";
            lblPorcentaje.Size = new Size(57, 32);
            lblPorcentaje.TabIndex = 3;
            lblPorcentaje.Text = "100%";
            // 
            // tmrCarga
            // 
            tmrCarga.Enabled = true;
            tmrCarga.Interval = 40;
            tmrCarga.Tick += tmrCarga_Tick;
            // 
            // FormPantallaCarga
            // 
            BackColor = Color.White;
            ClientSize = new Size(820, 520);
            ControlBox = false;
            Controls.Add(pnlFondo);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormPantallaCarga";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormPantallaCarga";
            Load += FormPantallaCarga_Load;
            pnlFondo.ResumeLayout(false);
            pnlFondo.PerformLayout();
            ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel pnlFondo;
        private Guna.UI2.WinForms.Guna2ProgressBar pgbCarga;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitulo;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPorcentaje;
        private System.Windows.Forms.Timer tmrCarga;
    }

    #endregion
}