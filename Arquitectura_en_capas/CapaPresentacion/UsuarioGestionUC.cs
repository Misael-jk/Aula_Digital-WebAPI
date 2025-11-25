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
        private Usuarios UsuarioActual;

        #region Datos actuales del usuario
        private string? _usuario = "";
        private string? _Nombre = "";
        private string? _apellido = "";
        private string? _email = "";
        private bool? _habilitado = true;
        private string? _rutaFoto = "";
        private string? _contraseña = "";
        private int? _idRol;
        #endregion

        private string? PerfilCambio = "";
        private int? numeroFoto = 0;

        public UsuarioGestionuc(UsuariosCN usuariosCN, UsuariosBajasCN usuariosBajasCN, int usuarioSeleccionado, IRepoHistorialCambio repoHistorialCambio, Usuarios userActual)
        {
            InitializeComponent();
            this.usuarioCN = usuariosCN;
            this.usuarioBajas = usuariosBajasCN;
            this._idActual = usuarioSeleccionado;
            this.repoHistorialCambio = repoHistorialCambio;
            this.UsuarioActual = userActual;
        }

        private void UsuarioGestion_Load(object sender, EventArgs e)
        {
            ActualizarUC(_idActual);
        }

        public void ActualizarUC(int idUsuario)
        {
            _idActual = idUsuario;

            Usuarios? usuarios = usuarioCN.ObtenerID(idUsuario);

            _usuario = usuarios?.Usuario;
            _Nombre = usuarios?.Nombre;
            _apellido = usuarios?.Apellido;
            _email = usuarios?.Email;
            _habilitado = usuarios?.Habilitado;
            _rutaFoto = usuarios?.FotoPerfil;
            _contraseña = usuarios?.Password;
            _idRol = usuarios?.IdRol;

            txtUsuario.Text = _usuario;
            txtApellido.Text = _apellido;
            txtNombre.Text = _Nombre;
            txtEmail.Text = _email;
            txtContraseña.Text = _contraseña;
            lblRol.Tag = _idRol;

            VerificarHabilitado();

            if (_rutaFoto == null)
            {
                PerfilCambio = null;
                numeroFoto = 0;
                ptbPerfil.Image = Properties.Resources.Perfil_default;
            }

            if (_rutaFoto == "fotoperfil1")
            {
                PerfilCambio = "fotoperfil1";
                numeroFoto = 1;
                ptbPerfil.Image = Properties.Resources.fotoperfil1;
            }

            if (_rutaFoto == "fotoperfil2")
            {
                PerfilCambio = "fotoperfil2";
                numeroFoto = 2;
                ptbPerfil.Image = Properties.Resources.fotoperfil2;
            }

            if (_rutaFoto == "fotoperfil3")
            {
                PerfilCambio = "fotoperfil3";
                numeroFoto = 3;
                ptbPerfil.Image = Properties.Resources.fotoperfil3;
            }

            if (_rutaFoto == "fotoperfil4")
            {
                PerfilCambio = "fotoperfil4";
                numeroFoto = 4;
                ptbPerfil.Image = Properties.Resources.fotoperfil4;
            }

            RenovarCantidadAcciones();
        }

        private void VerificarHabilitado()
        {
            if (UsuarioActual.IdUsuario != _idActual)
            {
                if (_habilitado == true)
                {
                    btnDeshabilitar2.Visible = true;
                    btnActualizar.Visible = true;
                    btnRestablecerCambios.Visible = true;
                    btnHabilitar.Visible = false;
                    btnDeshabilitar.Visible = false;
                    btnCancelar.Visible = false;
                }
                else
                {
                    btnDeshabilitar2.Visible = false;
                    btnActualizar.Visible = false;
                    btnRestablecerCambios.Visible = false;
                    btnHabilitar.Visible = true;
                    btnDeshabilitar.Visible = true;
                    btnCancelar.Visible = true;

                    txtUsuario.Enabled = false;
                    txtNombre.Enabled = false;
                    txtApellido.Enabled = false;
                    txtEmail.Enabled = false;
                    txtContraseña.Enabled = false;
                }
            }
            else
            {
                btnDeshabilitar2.Visible = false;
                btnActualizar.Visible = true;
                btnRestablecerCambios.Visible = true;
                btnHabilitar.Visible = false;
                btnDeshabilitar.Visible = false;
                btnCancelar.Visible = false;
            }
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
                Habilitado = true,
                FotoPerfil = PerfilCambio,
            };

            usuarioCN.ActualizarUsuario(usuario);
            HabilitarBotones(false, false);
            var form = (FormPrincipal)this.FindForm();
            form.CargarDatosDelUsuarioActual();

            ActualizarUC(_idActual);
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
            if (txtUsuario.Text != (_usuario ?? "") || txtNombre.Text != (_Nombre ?? "") || txtApellido.Text != (_apellido ?? "") || txtEmail.Text != (_email ?? "") || PerfilCambio != (_rutaFoto ?? ""))
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

        private void btnCambiarPerfil_Click(object sender, EventArgs e)
        {
            numeroFoto++;

            if (numeroFoto > 4)
            {
                numeroFoto = 1;
            }

            if (numeroFoto == 1)
            {
                PerfilCambio = "fotoperfil1";
                ptbPerfil.Image = Properties.Resources.fotoperfil1;
            }
            else if (numeroFoto == 2)
            {
                PerfilCambio = "fotoperfil2";
                ptbPerfil.Image = Properties.Resources.fotoperfil2;
            }
            else if (numeroFoto == 3)
            {
                PerfilCambio = "fotoperfil3";
                ptbPerfil.Image = Properties.Resources.fotoperfil3;
            }
            else if (numeroFoto == 4)
            {
                PerfilCambio = "fotoperfil4";
                ptbPerfil.Image = Properties.Resources.fotoperfil4;
            }

            VerificarCambios(sender, e);
        }

        private void btnDeshabilitar2_Click(object sender, EventArgs e)
        {
            btnActualizar.Visible = false;
            btnRestablecerCambios.Visible = false;
            btnDeshabilitar.Visible = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ActualizarUC(_idActual);
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            btnCancelar.Enabled = true;
            btnDeshabilitar.Enabled = true;
        }
    }
}
