using CapaEntidad;
using CapaNegocio;
using Guna.UI2.WinForms;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CapaPresentacion
{
    public partial class ElementoGestionUC : UserControl
    {
        private FormPrincipal formPrincipal;
        private ElementosUC elementosUC;
        private readonly ElementosCN elementosCN;
        private readonly ElementosBajasCN elementosBajasCN;
        private int _idElementoSeleccionado;
        private int _idHistorialSeleccionado;
        private Usuarios usuarios;

        #region GUARDANDO DATOS EN VARIABLES
        private string? _NroSerie = "";
        private string? _CodBarra = "";
        private string? _Patrimonio = "";
        private int? _idVariante = 0;
        private bool? _habilitado = true;
        private int? _idUbicacion = 0;
        private int? _idEstado = 0;
        private int? _idTipo = 0;
        private int? _idModelo = 0;
        private string? _NombreModelo = "";
        private string? _NombreVariante = "";
        private string? _NombreUbicacion = "";
        private string? _NombreEstado = "";
        #endregion

        public ElementoGestionUC(FormPrincipal formPrincipal, ElementosUC elementosUC, ElementosCN elementosCN, ElementosBajasCN elementosBajasCN,  int idElementoSeleccionado, Usuarios user)
        {
            InitializeComponent();

            this.formPrincipal = formPrincipal;
            this.elementosUC = elementosUC;
            this.elementosCN = elementosCN;
            this.elementosBajasCN = elementosBajasCN;
            this._idElementoSeleccionado = idElementoSeleccionado;
            this.usuarios = user;

        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            elementosUC.ActualizarDataGrid(0);
            formPrincipal.MostrarUserControl(elementosUC);
        }

        private void ElementoGestionUC_Load(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idElementoSeleccionado);
        }

        private void CargarTodaLaGestion(int idElemento)
        {
            dgvHistorial.DataSource = elementosCN.ObtenerHistorialPorID(_idElementoSeleccionado);
            var elementos = elementosCN.ObtenerPorId(_idElementoSeleccionado);

            _idTipo = elementos?.IdTipoElemento;
            _NroSerie = elementos?.NumeroSerie;
            _CodBarra = elementos?.CodigoBarra;
            _Patrimonio = elementos?.Patrimonio;
            _idVariante = elementos?.IdVarianteElemento;
            _habilitado = elementos?.Habilitado;
            _idUbicacion = elementos?.IdUbicacion;
            _idEstado = elementos?.IdEstadoMantenimiento;
            _idModelo = elementos?.IdModelo;

            CargarComboboxes(Convert.ToInt32(_idTipo));

            TipoElemento? tipoElemento = elementosCN.ObtenerTipoElementoPorID(Convert.ToInt32(elementos?.IdTipoElemento));
            lblTipoElemento.Text = tipoElemento?.ElementoTipo;
            lblTipoElemento.Tag = tipoElemento?.IdTipoElemento;

            _NombreVariante = cmbVarianteElemento.Text;
            _NombreUbicacion = cmbUbicacion.Text;
            _NombreEstado = cmbEstado.Text;

            CargarDatos();

            HabilitarModificado(Convert.ToBoolean(_habilitado));

            HabilitarBotones(false, false);
        }

        private void CargarDatos()
        {
            txtNroSerie.Text = _NroSerie;
            txtCodBarra.Text = _CodBarra;
            txtPatrimonio.Text = _Patrimonio;
            cmbVarianteElemento.SelectedValue = _idVariante;
            cmbUbicacion.SelectedValue = _idUbicacion;
            cmbEstado.SelectedValue = _idEstado;

            if (_idModelo != null)
            {
                Modelos? modelos = elementosCN.ObtenerModeloPorID(Convert.ToInt32(_idModelo));
                txtModelo.Text = modelos?.NombreModelo;
                txtModelo.Tag = modelos?.IdModelo;
                _NombreModelo = modelos?.NombreModelo;
            }
        }

        private void CargarComboboxes(int idTipo)
        {
            cmbUbicacion.DataSource = elementosCN.ObtenerUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";

            cmbVarianteElemento.DataSource = elementosCN.ObtenerVariantesPorTipo(Convert.ToInt32(_idTipo));
            cmbVarianteElemento.ValueMember = "IdVarianteElemento";
            cmbVarianteElemento.DisplayMember = "Variante";

            cmbEstado.DataSource = elementosCN.ObtenerEstadoMantenimientoParaActualizar();
            cmbEstado.ValueMember = "IdEstadoMantenimiento";
            cmbEstado.DisplayMember = "EstadoMantenimientoNombre";
        }

        private void dgvHistorial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvHistorial.Rows[e.RowIndex];

            _idHistorialSeleccionado = Convert.ToInt32(fila.Cells["IdHistorialElemento"].Value);

            HistorialCambios? historialCambios = elementosCN.ObtenerHistorialPorIDHistorial(_idHistorialSeleccionado);

            txtMotivo.Text = historialCambios?.Motivo;

            txtDescripcion.Text = historialCambios?.Descripcion;
        }

        private void cmbVarianteElemento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVarianteElemento.SelectedValue is int selectedValue)
            {
                CargarModelo(selectedValue);
                cmbVarianteElemento.Tag = selectedValue;
                _NombreVariante = cmbVarianteElemento.Text;
            }

            VerificarCambios(sender, e);
        }
        private void CargarModelo(int idVariante)
        {
            VariantesElemento? variantes = elementosCN.ObtenerVariantePorID(idVariante);

            if (variantes?.IdModelo is not null)
            {
                Modelos? modelos = elementosCN.ObtenerModeloPorID(variantes.IdModelo);
                txtModelo.Text = modelos?.NombreModelo;
                txtModelo.Tag = modelos?.IdModelo;
            }
            else
            {
                txtModelo.Text = "Sin modelo";
                txtModelo.Tag = null;
            }
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var elemento = new Elemento
            {
                IdElemento = _idElementoSeleccionado,
                IdTipoElemento = Convert.ToInt32(lblTipoElemento.Tag),
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdVarianteElemento = Convert.ToInt32(cmbVarianteElemento.SelectedValue),
                IdUbicacion = Convert.ToInt32(cmbUbicacion.SelectedValue),
                IdModelo = Convert.ToInt32(txtModelo.Tag),
                IdEstadoMantenimiento = Convert.ToInt32(cmbEstado.SelectedValue),
                Habilitado = Convert.ToBoolean(_habilitado),
                FechaBaja = null
            };

            if (string.IsNullOrEmpty(txtExplicarMotivo.Text))
            {
                txtExplicarMotivo.Text = "No especifico motivo";
            }

            string descripcionCambios = GenerarDescripcionDeCambios();
            MessageBox.Show(descripcionCambios, "Cambios detectados", MessageBoxButtons.OK, MessageBoxIcon.Information);

            elementosCN.ActualizarElemento(elemento, usuarios.IdUsuario, txtExplicarMotivo.Text, descripcionCambios);
            CargarTodaLaGestion(_idElementoSeleccionado);
        }

        private void VerificarCambios(object sender, EventArgs e)
        {
            if (txtNroSerie.Text != _NroSerie ||
                txtCodBarra.Text != _CodBarra ||
                txtPatrimonio.Text != _Patrimonio ||
                Convert.ToInt32(txtModelo.Tag) != _idModelo ||
                Convert.ToInt32(cmbVarianteElemento.SelectedValue) != _idVariante ||
                Convert.ToInt32(cmbUbicacion.SelectedValue) != _idUbicacion ||
                Convert.ToInt32(cmbEstado.SelectedValue) != _idEstado)
            {
                HabilitarBotones(true, true);
            }
            else
            {
                HabilitarBotones(false, false);
            }
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
            btnActualizar.Enabled = actu;
            btnRestablecerCambios.Enabled = rest;
        }

        private void btnRestablecerCambios_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idElementoSeleccionado);
        }

        private string GenerarDescripcionDeCambios()
        {
            StringBuilder descripcion = new StringBuilder();
            descripcion.AppendLine("Se realizaron los siguientes cambios:");
            descripcion.AppendLine();

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

            if (Convert.ToInt32(cmbVarianteElemento.SelectedValue) != _idVariante)
            {
                descripcion.AppendLine("Variante de elemento:");
                descripcion.AppendLine($"   Antes: {_NombreVariante}");
                descripcion.AppendLine($"   Ahora: {cmbVarianteElemento.Text}");
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

            // Si no hubo cambios, avisamos
            if (descripcion.Length == "Se realizaron los siguientes cambios:\r\n\r\n".Length)
            {
                descripcion.Clear();
                descripcion.Append("No se detectaron cambios en los datos principales.");
            }

            return descripcion.ToString();
        }

        private void HabilitarModificado(bool Habilitar)
        {
            cmbVarianteElemento.Enabled = Habilitar;
            cmbUbicacion.Enabled = Habilitar;
            cmbEstado.Enabled = Habilitar;
            txtCodBarra.Enabled = Habilitar;
            txtNroSerie.Enabled = Habilitar;
            txtPatrimonio.Enabled = Habilitar;
            txtModelo.Enabled = Habilitar;
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

            cmbVarianteElemento.Enabled = false;
            cmbUbicacion.Enabled = false;
            cmbEstado.Enabled = false;
            txtCodBarra.Enabled = false;
            txtNroSerie.Enabled = false;
            txtPatrimonio.Enabled = false;
            txtModelo.Enabled = false;

            CargarDatos();

            lblNoPuedeActualizar.Visible = true;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if(_habilitado == true)
            {
                elementosCN.DeshabilitarElemento(_idElementoSeleccionado, 3, usuarios.IdUsuario);
                CargarTodaLaGestion(_idElementoSeleccionado);
            }
            else
            {
                elementosBajasCN.HabilitarElemento(_idElementoSeleccionado, usuarios.IdUsuario);
                CargarTodaLaGestion(_idElementoSeleccionado);
            }

            btnConfirmar.Enabled = false;
            btnCancelar.Enabled = false;


        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CargarTodaLaGestion(_idElementoSeleccionado);
        }
    }
}
