using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.MappersDTO;
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

namespace CapaPresentacion
{
    public partial class CarritoUC : UserControl
    {
        private readonly CarritosCN carritosCN;
        private readonly IMapperModelo mapperModelo;
        private List<Button> botonesCarrito;
        private int _idCarritoActual = 0;
        private int posicion;
        private readonly Usuarios userVerificado;

        public CarritoUC(CarritosCN carritosCN, Usuarios userVerificado, IMapperModelo mapperModelo)
        {
            InitializeComponent();

            this.carritosCN = carritosCN;
            this.userVerificado = userVerificado;
            this.mapperModelo = mapperModelo;

        }


        private void CarritoUC_Load(object sender, EventArgs e)
        {
            dtgCarrito.DataSource = carritosCN.MostrarCarritos();

            CambiarDisponibilidadDatos(false, false, false);

            CambiarDisponibilidadBotones(false, false);

            RenovarDatos();

            botonesCarrito = new List<Button>
            {
                btnNotebook1, btnNotebook2, btnNotebook3, btnNotebook4, btnNotebook5,
                btnNotebook6, btnNotebook7, btnNotebook8, btnNotebook9, btnNotebook10,
                btnNotebook11, btnNotebook12, btnNotebook13, btnNotebook14, btnNotebook15,
                btnNotebook16, btnNotebook17, btnNotebook18, btnNotebook19, btnNotebook20,
                btnNotebook21, btnNotebook22, btnNotebook23, btnNotebook24, btnNotebook25
            };

            IEnumerable<EstadosMantenimiento> todo = carritosCN.ListarEstadosMatenimiento();
            cmbEstados.DataSource = todo;
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";

            foreach (var btn in botonesCarrito)
            {
                btn.Click += btnNotebook_Click;
            }
        }

        private void RenovarDatos()
        {
            var numSeriesBD = carritosCN.ObtenerSeriePorNotebook();

            string[] numSeries = numSeriesBD.Select(p => p.NumeroSerie).ToArray();

            var lista = new AutoCompleteStringCollection();

            lista.AddRange(numSeries);

            txtNroSerie.AutoCompleteCustomSource = lista;

            var codBarraBD = carritosCN.ObtenerCodBarraPorNotebook();

            string[] codBarras = codBarraBD.Select(p => p.CodigoBarra).ToArray();

            var lista2 = new AutoCompleteStringCollection();

            lista2.AddRange(codBarras);

            txtCodBarra.AutoCompleteCustomSource = lista2;
        }

        private void dtgCarrito_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dtgCarrito.Rows[e.RowIndex];
            _idCarritoActual = Convert.ToInt32(fila.Cells["IdCarrito"].Value);

            var notebooks = carritosCN.ObtenerNotebooksPorCarrito(_idCarritoActual);

            for (int i = 0; i < botonesCarrito.Count; i++)
            {
                var boton = botonesCarrito[i];

                boton.Tag = i + 1;

                var nb = notebooks.FirstOrDefault(n => n.PosicionCarrito == i + 1);

                if (nb == null)
                {
                    boton.BackColor = Color.LightGray;
                }
                else
                {
                    switch (nb.IdEstadoMantenimiento)
                    {
                        case 1: boton.BackColor = Color.Green; break; // Disponible
                        case 2: boton.BackColor = Color.Orange; break; // Mantenimiento
                        case 3: boton.BackColor = Color.Red; break; // Prestado
                        default: boton.BackColor = SystemColors.Control; break;
                    }
                }
            }

            RestaurarValores();
        }

