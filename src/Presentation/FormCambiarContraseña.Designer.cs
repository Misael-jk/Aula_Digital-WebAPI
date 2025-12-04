namespace CapaPresentacion
{
    partial class FormCambiarContraseña
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            BtnCerrar1 = new FontAwesome.Sharp.IconButton();
            btnActualizar = new Guna.UI2.WinForms.Guna2Button();
            lblNoPuedeActualizar = new Guna.UI2.WinForms.Guna2HtmlLabel();
            label1 = new Label();
            lblEquipo = new Label();
            txtContraseñaNueva = new Guna.UI2.WinForms.Guna2TextBox();
            txtContraseña = new Guna.UI2.WinForms.Guna2TextBox();
            lblRol = new Guna.UI2.WinForms.Guna2HtmlLabel();
            panel2 = new Panel();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = SystemColors.HighlightText;
            guna2Panel1.BorderColor = Color.Silver;
            guna2Panel1.BorderThickness = 2;
            guna2Panel1.Controls.Add(BtnCerrar1);
            guna2Panel1.Controls.Add(btnActualizar);
            guna2Panel1.Controls.Add(lblNoPuedeActualizar);
            guna2Panel1.Controls.Add(label1);
            guna2Panel1.Controls.Add(lblEquipo);
            guna2Panel1.Controls.Add(txtContraseñaNueva);
            guna2Panel1.Controls.Add(txtContraseña);
            guna2Panel1.Controls.Add(lblRol);
            guna2Panel1.Controls.Add(panel2);
            guna2Panel1.CustomizableEdges = customizableEdges7;
            guna2Panel1.Dock = DockStyle.Fill;
            guna2Panel1.Location = new Point(0, 0);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2Panel1.Size = new Size(356, 269);
            guna2Panel1.TabIndex = 0;
            // 
            // BtnCerrar1
            // 
            BtnCerrar1.BackColor = Color.Transparent;
            BtnCerrar1.FlatAppearance.BorderSize = 0;
            BtnCerrar1.FlatStyle = FlatStyle.Flat;
            BtnCerrar1.ForeColor = Color.Transparent;
            BtnCerrar1.IconChar = FontAwesome.Sharp.IconChar.Close;
            BtnCerrar1.IconColor = SystemColors.ActiveCaptionText;
            BtnCerrar1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            BtnCerrar1.IconSize = 20;
            BtnCerrar1.Location = new Point(324, 3);
            BtnCerrar1.Name = "BtnCerrar1";
            BtnCerrar1.Padding = new Padding(19, 22, 20, 20);
            BtnCerrar1.Size = new Size(29, 29);
            BtnCerrar1.TabIndex = 110;
            BtnCerrar1.TabStop = false;
            BtnCerrar1.UseVisualStyleBackColor = false;
            BtnCerrar1.Click += BtnCerrar1_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.Transparent;
            btnActualizar.BorderRadius = 8;
            btnActualizar.Cursor = Cursors.Hand;
            btnActualizar.CustomizableEdges = customizableEdges1;
            btnActualizar.DisabledState.BorderColor = Color.DarkGray;
            btnActualizar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnActualizar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnActualizar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnActualizar.FillColor = Color.FromArgb(252, 201, 52);
            btnActualizar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnActualizar.ForeColor = Color.Firebrick;
            btnActualizar.HoverState.FillColor = Color.FromArgb(235, 115, 125);
            btnActualizar.HoverState.ForeColor = Color.White;
            btnActualizar.Location = new Point(90, 228);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.PressedColor = Color.FromArgb(255, 170, 20);
            btnActualizar.ShadowDecoration.Color = Color.FromArgb(255, 200, 200);
            btnActualizar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnActualizar.ShadowDecoration.Enabled = true;
            btnActualizar.ShadowDecoration.Shadow = new Padding(2, 2, 4, 4);
            btnActualizar.Size = new Size(164, 26);
            btnActualizar.TabIndex = 109;
            btnActualizar.Text = "Confirmar";
            btnActualizar.Click += btnActualizar_Click;
            // 
            // lblNoPuedeActualizar
            // 
            lblNoPuedeActualizar.BackColor = Color.Transparent;
            lblNoPuedeActualizar.Font = new Font("Segoe UI Variable Small", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoPuedeActualizar.ForeColor = Color.Red;
            lblNoPuedeActualizar.Location = new Point(98, 206);
            lblNoPuedeActualizar.Name = "lblNoPuedeActualizar";
            lblNoPuedeActualizar.Size = new Size(146, 17);
            lblNoPuedeActualizar.TabIndex = 108;
            lblNoPuedeActualizar.Text = "Contraseña actual incorrecta";
            lblNoPuedeActualizar.TextAlignment = ContentAlignment.MiddleLeft;
            lblNoPuedeActualizar.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(35, 126);
            label1.Name = "label1";
            label1.Size = new Size(144, 15);
            label1.TabIndex = 107;
            label1.Text = "Ingrese nueva contraseña:";
            // 
            // lblEquipo
            // 
            lblEquipo.AutoSize = true;
            lblEquipo.BackColor = Color.Transparent;
            lblEquipo.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblEquipo.ForeColor = Color.Black;
            lblEquipo.Location = new Point(35, 53);
            lblEquipo.Name = "lblEquipo";
            lblEquipo.Size = new Size(148, 15);
            lblEquipo.TabIndex = 106;
            lblEquipo.Text = "Ingresar contraseña actual:";
            // 
            // txtContraseñaNueva
            // 
            txtContraseñaNueva.Animated = true;
            txtContraseñaNueva.BorderRadius = 10;
            txtContraseñaNueva.Cursor = Cursors.IBeam;
            txtContraseñaNueva.CustomizableEdges = customizableEdges3;
            txtContraseñaNueva.DefaultText = "";
            txtContraseñaNueva.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtContraseñaNueva.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtContraseñaNueva.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtContraseñaNueva.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtContraseñaNueva.FocusedState.BorderColor = Color.FromArgb(105, 80, 165);
            txtContraseñaNueva.FocusedState.FillColor = Color.White;
            txtContraseñaNueva.FocusedState.ForeColor = Color.Black;
            txtContraseñaNueva.Font = new Font("Segoe UI", 10F);
            txtContraseñaNueva.ForeColor = Color.Black;
            txtContraseñaNueva.HoverState.BorderColor = Color.FromArgb(125, 100, 180);
            txtContraseñaNueva.HoverState.FillColor = Color.FromArgb(248, 246, 252);
            txtContraseñaNueva.Location = new Point(35, 144);
            txtContraseñaNueva.Margin = new Padding(4, 5, 4, 5);
            txtContraseñaNueva.Name = "txtContraseñaNueva";
            txtContraseñaNueva.PlaceholderForeColor = Color.Gray;
            txtContraseñaNueva.PlaceholderText = "Nueva contraseña";
            txtContraseñaNueva.SelectedText = "";
            txtContraseñaNueva.ShadowDecoration.BorderRadius = 10;
            txtContraseñaNueva.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtContraseñaNueva.ShadowDecoration.Depth = 5;
            txtContraseñaNueva.Size = new Size(285, 43);
            txtContraseñaNueva.TabIndex = 105;
            txtContraseñaNueva.TextOffset = new Point(5, 0);
            // 
            // txtContraseña
            // 
            txtContraseña.Animated = true;
            txtContraseña.BorderRadius = 10;
            txtContraseña.Cursor = Cursors.IBeam;
            txtContraseña.CustomizableEdges = customizableEdges5;
            txtContraseña.DefaultText = "";
            txtContraseña.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtContraseña.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtContraseña.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtContraseña.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtContraseña.FocusedState.BorderColor = Color.FromArgb(105, 80, 165);
            txtContraseña.FocusedState.FillColor = Color.White;
            txtContraseña.FocusedState.ForeColor = Color.Black;
            txtContraseña.Font = new Font("Segoe UI", 10F);
            txtContraseña.ForeColor = Color.Black;
            txtContraseña.HoverState.BorderColor = Color.FromArgb(125, 100, 180);
            txtContraseña.HoverState.FillColor = Color.FromArgb(248, 246, 252);
            txtContraseña.Location = new Point(35, 71);
            txtContraseña.Margin = new Padding(4, 5, 4, 5);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.PlaceholderForeColor = Color.Gray;
            txtContraseña.PlaceholderText = "Contraseña actual";
            txtContraseña.SelectedText = "";
            txtContraseña.ShadowDecoration.BorderRadius = 10;
            txtContraseña.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtContraseña.ShadowDecoration.Depth = 5;
            txtContraseña.Size = new Size(285, 43);
            txtContraseña.TabIndex = 104;
            txtContraseña.TextOffset = new Point(5, 0);
            txtContraseña.UseSystemPasswordChar = true;
            // 
            // lblRol
            // 
            lblRol.BackColor = Color.Transparent;
            lblRol.Font = new Font("Segoe UI Semibold", 14F);
            lblRol.ForeColor = Color.Indigo;
            lblRol.Location = new Point(92, 7);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(174, 27);
            lblRol.TabIndex = 103;
            lblRol.Text = "<b>Cambiar contraseña</b>";
            lblRol.TextAlignment = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.BackColor = Color.LightGray;
            panel2.Location = new Point(19, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(318, 1);
            panel2.TabIndex = 102;
            // 
            // FormCambiarContraseña
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 269);
            Controls.Add(guna2Panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FormCambiarContraseña";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormCambiarContraseña";
            TopMost = true;
            Load += FormCambiarContraseña_Load;
            guna2Panel1.ResumeLayout(false);
            guna2Panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblRol;
        private Panel panel2;
        private Guna.UI2.WinForms.Guna2TextBox txtContraseñaNueva;
        private Guna.UI2.WinForms.Guna2TextBox txtContraseña;
        private Label label1;
        private Label lblEquipo;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNoPuedeActualizar;
        private Guna.UI2.WinForms.Guna2Button btnActualizar;
        private FontAwesome.Sharp.IconButton BtnCerrar1;
    }
}