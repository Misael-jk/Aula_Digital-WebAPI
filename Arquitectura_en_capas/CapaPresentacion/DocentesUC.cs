using CapaDatos.Interfaces;
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
    public partial class DocentesUC : UserControl
    {
        private readonly DocentesCN docentesCN;
        private readonly DocentesBajasCN docentesBajasCN;
        private int IdActual = 0;
        private AutoCompleteStringCollection AcDocente;

        public DocentesUC(DocentesCN docentesCN, DocentesBajasCN docentesBajasCN)
        {
            InitializeComponent();
            this.docentesCN = docentesCN;
            this.docentesBajasCN = docentesBajasCN;
            AcDocente = new AutoCompleteStringCollection();
        }

        private void DocentesUC_Load(object sender, EventArgs e)
        {
            cmbHabilitado.Items.Add("Habilitados");
            cmbHabilitado.Items.Add("Deshabilitados");
            cmbHabilitado.SelectedIndex = 0;

            txtBuscarDocente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtBuscarDocente.AutoCompleteSource = AutoCompleteSource.CustomSource;
            CargarAutoComplete();

            dgvDocentes.DataSource = docentesCN.MostrarDocente();
            dgvDocentes.Columns["IdDocente"].HeaderText = "ID";
            dgvDocentes.Columns["IdDocente"].Width = 30;
            dgvDocentes.Columns["Nombre"].HeaderText = "Nombre";
            dgvDocentes.Columns["Nombre"].Width = 75;
            dgvDocentes.Columns["Apellido"].HeaderText = "Apellido";
            dgvDocentes.Columns["Apellido"].Width = 75;
            dgvDocentes.Columns["Dni"].HeaderText = "DNI";
            dgvDocentes.Columns["Dni"].Width = 70;
            dgvDocentes.Columns["Email"].HeaderText = "Email";
            dgvDocentes.Columns["EstadoPrestamo"].HeaderText = "Actividad";
            dgvDocentes.Columns["EstadoPrestamo"].Width = 100;
        }

        private void CargarAutoComplete()
        {
            AcDocente.Clear();
            var nombresDocentes = docentesCN.MostrarDocente().Select(d => d.Nombre).Distinct();
            foreach (var nombre in nombresDocentes)
            {
                AcDocente.Add(nombre);
            }
            txtBuscarDocente.AutoCompleteCustomSource = AcDocente;

            if (txtBuscarDocente.AutoCompleteCustomSource.Count > 0)
            {
                txtBuscarDocente.AutoCompleteCustomSource = AcDocente;
            }
        }

        public void MostrarDocentes()
        {
            dgvDocentes.DataSource = docentesCN.MostrarDocente();
        }

        private void dgvDocentes_M_CellClick(object sender, DataGridViewCellEventArgs e)
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
            string nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtNombreCrear.Text.Trim().ToLower());
            string apellido = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtApellidoCrear.Text.Trim().ToLower());

            var docente = new Docentes
            {
                Nombre = nombre,
                Apellido = apellido,
                Dni = txtDNICrear.Text.Trim(),
                Email = txtMailCrear.Text.Trim().ToLower(),
                Habilitado = true,
                FechaBaja = null
            };

            docentesCN.CrearDocente(docente);
            MostrarDocentes();

            txtNombreCrear.Clear();
            txtApellido.Clear();
            txtDNICrear.Clear();
            txtMailCrear.Clear();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            string nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtNombre.Text.Trim().ToLower());
            string apellido = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtApellido.Text.Trim().ToLower());

            var docente = new Docentes
            {
                IdDocente = IdActual,
                Nombre = nombre,
                Apellido = apellido,
                Dni = txtDNI.Text,
                Email = txtMail.Text,
                Habilitado = true,
                FechaBaja = null
            };

            docentesCN.ActualizarDocente(docente);
            MostrarDocentes();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            docentesCN.DeshabilitarDocente(IdActual);
            cmbHabilitado.SelectedIndex = 1;
        }

        private void cmbHabilitado_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbHabilitado.SelectedItem?.ToString())
            {
                case "Habilitados":
                    dgvDocentes.DataSource = docentesCN.MostrarDocente();
                    break;
                case "Deshabilitados":
                    dgvDocentes.DataSource = docentesBajasCN.GetAllDTO();
                    break;
                default:
                    break;
            }
        }

        private void dgvDocentes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDocentes.Columns[e.ColumnIndex].Name == "EstadoPrestamo" && e.Value != null)
            {
                string? estado = e.Value.ToString();

                if (estado == "En Prestamo")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 230, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    return;
                }

                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
        }

        private void txtBuscarDocente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nombreBusqueda = txtBuscarDocente.Text.Trim();
                var resultados = docentesCN.MostrarDocente()
                    .Where(d => d.Nombre.Contains(nombreBusqueda, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                dgvDocentes.DataSource = resultados;
            }
        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e)
        {
            txtBuscarDocente.Clear();
            MostrarDocentes();
        }
    }
}
