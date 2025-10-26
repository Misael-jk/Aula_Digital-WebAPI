namespace CapaPresentacion
{
    partial class FormDeshabilitarCNE
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDeshabilitarCNE));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlBanner = new Panel();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            cmbEstadoMantenimiento = new Guna.UI2.WinForms.Guna2ComboBox();
            txtMotivo = new Guna.UI2.WinForms.Guna2TextBox();
            btnDeshabiliar = new Guna.UI2.WinForms.Guna2Button();
            pnlBanner.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBanner
            // 
            pnlBanner.BackColor = Color.Indigo;
            pnlBanner.Controls.Add(iconButton1);
            pnlBanner.Dock = DockStyle.Top;
            pnlBanner.Location = new Point(0, 0);
            pnlBanner.Name = "pnlBanner";
            pnlBanner.Size = new Size(370, 28);
            pnlBanner.TabIndex = 18;
            // 
            // iconButton1
            // 
            iconButton1.BackColor = Color.Transparent;
            iconButton1.FlatAppearance.BorderSize = 0;
            iconButton1.FlatStyle = FlatStyle.Flat;
            iconButton1.ForeColor = Color.Transparent;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Close;
            iconButton1.IconColor = SystemColors.ActiveCaptionText;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 20;
            iconButton1.Location = new Point(701, -1);
            iconButton1.Name = "iconButton1";
            iconButton1.Padding = new Padding(19, 22, 20, 20);
            iconButton1.Size = new Size(28, 28);
            iconButton1.TabIndex = 2;
            iconButton1.TabStop = false;
            iconButton1.UseVisualStyleBackColor = false;
            // 
            // cmbEstadoMantenimiento
            // 
            cmbEstadoMantenimiento.AllowDrop = true;
            cmbEstadoMantenimiento.BackColor = Color.Transparent;
            cmbEstadoMantenimiento.CustomizableEdges = customizableEdges7;
            cmbEstadoMantenimiento.DrawMode = DrawMode.OwnerDrawFixed;
            cmbEstadoMantenimiento.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstadoMantenimiento.FocusedColor = Color.FromArgb(94, 148, 255);
            cmbEstadoMantenimiento.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cmbEstadoMantenimiento.Font = new Font("Segoe UI", 10F);
            cmbEstadoMantenimiento.ForeColor = Color.FromArgb(68, 88, 112);
            cmbEstadoMantenimiento.ItemHeight = 30;
            cmbEstadoMantenimiento.Location = new Point(42, 117);
            cmbEstadoMantenimiento.Name = "cmbEstadoMantenimiento";
            cmbEstadoMantenimiento.ShadowDecoration.CustomizableEdges = customizableEdges8;
            cmbEstadoMantenimiento.Size = new Size(288, 36);
            cmbEstadoMantenimiento.TabIndex = 34;
            // 
            // txtMotivo
            // 
            txtMotivo.BorderRadius = 7;
            txtMotivo.CustomizableEdges = customizableEdges9;
            txtMotivo.DefaultText = "";
            txtMotivo.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtMotivo.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtMotivo.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtMotivo.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtMotivo.FocusedState.BorderColor = Color.DodgerBlue;
            txtMotivo.Font = new Font("Segoe UI", 9F);
            txtMotivo.ForeColor = Color.Black;
            txtMotivo.HoverState.BorderColor = Color.DodgerBlue;
            txtMotivo.Location = new Point(42, 186);
            txtMotivo.Multiline = true;
            txtMotivo.Name = "txtMotivo";
            txtMotivo.PlaceholderForeColor = Color.DarkGray;
            txtMotivo.PlaceholderText = "Ingresar motivo (opcional)";
            txtMotivo.SelectedText = "";
            txtMotivo.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtMotivo.Size = new Size(288, 165);
            txtMotivo.TabIndex = 37;
            // 
            // btnDeshabiliar
            // 
            btnDeshabiliar.BackColor = Color.Transparent;
            btnDeshabiliar.BorderColor = Color.Maroon;
            btnDeshabiliar.BorderRadius = 11;
            btnDeshabiliar.BorderThickness = 1;
            btnDeshabiliar.CustomizableEdges = customizableEdges11;
            btnDeshabiliar.DisabledState.BorderColor = Color.DarkGray;
            btnDeshabiliar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnDeshabiliar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnDeshabiliar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnDeshabiliar.FillColor = Color.Maroon;
            btnDeshabiliar.Font = new Font("Segoe UI", 9F);
            btnDeshabiliar.ForeColor = Color.White;
            btnDeshabiliar.HoverState.FillColor = Color.FromArgb(150, 30, 30, 30);
            btnDeshabiliar.Image = (Image)resources.GetObject("btnDeshabiliar.Image");
            btnDeshabiliar.ImageAlign = HorizontalAlignment.Left;
            btnDeshabiliar.Location = new Point(129, 372);
            btnDeshabiliar.Name = "btnDeshabiliar";
            btnDeshabiliar.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnDeshabiliar.Size = new Size(116, 41);
            btnDeshabiliar.TabIndex = 42;
            btnDeshabiliar.Text = "Deshabilitar";
            btnDeshabiliar.TextAlign = HorizontalAlignment.Left;
            btnDeshabiliar.Click += btnDeshabiliar_Click;
            // 
            // FormDeshabilitarCNE
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 425);
            Controls.Add(btnDeshabiliar);
            Controls.Add(txtMotivo);
            Controls.Add(cmbEstadoMantenimiento);
            Controls.Add(pnlBanner);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormDeshabilitarCNE";
            Text = "FormDeshabilitarCNE";
            Load += FormDeshabilitarCNE_Load;
            pnlBanner.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlBanner;
        private FontAwesome.Sharp.IconButton iconButton1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbEstadoMantenimiento;
        private Guna.UI2.WinForms.Guna2TextBox txtMotivo;
        private Guna.UI2.WinForms.Guna2Button btnDeshabiliar;
    }
}