using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.Interfaces;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class UsuarioGestionuc : UserControl
    {
        private readonly UsuariosCN usuarioCN;
        private readonly UsuariosBajasCN usuarioBajas;
        private int _idActual = 0;
        private readonly IRepoHistorialCambio repoHistorialCambio;

        #region Datos actuales del usuario
        private string? _usuario = "";
        private string? _Nombre = "";
        private string? _apellido = "";
        private string? _email = "";
        private bool? _habilitado = true;
        private string? _rutaFoto = "";
        private string? _contraseña = "";
        #endregion

        public UsuarioGestionuc(UsuariosCN usuariosCN, UsuariosBajasCN usuariosBajasCN, int usuarioSeleccionado, IRepoHistorialCambio repoHistorialCambio)
        {
            InitializeComponent();
            this.usuarioCN = usuariosCN;
            this.usuarioBajas = usuariosBajasCN;
            this._idActual = usuarioSeleccionado;
            this.repoHistorialCambio = repoHistorialCambio;
        }

        private void UsuarioGestion_Load(object sender, EventArgs e)
        {
            ActualizarUC(_idActual);
        }

        public void ActualizarUC(int idUsuario)
        {
            Usuarios? usuarios = usuarioCN.ObtenerID(idUsuario);

            _usuario = usuarios?.Usuario;
            _Nombre = usuarios?.Nombre;
            _apellido = usuarios?.Apellido;
            _email = usuarios?.Email;
            _habilitado = usuarios?.Habilitado;
            _rutaFoto = usuarios?.FotoPerfil;
            _contraseña = usuarios?.Password;

            txtUsuario.Text = _usuario;
            txtApellido.Text = _apellido;
            txtNombre.Text = _Nombre;
            txtEmail.Text = _email;
            txtContraseña.Text = _contraseña;

            RenovarCantidadAcciones();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Usuarios usuario = new Usuarios()
            {
                IdUsuario = _idActual,
                Usuario = txtUsuario.Text,
                Password = txtContraseña.Text,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                IdRol = Convert.ToInt32(lblRol.Tag),
                FotoPerfil = _rutaFoto,
            };

            usuarioCN.ActualizarUsuario(usuario);
            HabilitarBotones(false, false);
        }

        private void btnRestablecerCambios_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = _usuario;
            txtNombre.Text = _Nombre;
            txtApellido.Text = _apellido;
            txtEmail.Text = _email;

            HabilitarBotones(false, false);
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_habilitado == false)
                {
                    usuarioCN.DeshabilitarUsuario(_idActual);
                    MessageBox.Show("Usuario deshabilitado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    usuarioBajas.HabilitarUsuario(_idActual);
                    MessageBox.Show("Usuario habilitado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VerificarCambios(object sender, EventArgs e)
        {
            if (txtUsuario.Text != _usuario || txtNombre.Text != _Nombre || txtApellido.Text != _apellido || txtEmail.Text != _email)
            {
                HabilitarBotones(true, true);
            }
            else
            {
                HabilitarBotones(false, false);
            }
        }

        private void HabilitarBotones(bool actu, bool rest)
        {
            btnActualizar.Enabled = actu;
            btnRestablecerCambios.Enabled = rest;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            var form = (FormPrincipal)this.FindForm();
            form.CerrarGestionUsuario();
        }

        private void RenovarCantidadAcciones()
        {
            lblCantImplemento.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 1).ToString();
            lblCantModifico.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 2).ToString();
            lblCantInhabilito.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 3).ToString();
            lblCantRehabilito.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 4).ToString();
            lblCantPrestamos.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 5).ToString();
            lblCantDevoluciones.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 6).ToString();
        }
    }
}
