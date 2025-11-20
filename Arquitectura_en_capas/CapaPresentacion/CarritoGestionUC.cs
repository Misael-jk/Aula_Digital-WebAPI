using CapaEntidad;
using CapaNegocio;
using Guna.UI2.WinForms;
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

                GenerarBotonesCarrito(32);
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
                descripcion.AppendLine("Modelo de carrito:");
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

        private void GenerarBotonesCarrito(int capacidad)
        {
            pnlContenedor.Controls.Clear();

            if (capacidad <= 0)
                return;

            // --- OPCIÓN RECOMENDADA ---
            // El contenedor debe ser UN FlowLayoutPanel
            // pnlContenedor.WrapContents = true;
            // pnlContenedor.FlowDirection = FlowDirection.LeftToRight;

            var notebooks = carritosCN.ObtenerNotebooksPorCarrito(_idCarritoSeleccionado)
                                      .ToList();

            for (int i = 1; i <= capacidad; i++)
            {
                var notebook = notebooks.FirstOrDefault(n => n.PosicionCarrito == i);

                var btn = CrearBotonCasillero(i, notebook);

                pnlContenedor.Controls.Add(btn);
            }
        }

        private Guna2Button CrearBotonCasillero(int numero, Notebooks notebook)
        {
            var btn = new Guna2Button();

            // ------------------------------
            // 🔹 Estilo base (TU diseño)
            // ------------------------------
            btn.BackColor = Color.Transparent;
            btn.BorderRadius = 8;
            btn.Cursor = Cursors.Hand;

            btn.DisabledState.BorderColor = Color.DarkGray;
            btn.DisabledState.CustomBorderColor = Color.DarkGray;
            btn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);

            btn.FillColor = Color.WhiteSmoke;
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btn.ForeColor = Color.Firebrick;

            btn.HoverState.FillColor = Color.FromArgb(235, 115, 125);
            btn.HoverState.ForeColor = Color.White;

            btn.PressedColor = Color.FromArgb(255, 170, 20);
            btn.ShadowDecoration.Color = Color.FromArgb(255, 200, 200);
            btn.ShadowDecoration.Enabled = true;
            btn.ShadowDecoration.Shadow = new Padding(2, 2, 4, 4);

            btn.Size = new Size(150, 42); // puedes ajustar
            btn.Margin = new Padding(5);
            btn.TextAlign = HorizontalAlignment.Left;

            // ------------------------------
            // 🔹 Texto
            // ------------------------------
            btn.Text = $"Casillero {numero}";

            // ------------------------------
            // 🔹 Icono según estado
            // ------------------------------
            if (notebook == null)
            {
                btn.FillColor = SystemColors.InactiveCaption;
                btn.Tag = null;
                btn.Image = Properties.Resources.iconVacio;  // <-- Pone icono vacío
            }
            else
            {
                btn.Tag = notebook.IdElemento; // solo ID como pediste

                switch (notebook.IdEstadoMantenimiento)
                {
                    case 1: // Disponible
                        btn.FillColor = Color.FromArgb(128, 255, 128);
                        btn.Image = Properties.Resources.iconDisponible;
                        break;

                    case 2: // Prestado
                        btn.FillColor = Color.FromArgb(252, 201, 52);
                        btn.Image = Properties.Resources.iconPrestado;
                        break;

                    default: // Otro estado
                        btn.FillColor = Color.WhiteSmoke;
                        break;
                }
            }

            // Icono a la izquierda
            btn.ImageAlign = HorizontalAlignment.Left;
            btn.ImageSize = new Size(24, 24);

            // Click
            btn.Click += Casillero_Click;

            return btn;
        }

        private void Casillero_Click(object sender, EventArgs e)
        {
            var btn = (Guna2Button)sender;

            int numeroCasillero = int.Parse(btn.Text.Replace("Casillero ", ""));

            if (btn.Tag == null)
            {
                MessageBox.Show($"El casillero {numeroCasillero} está vacío.");
                // lógica de agregar
                return;
            }

            int idNotebook = (int)btn.Tag;

            MessageBox.Show($"Notebook ID: {idNotebook} en casillero {numeroCasillero}");
            // lógica de editar / quitar
        }

    }
}
