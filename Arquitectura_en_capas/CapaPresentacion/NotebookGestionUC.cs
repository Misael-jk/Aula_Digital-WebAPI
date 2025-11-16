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
    public partial class NotebookGestionUC : UserControl
    {
        private readonly FormPrincipal formPrincipal;
        private readonly NotebooksUC notebooksUC;
        private readonly NotebooksCN notebooksCN;
        private readonly NotebookBajasCN notebookBajasCN;
        private int _idNotebookSeleccionado;
        private int _idHistorialSeleccionado;
        private Usuarios usuarios;
        private bool _cargando = false;

        #region GUARDANDO DATOS EN VARIABLES

        private string? _Equipo = "";
        private string? _NroSerie = "";
        private string? _CodBarra = "";
        private string? _Patrimonio = "";
        private bool? _habilitado = true;
        private int? _idUbicacion = 0;
        private int? _idEstado = 0;
        private int? _idTipo = 0;
        private int? _idModelo = 0;
        private string? _NombreModelo = "";
        private string? _NombreUbicacion = "";
        private string? _NombreEstado = "";
        private string? _Carrito = "";
        private int? _Casillero = 0;
        #endregion

        public NotebookGestionUC(FormPrincipal formPrincipal, NotebooksUC notebooksUC, NotebooksCN notebooksCN, int idNotebookSeleccionado, Usuarios user, NotebookBajasCN notebookBajasCN)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;
            this.notebooksUC = notebooksUC;
            this.notebooksCN = notebooksCN;
            this._idNotebookSeleccionado = idNotebookSeleccionado;
            this.usuarios = user;
            this.notebookBajasCN = notebookBajasCN;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            notebooksUC.ActualizarDataGrid(0);
            formPrincipal.MostrarUserControl(notebooksUC);
        }

        private void NotebookGestionUC_Load(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idNotebookSeleccionado);

            if(_Carrito is not null && _Casillero is not null)
            {
                cmbUbicacion.Enabled = false;
            }

            if (_idEstado == 2)
            {
                cmbEstado.Enabled = false;
            }
        }

        private void CargarTodaLaGestion(int idElemento)
        {
            _cargando = true;

            try
            {
                dgvHistorial.DataSource = notebooksCN.ObtenerHistorialPorID(_idNotebookSeleccionado);

                Notebooks? notebooks = notebooksCN.ObtenerNotebookPorID(_idNotebookSeleccionado);
                var carrito = notebooksCN.ObtenerCarritoPorNotebook(_idNotebookSeleccionado);

                _Equipo = notebooks?.Equipo;
                _idTipo = notebooks?.IdTipoElemento;
                _NroSerie = notebooks?.NumeroSerie;
                _CodBarra = notebooks?.CodigoBarra;
                _Patrimonio = notebooks?.Patrimonio;
                _habilitado = notebooks?.Habilitado;
                _idUbicacion = notebooks?.IdUbicacion;
                _idEstado = notebooks?.IdEstadoMantenimiento;
                _idModelo = notebooks?.IdModelo;
                _Carrito = carrito?.EquipoCarrito;
                _Casillero = notebooks?.PosicionCarrito;

                CargarComboboxes(Convert.ToInt32(_idTipo));

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
            lblCarroAsignado.Text = "Carro asignado: ";
            lblCasillero.Text = "Casillero: ";

            txtEquipo.Text = _Equipo;
            txtNroSerie.Text = _NroSerie;
            txtCodBarra.Text = _CodBarra;
            txtPatrimonio.Text = _Patrimonio;
            cmbUbicacion.SelectedValue = _idUbicacion;
            cmbEstado.SelectedValue = _idEstado;
            cmbModelo.SelectedValue = _idModelo;
            lblCarroAsignado.Text += _Carrito ?? "Sin Carrito";
            lblCasillero.Text += _Casillero.ToString() ?? "0";
        }

        private void CargarComboboxes(int idTipo)
        {
            cmbUbicacion.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";

            cmbModelo.DataSource = notebooksCN.ListarModelosPorTipo(1);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            if (_idEstado == 2)
            {
                cmbEstado.DataSource = notebooksCN.ListarEstadoMantenimiento();
                cmbEstado.ValueMember = "IdEstadoMantenimiento";
                cmbEstado.DisplayMember = "EstadoMantenimientoNombre";
            }
            else
            {
                cmbEstado.DataSource = notebooksCN.ListarEstadoParaActualizar();
                cmbEstado.ValueMember = "IdEstadoMantenimiento";
                cmbEstado.DisplayMember = "EstadoMantenimientoNombre";
            }
        }

        private void dgvHistorial_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvHistorial.Rows[e.RowIndex];

            _idHistorialSeleccionado = Convert.ToInt32(fila.Cells["IdHistorialNotebook"].Value);

            HistorialCambios? historialCambios = notebooksCN.ObtenerHistorialPorIDHistorial(_idHistorialSeleccionado);

            txtMotivo.Text = historialCambios?.Motivo;
            txtDescripcion.Text = historialCambios?.Descripcion;
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var notebooks = notebooksCN.ObtenerNotebookPorID(_idNotebookSeleccionado);

            var notebook = new Notebooks
            {
                IdElemento = _idNotebookSeleccionado,
                Equipo = txtEquipo.Text,
                IdTipoElemento = 1,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdModelo = Convert.ToInt32(cmbModelo.SelectedValue),
                IdVarianteElemento = null,
                IdUbicacion = Convert.ToInt32(cmbUbicacion.SelectedValue),
                IdEstadoMantenimiento = Convert.ToInt32(cmbEstado.SelectedValue),
                IdCarrito = notebooks?.IdCarrito,
                PosicionCarrito = notebooks?.PosicionCarrito,
                Habilitado = Convert.ToBoolean(_habilitado),
                FechaBaja = null
            };

            if (string.IsNullOrEmpty(txtExplicarMotivo.Text))
            {
                txtExplicarMotivo.Text = "No especifico motivo";
            }

            string descripcionCambios = GenerarDescripcionDeCambios();
            MessageBox.Show(descripcionCambios, "Cambios detectados", MessageBoxButtons.OK, MessageBoxIcon.Information);

            notebooksCN.ActualizarNotebook(notebook, usuarios.IdUsuario, txtExplicarMotivo.Text, descripcionCambios);
            CargarTodaLaGestion(_idNotebookSeleccionado);
        }

        private void VerificarCambios(object sender, EventArgs e)
        {
            if (_cargando) return;

            bool huboCambios =
            txtEquipo.Text != (_Equipo ?? "") ||
            txtNroSerie.Text != (_NroSerie ?? "") ||
            txtCodBarra.Text != (_CodBarra ?? "") ||
            txtPatrimonio.Text != (_Patrimonio ?? "") ||
            (cmbModelo.SelectedValue != null && Convert.ToInt32(cmbModelo.SelectedValue) != _idModelo) ||
            (cmbUbicacion.SelectedValue != null && Convert.ToInt32(cmbUbicacion.SelectedValue) != _idUbicacion) ||
            (cmbEstado.SelectedValue != null && Convert.ToInt32(cmbEstado.SelectedValue) != _idEstado);

            HabilitarBotones(huboCambios, huboCambios);
        }


        private void cmbUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCambios(sender, e);
        }

        private void HabilitarBotones(bool actu, bool rest)
        {
            btnActualizar.Enabled = actu;
            btnRestablecerCambios.Enabled = rest;
        }

        private void btnRestablecerCambios_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idNotebookSeleccionado);
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

            if (txtCodBarra.Text != _CodBarra)
            {
                descripcion.AppendLine("Código de barra:");
                descripcion.AppendLine($"   Antes: {_CodBarra}");
                descripcion.AppendLine($"   Ahora: {txtCodBarra.Text}");
                descripcion.AppendLine();
            }

            if (txtPatrimonio.Text != _Patrimonio)
            {
                descripcion.AppendLine("Patrimonio:");
                descripcion.AppendLine($"   Antes: {_Patrimonio}");
                descripcion.AppendLine($"   Ahora: {txtPatrimonio.Text}");
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
            txtCodBarra.Enabled = Habilitar;
            txtNroSerie.Enabled = Habilitar;
            txtPatrimonio.Enabled = Habilitar;
            txtEquipo.Enabled = Habilitar;
            txtExplicarMotivo.Text = "";

            btnActualizar.Visible = Habilitar;
            btnRestablecerCambios.Visible = Habilitar;
            btnInhabilitar.Visible = Habilitar;

            btnHabilitar.Visible = !Habilitar;
            btnConfirmar.Visible = !Habilitar;
            btnCancelar.Visible = !Habilitar;
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            btnConfirmar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void btnInhabilitar_Click(object sender, EventArgs e)
        {
            btnConfirmar.Enabled = true;
            btnCancelar.Enabled = true;

            btnConfirmar.Visible = true;
            btnCancelar.Visible = true;
            btnActualizar.Visible = false;
            btnRestablecerCambios.Visible = false;

            cmbModelo.Enabled = false;
            cmbUbicacion.Enabled = false;
            cmbEstado.Enabled = false;
            txtCodBarra.Enabled = false;
            txtNroSerie.Enabled = false;
            txtPatrimonio.Enabled = false;
            txtEquipo.Enabled = false;

            CargarDatos();

            lblNoPuedeActualizar.Visible = true;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_habilitado == true)
            {
                notebooksCN.DeshabilitarNotebook(_idNotebookSeleccionado, txtExplicarMotivo.Text, usuarios.IdUsuario);
                CargarTodaLaGestion(_idNotebookSeleccionado);
            }
            else
            {
                notebookBajasCN.HabilitarNotebook(_idNotebookSeleccionado, usuarios.IdUsuario, txtExplicarMotivo.Text);
                CargarTodaLaGestion(_idNotebookSeleccionado);
            }

            btnConfirmar.Enabled = false;
            btnCancelar.Enabled = false;


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idNotebookSeleccionado);
        }
    }
}
