using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaEntidad;
using CapaNegocio;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CapaPresentacion
{
    public partial class MantenimientoUC : UserControl
    {
        //private readonly MantenimientoCN mantenimientoCN;
        private readonly NotebookBajasCN notebookBajasCN;
        private readonly ElementosBajasCN elementosBajasCN;
        private Usuarios usuarioActual;
        private int _idActualElemento;
        private int _idActualNotebook;

        public MantenimientoUC(NotebookBajasCN notebookBajasCN, ElementosBajasCN elementosBajasCN, Usuarios user)
        {
            InitializeComponent();
            this.notebookBajasCN = notebookBajasCN;
            this.elementosBajasCN = elementosBajasCN;
            this.usuarioActual = user;
        }

        private void MantenimientoUC_Load(object sender, EventArgs e)
        {
            MostrarDatos();

            seleccionarPrimeraFila(dgvMatenimientoNotebook);
            if (dgvMatenimientoNotebook.Rows.Count > 0)
            {
                MostrarDatosDeLaFilaNotebooks(0);
            }

            seleccionarPrimeraFila(dgvMantenimientoElemento);
            if (dgvMantenimientoElemento.Rows.Count > 0)
            {
                MostrarDatosDeFilaSeleccionada(0);
            }
        }

        public void MostrarDatos()
        {
            dgvMantenimientoElemento.DataSource = elementosBajasCN.GetAllElementos();
            dgvMatenimientoNotebook.DataSource = notebookBajasCN.GetAllNotebooks();
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            elementosBajasCN.HabilitarElemento(_idActualElemento, usuarioActual.IdUsuario, null);
            MostrarDatos();
        }

        private void dgvMantenimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            MostrarDatosDeFilaSeleccionada(e.RowIndex);
        }

        private void MostrarDatosDeFilaSeleccionada(int rowIndex)
        {
            lblIDElemento.Text = "ID: ";
            txtTipoElemento.Text = "Tipo: ";
            txtVarianteElemento.Text = "Variante: ";
            txtSerieElemento.Text = "Nro. de serie: ";
            txtBarraElemento.Text = "Cod. de barra: ";

            var fila = dgvMantenimientoElemento.Rows[rowIndex];

            _idActualElemento = Convert.ToInt32(fila.Cells["IdElemento"].Value);

            Elemento? elementoBaja = elementosBajasCN.ObtenerElementoPorID(_idActualElemento);

            lblIDElemento.Text += _idActualElemento;
            txtSerieElemento.Text += elementoBaja?.NumeroSerie;
            txtBarraElemento.Text += elementoBaja?.CodigoBarra;

            if (elementoBaja?.IdTipoElemento is not null)
            {
                TipoElemento? tipoElemento = elementosBajasCN.ObtenerTipoElementoPorID(elementoBaja.IdTipoElemento);

                txtTipoElemento.Text += tipoElemento?.IdTipoElemento;
            }

            if (elementoBaja?.IdVarianteElemento is not null)
            {
                VariantesElemento? variantesElemento = elementosBajasCN.ObtenerVariantePorID(elementoBaja.IdVarianteElemento.Value);

                txtVarianteElemento.Text += variantesElemento?.Variante;
            }
        }

        private void seleccionarPrimeraFila(DataGridView dgv)
        {
            if (dgv.Rows.Count >= 1)
            {
                dgv.ClearSelection();
                dgv.Rows[0].Selected = true;
                dgv.CurrentCell = dgv.Rows[0].Cells[0];
            }
        }

        private void MostrarDatosDeLaFilaNotebooks(int rowIndex)
        {
            lblIDNotebook.Text = "ID: ";
            txtEquipoNotebook.Text = "Equipo: ";
            txtSerieElemento.Text = "Nro. de serie: ";
            txtCodBarraNotebook.Text = "Cod. de barra: ";

            var fila = dgvMatenimientoNotebook.Rows[rowIndex];
            _idActualNotebook = Convert.ToInt32(fila.Cells["IdNotebook"].Value);
            Notebooks? notebookBaja = notebookBajasCN.ObtenerNotebookPorID(_idActualNotebook);

            lblIDNotebook.Text += _idActualNotebook;
            txtEquipoNotebook.Text += notebookBaja?.Equipo;
            txtNumSerieNotebook.Text += notebookBaja?.NumeroSerie;
            txtCodBarraNotebook.Text += notebookBaja?.CodigoBarra;
        }

        private void dgvMatenimientoNotebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            MostrarDatosDeLaFilaNotebooks(e.RowIndex);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            notebookBajasCN.HabilitarNotebook(_idActualNotebook, usuarioActual.IdUsuario);
            MostrarDatos();
        }
    }
}
