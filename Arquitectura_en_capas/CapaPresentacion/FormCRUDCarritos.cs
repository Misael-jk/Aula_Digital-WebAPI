using System;
using System.Collections.Generic;
using CapaNegocio;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class FormCRUDCarritos : Form
    {
        private readonly CarritosCN carritosCN;
        public FormCRUDCarritos(CarritosCN carritosCN)
        {
            InitializeComponent();

            this.carritosCN = carritosCN;
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCRUDCarritos_Load(object sender, EventArgs e)
        {
            cmbModelo.DataSource = carritosCN.ListarModelosCarritos();
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = carritosCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";
        }

        private void btnCrearCarrito_Click(object sender, EventArgs e)
        {
            var carrito = new Carritos()
            {
                EquipoCarrito = txtEquipo.Text,
                NumeroSerieCarrito = txtNroSerie.Text,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                IdEstadoMantenimiento = 1,
            };

            carritosCN.CrearCarrito(carrito, 1);

            MessageBox.Show("Carrito creado correctamente");

            this.Close();
        }
    }
}
