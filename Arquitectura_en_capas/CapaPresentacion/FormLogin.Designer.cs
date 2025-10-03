namespace CapaPresentacion
{
    partial class LoginState
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginState));
            MoveLogin = new Guna.UI2.WinForms.Guna2DragControl(components);
            BarraTopLogin1 = new Panel();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            BtnCerrar1 = new FontAwesome.Sharp.IconButton();
            PanelWelcome = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            BarraTopLogin2 = new Panel();
            TxtWelcome = new Label();
            PictED = new PictureBox();
            ChckPass = new Guna.UI2.WinForms.Guna2CheckBox();
            BtnLogin = new Guna.UI2.WinForms.Guna2Button();
            Entrada = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            PictET12 = new PictureBox();
            MoveLogin2 = new Guna.UI2.WinForms.Guna2DragControl(components);
            TxtUser = new Guna.UI2.WinForms.Guna2TextBox();
            TxtPass = new Guna.UI2.WinForms.Guna2TextBox();
            TxtError = new Label();
            BarraTopLogin1.SuspendLayout();
            PanelWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictED).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PictET12).BeginInit();
            SuspendLayout();
            // 
            // MoveLogin
            // 
            MoveLogin.DockIndicatorTransparencyValue = 0.6D;
            MoveLogin.TargetControl = BarraTopLogin1;
            MoveLogin.UseTransparentDrag = true;
            // 
            // BarraTopLogin1
            // 
            BarraTopLogin1.BackColor = Color.Transparent;
            BarraTopLogin1.Controls.Add(iconButton1);
            BarraTopLogin1.Controls.Add(BtnCerrar1);
            BarraTopLogin1.Dock = DockStyle.Top;
            BarraTopLogin1.Location = new Point(339, 0);
            BarraTopLogin1.Name = "BarraTopLogin1";
            BarraTopLogin1.Size = new Size(341, 29);
            BarraTopLogin1.TabIndex = 14;
            // 
            // iconButton1
            // 
            iconButton1.BackColor = Color.Transparent;
            iconButton1.FlatStyle = FlatStyle.Flat;
            iconButton1.ForeColor = Color.Transparent;
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Close;
            iconButton1.IconColor = SystemColors.ActiveCaptionText;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 20;
            iconButton1.Location = new Point(1055, 0);
            iconButton1.Name = "iconButton1";
            iconButton1.Padding = new Padding(19, 22, 20, 20);
            iconButton1.Size = new Size(29, 29);
            iconButton1.TabIndex = 2;
            iconButton1.TabStop = false;
            iconButton1.UseVisualStyleBackColor = false;
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
            BtnCerrar1.Location = new Point(312, 0);
            BtnCerrar1.Name = "BtnCerrar1";
            BtnCerrar1.Padding = new Padding(19, 22, 20, 20);
            BtnCerrar1.Size = new Size(29, 29);
            BtnCerrar1.TabIndex = 1;
            BtnCerrar1.TabStop = false;
            BtnCerrar1.UseVisualStyleBackColor = false;
            BtnCerrar1.Click += iconButton1_Click;
            // 
            // PanelWelcome
            // 
            PanelWelcome.Controls.Add(BarraTopLogin2);
            PanelWelcome.Controls.Add(TxtWelcome);
            PanelWelcome.Controls.Add(PictED);
            PanelWelcome.CustomizableEdges = customizableEdges1;
            PanelWelcome.Dock = DockStyle.Left;
            PanelWelcome.FillColor = Color.Indigo;
            PanelWelcome.FillColor2 = Color.MediumPurple;
            PanelWelcome.FillColor3 = Color.Navy;
            PanelWelcome.FillColor4 = Color.DarkMagenta;
            PanelWelcome.Location = new Point(0, 0);
            PanelWelcome.Name = "PanelWelcome";
            PanelWelcome.ShadowDecoration.CustomizableEdges = customizableEdges2;
            PanelWelcome.Size = new Size(339, 466);
            PanelWelcome.TabIndex = 3;
            // 
            // BarraTopLogin2
            // 
            BarraTopLogin2.BackColor = Color.Transparent;
            BarraTopLogin2.Location = new Point(0, 0);
            BarraTopLogin2.Name = "BarraTopLogin2";
            BarraTopLogin2.Size = new Size(339, 29);
            BarraTopLogin2.TabIndex = 2;
            // 
            // TxtWelcome
            // 
            TxtWelcome.AutoSize = true;
            TxtWelcome.BackColor = Color.Transparent;
            TxtWelcome.Font = new Font("Segoe UI Emoji", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtWelcome.ForeColor = SystemColors.ButtonHighlight;
            TxtWelcome.Location = new Point(62, 84);
            TxtWelcome.Name = "TxtWelcome";
            TxtWelcome.Size = new Size(211, 38);
            TxtWelcome.TabIndex = 1;
            TxtWelcome.Text = "BIENVENIDO A";
            // 
            // PictED
            // 
            PictED.BackColor = Color.Transparent;
            PictED.Image = Properties.Resources.Logo_ED;
            PictED.Location = new Point(24, 84);
            PictED.Name = "PictED";
            PictED.Size = new Size(289, 181);
            PictED.SizeMode = PictureBoxSizeMode.Zoom;
            PictED.TabIndex = 0;
            PictED.TabStop = false;
            // 
            // ChckPass
            // 
            ChckPass.AutoSize = true;
            ChckPass.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            ChckPass.CheckedState.BorderRadius = 0;
            ChckPass.CheckedState.BorderThickness = 0;
            ChckPass.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            ChckPass.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ChckPass.Location = new Point(515, 342);
            ChckPass.Name = "ChckPass";
            ChckPass.Size = new Size(128, 19);
            ChckPass.TabIndex = 8;
            ChckPass.Text = "Mostrar contraseña";
            ChckPass.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            ChckPass.UncheckedState.BorderRadius = 0;
            ChckPass.UncheckedState.BorderThickness = 0;
            ChckPass.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            ChckPass.CheckedChanged += guna2CheckBox1_CheckedChanged;
            // 
            // BtnLogin
            // 
            BtnLogin.BorderRadius = 20;
            BtnLogin.CustomizableEdges = customizableEdges3;
            BtnLogin.DisabledState.BorderColor = Color.DarkGray;
            BtnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            BtnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            BtnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            BtnLogin.FillColor = Color.FromArgb(255, 204, 51);
            BtnLogin.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BtnLogin.ForeColor = Color.FromArgb(216, 27, 96);
            BtnLogin.Location = new Point(417, 409);
            BtnLogin.Name = "BtnLogin";
            BtnLogin.ShadowDecoration.CustomizableEdges = customizableEdges4;
            BtnLogin.Size = new Size(180, 45);
            BtnLogin.TabIndex = 0;
            BtnLogin.Text = "Iniciar sesion";
            BtnLogin.Click += BtnLogin_Click;
            // 
            // Entrada
            // 
            Entrada.AnimationType = Guna.UI2.WinForms.Guna2AnimateWindow.AnimateWindowType.AW_BLEND;
            Entrada.Interval = 700;
            Entrada.TargetForm = this;
            // 
            // PictET12
            // 
            PictET12.Image = Properties.Resources.et12;
            PictET12.Location = new Point(436, 35);
            PictET12.Name = "PictET12";
            PictET12.Size = new Size(148, 139);
            PictET12.SizeMode = PictureBoxSizeMode.Zoom;
            PictET12.TabIndex = 11;
            PictET12.TabStop = false;
            // 
            // MoveLogin2
            // 
            MoveLogin2.DockIndicatorTransparencyValue = 0.6D;
            MoveLogin2.TargetControl = BarraTopLogin2;
            MoveLogin2.UseTransparentDrag = true;
            // 
            // TxtUser
            // 
            TxtUser.BorderRadius = 7;
            TxtUser.CustomizableEdges = customizableEdges7;
            TxtUser.DefaultText = "";
            TxtUser.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TxtUser.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TxtUser.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TxtUser.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TxtUser.FocusedState.BorderColor = Color.DodgerBlue;
            TxtUser.Font = new Font("Segoe UI", 9F);
            TxtUser.HoverState.BorderColor = Color.DodgerBlue;
            TxtUser.IconLeft = Properties.Resources.UserIcon;
            TxtUser.IconLeftSize = new Size(25, 25);
            TxtUser.Location = new Point(374, 219);
            TxtUser.Name = "TxtUser";
            TxtUser.PlaceholderForeColor = Color.DarkGray;
            TxtUser.PlaceholderText = "Ingresar usuario";
            TxtUser.SelectedText = "";
            TxtUser.ShadowDecoration.CustomizableEdges = customizableEdges8;
            TxtUser.Size = new Size(269, 36);
            TxtUser.TabIndex = 15;
            TxtUser.KeyDown += TxtUser_KeyDown;
            // 
            // TxtPass
            // 
            TxtPass.BorderRadius = 7;
            TxtPass.CustomizableEdges = customizableEdges5;
            TxtPass.DefaultText = "";
            TxtPass.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            TxtPass.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            TxtPass.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            TxtPass.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            TxtPass.FocusedState.BorderColor = Color.DodgerBlue;
            TxtPass.Font = new Font("Segoe UI", 9F);
            TxtPass.HoverState.BorderColor = Color.DodgerBlue;
            TxtPass.IconLeft = Properties.Resources.PasswordIcon;
            TxtPass.IconLeftSize = new Size(25, 25);
            TxtPass.Location = new Point(374, 289);
            TxtPass.Name = "TxtPass";
            TxtPass.PlaceholderForeColor = Color.DarkGray;
            TxtPass.PlaceholderText = "Ingresar contraseña";
            TxtPass.SelectedText = "";
            TxtPass.ShadowDecoration.CustomizableEdges = customizableEdges6;
            TxtPass.Size = new Size(269, 36);
            TxtPass.TabIndex = 16;
            TxtPass.UseSystemPasswordChar = true;
            TxtPass.KeyDown += TxtPass_KeyDown;
            // 
            // TxtError
            // 
            TxtError.AutoSize = true;
            TxtError.BackColor = Color.Transparent;
            TxtError.Font = new Font("Segoe UI Semilight", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtError.ForeColor = Color.Red;
            TxtError.Location = new Point(374, 197);
            TxtError.Name = "TxtError";
            TxtError.Size = new Size(240, 13);
            TxtError.TabIndex = 17;
            TxtError.Text = "Usuario o contraseña incorrecta, intente de nuevo";
            TxtError.Visible = false;
            // 
            // LoginState
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 466);
            Controls.Add(TxtError);
            Controls.Add(TxtPass);
            Controls.Add(TxtUser);
            Controls.Add(BarraTopLogin1);
            Controls.Add(PictET12);
            Controls.Add(BtnLogin);
            Controls.Add(ChckPass);
            Controls.Add(PanelWelcome);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginState";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLogin";
            Load += LoginState_Load;
            BarraTopLogin1.ResumeLayout(false);
            PanelWelcome.ResumeLayout(false);
            PanelWelcome.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictED).EndInit();
            ((System.ComponentModel.ISupportInitialize)PictET12).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2DragControl MoveLogin;
        private FontAwesome.Sharp.IconButton BtnCerrar1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel PanelWelcome;
        private Label TxtWelcome;
        private PictureBox PictED;
        private Guna.UI2.WinForms.Guna2CheckBox ChckPass;
        private Guna.UI2.WinForms.Guna2Button BtnLogin;
        private Guna.UI2.WinForms.Guna2AnimateWindow Entrada;
        private PictureBox PictET12;
        private Panel BarraTopLogin1;
        private FontAwesome.Sharp.IconButton iconButton1;
        private Panel BarraTopLogin2;
        private Guna.UI2.WinForms.Guna2DragControl MoveLogin2;
        private Guna.UI2.WinForms.Guna2TextBox TxtUser;
        private Guna.UI2.WinForms.Guna2TextBox TxtPass;
        private Label TxtError;
    }
}