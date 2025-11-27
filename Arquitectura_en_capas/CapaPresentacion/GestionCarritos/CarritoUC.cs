using CapaDTOs;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class CarritoUC : UserControl
    {
        private readonly FormPrincipal formPrincipal;
        private readonly CarritosCN carritosCN;
        private readonly CarritosBajasCN carritosBajas;
        private int _idCarritoActual = 0;
        private readonly Usuarios userVerificado;

        public CarritoUC(CarritosCN carritosCN, Usuarios userVerificado, CarritosBajasCN carritosBajas, FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.carritosCN = carritosCN;
            this.userVerificado = userVerificado;
            this.carritosBajas = carritosBajas;
            this.formPrincipal = formPrincipal;
        }

        public void MostrarDatos()
        {
            var carrito = carritosCN.MostrarCarritos();

            dgvCarritos_M.DataSource = carrito.ToList();
        }
        public void ActualizarDatagrid(int idEstado)
        {
            IEnumerable<CarritosDTO> carritosFiltrados;

            if (idEstado == 0)
            {
                carritosFiltrados = carritosCN.MostrarCarritos();
            }
            else
            {
                var estado = carritosCN.ObtenerEstadoMantenimientoPorID(idEstado);
                carritosFiltrados = carritosCN.ObtenerPorEstado(estado?.EstadoMantenimientoNombre);
            }

            dgvCarritos_M.DataSource = carritosFiltrados.ToList();
        }

        private void CarritoUC_Load(object sender, EventArgs e)
        {
            if (userVerificado.IdRol == 3)
            {
                btnCrearElemento_M.Enabled = false;
                btnGestionarElemento_M.Enabled = false;
            }

            ActualizarDatagrid(0);

            RenovarDatos();

            dgvCarritos_M.Columns["IdCarrito"].HeaderText = "ID";
            dgvCarritos_M.Columns["IdCarrito"].Width = 40;
            dgvCarritos_M.Columns["IdCarrito"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCarritos_M.Columns["NumeroSerieCarrito"].HeaderText = "Serie";
            dgvCarritos_M.Columns["Capacidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCarritos_M.Columns["Capacidad"].Width = 80;
            dgvCarritos_M.Columns["EstadoMantenimiento"].HeaderText = "Estado";


            List<EstadosMantenimiento> estados = carritosCN.ListarEstadosMatenimiento().ToList();
            estados.Insert(0, new EstadosMantenimiento { IdEstadoMantenimiento = 0, EstadoMantenimientoNombre = "Todos" });
            cmbEstado_B.DataSource = estados;
            cmbEstado_B.ValueMember = "IdEstadoMantenimiento";
            cmbEstado_B.DisplayMember = "EstadoMantenimientoNombre";
            cmbEstado_B.SelectedIndex = 0;
        }

        public void RenovarDatos()
        {
            //var numSeriesBD = carritosCN.ObtenerSeriePorNotebook();

            //string[] numSeries = numSeriesBD.Select(p => p.NumeroSerie).ToArray();

            //var lista = new AutoCompleteStringCollection();
            //lista.AddRange(numSeries);
            //txtNroSerie.AutoCompleteCustomSource = lista;

            //var codBarraBD = carritosCN.ObtenerCodBarraPorNotebook();

            //string[] codBarras = codBarraBD.Select(p => p.CodigoBarra).ToArray();

            //var lista2 = new AutoCompleteStringCollection();
            //lista2.AddRange(codBarras);
            //txtCodBarra.AutoCompleteCustomSource = lista2;
        }

        private void dgvCarritos_M_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvCarritos_M.Rows[e.RowIndex];
            _idCarritoActual = Convert.ToInt32(fila.Cells["IdCarrito"].Value);

        }

        private void btnCrearElemento_M_Click(object sender, EventArgs e)
        {
            var CrearCarrito = new FormCRUDCarritos(carritosCN, MostrarDatos);

            CrearCarrito.Show();
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var formGestionarCarrito = new CarritoGestionUC(formPrincipal, this, carritosCN, _idCarritoActual, userVerificado, carritosBajas);
            formPrincipal.MostrarUserControl(formGestionarCarrito);
        }

        private void cmbEstado_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstado_B.SelectedValue == null) return;

            try
            {
                int idEstado = Convert.ToInt32(cmbEstado_B.SelectedValue);
                ActualizarDatagrid(idEstado);
            }
            catch (Exception)
            {
            }
        }

        private void dgvCarritos_M_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCarritos_M.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string? estado = e.Value.ToString();

                if (estado == "En reparacion")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 230, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Prestado")
                {
                    e.CellStyle.BackColor = Color.FromArgb(59, 130, 246);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Roto")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 150, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Faltantes")
                {
                    e.CellStyle.BackColor = Color.FromArgb(105, 80, 165);
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
}
