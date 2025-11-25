using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.MappersDTO;
using CapaDatos.Repos;
using CapaDTOs;
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

        public void ActualizarDatagrid()
        {
            dgvCarritos_M.DataSource = carritosCN.MostrarCarritos();
        }

        private void CarritoUC_Load(object sender, EventArgs e)
        {
            ActualizarDatagrid();

            RenovarDatos();

            dgvCarritos_M.Columns["IdCarrito"].HeaderText = "ID";
            dgvCarritos_M.Columns["IdCarrito"].Width = 40;
            dgvCarritos_M.Columns["IdCarrito"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCarritos_M.Columns["NumeroSerieCarrito"].HeaderText = "Serie";
            dgvCarritos_M.Columns["Capacidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCarritos_M.Columns["Capacidad"].Width = 80;
            dgvCarritos_M.Columns["EstadoMantenimiento"].HeaderText = "Estado";
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
            var CrearCarrito = new FormCRUDCarritos(carritosCN, ActualizarDatagrid);

            CrearCarrito.Show();
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var formGestionarCarrito = new CarritoGestionUC(formPrincipal, this, carritosCN, _idCarritoActual, userVerificado, carritosBajas);
            formPrincipal.MostrarUserControl(formGestionarCarrito);
        }
    }
}
