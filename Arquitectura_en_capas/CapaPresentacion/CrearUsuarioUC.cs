using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        private void btnCrear_Click(object sender, EventArgs e)
        {
            var usuario = new Usuarios
            {
                Usuario = txtUsuario.Text,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
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

        private void CrearUsuarioUC_Load(object sender, EventArgs e)
        {
            var roles = usuariosCN.ObtenerRoles();
            cmbRol.DataSource = roles.ToList();
            cmbRol.DisplayMember = "Rol";
            cmbRol.ValueMember = "IdRol";
        }

        private void cmbRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
