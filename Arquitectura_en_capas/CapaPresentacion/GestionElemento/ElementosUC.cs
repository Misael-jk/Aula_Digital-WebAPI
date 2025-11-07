using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using static System.Net.Mime.MediaTypeNames;

namespace CapaPresentacion
{
    public partial class ElementosUC : UserControl
    {
        private readonly ElementosCN elementosCN;
        private readonly Usuarios userVerificado;
        private int idElementoActual = 0;

        private int idVariante = 0;
        public ElementosUC(ElementosCN elementosCN, Usuarios userVerificado)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.userVerificado = userVerificado;
        }

        public void CargarElementos()
        {
            try
            {
                var elementos = elementosCN.ObtenerElementos();
                dgvElementos_M.DataSource = elementos;
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

            //Guna2DataGridViewStyler.SetupPrestamosColumns(dgvElementos, addActionButton: true);

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
            //var tipos = tiposElementoCN.GetTiposByElemento();

            //cmbTipoElemento.DataSource = tipos;
            //cmbTipoElemento.ValueMember = "IdTipoElemento";
            //cmbTipoElemento.DisplayMember = "ElementoTipo";

            //cmbUbicaciones.DataSource = elementosCN.ObtenerUbicaciones();
            //cmbUbicaciones.ValueMember = "IdUbicacion";
            //cmbUbicaciones.DisplayMember = "NombreUbicacion";

            //var tiposBusqueda = tipos.ToList();
            //tiposBusqueda.Insert(0, new TipoElemento { IdTipoElemento = 0, ElementoTipo = "Ningún tipo" });

            //cmbBuscarTipo.DataSource = tiposBusqueda;
            //cmbBuscarTipo.ValueMember = "IdTipoElemento";
            //cmbBuscarTipo.DisplayMember = "ElementoTipo";
        }

        private void dgvElementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvElementos_M.Rows[e.RowIndex];

            idElementoActual = Convert.ToInt32(fila.Cells["IdElemento"].Value);

            //var elemento = repoElemento.GetById(idElemento);

            //txtNroSerie.Text = elemento?.NumeroSerie;
            //txtCodBarra.Text = elemento?.CodigoBarra;
            //txtPatrimonio.Text = elemento?.Patrimonio;
            //cmbTipoElemento.SelectedValue = elemento.IdTipoElemento;
            //cmbVarianteElementos.SelectedValue = elemento.IdVarianteElemento.HasValue ? elemento.IdVarianteElemento.Value : -1;

            //cmbUbicaciones.SelectedIndex = elemento.IdUbicacion;
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
            
        }

        private void btnActualizarElemento_Click(object sender, EventArgs e)
        {
            //VariantesElemento? variante = elementosCN.ObtenerVariantePorID(idVariante);
            //Elemento? elementoOLD = elementosCN.ObtenerPorId(idElemento);

            //var elemento = new Elemento
            //{
            //    IdElemento = idElemento,
            //    IdTipoElemento = (int)cmbTipoElemento.SelectedValue,
            //    NumeroSerie = txtNroSerie.Text,
            //    CodigoBarra = txtCodBarra.Text,
            //    Patrimonio = txtPatrimonio.Text,
            //    IdVarianteElemento = idVariante,
            //    IdUbicacion = (int)cmbUbicaciones.SelectedValue,
            //    IdModelo = variante.IdModelo,
            //    IdEstadoMantenimiento = 1,
            //    Habilitado = true,
            //    FechaBaja = null
            //};

            //elementosCN.ActualizarElemento(elemento, userVerificado.IdUsuario);
            //CargarElementos();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            //elementosCN.DeshabilitarElemento(txtNroSerie.Text);
            CargarElementos();
        }

        private void dgvElementos_M_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvElementos_M.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string? estado = e.Value.ToString();

                if (estado == "En mantenimiento")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 150, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Prestamo")
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

        private void btnCrearElemento_M_Click(object sender, EventArgs e)
        {
            var CrearElemento = new FormCRUDElementos(elementosCN, userVerificado, CargarElementos);
            CrearElemento.ShowDialog();
        }
    }
}
