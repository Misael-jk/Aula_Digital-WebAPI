using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using CapaNegocio;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class UsuariosUC : UserControl
    {
        private readonly UsuariosCN usuariosCN = null!;
        private bool mostrarPassword = false;
        private readonly IRepoRoles repoRoles;
        private int IdUser = 4;
        private readonly IRepoUsuarios repoUsuarios;

        public UsuariosUC(UsuariosCN usuariosCN, IRepoRoles repoRoles, IRepoUsuarios repoUsuarios)
        {
            InitializeComponent();
            this.usuariosCN = usuariosCN;
            this.repoRoles = repoRoles;
            this.repoUsuarios = repoUsuarios;
        }
        private void UsuariosUC_Load_1(object sender, EventArgs e)
        {
            IEnumerable<Roles> todo = repoRoles.GetAll();
            cmbRoles.DataSource = todo;
            cmbRoles.ValueMember = "IdRol";
            cmbRoles.DisplayMember = "Rol";

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

        private void dtgUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dtgUsuarios.Rows[e.RowIndex];

            txtUsuario.Text = fila.Cells["Usuario"].Value?.ToString();
            txtContraseña.Text = fila.Cells["Password"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value?.ToString();
            txtEmail.Text = fila.Cells["Email"].Value?.ToString();

            Usuarios? user = repoUsuarios.GetByUser(txtUsuario.Text);
            cmbRoles.SelectedIndex = user.IdRol - 1;

            string nombreFoto = fila.Cells["FotoPerfil"].Value?.ToString();
            string carpetaFotos = Path.Combine(Application.StartupPath, "FotosUsuarios");

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
            string fotoPerfil = null;

            if (ptbPerfil.Tag != null && ptbPerfil.Tag.ToString() != "__ELIMINAR__")
            {
                string carpetaDestino = Path.Combine(Application.StartupPath, "FotosUsuarios");
                Directory.CreateDirectory(carpetaDestino);

                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(ptbPerfil.Tag.ToString());
                string destino = Path.Combine(carpetaDestino, nombreArchivo);

                File.Copy(ptbPerfil.Tag.ToString(), destino, true);
                fotoPerfil = nombreArchivo;
            }

            Usuarios usuario = new Usuarios()
            {
                IdUsuario = IdUser,
                Usuario = txtUsuario.Text,
                Password = txtContraseña.Text,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                IdRol = (int)cmbRoles.SelectedValue,
                FotoPerfil = fotoPerfil
            };

            usuariosCN.CrearDocente(usuario);

            ptbPerfil.Tag = null;
            ptbPerfil.Image = null;
            MostrarUsuarios();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (dtgUsuarios.CurrentRow == null) return;

            DataGridViewRow fila = dtgUsuarios.CurrentRow;

            string fotoAnterior = fila.Cells["FotoPerfil"].Value?.ToString();
            string fotoPerfil = fotoAnterior;

            if (ptbPerfil.Tag != null)
            {
                if (ptbPerfil.Tag.ToString() == "__ELIMINAR__")
                {
                    if (!string.IsNullOrEmpty(fotoAnterior))
                    {
                        string rutaVieja = Path.Combine(Application.StartupPath, "FotosUsuarios", fotoAnterior);
                        if (File.Exists(rutaVieja)) File.Delete(rutaVieja);
                    }
                    fotoPerfil = null;
                }
                else
                {
                    string carpetaDestino = Path.Combine(Application.StartupPath, "FotosUsuarios");
                    Directory.CreateDirectory(carpetaDestino);

                    string extension = Path.GetExtension(ptbPerfil.Tag.ToString());
                    string nombreUnico = Guid.NewGuid().ToString() + extension;
                    string destino = Path.Combine(carpetaDestino, nombreUnico);

                    File.Copy(ptbPerfil.Tag.ToString(), destino, true);

                    if (!string.IsNullOrEmpty(fotoAnterior))
                    {
                        string rutaVieja = Path.Combine(Application.StartupPath, "FotosUsuarios", fotoAnterior);
                        if (File.Exists(rutaVieja)) File.Delete(rutaVieja);
                    }

                    fotoPerfil = nombreUnico;
                }
            }

            Usuarios usuario = new Usuarios()
            {
                IdUsuario = Convert.ToInt32(fila.Cells["IdUsuario"].Value),
                Usuario = txtUsuario.Text,
                Password = txtContraseña.Text,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                IdRol = (int)cmbRoles.SelectedValue,
                FotoPerfil = fotoPerfil
            };

            usuariosCN.ActualizarUsuario(usuario);

            ptbPerfil.Tag = null;
            MostrarUsuarios();
        }

        private void btnDeseleccion_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtContraseña.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            cmbRoles.SelectedIndex = 0;
            dtgUsuarios.ClearSelection();
            ptbPerfil.Image = Properties.Resources.Perfil_default;
            ptbPerfil.Tag = null;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
        }
    }
}
