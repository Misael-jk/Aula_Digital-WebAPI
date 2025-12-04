using CapaDatos.InterfacesDTO;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class PrestamosYDevolucionesUC : UserControl
    {
        private readonly IMapperTransaccion mapperTransaccion;
        private readonly FormPrincipal _formPrincipal;
        private readonly PrestamosCN prestamosCN;
        private readonly DevolucionCN devolucionCN;
        private Usuarios userActual;
        private int _idPrestamoSeleccionado = 0;
        public PrestamosYDevolucionesUC(IMapperTransaccion mapperTransaccion, FormPrincipal formPrincipal, PrestamosCN prestamosCN, DevolucionCN devolucionCN, Usuarios usuarios)
        {
            InitializeComponent();
            this.mapperTransaccion = mapperTransaccion;
            this._formPrincipal = formPrincipal;
            this.prestamosCN = prestamosCN;
            this.devolucionCN = devolucionCN;
            this.userActual = usuarios;
        }

        private void PrestamosYDevolucionesUC_Load(object sender, EventArgs e)
        {
            if (userActual.IdRol == 3)
            {
                btnCrearNotebook_M.Enabled = false;
                btnActualizarPrestamos.Enabled = false;
                btnDevoluciones.Enabled = false;
            }

            ActualizarDataGrid();
        }

        public void ActualizarDataGrid()
        {
            dvgPrestamosYDevoluciones.DataSource = mapperTransaccion.GetAllDTO();
        }

        private void btnCrearNotebook_M_Click(object sender, EventArgs e)
        {
            var prestamosUC = new CrearPrestamosUC(this, _formPrincipal, prestamosCN, userActual);
            _formPrincipal.MostrarUserControl(prestamosUC);
        }

        private void dvgPrestamosYDevoluciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _idPrestamoSeleccionado = Convert.ToInt32(dvgPrestamosYDevoluciones.Rows[e.RowIndex].Cells["IdPrestamo"].Value);
        }

        private void btnDevoluciones_Click(object sender, EventArgs e)
        {
            var devolucionesUC = new DevolucionGestionUC(_formPrincipal, prestamosCN, userActual, devolucionCN, _idPrestamoSeleccionado, this);
            _formPrincipal.MostrarUserControl(devolucionesUC);
        }

        private void btnActualizarPrestamos_Click(object sender, EventArgs e)
        {
            ActualizarDataGrid();
        }
    }
}
