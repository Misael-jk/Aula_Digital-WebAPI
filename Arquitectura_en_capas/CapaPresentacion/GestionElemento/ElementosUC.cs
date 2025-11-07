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
                //dgvElementos.DataSource = elementos;
                dgvElementos_M.DataSource = elementos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los elementos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarDataGrid()
        { 
            int es = (int)cmbEstados.SelectedValue;

            var estado = elementosCN.ObtenerEstadoMantenimientoPorID(es);

            switch(estado?.EstadoMantenimientoNombre)
            {
                case "Disponible":
                    dgvElementos_M.DataSource = elementosCN.ObtenerEstadoMantenimientoPorID(1);
                    break;
                case "En mantenimiento":
                    dgvElementos_M.DataSource = elementosCN.ObtenerEstadoMantenimientoPorID(3);
                    break;
                case "Prestado":
                    dgvElementos_M.DataSource = elementosCN.ObtenerEstadoMantenimientoPorID(2);
                    break;
                default:
                    dgvElementos_M.DataSource = elementosCN.ObtenerElementos();
                    break;
            }
        }

        private void ElementosUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1100);

            ApplyModernStyleCompact(dgvElementos);
            //Guna2DataGridViewStyler.SetupPrestamosColumns(dgvElementos, addActionButton: true);

            IEnumerable<EstadosMantenimiento> estados = repoEstadosMantenimiento.GetAll();
            //IEnumerable<EstadosMantenimiento> estados = elementosCN.ListarEstadoMantenimiento();

            cmbEstados.DataSource = estados;
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";

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

            cmbTipoElemento.DataSource = tipos;
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
            var CrearElemento = new FormCRUDElementos(elementosCN, userVerificado, CargarElementos);
            CrearElemento.ShowDialog();
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

        private void ApplyModernStyleCompact(Guna2DataGridView dgv)
        {
            // Tamaño y comportamiento general
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(215, 230, 215);
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersHeight = 38;
            dgv.RowTemplate.Height = 34;

            // Header verde brillante
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(67, 160, 71);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Celdas base
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 45);
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.Padding = new Padding(6, 3, 6, 3);

            // Alternancia
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);

            // Selección verde suave
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 200);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 30, 35);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 200);

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            EnableDoubleBuffering(dgv);

        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            Type dgvType = dgv.GetType();
            System.Reflection.PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (pi != null)
            {
                pi.SetValue(dgv, true, null);
            }
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

        private void cmbEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstados.SelectedValue is int es)
            {
                ActualizarDataGrid();
            }
            else
            {
                MessageBox.Show("El valor seleccionado no es válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
