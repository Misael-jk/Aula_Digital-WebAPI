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
        private readonly UsuariosCN usuariosCN;
        private readonly UsuariosBajasCN usuariosBajasCN;
        private readonly FormPrincipal formPrincipal;
        private bool mostrarPassword = false;
        private int _IdUserActual = 0;

        public UsuariosUC(UsuariosCN usuariosCN, UsuariosBajasCN usuariosBajasCN, FormPrincipal formPrincipal)
        {
            InitializeComponent();
            this.usuariosCN = usuariosCN;
            this.usuariosBajasCN = usuariosBajasCN;
            this.formPrincipal = formPrincipal;
        }
        private void UsuariosUC_Load_1(object sender, EventArgs e)
        {
            cmbHabilitado.Items.Add("Habilitados");
            cmbHabilitado.Items.Add("Deshabilitados");
            cmbHabilitado.SelectedIndex = 0;

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
            dtgUsuarios.Columns[6].Width = 200;
        }

        private void dtgUsuarios_CellFormatting_1(object sender, DataGridViewCellFormattingEventArgs e)
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

        #region Obteniendo datos de un usuario seleccionado
        private void dtgUsuarios_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dtgUsuarios.Rows[e.RowIndex];

            _IdUserActual = Convert.ToInt32(fila.Cells["IdUsuario"].Value);

            btnGestionar.Enabled = true;
        }
        #endregion

        private void cbxMostraPassword_CheckedChanged(object sender, EventArgs e)
        {
            mostrarPassword = cbxMostraPassword.Checked;
            dtgUsuarios.Refresh();
        }

        private void cmbHabilitado_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbHabilitado.SelectedItem?.ToString())
            {
                case "Habilitados":
                    dtgUsuarios.DataSource = usuariosCN.ObtenerElementos();
                    break;
                case "Deshabilitados":
                    dtgUsuarios.DataSource = usuariosBajasCN.GetAllDTO();
                    break;
                default:
                    break;
            }
        }

        private void btnAgregarUsuario_Click(object sender, EventArgs e)
        {
            CrearUsuarioUC? formAgregarUsuario = new CrearUsuarioUC(usuariosCN, MostrarUsuarios, formPrincipal, this);
            formPrincipal.MostrarUserControl(formAgregarUsuario);
        }

        private void btnGestionar_Click(object sender, EventArgs e)
        {
            var form = (FormPrincipal)this.FindForm();
            form.AbrirUsuarioGestion(_IdUserActual);
        }
    }
}