        private void btnNotebook_Click(object sender, EventArgs e)
        {
            if (_idCarritoActual == 0)
            {
                MessageBox.Show("Primero seleccioná un carrito.");
                return;
            }

            var boton = (Button)sender;

            posicion = (int)boton.Tag;

            var notebook = carritosCN.ObtenerNotebookPorPosicion(_idCarritoActual, posicion);

            if (notebook == null)
            {
                MessageBox.Show($"Casillero {posicion} vacío en el carrito.");
                CambiarDisponibilidadBotones(true, false);
                CambiarDisponibilidadDatos(true, true, true);
                RestaurarValores();
                return;
            }

            txtNroSerie.Text = notebook.NumeroSerie;
            txtCodBarra.Text = notebook.CodigoBarra;
            cmbEstados.SelectedValue = notebook.IdEstadoMantenimiento;

            CambiarDisponibilidadBotones(false, true);
            CambiarDisponibilidadDatos(false, false, true);
        }

        private void CambiarDisponibilidadBotones(bool Estado, bool Estado1)
        {
            btnAgregar.Enabled = Estado;
            btnQuitar.Enabled = Estado1;
        }

        private void RestaurarValores()
        {
            txtNroSerie.Clear();
            txtCodBarra.Clear();
            cmbEstados.SelectedIndex = 0;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Notebooks? notebook = carritosCN.ObtenerPorSerie(txtNroSerie.Text);
            int idNotebook = notebook.IdElemento;

            carritosCN.AddNotebook(_idCarritoActual, posicion, idNotebook, userVerificado.IdUsuario);

            dtgCarrito_CellClick(this, new DataGridViewCellEventArgs(0, dtgCarrito.CurrentCell.RowIndex));

            RestaurarValores();

            CambiarDisponibilidadBotones(false, false);

            CambiarDisponibilidadDatos(false, false, false);

            RenovarDatos();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            Notebooks? notebook = carritosCN.ObtenerPorSerie(txtNroSerie.Text);
            int idNotebook = notebook.IdElemento;

            //carritosCN.RemoveNotebook(_idCarritoActual, idNotebook, userVerificado.IdUsuario, );

            dtgCarrito_CellClick(this, new DataGridViewCellEventArgs(0, dtgCarrito.CurrentCell.RowIndex));

            RestaurarValores();

            CambiarDisponibilidadBotones(false, false);

            CambiarDisponibilidadDatos(false, false, false);

            RenovarDatos();
        }

        private void txtNroSerie_TextChanged(object sender, EventArgs e)
        {
            if (!txtNroSerie.Focused)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNroSerie.Text))
            {
                return;
            }

            var datos = carritosCN.ObtenerPorSerieOCodBarra(txtNroSerie.Text, null);

            if (datos != null)
            {
                txtCodBarra.Text = datos.CodigoBarra;
                cmbEstados.SelectedValue = datos.IdEstadoMantenimiento;
                CambiarDisponibilidadDatos(true, false, false);

            }
            else
            {
                txtCodBarra.Clear();
                cmbEstados.SelectedIndex = 0;
                CambiarDisponibilidadDatos(true, true, true);
            }
        }

        private void CambiarDisponibilidadDatos(bool Estado1, bool Estado2, bool Estado3)
        {
            txtNroSerie.Enabled = Estado1;
            txtCodBarra.Enabled = Estado2;
            cmbEstados.Enabled = Estado3;
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
        {
            if (!txtCodBarra.Focused)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCodBarra.Text))
            {
                return;
            }

            var datos = carritosCN.ObtenerPorSerieOCodBarra(null, txtCodBarra.Text);

            if (datos != null)
            {
                txtNroSerie.Text = datos.NumeroSerie;
                cmbEstados.SelectedValue = datos.IdEstadoMantenimiento;
                CambiarDisponibilidadDatos(false, true, false);
            }
            else
            {
                txtNroSerie.Clear();
                cmbEstados.SelectedIndex = 0;
                CambiarDisponibilidadDatos(true, true, true);
            }
        }

        private void btnAddCarrito_Click(object sender, EventArgs e)
        {
            var CrearCarrito = new FormCRUDCarritos(carritosCN, mapperModelo);
            CrearCarrito.Show();
        }
    }
}
