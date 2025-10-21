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
using CapaDatos.InterfacesDTO;

namespace CapaPresentacion
{
    public partial class FormCRUDCarritos : Form
    {
        private UserControl carritoUC;
        private readonly CarritosCN carritosCN;
        private readonly IMapperModelo mapperModelos;
        public FormCRUDCarritos(CarritosCN carritosCN, IMapperModelo mapperModelo, UserControl uc)
        {
            InitializeComponent();
            this.mapperModelos = mapperModelo;
            this.carritosCN = carritosCN;
            carritoUC = uc;
            
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCRUDCarritos_Load(object sender, EventArgs e)
        {
            cmbModelo.DataSource = mapperModelos.GetAll();
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "Modelo";

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
                Habilitado = true,
                FechaBaja = null
            };

            carritosCN.CrearCarrito(carrito, 1);
        }
    }
}
