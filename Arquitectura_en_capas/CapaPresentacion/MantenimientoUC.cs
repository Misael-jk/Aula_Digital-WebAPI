using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CapaPresentacion
{
    public partial class MantenimientoUC : UserControl
    {
        //private readonly MantenimientoCN mantenimientoCN;
        private readonly CarritosBajasCN carritosBajasCN;
        private readonly ElementosBajasCN elementosBajasCN;
        public MantenimientoUC(CarritosBajasCN carritosBajasCN, ElementosBajasCN elementosBajasCN)
        {
            InitializeComponent();
            //this.mantenimientoCN = mantenimientoCN;
            this.carritosBajasCN = carritosBajasCN;
            this.elementosBajasCN = elementosBajasCN;
        }

        public void MostrarDatos()
        {
            dtgMantenimientoCarrito.DataSource = carritosBajasCN.GetAllDTO();
            h.DataSource = elementosBajasCN.GetAllElementos(); 
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            //mantenimientoCN.HabilitarElemento(txtNroSerie.Text);
            MostrarDatos();
        }

        private void dgvMantenimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void MantenimientoUC_Load(object sender, EventArgs e)
        {

        }
    }
}
