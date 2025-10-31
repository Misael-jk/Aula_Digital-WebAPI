using CapaDatos.Interfaces;
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
    public partial class DocentesUC : UserControl
    {
        private readonly DocentesCN docentesCN;
        private readonly DocentesBajasCN docentesBajasCN;
        private int IdActual = 0;

        public DocentesUC(DocentesCN docentesCN, DocentesBajasCN docentesBajasCN)
        {
            InitializeComponent();
            this.docentesCN = docentesCN;
            this.docentesBajasCN = docentesBajasCN;
        }

        private void DocentesUC_Load(object sender, EventArgs e)
        {
            cmbHabilitado.Items.Add("Habilitados");
            cmbHabilitado.Items.Add("Deshabilitados");
            cmbHabilitado.SelectedIndex = 0;
        }

        //public void MostrarDocentes()
        //{
        //    dgvDocentes.DataSource = docentesCN.MostrarDocente();
        //}

        private void dgvDocentes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdActual = Convert.ToInt32(dgvDocentes.Rows[e.RowIndex].Cells["IdDocente"].Value);
            var docentes = docentesCN.GetById(IdActual);

            txtNombre.Text = docentes?.Nombre;
            txtApellido.Text = docentes?.Apellido;
            txtDNI.Text = docentes?.Dni;
            txtMail.Text = docentes?.Email;
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            var docente = new Docentes
            {
                Nombre = txtNombreCrear.Text,
                Apellido = txtApellidoCrear.Text,
                Dni = txtDNICrear.Text,
                Email = txtMailCrear.Text,
                Habilitado = true,
                FechaBaja = null
            };

            docentesCN.CrearDocente(docente);
            cmbHabilitado.SelectedIndex = 0;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            var docente = new Docentes
            {
                IdDocente = IdActual,
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Dni = txtDNI.Text,
                Email = txtMail.Text,
                Habilitado = true,
                FechaBaja = null
            };

            docentesCN.ActualizarDocente(docente);
            cmbHabilitado.SelectedIndex = 0;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            docentesCN.DeshabilitarDocente(IdActual);
            cmbHabilitado.SelectedIndex = 1;
        }

        private void cmbHabilitado_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cmbHabilitado.SelectedItem?.ToString())
            {
                case "Habilitados":
                    dgvDocentes.DataSource = docentesCN.MostrarDocente();
                    break;
                case "Deshabilitados":
                    dgvDocentes.DataSource = docentesBajasCN.GetAllDTO();
                    MessageBox.Show("Funca");
                    break;
                default:
                    break;
            }
        }
    }
}
