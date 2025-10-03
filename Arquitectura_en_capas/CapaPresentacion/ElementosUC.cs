using CapaDatos.Interfaces;
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
using static System.Net.Mime.MediaTypeNames;

namespace CapaPresentacion
{
    public partial class ElementosUC : UserControl
    {
        private readonly ElementosCN elementosCN = null!;
        private readonly TiposElementoCN tiposElementoCN = null!;
        private readonly IRepoEstadosMantenimiento repoEstadosMantenimiento;
        private readonly IRepoElemento repoElemento;
        private int idElemento = 0;
        public ElementosUC(ElementosCN elementosCN, IRepoEstadosMantenimiento repoEstadosMantenimiento, IRepoElemento repoElemento, TiposElementoCN tiposElementoCN)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.tiposElementoCN = tiposElementoCN;
            this.repoEstadosMantenimiento = repoEstadosMantenimiento;
            this.repoElemento = repoElemento;
        }

        public void CargarElementos()
        {
            try
            {
                var elementos = elementosCN.ObtenerElementos();
                dgvElementos.DataSource = elementos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los elementos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ElementosUC_Load(object sender, EventArgs e)
        {
           
            IEnumerable<EstadosMantenimiento> estados = repoEstadosMantenimiento.GetAll();
            cmbEstados.DataSource = estados.ToList(); 
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";

    
            var estadosBusqueda = estados
                .Where(e => e.IdEstadoMantenimiento != 2) 
                .ToList();

     
            estadosBusqueda.Insert(0, new EstadosMantenimiento
            {
                IdEstadoMantenimiento = 0,
                EstadoMantenimientoNombre = "Ningún estado"
            });

            cmbBuscarEstado.DataSource = estadosBusqueda;
            cmbBuscarEstado.ValueMember = "IdEstadoMantenimiento";
            cmbBuscarEstado.DisplayMember = "EstadoMantenimientoNombre";


            // -------------------- TIPOS --------------------
            var tipos = tiposElementoCN.GetAllTipo();
            cmbTipoElemento.DataSource = tipos.ToList();
            cmbTipoElemento.ValueMember = "IdTipoElemento";
            cmbTipoElemento.DisplayMember = "ElementoTipo";

            var tiposBusqueda = tipos.ToList();
            tiposBusqueda.Insert(0, new TipoElemento { IdTipoElemento = 0, ElementoTipo = "Ningún tipo" });

            cmbBuscarTipo.DataSource = tiposBusqueda;
            cmbBuscarTipo.ValueMember = "IdTipoElemento";
            cmbBuscarTipo.DisplayMember = "ElementoTipo";
        }

        private void dgvElementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvElementos.Rows[e.RowIndex];

            txtCodBarra.Text = fila.Cells["codigoBarra"].Value?.ToString();
            txtNroSerie.Text = fila.Cells["numeroSerie"].Value?.ToString();

            Elemento? elemento = repoElemento.GetByNumeroSerie(txtNroSerie.Text);
            cmbEstados.SelectedIndex = elemento.IdEstadoMantenimiento - 1;
            cmbTipoElemento.SelectedIndex = elemento.IdTipoElemento - 1;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //var elementos = elementosCN.GetElementoByFiltro(txtBuscarNroSerie.Text, txtBuscarCodBarra.Text, (int)cmbBuscarEstado.SelectedValue, txtBuscarCarrito.Text, (int)cmbBuscarTipo.SelectedValue);
            //dgvElementos.DataSource = elementos;
        }

        private void btnQuitarFiltros_Click(object sender, EventArgs e)
        {
            txtBuscarNroSerie.Clear();
            txtBuscarCodBarra.Clear();
            txtBuscarCarrito.Clear();
            cmbBuscarEstado.SelectedIndex = 0;
            cmbBuscarTipo.SelectedIndex = 0;
            CargarElementos();
        }

        private void btnCrearElemento_Click(object sender, EventArgs e)
        {
            //Elemento elemento = new Elemento()
            //{
            //    IdElemento = idElemento,
            //    IdTipoElemento = (int)cmbTipoElemento.SelectedValue,
            //    IdCarrito = null,
            //    PosicionCarrito = null,
            //    IdEstadoElemento = (int)cmbEstados.SelectedValue,
            //    numeroSerie = txtNroSerie.Text,
            //    codigoBarra = txtCodBarra.Text,
            //    Disponible = true,
            //};

            //elementosCN.CrearElemento(elemento);
            CargarElementos();
        }

        private void btnActualizarElemento_Click(object sender, EventArgs e)
        {

        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            //elementosCN.DeshabilitarElemento(txtNroSerie.Text);
            CargarElementos();
        }
    }
}
