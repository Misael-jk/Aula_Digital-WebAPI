using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using CapaNegocio;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class UsuariosUC : UserControl
    {
        private readonly UsuariosCN usuariosCN = null!;
        private bool mostrarPassword = false;
        private string RutaFoto;
        private string _Password;
        private int _IdUserActual = 0;
        private int IdUser = 0;

        #region Datos para comprobar que hubo cambios al seleccionar usuario
        private string _usuario;
        private string _Nombre;
        private string _apellido;
        private string _email;
        #endregion

        public UsuariosUC(UsuariosCN usuariosCN)
        {
            InitializeComponent();
            this.usuariosCN = usuariosCN;
        }
        private void UsuariosUC_Load_1(object sender, EventArgs e)
        {
            MostrarUsuarios();
        }

        public void MostrarUsuarios()
        {
            dtgUsuarios.DataSource = usuariosCN.ObtenerElementos();

            dtgUsuarios.Columns["IdUsuario"].HeaderText = "ID";
            dtgUsuarios.Columns[0].Width = 35;
            dtgUsuarios.Columns["Usuario"].HeaderText = "Usuario";
            dtgUsuarios.Columns[1].Width = 120;
            dtgUsuarios.Columns["Password"].HeaderText = "Contraseña";
            dtgUsuarios.Columns[2].Width = 120;
            dtgUsuarios.Columns["Nombre"].HeaderText = "Nombre";
            dtgUsuarios.Columns[3].Width = 100;
            dtgUsuarios.Columns["Apellido"].HeaderText = "Apellido";
            dtgUsuarios.Columns[4].Width = 100;
            dtgUsuarios.Columns["Rol"].HeaderText = "Rol";
            dtgUsuarios.Columns[5].Width = 100;
            dtgUsuarios.Columns["Email"].HeaderText = "Email";
            dtgUsuarios.Columns[6].Width = 140;
        }
        private void dtgUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dtgUsuarios.Columns[e.ColumnIndex].Name == "Password" && e.Value != null)
            {
                if (!mostrarPassword)
                {
                    e.Value = new string('•', e.Value.ToString().Length);
                    e.FormattingApplied = true;
                }
            }
        }

        private void HabilitarBotones(bool actu, bool rest)
        {
            btnActualizar.Enabled = actu;
            btnRestablecerCambios.Enabled = rest;
        }

        #region Obteniendo datos de un usuario seleccionado
        private void dtgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dtgUsuarios.Rows[e.RowIndex];

            lblIDEncargado.Text = "ID: ";
            lblUltimoAporte.Text = "Ultimo aporte: ";

            _IdUserActual = Convert.ToInt32(fila.Cells["IdUsuario"].Value);

            lblIDEncargado.Text += _IdUserActual;

            Usuarios? usuarios = usuariosCN.ObtenerID(_IdUserActual);

            #region Datos modificables de un usuario
            txtUsuario.Text = usuarios?.Usuario;
            txtNombre.Text = usuarios?.Nombre;
            txtApellido.Text = usuarios?.Apellido;
            txtEmail.Text = usuarios?.Email;
            #endregion

            if (usuarios is not null)
            {
                #region Guardando datos en variables
                _usuario = usuarios.Usuario;
                _Nombre = usuarios.Nombre;
                _apellido = usuarios.Apellido;
                _email = usuarios.Email;
                #endregion

                Roles? roles = usuariosCN.ObtenerRolPorID(usuarios.IdRol);
                lblRol.Text = roles?.Rol;
                lblRol.Tag = roles?.IdRol;

                _Password = usuarios.Password;
            }

            if (usuarios?.FotoPerfil is not null)
            {
                RutaFoto = usuarios.FotoPerfil;
            }

            if (usuarios?.Habilitado == true)
            {
                lblHabilitado.Text = "Usuario habilitado";
                ptbEstado.Image = Properties.Resources.disponibleIcon;
            }
            else
            {
                lblHabilitado.Text = "Usuario inhabilitado";
                ptbEstado.Image = Properties.Resources.prestadoIcon;
            }

            string? UltimaAportacion = usuariosCN.ObtenerUltimaAportacion(_IdUserActual);

            if (UltimaAportacion != null)
            {
                lblUltimoAporte.Text += UltimaAportacion;
            }
            else
            {
                lblUltimoAporte.Text += "Sin aportaciones";
            }

            HabilitarBotones(false, false);

            //Usuarios? user = repoUsuarios.GetByUser(txtUsuario.Text);
            //cmbRoles.SelectedIndex = user.IdRol - 1;

            //string nombreFoto = fila.Cells["FotoPerfil"].Value?.ToString();
            //string carpetaFotos = Path.Combine(Application.StartupPath, "FotosUsuarios");

            //if (!string.IsNullOrEmpty(nombreFoto))
            //{
            //    string rutaFoto = Path.Combine(carpetaFotos, nombreFoto);
            //    if (File.Exists(rutaFoto))
            //        using (var fs = new FileStream(rutaFoto, FileMode.Open, FileAccess.Read))
            //            ptbPerfil.Image = Image.FromStream(fs);
            //    else
            //        ptbPerfil.Image = Properties.Resources.Perfil_default;
            //}
            //else
            //{
            //    ptbPerfil.Image = Properties.Resources.Perfil_default;
            //}

            //ptbPerfil.Tag = nombreFoto;
        }
        #endregion

        private void cbxMostraPassword_CheckedChanged(object sender, EventArgs e)
        {
            mostrarPassword = cbxMostraPassword.Checked;
            dtgUsuarios.Refresh();
        }
        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            //Codigo en proceso

            //using OpenFileDialog ofd = new OpenFileDialog
            //{
            //    Filter = "Imágenes|*.jpg;*.jpeg;*.png",
            //    Title = "Seleccione una imagen"
            //};

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    if (ptbPerfil.Image != null && ptbPerfil.Image != Properties.Resources.Perfil_default)
            //    {
            //        ptbPerfil.Image.Dispose();
            //        ptbPerfil.Image = null;
            //    }

            //    using (Image imgTemp = Image.FromFile(ofd.FileName))
            //        ptbPerfil.Image = new Bitmap(imgTemp);

            //    ptbPerfil.Tag = ofd.FileName;
            //}
        }

        private void btnQuitarPerfil_Click(object sender, EventArgs e)
        {
            //ptbPerfil.Image = Properties.Resources.Perfil_default;
            //ptbPerfil.Tag = "__ELIMINAR__";
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            //string fotoPerfil = null;

            //if (ptbPerfil.Tag != null && ptbPerfil.Tag.ToString() != "__ELIMINAR__")
            //{
            //    string carpetaDestino = Path.Combine(Application.StartupPath, "FotosUsuarios");
            //    Directory.CreateDirectory(carpetaDestino);

            //    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(ptbPerfil.Tag.ToString());
            //    string destino = Path.Combine(carpetaDestino, nombreArchivo);

            //    File.Copy(ptbPerfil.Tag.ToString(), destino, true);
            //    fotoPerfil = nombreArchivo;
            //}

            //Usuarios usuario = new Usuarios()
            //{
            //    IdUsuario = IdUser,
            //    Usuario = txtUsuario.Text,
            //    Password = txtApellido.Text,
            //    Nombre = txtNombre.Text,
            //    Apellido = txtApellido.Text,
            //    Email = txtEmail.Text,
            //    IdRol = Convert.ToInt32(lblRol),
            //    FotoPerfil = fotoPerfil
            //};

            //usuariosCN.CrearDocente(usuario);

            //ptbPerfil.Tag = null;
            //ptbPerfil.Image = null;
            //MostrarUsuarios();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            //if (dtgUsuarios.CurrentRow == null) return;

            //DataGridViewRow fila = dtgUsuarios.CurrentRow;

            //string fotoAnterior = fila.Cells["FotoPerfil"].Value?.ToString();
            //string fotoPerfil = fotoAnterior;

            //if (ptbPerfil.Tag != null)
            //{
            //    if (ptbPerfil.Tag.ToString() == "__ELIMINAR__")
            //    {
            //        if (!string.IsNullOrEmpty(fotoAnterior))
            //        {
            //            string rutaVieja = Path.Combine(Application.StartupPath, "FotosUsuarios", fotoAnterior);
            //            if (File.Exists(rutaVieja)) File.Delete(rutaVieja);
            //        }
            //        fotoPerfil = null;
            //    }
            //    else
            //    {
            //        string carpetaDestino = Path.Combine(Application.StartupPath, "FotosUsuarios");
            //        Directory.CreateDirectory(carpetaDestino);

            //        string extension = Path.GetExtension(ptbPerfil.Tag.ToString());
            //        string nombreUnico = Guid.NewGuid().ToString() + extension;
            //        string destino = Path.Combine(carpetaDestino, nombreUnico);

            //        File.Copy(ptbPerfil.Tag.ToString(), destino, true);

            //        if (!string.IsNullOrEmpty(fotoAnterior))
            //        {
            //            string rutaVieja = Path.Combine(Application.StartupPath, "FotosUsuarios", fotoAnterior);
            //            if (File.Exists(rutaVieja)) File.Delete(rutaVieja);
            //        }

            //        fotoPerfil = nombreUnico;
            //    }
            //}

            Usuarios usuario = new Usuarios()
            {
                IdUsuario = _IdUserActual,
                Usuario = txtUsuario.Text,
                Password = _Password,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                IdRol = Convert.ToInt32(lblRol.Tag),
                FotoPerfil = RutaFoto
            };

            usuariosCN.ActualizarUsuario(usuario);
            MostrarUsuarios();
            HabilitarBotones(false, false);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

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

        private void btnRestablecerCambios_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = _usuario;
            txtNombre.Text = _Nombre;
            txtApellido.Text = _apellido;
            txtEmail.Text = _email;

            HabilitarBotones(false, false);
        }
    }
}
