using CapaNegocio;
using CapaEntidad;
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
    public partial class FormDeshabilitarCNE : Form
    {
        private readonly CarritosCN _carritosCN;
        private readonly Action ActualizarDataGrid;
        private int _id;
        private int idUsuario;
        public FormDeshabilitarCNE(int id, CarritosCN carritosCN, int usuario, Action ActualizarDataGrid)
        {
            InitializeComponent();

            _carritosCN = carritosCN;
            this.ActualizarDataGrid = ActualizarDataGrid;
            _id = id;
            idUsuario = usuario;
        }

        private void FormDeshabilitarCNE_Load(object sender, EventArgs e)
        {
            cmbEstadoMantenimiento.DataSource = _carritosCN.ListarEstadosMatenimiento();
            cmbEstadoMantenimiento.ValueMember = "IdEstadoMantenimiento";
            cmbEstadoMantenimiento.DisplayMember = "EstadoMantenimientoNombre";
        }

        private void btnDeshabiliar_Click(object sender, EventArgs e)
        {
            Carritos? carrito = _carritosCN.ObtenerCarritoPorID(_id);

            _carritosCN.DeshabilitarCarrito(_id, (int)cmbEstadoMantenimiento.SelectedValue, idUsuario);
            ActualizarDataGrid.Invoke();
            this.Close();
        }
    }
}
