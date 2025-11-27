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
        private readonly IRepoHistorialCambio repoHistorialCambio;

        private Usuarios? UsuarioActual;
        private Usuarios? UsuarioCopia;

        private int _idActual;
        private string? PerfilCambio;
        private int numeroFoto;

        private readonly List<(string Nombre, Image Imagen)> _fotosPerfil = new()
        {
            ("fotoperfil1", Properties.Resources.fotoperfil1),
            ("fotoperfil2", Properties.Resources.fotoperfil2),
            ("fotoperfil3", Properties.Resources.fotoperfil3),
            ("fotoperfil4", Properties.Resources.fotoperfil4),
            ("fotoperfil5", Properties.Resources.fotoperfil5),
            ("fotoperfil6", Properties.Resources.fotoperfil6)
        };


        public UsuarioGestionuc(UsuariosCN usuariosCN, UsuariosBajasCN usuariosBajasCN, int usuarioSeleccionado, IRepoHistorialCambio repoHistorialCambio, Usuarios userActual)
        {
            InitializeComponent();

            usuarioCN = usuariosCN;
            usuarioBajas = usuariosBajasCN;
            _idActual = usuarioSeleccionado;
            this.repoHistorialCambio = repoHistorialCambio;
            UsuarioActual = userActual;
        }

        private void UsuarioGestion_Load(object sender, EventArgs e)
        {
            ActualizarUC(_idActual);
        }

        public void ActualizarUC(int idUsuario)
        {
            _idActual = idUsuario;

            Usuarios? datos = usuarioCN.ObtenerID(idUsuario);
            if (datos == null) return;

            UsuarioCopia = datos;

            txtUsuario.Text = datos.Usuario;
            txtNombre.Text = datos.Nombre;
            txtApellido.Text = datos.Apellido;
            txtEmail.Text = datos.Email;
            txtContraseña.Text = datos.Password;
            lblRol.Tag = datos.IdRol;

            CargarFotoPerfil(datos.FotoPerfil);
            VerificarHabilitado();
            RenovarCantidadAcciones();
        }

        private void CargarFotoPerfil(string? ruta)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                PerfilCambio = null;
                numeroFoto = 0;
                ptbPerfil.Image = Properties.Resources.Perfil_default;
                return;
            }

            int indexEncontrado = -1;

            // Buscar el índice según el nombre
            for (int i = 0; i < _fotosPerfil.Count; i++)
            {
                if (_fotosPerfil[i].Nombre == ruta)
                {
                    indexEncontrado = i;
                    break;
                }
            }

            if (indexEncontrado != -1)
            {
                numeroFoto = indexEncontrado;
                PerfilCambio = ruta;
                ptbPerfil.Image = _fotosPerfil[indexEncontrado].Imagen;
            }
        }


        private void btnCambiarPerfil_Click(object sender, EventArgs e)
        {
            numeroFoto++;

            // Si pasa el último, vuelve al primero
            if (numeroFoto >= _fotosPerfil.Count)
                numeroFoto = 0;

            PerfilCambio = _fotosPerfil[numeroFoto].Nombre;
            ptbPerfil.Image = _fotosPerfil[numeroFoto].Imagen;

            VerificarCambios(sender, e);
        }

        private void VerificarHabilitado()
        {
            bool esMismoUsuario = UsuarioActual?.IdUsuario == _idActual;
            bool estaHabilitado = UsuarioCopia?.Habilitado == true;

            btnActualizar.Visible = estaHabilitado;
            btnRestablecerCambios.Visible = estaHabilitado;
            btnDeshabilitar2.Visible = estaHabilitado && !esMismoUsuario;

            btnHabilitar.Visible = !estaHabilitado && !esMismoUsuario;
            btnDeshabilitar.Visible = !estaHabilitado && !esMismoUsuario;
            btnCancelar.Visible = !estaHabilitado && !esMismoUsuario;

            SetEditable(estaHabilitado);
        }

        private void SetEditable(bool estado)
        {
            txtUsuario.Enabled = estado;
            txtNombre.Enabled = estado;
            txtApellido.Enabled = estado;
            txtEmail.Enabled = estado;
            txtContraseña.Enabled = estado;

            btnDeshabilitar.Enabled = estado;
            btnCancelar.Enabled = estado;
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
                FotoPerfil = PerfilCambio
            };

            usuarioCN.ActualizarUsuario(usuario);
            HabilitarBotones(false, false);

            var form = (FormPrincipal)this.FindForm();
            form?.CargarDatosDelUsuarioActual();

            ActualizarUC(_idActual);
        }

        private void btnRestablecerCambios_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = UsuarioCopia?.Usuario;
            txtNombre.Text = UsuarioCopia?.Nombre;
            txtApellido.Text = UsuarioCopia?.Apellido;
            txtEmail.Text = UsuarioCopia?.Email;

            CargarFotoPerfil(UsuarioCopia?.FotoPerfil);

            HabilitarBotones(false, false);
        }

        // ======================================================================
        // HABILITAR / DESHABILITAR USUARIO
        // ======================================================================

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsuarioCopia?.Habilitado == true)
                {
                    usuarioCN.DeshabilitarUsuario(_idActual);
                    MessageBox.Show("Usuario deshabilitado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    usuarioBajas?.HabilitarUsuario(_idActual);
                    MessageBox.Show("Usuario habilitado correctamente.",
                        "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ActualizarUC(_idActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

<<<<<<< HEAD
=======
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
            FormPrincipal? form = this.FindForm() as FormPrincipal;

            if (form != null)
            {
                form.CerrarGestionUsuario();
            }
        }

        private void RenovarCantidadAcciones()
        {
            lblCantImplemento.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 1).ToString();
            lblCantModifico.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 2).ToString();
            lblCantInhabilito.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 3).ToString();
            lblCantRehabilito.Text = repoHistorialCambio.CantidadAccionByUser(_idActual, 4).ToString();
            lblCantPrestamos.Text = repoHistorialCambio.CantidadPrestamosByUser(_idActual).ToString();
            lblCantDevoluciones.Text = repoHistorialCambio.CantidadDevolucionByUser(_idActual).ToString();
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

>>>>>>> 15ea417 (Arreglando errores (Restrospectiva))
        private void btnDeshabilitar2_Click(object sender, EventArgs e)
        {
            btnActualizar.Visible = false;
            btnRestablecerCambios.Visible = false;
            btnDeshabilitar.Visible = true;
            btnCancelar.Visible = true;
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

        private void VerificarCambios(object sender, EventArgs e)
        {
            bool cambio =
                txtUsuario.Text != UsuarioCopia?.Usuario ||
                txtNombre.Text != UsuarioCopia?.Nombre ||
                txtApellido.Text != UsuarioCopia?.Apellido ||
                txtEmail.Text != UsuarioCopia?.Email ||
                PerfilCambio != UsuarioCopia?.FotoPerfil;

            HabilitarBotones(cambio, cambio);
        }

        private void HabilitarBotones(bool actu, bool rest)
        {
            btnActualizar.Enabled = actu;
            btnRestablecerCambios.Enabled = rest;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            var form = (FormPrincipal)this.FindForm();
            form?.CerrarGestionUsuario();
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
