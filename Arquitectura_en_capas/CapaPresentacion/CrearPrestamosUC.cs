using CapaEntidad;
using CapaNegocio;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace CapaPresentacion
{
    public partial class CrearPrestamosUC : UserControl
    {
        private readonly PrestamosCN prestamosCN;
        private readonly PrestamosYDevolucionesUC prestamosYDevolucionesUC;
        private readonly FormPrincipal formPrincipal;
        private Usuarios userActual;
        private int _idPrestamo = 0;

        #region Varibales para identificar textbox usados
        private Guna.UI2.WinForms.Guna2TextBox? txtActivoParaFiltro = null;
        private bool TipeoNroSerie = false;
        private bool TipeoCodBarra = false;
        private bool TipeoPatrimonio = false;
        #endregion

        #region Algunas variables para guardar por si acaso
        private int? _idDocenteDelPrestamo;
        private List<int> ElementosSeleccionados = new List<int>();
        private int? _idElementoEncontrado;
        private int? fila = 0;
        private int idElementoSeleccionado = 0;
        private bool LlevaCarrito = false;
        #endregion

        public CrearPrestamosUC(PrestamosYDevolucionesUC prestamosYDevolucionesUC, FormPrincipal formPrincipal, PrestamosCN prestamosCN, Usuarios usuarios)
        {
            InitializeComponent();
            this.prestamosYDevolucionesUC = prestamosYDevolucionesUC;
            this.formPrincipal = formPrincipal;
            this.prestamosCN = prestamosCN;
            this.userActual = usuarios;
        }

        private void CrearPrestamosUC_Load(object sender, EventArgs e)
        {
            CargarDatos();

            Curso? curso = prestamosCN.ObtenerCursoPorID(1);

            txtDNI.Text = curso?.NombreCurso;
        }

        private void CargarDatos()
        {
            cmbCurso.DataSource = prestamosCN.ObtenerTodosLosCursos();
            cmbCurso.ValueMember = "IdCurso";
            cmbCurso.DisplayMember = "NombreCurso";

            cmbCarros.DataSource = prestamosCN.ObtenerTodosLosCarritosDiponibles();
            cmbCarros.ValueMember = "IdCarrito";
            cmbCarros.DisplayMember = "EquipoCarrito";
        }

        private void chkLlevarCarro_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLlevarCarro.Checked == true)
            {
                LlevarCarro(true);

                if (_idDocenteDelPrestamo != null)
                {
                    btnConfirmarPrestamo.Enabled = false;
                }
                else
                {
                    btnConfirmarPrestamo.Enabled = true;
                }
            }
            else
            {
                LlevarCarro(false);
            }

            ActualizarEstadoBotonConfirmar();
        }

        private void LlevarCarro(bool Lleva)
        {
            txtNroSerieCarro.Enabled = Lleva;
            cmbCarros.Enabled = Lleva;
            LlevaCarrito = Lleva;
            dgvNotebookDetalle.Visible = Lleva;
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
            tmrEsperarPausaTip.Stop();
            tmrEsperarPausaTip.Start();
        }

        private void tmrEsperarPausaTip_Tick(object sender, EventArgs e)
        {
            tmrEsperarPausaTip.Stop();

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                txtDocente.Text = "";
                _idDocenteDelPrestamo = null;
                ActualizarEstadoBotonConfirmar();
                return;
            }

            Docentes? docentes = prestamosCN.ObtenerDocentePorDNI(txtDNI.Text);

            if (docentes is not null)
            {
                txtDocente.Text = docentes?.Nombre + " " + docentes?.Apellido;
                _idDocenteDelPrestamo = docentes?.IdDocente;
            }
            else
            {
                txtDocente.Text = "";
                _idDocenteDelPrestamo = null;
            }

            ActualizarEstadoBotonConfirmar();
        }

        private void cmbCarros_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvNotebookDetalle.DataSource = prestamosCN.ObtenerNotebooksPorCarrito(Convert.ToInt32(cmbCurso.SelectedValue));
        }

        private void txtNroSerie_TextChanged(object sender, EventArgs e)
        {
            if (!txtNroSerie.Focused)
            {
                return;
            }

            txtActivoParaFiltro = txtNroSerie;
            TipeoNroSerie = true;
            TipeoCodBarra = false;
            TipeoPatrimonio = false;

            CambiarDisponibilidadDatos(TipeoNroSerie, TipeoCodBarra, TipeoPatrimonio);

            ReiniciarTimerFiltro();
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
        {
            if (!txtCodBarra.Focused)
            {
                return;
            }

            txtActivoParaFiltro = txtCodBarra;
            TipeoNroSerie = false;
            TipeoCodBarra = true;
            TipeoPatrimonio = false;

            CambiarDisponibilidadDatos(TipeoNroSerie, TipeoCodBarra, TipeoPatrimonio);

            ReiniciarTimerFiltro();
        }

        private void txtPatrimonio_TextChanged(object sender, EventArgs e)
        {
            if (!txtPatrimonio.Focused)
            {
                return;
            }

            txtActivoParaFiltro = txtPatrimonio;
            TipeoNroSerie = false;
            TipeoCodBarra = false;
            TipeoPatrimonio = true;

            CambiarDisponibilidadDatos(TipeoNroSerie, TipeoCodBarra, TipeoPatrimonio);

            ReiniciarTimerFiltro();
        }

        private void ProcesarFiltro(Guna.UI2.WinForms.Guna2TextBox activo)
        {
            if (string.IsNullOrWhiteSpace(activo.Text))
            {
                CambiarDisponibilidadDatos(true, true, true);
                BorrarElementosExcepto(activo);
                return;
            }

            string? TextoNroSerie = null;
            string? TextCodBarra = null;
            string? TextPatrimonio = null;

            if (activo == txtNroSerie)
            {
                TextoNroSerie = activo.Text;
            }
            else if (activo == txtCodBarra)
            {
                TextCodBarra = activo.Text;
            }
            else if (activo == txtPatrimonio)
            {
                TextPatrimonio = activo.Text;
            }

            var elemento = prestamosCN.ObtenerElementoPorFiltro(TextoNroSerie, TextCodBarra, TextPatrimonio);

            if (ElementosSeleccionados.Contains(Convert.ToInt32(elemento?.IdElemento)))
            {
                btnAgregarElemento.Enabled = false;
                return;
            }

            if (elemento != null)
            {
                _idElementoEncontrado = elemento?.IdElemento;

                if (activo != txtNroSerie) txtNroSerie.Text = elemento?.NumeroSerie;
                if (activo != txtCodBarra) txtCodBarra.Text = elemento?.CodigoBarra;
                if (activo != txtPatrimonio) txtPatrimonio.Text = elemento?.Patrimonio;

                TipoElemento? tipoElemento = prestamosCN.ObtenerTipoElementoPorID(Convert.ToInt32(elemento?.IdTipoElemento));
                txtTipoElemento.Text = tipoElemento?.ElementoTipo;

                Modelos? modelo = prestamosCN.ObtenerModeloPorID(Convert.ToInt32(elemento?.IdModelo));
                txtModelo.Text = modelo?.NombreModelo ?? "Sin modelo";

                if (elemento?.IdTipoElemento == 1)
                {
                    Notebooks? notebook = prestamosCN.ObtenerNotebookPorID(elemento.IdElemento);
                    txtEquipo.Text = notebook?.Equipo ?? "Sin datos";
                }
                else
                {
                    VariantesElemento? variante = prestamosCN.ObtenerVariantePorID(Convert.ToInt32(elemento?.IdVarianteElemento));
                    txtEquipo.Text = variante?.Variante ?? "Sin datos";
                }

                btnAgregarElemento.Enabled = true;
            }
            else
            {
                BorrarElementosExcepto(activo);
                btnAgregarElemento.Enabled = false;
            }
        }

        private void BorrarElementosExcepto(Guna.UI2.WinForms.Guna2TextBox activo)
        {
            if (activo != txtNroSerie)
                txtNroSerie.Text = "";

            if (activo != txtCodBarra)
                txtCodBarra.Text = "";

            if (activo != txtPatrimonio)
                txtPatrimonio.Text = "";

            txtTipoElemento.Clear();
            txtEquipo.Clear();
            txtModelo.Clear();
        }

        private void BorrarTodoLosDatos()
        {
            txtNroSerie.Text = "";
            txtCodBarra.Text = "";
            txtPatrimonio.Text = "";
            txtTipoElemento.Clear();
            txtEquipo.Clear();
            txtModelo.Clear();
        }

        private void CambiarDisponibilidadDatos(bool nroSerie, bool codBarra, bool patrimonio)
        {
            txtNroSerie.Enabled = nroSerie;
            txtCodBarra.Enabled = codBarra;
            txtPatrimonio.Enabled = patrimonio;
        }

        private void tmrEsperarPausaTip2_Tick(object sender, EventArgs e)
        {
            tmrEsperarPausaTip2.Stop();

            if (txtActivoParaFiltro != null)
            {
                ProcesarFiltro(txtActivoParaFiltro);
            }
        }

        private void ReiniciarTimerFiltro()
        {
            tmrEsperarPausaTip2.Stop();
            tmrEsperarPausaTip2.Start();
        }

        private void btnAgregarElemento_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados.Add(Convert.ToInt32(_idElementoEncontrado));

            dgvElementosDetalle.Rows.Add(
                txtTipoElemento.Text,
                txtEquipo.Text,
                txtNroSerie.Text,
                txtPatrimonio.Text,
                _idElementoEncontrado
            );

            BorrarTodoLosDatos();
            btnAgregarElemento.Enabled = false;
            txtNroSerie.Enabled = true;
            txtCodBarra.Enabled = true;
            txtPatrimonio.Enabled = true;

            ActualizarEstadoBotonConfirmar();
        }

        private void dgvElementosDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            fila = e.RowIndex;

            idElementoSeleccionado = Convert.ToInt32(dgvElementosDetalle.Rows[e.RowIndex].Cells["IdElemento"].Value);

            btnEliminarElemento.Enabled = true;
        }

        private void btnEliminarElemento_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados.Remove(idElementoSeleccionado);

            dgvElementosDetalle.Rows.RemoveAt(Convert.ToInt32(fila));

            btnEliminarElemento.Enabled = false;

            ActualizarEstadoBotonConfirmar();
        }

        private void cmbListadoDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbListadoDetalle.SelectedIndex)
            {
                case 0:
                    if (LlevaCarrito == false)
                    {
                        dgvNotebookDetalle.Visible = false;
                    }
                    else
                    {
                        dgvNotebookDetalle.Visible = true;
                    }
                    dgvElementosDetalle.Visible = false;
                    break;

                case 1:
                    dgvNotebookDetalle.Visible = false;
                    dgvElementosDetalle.Visible = true;
                    break;
            }
        }

        private void btnConfirmarPrestamo_Click(object sender, EventArgs e)
        {
            if (LlevaCarrito == true)
            {
                Prestamos prestamos = new Prestamos
                {
                    IdPrestamo = _idPrestamo,
                    IdCurso = Convert.ToInt32(cmbCurso.SelectedValue),
                    IdUsuario = userActual.IdUsuario,
                    IdDocente = Convert.ToInt32(_idDocenteDelPrestamo),
                    IdEstadoPrestamo = 1,
                    IdCarrito = Convert.ToInt32(cmbCarros.SelectedValue),
                };

                IEnumerable<int> notebooks = prestamosCN.ObtenerIDsPorCarrito(Convert.ToInt32(cmbCarros.SelectedValue));

                IEnumerable<int> ElementosQueLleva = notebooks.Concat(ElementosSeleccionados);

                prestamosCN.CrearPrestamo(prestamos, ElementosQueLleva, Convert.ToInt32(cmbCarros.SelectedValue));
            }
            else
            {
                Prestamos prestamos = new Prestamos
                {
                    IdPrestamo = _idPrestamo,
                    IdCurso = Convert.ToInt32(cmbCurso.SelectedValue),
                    IdUsuario = userActual.IdUsuario,
                    IdDocente = Convert.ToInt32(_idDocenteDelPrestamo),
                    IdEstadoPrestamo = 1,
                    IdCarrito = null,
                    FechaPrestamo = DateTime.Now
                };

                prestamosCN.CrearPrestamo(prestamos, ElementosSeleccionados, null);
            }

            MessageBox.Show("Prestamo realizado correctamente");

            prestamosYDevolucionesUC.ActualizarDataGrid();
            formPrincipal.MostrarUserControl(prestamosYDevolucionesUC);
        }

        private void ActualizarEstadoBotonConfirmar()
        {
            bool hayDocente = _idDocenteDelPrestamo != null;
            bool hayElementos = ElementosSeleccionados.Any();
            bool llevaCarrito = LlevaCarrito;

            if (!hayDocente)
            {
                btnConfirmarPrestamo.Enabled = false;
                return;
            }

            if (!llevaCarrito && !hayElementos)
            {
                btnConfirmarPrestamo.Enabled = false;
                return;
            }

            btnConfirmarPrestamo.Enabled = true;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            prestamosYDevolucionesUC.ActualizarDataGrid();
            formPrincipal.MostrarUserControl(prestamosYDevolucionesUC);
        }
    }
}
