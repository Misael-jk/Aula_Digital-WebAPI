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
    public partial class CarritoGestionUC : UserControl
    {
        private readonly FormPrincipal formPrincipal;
        private readonly UserControl acAnterior;
        private readonly CarritosCN carritosCN;
        private readonly CarritosBajasCN carritosBajasCN;
        private int _idCarritoSeleccionado;
        private int _idHistorialSeleccionado;
        private Usuarios usuarios;
        private bool _cargando = false;

        #region GUARDANDO DATOS EN VARIABLES

        private string? _Equipo = "";
        private string? _NroSerie = "";
        private bool? _habilitado = true;
        private int? _idUbicacion = 0;
        private int? _idEstado = 0;
        private int? _idModelo = 0;
        private string? _NombreModelo = "";
        private string? _NombreUbicacion = "";
        private string? _NombreEstado = "";
        #endregion
        public CarritoGestionUC(FormPrincipal formPrincipal, UserControl acAnterior, CarritosCN carritosCN, int idCarritoSeleccionado, Usuarios user, CarritosBajasCN carritosBajasCN)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;
            this.acAnterior = acAnterior;
            this.carritosCN = carritosCN;
            _idCarritoSeleccionado = idCarritoSeleccionado;
            usuarios = user;
            this.carritosBajasCN = carritosBajasCN;

            txtEquipo.TextChanged += VerificarCambios;
            txtNroSerie.TextChanged += VerificarCambios;
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            if (acAnterior is CarritoUC carritoUC)
            {
                carritoUC.ActualizarDatagrid();
            }

            formPrincipal.MostrarUserControl(acAnterior);
        }

        private void CarritoGestionUC_Load(object sender, EventArgs e)
        {

            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1100);

            circleButton2.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            circleButton1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            CargarTodaLaGestion(_idCarritoSeleccionado);

            dgvHistorial.Columns["IdHistorialCarrito"].HeaderText = "ID";
            dgvHistorial.Columns["IdHistorialCarrito"].Width = 40;
            dgvHistorial.Columns["IdHistorialCarrito"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void CargarTodaLaGestion(int idCarrito)
        {
            _cargando = true;

            try
            {
                dgvHistorial.DataSource = carritosCN.ObtenerHistorialPorID(_idCarritoSeleccionado);

                Carritos? carrito = carritosCN.ObtenerCarritoPorID(_idCarritoSeleccionado);

                _Equipo = carrito?.EquipoCarrito;
                _NroSerie = carrito?.NumeroSerieCarrito;
                _habilitado = carrito?.Habilitado;
                _idUbicacion = carrito?.IdUbicacion;
                _idEstado = carrito?.IdEstadoMantenimiento;
                _idModelo = carrito?.IdModelo;

                CargarComboboxes();

                CargarDatos();

                _NombreModelo = cmbModelo.Text;
                _NombreUbicacion = cmbUbicacion.Text;
                _NombreEstado = cmbEstado.Text;

                HabilitarModificado(Convert.ToBoolean(_habilitado));

                HabilitarBotones(false, false);
            }
            finally
            {
                _cargando = false;
            }
        }

        private void CargarDatos()
        {
            lblCapacidad.Text = "Capacidad: ";
            lblOcupados.Text = "Notebooks: ";
            Carritos? carrito = carritosCN.ObtenerCarritoPorID(_idCarritoSeleccionado);

            txtEquipo.Text = _Equipo ?? "";
            txtNroSerie.Text = _NroSerie ?? "";

            if (_idUbicacion.HasValue)
                cmbUbicacion.SelectedValue = _idUbicacion.Value;
            else
                cmbUbicacion.SelectedIndex = -1;

            if (_idEstado.HasValue)
                cmbEstado.SelectedValue = _idEstado.Value;
            else
                cmbEstado.SelectedIndex = -1;

            if (_idModelo.HasValue)
                cmbModelo.SelectedValue = _idModelo.Value;
            else
                cmbModelo.SelectedIndex = -1;

            int CantidadNotebooks = carritosCN.ObtenerCantidadPorCarrito(_idCarritoSeleccionado);

            lblOcupados.Text += CantidadNotebooks + "/" + carrito?.Capacidad;
            lblCapacidad.Text += carrito?.Capacidad.ToString(); 
        }

        private void CargarComboboxes()
        {
            cmbUbicacion.DataSource = carritosCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";

            cmbModelo.DataSource = carritosCN.ListarModelosPorTipo(2);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            if (_idEstado == 2)
            {
                cmbEstado.DataSource = carritosCN.ListarEstadosMatenimiento();
                cmbEstado.ValueMember = "IdEstadoMantenimiento";
                cmbEstado.DisplayMember = "EstadoMantenimientoNombre";
            }
            else
            {
                cmbEstado.DataSource = carritosCN.ListarEstadoParaActualizar();
                cmbEstado.ValueMember = "IdEstadoMantenimiento";
                cmbEstado.DisplayMember = "EstadoMantenimientoNombre";
            }
        }

        private void dgvHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvHistorial.Rows[e.RowIndex];

            _idHistorialSeleccionado = Convert.ToInt32(fila.Cells["IdHistorialCarrito"].Value);

            HistorialCambios? historialCambios = carritosCN.ObtenerHistorialPorIDHistorial(_idHistorialSeleccionado);

            txtMotivo.Text = historialCambios?.Motivo;
            txtDescripcion.Text = historialCambios?.Descripcion;
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var carrito = carritosCN.ObtenerCarritoPorID(_idCarritoSeleccionado);

            var Carrito = new Carritos
            {
                IdCarrito = _idCarritoSeleccionado,
                EquipoCarrito = txtEquipo.Text,
                NumeroSerieCarrito = txtNroSerie.Text,
                IdModelo = Convert.ToInt32(cmbModelo.SelectedValue),
                IdUbicacion = Convert.ToInt32(cmbUbicacion.SelectedValue),
                IdEstadoMantenimiento = Convert.ToInt32(cmbEstado.SelectedValue),
                Capacidad = carrito?.Capacidad ?? 0,
                Habilitado = Convert.ToBoolean(_habilitado),
                FechaBaja = null
            };

            if (string.IsNullOrEmpty(txtExplicarMotivo.Text))
            {
                txtExplicarMotivo.Text = "No especifico motivo";
            }

            string descripcionCambios = GenerarDescripcionDeCambios();
            MessageBox.Show(descripcionCambios, "Cambios detectados", MessageBoxButtons.OK, MessageBoxIcon.Information);

            carritosCN.ActualizarCarrito(Carrito, usuarios.IdUsuario, txtExplicarMotivo.Text, descripcionCambios);
            CargarTodaLaGestion(_idCarritoSeleccionado);
        }

        private void VerificarCambios(object sender, EventArgs e)
        {
            if (_cargando) return;

            bool huboCambios =
            txtEquipo.Text != (_Equipo ?? "") ||
            txtNroSerie.Text != (_NroSerie ?? "") ||
            (cmbModelo.SelectedValue != null && Convert.ToInt32(cmbModelo.SelectedValue) != _idModelo) ||
            (cmbUbicacion.SelectedValue != null && Convert.ToInt32(cmbUbicacion.SelectedValue) != _idUbicacion) ||
            (cmbEstado.SelectedValue != null && Convert.ToInt32(cmbEstado.SelectedValue) != _idEstado);

            HabilitarBotones(huboCambios, huboCambios);
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void cmbUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void HabilitarBotones(bool actu, bool rest)
        {
            btnGestionarElemento_M.Enabled = actu;
            btnRestablecer.Enabled = rest;
        }

        private void btnRestablecer_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idCarritoSeleccionado);
        }

        private string GenerarDescripcionDeCambios()
        {
            StringBuilder descripcion = new StringBuilder();
            descripcion.AppendLine("Se realizaron los siguientes cambios:");
            descripcion.AppendLine();

            if (txtEquipo.Text != _Equipo)
            {
                descripcion.AppendLine("Nombre de quipo:");
                descripcion.AppendLine($"   Antes: {_Equipo}");
                descripcion.AppendLine($"   Ahora: {txtEquipo.Text}");
                descripcion.AppendLine();
            }

            if (txtNroSerie.Text != _NroSerie)
            {
                descripcion.AppendLine("Número de serie:");
                descripcion.AppendLine($"   Antes: {_NroSerie}");
                descripcion.AppendLine($"   Ahora: {txtNroSerie.Text}");
                descripcion.AppendLine();
            }

            if (Convert.ToInt32(cmbModelo.SelectedValue) != _idModelo)
            {
                descripcion.AppendLine("Modelo de notebook:");
                descripcion.AppendLine($"   Antes: {_NombreModelo}");
                descripcion.AppendLine($"   Ahora: {cmbModelo.Text}");
                descripcion.AppendLine();
            }

            if (Convert.ToInt32(cmbUbicacion.SelectedValue) != _idUbicacion)
            {
                descripcion.AppendLine("Ubicación:");
                descripcion.AppendLine($"   Antes: {_NombreUbicacion}");
                descripcion.AppendLine($"   Ahora: {cmbUbicacion.Text}");
                descripcion.AppendLine();
            }

            if (Convert.ToInt32(cmbEstado.SelectedValue) != _idEstado)
            {
                descripcion.AppendLine("Estado de mantenimiento:");
                descripcion.AppendLine($"   Antes: {_NombreEstado}");
                descripcion.AppendLine($"   Ahora: {cmbEstado.Text}");
                descripcion.AppendLine();
            }

            if (descripcion.Length == "Se realizaron los siguientes cambios:\r\n\r\n".Length)
            {
                descripcion.Clear();
                descripcion.Append("No se detectaron cambios en los datos principales.");
            }

            return descripcion.ToString();
        }

        private void HabilitarModificado(bool Habilitar)
        {
            cmbModelo.Enabled = Habilitar;
            cmbUbicacion.Enabled = Habilitar;
            cmbEstado.Enabled = Habilitar;
            txtNroSerie.Enabled = Habilitar;
            txtEquipo.Enabled = Habilitar;
            txtExplicarMotivo.Text = "";

            btnGestionarElemento_M.Visible = Habilitar;
            btnRestablecer.Visible = Habilitar;
            btnDeshabilitar.Visible = Habilitar;

            btnHabilitar.Visible = !Habilitar;
            btnConfirmar.Visible = !Habilitar;
            btnCancelar.Visible = !Habilitar;
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            btnConfirmar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            btnConfirmar.Enabled = true;
            btnCancelar.Enabled = true;

            btnConfirmar.Visible = true;
            btnCancelar.Visible = true;
            btnGestionarElemento_M.Visible = false;
            btnRestablecer.Visible = false;

            cmbModelo.Enabled = false;
            cmbUbicacion.Enabled = false;
            cmbEstado.Enabled = false;
            txtNroSerie.Enabled = false;
            txtEquipo.Enabled = false;

            CargarDatos();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_habilitado == true)
            {
                carritosCN.DeshabilitarCarrito(_idCarritoSeleccionado, txtExplicarMotivo.Text, usuarios.IdUsuario);
                CargarTodaLaGestion(_idCarritoSeleccionado);
            }
            else
            {
                carritosBajasCN.HabilitarCarrito(_idCarritoSeleccionado, usuarios.IdUsuario, txtExplicarMotivo.Text);
                CargarTodaLaGestion(_idCarritoSeleccionado);
            }

            btnConfirmar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idCarritoSeleccionado);
        }
    }
}
