using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaDatos.InterfacesDTO;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        private readonly NotebooksCN notebooksCN;
        private readonly CarritosCN carritosCN;
        private Usuarios usuarioActual;
        private int IdActual = 0;

        public NotebooksUC(NotebooksCN notebooksCN, Usuarios user, CarritosCN carritosCN)
        {
            InitializeComponent();
            this.notebooksCN = notebooksCN;
            this.usuarioActual = user;
            this.carritosCN = carritosCN;
        }
        public void ActualizarDataGrid()
        {
            dtgNotebook.DataSource = notebooksCN.GetAll();

            cmbModelo.DataSource = notebooksCN.ListarModelosPorTipo(1);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";
        }

        private void NotebooksUC_Load(object sender, EventArgs e)
        {
            ActualizarDataGrid();
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1200);
        }

        private void btnAgregarNotebook_Click(object sender, EventArgs e)
        {
            //Action ActualizarGrid = ActualizarDataGrid; No es necesario crear esta variable temporal, ya que puedo pasar el metodo directamente

            var Notebook = new FormCRUDNotebook(notebooksCN, ActualizarDataGrid);
            Notebook.ShowDialog();
        }

        private void dtgNotebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Limpiar los labels, porque el operador += provoca problemas y concatena las palabras al tocar varias veces el cellclick.

            lblCarroAsignado.Text = "Carrito Asignado: ";
            lblCasillero.Text = "Casillero: ";
            lblIDNotebook.Text = "ID: ";

            if (e.RowIndex < 0) return;

            IdActual = Convert.ToInt32(dtgNotebook.Rows[e.RowIndex].Cells["IdNotebook"].Value);

            Notebooks? notebook = notebooksCN.ObtenerNotebookPorID(IdActual);

            lblIDNotebook.Text += notebook?.IdElemento.ToString();
            txtEquipo.Text = notebook?.Equipo;
            txtNroSerie.Text = notebook?.NumeroSerie;
            txtCodBarra.Text = notebook?.CodigoBarra;
            txtPatrimonio.Text = notebook?.Patrimonio;
            cmbModelo.SelectedValue = notebook?.IdModelo;
            cmbUbicacion.SelectedValue = notebook?.IdUbicacion;

            EstadosMantenimiento? estadosMantenimiento = notebooksCN.ObtenerEstadoMantenimientoPorID(notebook.IdEstadoMantenimiento);

            lblEstado.Text = estadosMantenimiento?.EstadoMantenimientoNombre;
            lblEstado.Tag = estadosMantenimiento?.IdEstadoMantenimiento;

            /* 
             * Corregi el problema del metodo ObtenerCarritoPorID ya que esta propiedad no admite null por eso debemos usar
             * el .HasValue para identificar si tiene o no carrito antes de invocar el metodo, si ves arriba de este sms tambien
             * hay otro metodo ObtenerEstadoMantenimientoPorID y seguro pienses que tambien necesita esto, pero a diferencia de esto
             * el metodo de arriba si apuntas el cursor ahi veras que dice "puede ser NULL aqui" lo que no pasa nada si no hacemos 
             * validaciones lo de abajo decia que un tipo que acepta valores nullos recibe un valor null, por eso ahi que verificar eso.
             */
            if (notebook?.IdCarrito.HasValue == true)
            {
                Carritos? carritos = notebooksCN.ObtenerCarritoPorID(notebook.IdCarrito.Value);

                if (carritos != null)
                {
                    lblCarroAsignado.Text += carritos.EquipoCarrito;
                    lblCasillero.Text += notebook.PosicionCarrito.ToString();
                }
                else
                {
                    lblCarroAsignado.Text += "Sin Carrito";
                    lblCasillero.Text += "-";
                }
            }
            else
            {
                lblCarroAsignado.Text += "Sin Carrito";
                lblCasillero.Text += "-";
            }

            #region Agregar cellClick para agregar al carrito
            txtEquipo_AddCarrito.Text += notebook?.Equipo;
            #endregion
        }

        private void btnActualizarNotebook_Click(object sender, EventArgs e)
        {
            Notebooks? notebooks = notebooksCN.ObtenerNotebookPorID(IdActual);

            Notebooks? notebook = new Notebooks
            {
                IdElemento = IdActual,
                Equipo = txtEquipo.Text,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                IdEstadoMantenimiento = (int)lblEstado.Tag,   //(int)cmbEstados.SelectedValue, use el TAG y force a que sea de tipo int
                IdTipoElemento = 1,
                IdVarianteElemento = null,
                IdCarrito = notebooks?.IdCarrito,
                PosicionCarrito = notebooks?.PosicionCarrito,
                Habilitado = true,
                FechaBaja = null
            };

            notebooksCN.ActualizarNotebook(notebook, usuarioActual.IdUsuario);
            ActualizarDataGrid();
        }

        private void btnDeshabiliar_Click(object sender, EventArgs e)
        {
            //notebooksCN.DeshabilitarNotebook(IdActual, usuarioActual.IdUsuario, (int)cmbEstados.SelectedValue);

            ActualizarDataGrid();
        }

        private void btnAgragarAlCarrito_Click(object sender, EventArgs e)
        {
            Carritos? carrito = carritosCN.ObtenerCarritoPorEquipo(txtCarrito_AddCarrito.Text);
            int posicion = Convert.ToInt32(txtPosicion.Text);

            if(carrito == null)
            {
                MessageBox.Show("El carrito no existe.");
                return;
            }

            carritosCN.AddNotebook(carrito.IdCarrito, posicion, IdActual, usuarioActual.IdUsuario);
            ActualizarDataGrid();
        }
    }
}
