using CapaDatos.InterfacesDTO;
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
    public partial class FormCRUDNotebook : Form
    {
        private readonly NotebooksCN notebooksCN;
        private readonly Action _actualizarGrid;
        public FormCRUDNotebook(NotebooksCN notebooksCN, Action ActualizarGrid)
        {
            InitializeComponent();
            this.notebooksCN = notebooksCN;
            _actualizarGrid = ActualizarGrid;

        }

        private void FormCRUDNotebook_Load(object sender, EventArgs e)
        {
            cmbModelo.DataSource = notebooksCN.ListarModelosPorTipo(1);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCrearNotebook_Click(object sender, EventArgs e)
        {
            Notebooks notebooks = new Notebooks
            {
                Equipo = txtEquipo.Text,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                IdEstadoMantenimiento = 1,
                IdTipoElemento = 1,
                IdVarianteElemento = null,
                IdCarrito = null,
                PosicionCarrito = null,
                Habilitado = true,
                FechaBaja = null
            };

            notebooksCN.CrearNotebook(notebooks, 1);
            _actualizarGrid.Invoke();
            this.Close();
        }
    }
}
