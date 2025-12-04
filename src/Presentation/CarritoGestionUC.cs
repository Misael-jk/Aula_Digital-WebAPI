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
        private int _casilleroSeleccionado;
        private int? _idNotebookSeleccionada;
        private Guna.UI2.WinForms.Guna2TextBox? txtActivoParaFiltro = null;
        private int _idElementoEncontrado;
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
        }


        private void btnVolver_Click(object sender, EventArgs e)
        {
            if (acAnterior is CarritoUC carritoUC)
            {
                carritoUC.MostrarDatos();
            }

            formPrincipal.MostrarUserControl(acAnterior);
        }

        private void CarritoGestionUC_Load(object sender, EventArgs e)
        {

            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1100);

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

                GenerarBotonesCarrito(Convert.ToInt32(carrito?.Capacidad));
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

            var notebooks = carritosCN.ObtenerNotebooksPorCarrito(_idCarritoSeleccionado)
                                      .ToList();

            int posY = 3; // posición inicial en Y

            for (int i = 1; i <= capacidad; i++)
            {
                Notebooks? notebook = notebooks.FirstOrDefault(n => n.PosicionCarrito == i);

                Button btn = CrearBotonCasillero(i, notebook);

                // Ubicación vertical
                btn.Location = new Point(3, posY);

                // Siguiente botón  → abajo 46 px
                posY += 46;

                pnlContenedor.Controls.Add(btn);
            }
        }

        private Button CrearBotonCasillero(int numero, Notebooks? notebook)
        {
            Button btn = new Button();

            // Estilo base que pediste
            btn.Name = $"btnCasillero{numero}";
            btn.Size = new Size(362, 43);
            btn.Text = $"Casillero {numero}";
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;

            // Estado y color
            if (notebook == null)
            {
                btn.BackColor = SystemColors.InactiveCaption;   // vacío
                btn.Tag = null;
            }
            else
            {
                btn.Tag = notebook.IdElemento;

                switch (notebook.IdEstadoMantenimiento)
                {
                    case 1: // Disponible
                        btn.BackColor = Color.FromArgb(128, 255, 128);
                        break;

                    case 2: // Prestado
                        btn.BackColor = Color.FromArgb(252, 201, 52);
                        break;

                    default:
                        btn.BackColor = Color.LightGray;
                        break;
                }
            }

            btn.Click += Casillero_Click;

            return btn;
        }

        private void Casillero_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;

            _casilleroSeleccionado = int.Parse(btn.Text.Replace("Casillero ", ""));

            lblCasillero.Text = "Casillero " + _casilleroSeleccionado;
            lblCasillero.Visible = true;

            if (btn.Tag == null)
            {
                _idNotebookSeleccionada = null;
                MostrarPanelCasilleroVacio();
            }
            else
            {
                MostrarDatosNotebook(Convert.ToInt32(btn.Tag));
            }
        }

        private void MostrarDatosNotebook(int idNotebook)
        {
            pnlConNotebook.Visible = true;
            pnlAgregarNotebook.Visible = false;
            pnlCasilleroVacio.Visible = false;

            Notebooks? elemento = carritosCN.ObtenerNotebookPorID(idNotebook);

            _idNotebookSeleccionada = idNotebook;

            lblNroSerieNotebook.Text = elemento?.NumeroSerie;
            AplicarToolTip(lblNroSerieNotebook);

            lblCodBarraNotebook.Text = elemento?.CodigoBarra;
            AplicarToolTip(lblCodBarraNotebook);

            lblPatrimonioNotebook.Text = elemento?.Patrimonio;
            AplicarToolTip(lblPatrimonioNotebook);

            Modelos? modelo = carritosCN.ObtenerModeloPorID(Convert.ToInt32(elemento?.IdModelo));
            lblModeloNotebook.Text = modelo?.NombreModelo ?? "Sin modelo";
            AplicarToolTip(lblModeloNotebook);

            Notebooks? notebook = carritosCN.ObtenerNotebookPorID(Convert.ToInt32(elemento?.IdElemento));
            lblEquipoNotebook.Text = notebook?.Equipo ?? "Sin datos";
            AplicarToolTip(lblEquipoNotebook);

            if(notebook?.IdEstadoMantenimiento == 1)
            {
                pnlEstadoNotebook.FillColor = Color.FromArgb(128, 255, 128);
                lblEstadoNotebook.Text = "Disponible";
                btnSacarNotebook.Visible = true;
            }
            else
            {
                pnlEstadoNotebook.FillColor = Color.FromArgb(252, 201, 52);
                lblEstadoNotebook.Text = "En prestamo";
                btnSacarNotebook.Visible = false;
            }
        }

        private void MostrarPanelCasilleroVacio()
        {
            pnlConNotebook.Visible = false;
            pnlAgregarNotebook.Visible = false;
            pnlCasilleroVacio.Visible = true;

            lblCasilleroVacio.Text = "Casillero " + _casilleroSeleccionado + " esta vacio...";
        }

        private void btnAgregarNotebookCasillero_Click(object sender, EventArgs e)
        {
            pnlConNotebook.Visible = false;
            pnlAgregarNotebook.Visible = true;
            pnlCasilleroVacio.Visible = false;
        }

        private void btnAgregarNotebook_Click(object sender, EventArgs e)
        {
            carritosCN.AddNotebook(_idCarritoSeleccionado, _casilleroSeleccionado, _idElementoEncontrado, usuarios.IdUsuario);

            CargarTodaLaGestion(_idCarritoSeleccionado);

            MostrarDatosNotebook(_idElementoEncontrado);

        }

        private void txtNroSerieNotebook_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtNroSerieNotebook);
            txtActivoParaFiltro = txtNroSerieNotebook;
            ReiniciarTimerFiltro();
        }

        private void txtCodBarraNotebook_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtCodBarraNotebook);
            txtActivoParaFiltro = txtCodBarraNotebook;
            ReiniciarTimerFiltro();
        }

        private void txtPatrimonioNotebook_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtPatrimonioNotebook);
            txtActivoParaFiltro = txtPatrimonioNotebook;
            ReiniciarTimerFiltro();
        }

        private void ReiniciarTimerFiltro()
        {
            pnlDatosElemento.Visible = false;
            grbBusquedaNotebook.Visible = false;
            lblBuscando.Visible = true;
            lblNoEncontrado.Visible = false;
            tmrEsperarPausaTip2.Stop();
            tmrEsperarPausaTip2.Start();
        }

        private void ProcesarFiltro(Guna.UI2.WinForms.Guna2TextBox activo)
        {
            if (string.IsNullOrWhiteSpace(activo.Text))
            {
                CambiarDisponibilidadDatos(null);
                lblBuscando.Visible = false;
                lblNoEncontrado.Visible = true;
                grbBusquedaNotebook.Visible = false;
                pnlDatosElemento.Visible = false;
                return;
            }

            string? TextoNroSerie = activo == txtNroSerieNotebook ? activo.Text : null;
            string? TextCodBarra = activo == txtCodBarraNotebook ? activo.Text : null;
            string? TextPatrimonio = activo == txtPatrimonioNotebook ? activo.Text : null;

            var elemento = carritosCN.ObtenerPorSerieOCodBarraOPatrimonio(TextoNroSerie, TextCodBarra, TextPatrimonio);

            if (elemento == null)
            {
                grbBusquedaNotebook.Visible = false;
                pnlDatosElemento.Visible = false;
                lblBuscando.Visible = false;
                lblNoEncontrado.Visible = true;
                return;
            }

            grbBusquedaNotebook.Visible = true;

            _idElementoEncontrado = elemento.IdElemento;

            lblNroSerieE.Text = elemento.NumeroSerie;
            AplicarToolTip(lblNroSerieE);

            lblCodBarraE.Text = elemento.CodigoBarra;
            AplicarToolTip(lblCodBarraE);

            lblPatrimonioE.Text = elemento.Patrimonio;
            AplicarToolTip(lblPatrimonioE);

            Modelos? modelo = carritosCN.ObtenerModeloPorID(elemento.IdModelo);
            lblModeloE.Text = modelo?.NombreModelo ?? "Sin modelo";
            AplicarToolTip(lblModeloE);

            Notebooks? notebook = carritosCN.ObtenerNotebookPorID(elemento.IdElemento);
            lblEquipoE.Text = notebook?.Equipo ?? "Sin datos";
            AplicarToolTip(lblEquipoE);
        }

        private void CambiarDisponibilidadDatos(Guna.UI2.WinForms.Guna2TextBox? activo)
        {
            if (activo != null)
            {
                if (activo != txtNroSerieNotebook)
                {
                    txtNroSerieNotebook.Enabled = false;
                }
                else
                {
                    txtNroSerieNotebook.Enabled = true;
                }

                if (activo != txtCodBarraNotebook)
                {
                    txtCodBarraNotebook.Enabled = false;
                }
                else
                {
                    txtCodBarraNotebook.Enabled = true;
                }

                if (activo != txtPatrimonioNotebook)
                {
                    txtPatrimonioNotebook.Enabled = false;
                }
                else
                {
                    txtPatrimonioNotebook.Enabled = true;
                }
            }
            else
            {
                txtNroSerieNotebook.Enabled = true;
                txtCodBarraNotebook.Enabled = true;
                txtPatrimonioNotebook.Enabled = true;
            }
        }

        private void AplicarToolTip(Label label)
        {
            string texto = string.IsNullOrWhiteSpace(label.Text) ? "" : label.Text;
            tltDatos.SetToolTip(label, texto);
        }

        private void tmrEsperarPausaTip2_Tick_1(object sender, EventArgs e)
        {
            tmrEsperarPausaTip2.Stop();

            if (txtActivoParaFiltro != null)
            {
                ProcesarFiltro(txtActivoParaFiltro);
            }
        }

        private void btnSacarNotebook_Click(object sender, EventArgs e)
        {
            carritosCN.RemoveNotebook(_idCarritoSeleccionado, Convert.ToInt32(_idNotebookSeleccionada), usuarios.IdUsuario, 1);

            CargarTodaLaGestion(_idCarritoSeleccionado);

            MostrarPanelCasilleroVacio();
        }
    }
}
