using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaNegocio;
using CapaDatos.MappersDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CapaDatos.InterfacesDTO;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class LoginState : Form
    {
        private IDbConnection conexion;
        private readonly RepoRoles repoRoles;
        private readonly RepoUsuarios repoUsuarios;
        private readonly RepoHistorialCambio repoHistorialCambio;
        private readonly UsuariosCN usuariosCN;
        private readonly IMapperUsuarios mapperUsuarios;
        public LoginState(IDbConnection conexion)
        {
            InitializeComponent();
            this.conexion = conexion;

            repoUsuarios = new RepoUsuarios(conexion);
            repoRoles = new RepoRoles(conexion);
            mapperUsuarios = new MapperUsuarios(conexion);
            usuariosCN = new UsuariosCN(repoUsuarios, repoRoles, mapperUsuarios, repoHistorialCambio);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ChckPass.Checked)
            {
                TxtPass.UseSystemPasswordChar = false;
            }
            else
            {
                TxtPass.UseSystemPasswordChar = true;
            }
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string usuario = TxtUser.Text.Trim();
            string password = TxtPass.Text.Trim();

            try
            {
                Usuarios? userVerificado = usuariosCN.Login(usuario, password);

            Roles? rolUserVerificado = repoRoles.GetById(userVerificado.IdRol);

            FormPrincipal principal = new FormPrincipal(conexion, userVerificado, rolUserVerificado);
            principal.Show();
            this.Hide();
        }
            catch
            {
                TxtError.Visible = true;
            }


    //if (userVerificado != null)
    //{
    //    Roles? rolUserVerificado = repoRoles.GetById(userVerificado.IdRol);

    //    FormPrincipal principal = new FormPrincipal(conexion, userVerificado, rolUserVerificado);
    //    principal.Show();
    //    this.Hide();
    //}
    //else
    //{
    //    TxtError.Visible = true;
    //}
}

        private void TxtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TxtPass.Focus();
            }

        }

        private void TxtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                BtnLogin.PerformClick();
            }
        }

        private void LoginState_Load(object sender, EventArgs e)
        {
            TxtUser.Focus();
            TxtPass.PlaceholderForeColor = Color.DarkGray;
            TxtPass.PlaceholderText = "Ingresar contraseña";
            TxtUser.PlaceholderForeColor = Color.DarkGray;
            TxtUser.PlaceholderText = "Ingresar usuario";
        }
    }
}
