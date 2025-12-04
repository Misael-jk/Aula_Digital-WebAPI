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
    public partial class FormCambiarContraseña : Form
    {
        private readonly UsuariosCN usuariosCN;
        private int _idUsuario;
        private UsuarioGestionuc usuarioGestionuc;

        public FormCambiarContraseña(UsuariosCN usuariosCN, int idUsuarioSeleccionado, UsuarioGestionuc userControl)
        {
            InitializeComponent();

            this.usuariosCN = usuariosCN;
            this._idUsuario = idUsuarioSeleccionado;
            this.usuarioGestionuc = userControl;
        }

        private void FormCambiarContraseña_Load(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string? password;

            Usuarios? usuarios = usuariosCN.ObtenerID(_idUsuario);

            password = usuarios?.Password;

            if (txtContraseña.Text != password)
            {
                lblNoPuedeActualizar.Visible = true;
                lblNoPuedeActualizar.Text = "Contraseña actual incorrecta";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtContraseñaNueva.Text))
            {
                lblNoPuedeActualizar.Visible = true;
                lblNoPuedeActualizar.Text = "La nueva contraseña esta vacia";
                return;
            }

            usuariosCN.CambiarContraseña(_idUsuario, txtContraseñaNueva.Text);

            this.Close();
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            usuarioGestionuc.ActualizarUC(_idUsuario);
            this.Close();
        }
    }
}
