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
        private readonly ElementosCN elementosCN;
        private readonly TiposElementoCN tiposElementoCN;
        private readonly Usuarios userVerificado;
        private readonly IRepoEstadosMantenimiento repoEstadosMantenimiento;
        private readonly IRepoElemento repoElemento;
        private int idElemento = 0;

        private int idVariante = 0;
        public ElementosUC(ElementosCN elementosCN, IRepoEstadosMantenimiento repoEstadosMantenimiento, IRepoElemento repoElemento, TiposElementoCN tiposElementoCN, Usuarios userVerificado)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.tiposElementoCN = tiposElementoCN;
            this.repoEstadosMantenimiento = repoEstadosMantenimiento;
            this.repoElemento = repoElemento;
            this.userVerificado = userVerificado;
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
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1100);

            //IEnumerable<EstadosMantenimiento> estados = repoEstadosMantenimiento.GetAll(); ya es el Listar Estado Mantenimiento de ElementosCN
            //IEnumerable<EstadosMantenimiento> estados = elementosCN.ListarEstadoMantenimiento();

            //cmbEstados.DataSource = estados; 
            //cmbEstados.ValueMember = "IdEstadoMantenimiento";
            //cmbEstados.DisplayMember = "EstadoMantenimientoNombre";

            //var estadosBusqueda = estados
            //    .Where(e => e.IdEstadoMantenimiento != 2) 
            //    .ToList();


            //estadosBusqueda.Insert(0, new EstadosMantenimiento
            //{
            //    IdEstadoMantenimiento = 0,
            //    EstadoMantenimientoNombre = "Ningún estado"
            //});

            //cmbBuscarEstado.DataSource = estadosBusqueda;
            //cmbBuscarEstado.ValueMember = "IdEstadoMantenimiento";
            //cmbBuscarEstado.DisplayMember = "EstadoMantenimientoNombre";


            // -------------------- TIPOS --------------------
            var tipos = tiposElementoCN.GetTiposByElemento();

            cmbTipoElemento.DataSource = tipos.ToList();
            cmbTipoElemento.ValueMember = "IdTipoElemento";
            cmbTipoElemento.DisplayMember = "ElementoTipo";

            cmbUbicaciones.DataSource = elementosCN.ObtenerUbicaciones();
            cmbUbicaciones.ValueMember = "IdUbicacion";
            cmbUbicaciones.DisplayMember = "NombreUbicacion";

            //var tiposBusqueda = tipos.ToList();
            //tiposBusqueda.Insert(0, new TipoElemento { IdTipoElemento = 0, ElementoTipo = "Ningún tipo" });

            //cmbBuscarTipo.DataSource = tiposBusqueda;
            //cmbBuscarTipo.ValueMember = "IdTipoElemento";
            //cmbBuscarTipo.DisplayMember = "ElementoTipo";
        }

        private void dgvElementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvElementos.Rows[e.RowIndex];

            idElemento = Convert.ToInt32(fila.Cells["IdElemento"].Value);
            var elemento = repoElemento.GetById(idElemento);

            txtNroSerie.Text = elemento?.NumeroSerie;
            txtCodBarra.Text = elemento?.CodigoBarra;
            txtPatrimonio.Text = elemento?.Patrimonio;
            cmbTipoElemento.SelectedValue = elemento.IdTipoElemento;
            cmbVarianteElementos.SelectedValue = elemento.IdVarianteElemento.HasValue ? elemento.IdVarianteElemento.Value : -1;

            cmbUbicaciones.SelectedIndex = elemento.IdUbicacion;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //var elementos = elementosCN.GetElementoByFiltro(txtBuscarNroSerie.Text, txtBuscarCodBarra.Text, (int)cmbBuscarEstado.SelectedValue, txtBuscarCarrito.Text, (int)cmbBuscarTipo.SelectedValue);
            //dgvElementos.DataSource = elementos;
        }

        private void btnQuitarFiltros_Click(object sender, EventArgs e)
        {
            //txtBuscarNroSerie.Clear();
            //txtBuscarCodBarra.Clear();
            //txtBuscarCarrito.Clear();
            //cmbBuscarEstado.SelectedIndex = 0;
            //cmbBuscarTipo.SelectedIndex = 0;
            CargarElementos();
        }

        private void btnCrearElemento_Click(object sender, EventArgs e)
        {
            VariantesElemento? variante = elementosCN.ObtenerVariantePorID(idVariante);

            Elemento elemento = new Elemento()
            {
                IdTipoElemento = (int)cmbTipoElemento.SelectedValue,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdVarianteElemento = idVariante,
                IdUbicacion = (int)cmbUbicaciones.SelectedValue,
                IdModelo = variante.IdModelo,
                IdEstadoMantenimiento = 1,
                Habilitado = true,
                FechaBaja = null
            };

            elementosCN.CrearElemento(elemento, userVerificado.IdUsuario);
            CargarElementos();
        }

        private void btnActualizarElemento_Click(object sender, EventArgs e)
        {
            VariantesElemento? variante = elementosCN.ObtenerVariantePorID(idVariante);
            Elemento? elementoOLD = elementosCN.ObtenerPorId(idElemento);

            var elemento = new Elemento
            {
                IdElemento = idElemento,
                IdTipoElemento = (int)cmbTipoElemento.SelectedValue,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdVarianteElemento = idVariante,
                IdUbicacion = (int)cmbUbicaciones.SelectedValue,
                IdModelo = variante.IdModelo,
                IdEstadoMantenimiento = 1,
                Habilitado = true,
                FechaBaja = null
            };

            elementosCN.ActualizarElemento(elemento, userVerificado.IdUsuario);
            CargarElementos();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            //elementosCN.DeshabilitarElemento(txtNroSerie.Text);
            CargarElementos();
        }

        private void cmbTipoElemento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbTipoElemento.SelectedValue is int selectedValue)
            {
                idVariante = selectedValue - 1;

                cmbVarianteElementos.DataSource = elementosCN.ObtenerVariantesPorTipo(selectedValue);
                cmbVarianteElementos.ValueMember = "IdVarianteElemento";
                cmbVarianteElementos.DisplayMember = "Variante";
            }
        }
    }
}
