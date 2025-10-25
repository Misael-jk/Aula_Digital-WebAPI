using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaDatos.InterfacesDTO;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        private readonly NotebooksCN notebooksCN;
        private Usuarios usuarioActual;
        private int IdActual = 0;

        public NotebooksUC(NotebooksCN notebooksCN, Usuarios user)
        {
            InitializeComponent();
            this.notebooksCN = notebooksCN;
            this.usuarioActual = user;
        }
        public void ActualizarDataGrid()
        {
            dtgNotebook.DataSource = notebooksCN.GetAll();

            cmbModelo.DataSource = notebooksCN.ListarModelosPorTipo(1);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";
        }

        private void NotebooksUC_Load(object sender, EventArgs e)
        {
            ActualizarDataGrid();
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1200);
        }

        private void btnAgregarNotebook_Click(object sender, EventArgs e)
        {
            //Action ActualizarGrid = ActualizarDataGrid; No es necesario crear esta variable temporal, ya que puedo pasar el metodo directamente

            var Notebook = new FormCRUDNotebook(notebooksCN, ActualizarDataGrid);
            Notebook.Show();
        }

        private void dtgNotebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdActual = Convert.ToInt32(dtgNotebook.Rows[e.RowIndex].Cells["IdNotebook"].Value);

            Notebooks? notebook = notebooksCN.ObtenerNotebookPorID(IdActual);

            lblIDNotebook.Text = "ID: ";
            lblIDNotebook.Text += notebook?.IdElemento.ToString();
            txtEquipo.Text = notebook?.Equipo;
            txtNroSerie.Text = notebook?.NumeroSerie;
            txtCodBarra.Text = notebook?.CodigoBarra;
            txtPatrimonio.Text = notebook?.Patrimonio;
            cmbModelo.SelectedValue = notebook?.IdModelo;
            cmbUbicacion.SelectedValue = notebook?.IdUbicacion;

            EstadosMantenimiento? estadosMantenimiento = notebooksCN.ObtenerEstadoMantenimientoPorID(notebook.IdEstadoMantenimiento);

            lblEstado.Text = estadosMantenimiento?.EstadoMantenimientoNombre;
            lblEstado.Tag = estadosMantenimiento?.IdEstadoMantenimiento;

            if(estadosMantenimiento?.IdEstadoMantenimiento == 1)
            {
                ptbEstado.Image = Properties.Resources.disponibleIcon;
            } else
            {
                ptbEstado.Image = Properties.Resources.prestadoIcon;
            }

                Carritos? carritos = notebooksCN.ObtenerCarritoPorID((int)notebook.IdCarrito);
            lblCarroAsignado.Text = "Carro asignado: ";
            lblCasillero.Text = "Casillero: ";
            lblCarroAsignado.Text += carritos?.EquipoCarrito;
            lblCasillero.Text += notebook?.PosicionCarrito.ToString();

        }

        private void btnActualizarNotebook_Click(object sender, EventArgs e)
        {
            Notebooks? notebook = new Notebooks
            {
                IdElemento = IdActual,
                Equipo = txtEquipo.Text,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                //IdEstadoMantenimiento = (int)cmbEstados.SelectedValue,
                IdTipoElemento = 1,
                IdVarianteElemento = null,
                IdCarrito = null,
                PosicionCarrito = null,
                Habilitado = true,
                FechaBaja = null
            };

            notebooksCN.ActualizarNotebook(notebook, usuarioActual.IdUsuario);
            ActualizarDataGrid();
        }

        private void btnDeshabiliar_Click(object sender, EventArgs e)
        {
            //notebooksCN.DeshabilitarNotebook(IdActual, usuarioActual.IdUsuario, (int)cmbEstados.SelectedValue);

            ActualizarDataGrid();
        }
    }
}
