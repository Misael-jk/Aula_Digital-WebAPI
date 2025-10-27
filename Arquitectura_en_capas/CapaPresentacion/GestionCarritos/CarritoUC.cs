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
        private readonly CarritosCN carritosCN;
        private List<Button> botonesCarrito;
        private int _idCarritoActual = 0;
        private int posicion;
        private readonly Usuarios userVerificado;

        public CarritoUC(CarritosCN carritosCN, Usuarios userVerificado)
        {
            InitializeComponent();

            this.carritosCN = carritosCN;
            this.userVerificado = userVerificado;

        }

        public void ActualizarDatagrid()
        {
            dtgCarrito.DataSource = carritosCN.MostrarCarritos();
        }


        private void CarritoUC_Load(object sender, EventArgs e)
        {
            dtgCarrito.DataSource = carritosCN.MostrarCarritos();



            CambiarDisponibilidadDatos(false, false, false);

            CambiarDisponibilidadBotones(false, false);

            RenovarDatos();

            cmbModelo.DataSource = carritosCN.ListarModelosPorTipo(2); // Cambiar luego al modelo segun su tipo (ListarModelosPorTipo)
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = carritosCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";
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
            lblIDCarrito.Text = "ID: ";
            lblFechaModificacion.Text = "Fecha de modificacion: ";
            lblCapacidad.Text = "Capacidad: ";
            lblOcupados.Text = "Notebooks: ";

            if (e.RowIndex < 0) return;

            var fila = dtgCarrito.Rows[e.RowIndex];
            _idCarritoActual = Convert.ToInt32(fila.Cells["IdCarrito"].Value);

            Carritos? carrito = carritosCN.ObtenerCarritoPorID(_idCarritoActual);

            lblIDCarrito.Text += carrito?.IdCarrito;
            txtEquipoCarrito.Text = carrito?.EquipoCarrito;
            txtNroSerieCarrito.Text = carrito?.NumeroSerieCarrito;
            cmbModelo.SelectedValue = carrito?.IdModelo;
            cmbUbicacion.SelectedValue = carrito?.IdUbicacion;
            lblCapacidad.Text += carrito?.Capacidad;

            EstadosMantenimiento? estadosMantenimiento = carritosCN.ObtenerEstadoMantenimientoPorID(carrito.IdEstadoMantenimiento);

            lblEstado.Text = estadosMantenimiento?.EstadoMantenimientoNombre;
            lblEstado.Tag = estadosMantenimiento?.IdEstadoMantenimiento;

            if (estadosMantenimiento?.IdEstadoMantenimiento == 1)
            {
                ptbEstado.Image = Properties.Resources.disponibleIcon;
            }
            else
            {
                ptbEstado.Image = Properties.Resources.prestadoIcon;
            }

            int CantidadNotebooks = carritosCN.ObtenerCantidadPorCarrito(_idCarritoActual);

            lblOcupados.Text += CantidadNotebooks + "/" + carrito?.Capacidad;

            HistorialCambios? historialCambios = carritosCN.ObtenerUltimaFechaDeModiciacionPorID(_idCarritoActual);

            lblFechaModificacion.Text += historialCambios?.FechaCambio;

            // Genera dinámicamente los botones según la capacidad del carrito
            GenerarBotonesCarrito(carrito.Capacidad);
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
            Action _Actualizardatagrid = ActualizarDatagrid;

            var CrearCarrito = new FormCRUDCarritos(carritosCN, _Actualizardatagrid);

            CrearCarrito.Show();
        }

        private void GenerarBotonesCarrito(int capacidad)
        {
            pnlContenedorCasilleros1.Controls.Clear();
            pnlContenedorCasilleros2.Controls.Clear();
            botonesCarrito = new List<Button>();

            if (capacidad <= 0)
            {
                Console.WriteLine("Capacidad inválida o vacía. No se generaron botones.");
                return;
            }

            pnlContenedorCasilleros1.AutoScroll = false;
            pnlContenedorCasilleros2.AutoScroll = false;

            int arriba = (capacidad + 1) / 2;
            int abajo = capacidad - arriba;

            int panelWidth = pnlContenedorCasilleros1.Width;
            int panelHeight = pnlContenedorCasilleros1.Height;

            int margenX = 8;
            int margenY = 6;
            int espacio = 5;

            // Calcular ancho y alto según el tamaño del panel
            int ancho = (panelWidth - (margenX * 2) - (espacio * (arriba - 1))) / arriba;
            int alto = panelHeight - 15; // Reducido 15 px

            var notebooks = carritosCN.ObtenerNotebooksPorCarrito(_idCarritoActual);

            // Fila superior
            for (int i = 0; i < arriba; i++)
            {
                int numero = i + 1;
                var nb = notebooks.FirstOrDefault(n => n.PosicionCarrito == numero);
                Button btn = CrearBotonCasillero(numero, ancho, alto, nb);
                btn.Location = new Point(margenX + i * (ancho + espacio), margenY);
                pnlContenedorCasilleros1.Controls.Add(btn);
                botonesCarrito.Add(btn);
            }

            // Fila inferior
            for (int i = 0; i < abajo; i++)
            {
                int numero = arriba + i + 1;
                var nb = notebooks.FirstOrDefault(n => n.PosicionCarrito == numero);
                Button btn = CrearBotonCasillero(numero, ancho, alto, nb);
                btn.Location = new Point(margenX + i * (ancho + espacio), margenY);
                pnlContenedorCasilleros2.Controls.Add(btn);
                botonesCarrito.Add(btn);
            }

            Console.WriteLine($"Generados {botonesCarrito.Count} botones para el carrito {_idCarritoActual}");
        }

        private Button CrearBotonCasillero(int numero, int ancho, int alto, Notebooks notebook = null)
        {
            Button btn = new Button
            {
                Size = new Size(ancho, alto),
                Text = numero.ToString(),
                TextAlign = ContentAlignment.BottomCenter,
                UseVisualStyleBackColor = true,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                Tag = notebook
            };

            btn.BackColor = notebook == null ? Color.LightGray : notebook.IdEstadoMantenimiento switch
            {
                1 => Color.Green,
                2 => Color.Orange,
                3 => Color.Red,
                _ => SystemColors.Control
            };

            btn.Click += BotonNotebook_Click;

            return btn;
        }


        private void BotonNotebook_Click(object sender, EventArgs e)
        {
            if (_idCarritoActual == 0)
            {
                MessageBox.Show("Primero seleccioná un carrito.");
                return;
            }

            Button btn = sender as Button;
            Notebooks notebook = btn.Tag as Notebooks;
            posicion = int.Parse(btn.Text);

            if (notebook == null)
            {
                MessageBox.Show($"Casillero {btn.Text} vacío en el carrito.");
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

        private void btnActualizarCarrito_Click(object sender, EventArgs e)
        {
            Carritos carritos = new Carritos()
            {
                IdCarrito = _idCarritoActual,
                EquipoCarrito = txtEquipoCarrito.Text,
                NumeroSerieCarrito = txtNroSerieCarrito.Text,
                Capacidad = 32,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                //IdEstadoMantenimiento = (int)cmbEstadoMantenimientoCarrito.SelectedValue,
                Habilitado = true,
                FechaBaja = null
            };

            carritosCN.ActualizarCarrito(carritos, userVerificado.IdRol);

            ActualizarDatagrid();
        }

        private void btnDeshabiliarCarrito_Click(object sender, EventArgs e)
        {
            var formDeshabiltar = new FormDeshabilitarCNE(_idCarritoActual, carritosCN, userVerificado.IdRol);
            formDeshabiltar.Show();
        }
    }
}
