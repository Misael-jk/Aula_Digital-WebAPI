using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class CrearUsuarioUC : UserControl
    {
        private readonly UsuariosCN usuariosCN;
        private readonly FormPrincipal formPrincipal;
        private readonly UsuariosUC usuariosUC;
        private Action MostrarUsuarios;
        public CrearUsuarioUC(UsuariosCN usuariosCN, Action mostrarUsuarios, FormPrincipal formPrincipal, UsuariosUC usuariosUC)
        {
            InitializeComponent();
            this.usuariosCN = usuariosCN;
            MostrarUsuarios = mostrarUsuarios;
            this.formPrincipal = formPrincipal;
            this.usuariosUC = usuariosUC;
        }

        private void CrearUsuarioUC_Load(object sender, EventArgs e)
        {
            var roles = usuariosCN.ObtenerRoles();
            cmbRol.DataSource = roles.ToList();
            cmbRol.DisplayMember = "Rol";
            cmbRol.ValueMember = "IdRol";

            lblNoPuedeActualizar.Visible = false;
            ValidarPassword();
        }

        private void ValidarPassword()
        {
            if (string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                lblNoPuedeActualizar.Text = "Debes poner tu contraseña";
                lblNoPuedeActualizar.Visible = true;
                return;
            }

            if(txtContraseña.Text != txtRepetirContraseña.Text)
            {
                lblNoPuedeActualizar.Text = "Las contraseñas no coinciden";
                lblNoPuedeActualizar.Visible = true;
                return;
            }
        }
        private void cmbRol_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnCrear_Click_1(object sender, EventArgs e)
        {
            string nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtNombre.Text.Trim().ToLower());
            string apellido = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtApellido.Text.Trim().ToLower());

            var usuario = new Usuarios
            {
                Usuario = txtUsuario.Text,
                Nombre = nombre,
                Apellido = apellido,
                Password = txtContraseña.Text,
                Email = txtEmail.Text,
                Habilitado = true,
                FechaBaja = null,
                FotoPerfil = null,
                IdRol = Convert.ToInt32(cmbRol.SelectedValue)
            };

            usuariosCN.CrearUsuario(usuario);
            MostrarUsuarios.Invoke();
            formPrincipal.MostrarUserControl(usuariosUC);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            formPrincipal.MostrarUserControl(usuariosUC);
        }

        private void chcPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chcPassword.Checked)
            {
                txtContraseña.UseSystemPasswordChar = false;
                txtRepetirContraseña.UseSystemPasswordChar = false;
            }
            else
            {
                txtContraseña.UseSystemPasswordChar = true;
                txtRepetirContraseña.UseSystemPasswordChar = true;
            }
        }
    }
}
