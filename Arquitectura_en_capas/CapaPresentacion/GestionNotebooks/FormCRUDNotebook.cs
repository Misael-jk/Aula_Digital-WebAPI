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
        private readonly Usuarios userVerificado;
        private readonly Action _actualizarGrid;
        private readonly Action actualizarGrafico;
        public FormCRUDNotebook(NotebooksCN notebooksCN, Action ActualizarGrid, Action actualizarGrafico, Usuarios userVerificado)
        {
            InitializeComponent();
            this.notebooksCN = notebooksCN;
            this.userVerificado = userVerificado;
            _actualizarGrid = ActualizarGrid;
            this.actualizarGrafico = actualizarGrafico;
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

            notebooksCN.CrearNotebook(notebooks, userVerificado.IdUsuario);
            _actualizarGrid.Invoke();
            actualizarGrafico.Invoke();
            this.Close();
        }

        private void BtnCerrar1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
