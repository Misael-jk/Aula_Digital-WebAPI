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

        private Guna.UI2.WinForms.Guna2TextBox? txtActivoParaFiltro = null;

        private int? _idDocenteDelPrestamo;
        private List<int> ElementosSeleccionados = new List<int>();
        private int? _idElementoEncontrado;
        private int? fila = 0;
        private int idElementoSeleccionado = 0;
        private List<int> NotebooksDelCarrito = new List<int>();
        private bool LlevaCarrito = false;

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

            MostrarPanelCarrito(false);
            pnlDatosElemento.Visible = false;
            lblNinguno.Visible = false;
            lblBuscarElemento.Text = "Elemento encontrado:";
            lblSeleccionados.Text = "Elementos seleccionados: 0";
            ActualizarEstadoBotonConfirmar();
        }

        private void CargarDatos()
        {
            cmbCarros.DataSource = prestamosCN.ObtenerTodosLosCarritosDiponibles();
            cmbCarros.ValueMember = "IdCarrito";
            cmbCarros.DisplayMember = "EquipoCarrito";

            cmbCurso.DataSource = prestamosCN.ObtenerTodosLosCursos();
            cmbCurso.ValueMember = "IdCurso";
            cmbCurso.DisplayMember = "NombreCurso";
        }

        #region Carrito show/hide y sincronización
        private void chkLlevarCarro_CheckedChanged(object sender, EventArgs e)
        {
            LlevarCarro(chkLlevarCarro.Checked);
            MostrarPanelCarrito(chkLlevarCarro.Checked);

            if (!chkLlevarCarro.Checked)
            {
                NotebooksDelCarrito.Clear();
                dgvNotebookDetalle.DataSource = null;
                QuitarNotebooksDelCarritoDeSeleccion();
            }

            ActualizarEstadoBotonConfirmar();
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
            tmrEsperarPausaTip.Stop();
            tmrEsperarPausaTip.Start();
        }

        private void tmrEsperarPausaTip_Tick(object sender, EventArgs e)
        {
            tmrEsperarPausaTip.Stop();
            ProcesarBusquedaDocente();
        }


        private void ProcesarBusquedaDocente()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                _idDocenteDelPrestamo = null;
                lblDocente.Text = "";
                AplicarToolTip(lblDocente);
                ActualizarEstadoBotonConfirmar();
                return;
            }

            var docente = prestamosCN.ObtenerDocentePorDNI(txtDNI.Text);

            if (docente != null)
            {
                lblDocente.Text = $"{docente.Nombre} {docente.Apellido}";
                _idDocenteDelPrestamo = docente.IdDocente;
            }
            else
            {
                lblDocente.Text = "";
                _idDocenteDelPrestamo = null;
            }

            AplicarToolTip(lblDocente);
            ActualizarEstadoBotonConfirmar();
        }


        private void LlevarCarro(bool Lleva)
        {
            cmbCarros.Enabled = Lleva;
            LlevaCarrito = Lleva;
        }

        private void MostrarPanelCarrito(bool mostrar)
        {
            pnlLlevaCarro.Visible = mostrar;
            dgvNotebookDetalle.Visible = mostrar;
            lblNollevaCarro.Visible = !mostrar;
        }

        private void cmbCarros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!chkLlevarCarro.Checked) return;

            dgvNotebookDetalle.DataSource = prestamosCN.ObtenerNotebooksPorCarrito(Convert.ToInt32(cmbCarros.SelectedValue));

            Carritos? carritos = prestamosCN.ObtenerCarritoPorID(Convert.ToInt32(cmbCarros.SelectedValue));
            lblEquipoCarrito.Text = carritos?.EquipoCarrito;
            AplicarToolTip(lblEquipoCarrito);

            lblCapacidad.Text = Convert.ToString(carritos?.Capacidad);

            lblNroSerieCarrito.Text = carritos?.NumeroSerieCarrito;
            AplicarToolTip(lblNroSerieCarrito);

            Modelos? modelos = prestamosCN.ObtenerModeloPorID(Convert.ToInt32(carritos?.IdModelo));

            lblModeloCarrito.Text = modelos?.NombreModelo;
            AplicarToolTip(lblModelo);

            lblNotebooks.Text = Convert.ToString(prestamosCN.ObtenerCantidadDisponiblesPorCarrito(Convert.ToInt32(cmbCarros.SelectedValue)));

            lblNotebooksPrestadas.Text = Convert.ToString(prestamosCN.ObtenerCantidadPrestadosPorCarrito(Convert.ToInt32(cmbCarros.SelectedValue)));

            NotebooksDelCarrito = prestamosCN.ObtenerIDsElementosPorIdDisponibles (Convert.ToInt32(cmbCarros.SelectedValue)).ToList();

            QuitarNotebooksDelCarritoDeSeleccion();
        }

        private void QuitarNotebooksDelCarritoDeSeleccion()
        {
            if (NotebooksDelCarrito == null || NotebooksDelCarrito.Count == 0) return;

            var repetidos = ElementosSeleccionados.Intersect(NotebooksDelCarrito).ToList();
            if (!repetidos.Any()) return;

            foreach (var id in repetidos)
            {
                ElementosSeleccionados.Remove(id);
            }

            for (int i = dgvElementosDetalle.Rows.Count - 1; i >= 0; i--)
            {
                var row = dgvElementosDetalle.Rows[i];
                if (row.Cells["IdElemento"].Value == null) continue;
                int idElem = Convert.ToInt32(row.Cells["IdElemento"].Value);
                if (repetidos.Contains(idElem))
                    dgvElementosDetalle.Rows.RemoveAt(i);
            }

            lblSeleccionados.Text = "Elementos seleccionados: " + ElementosSeleccionados.Count;
        }
        #endregion

        #region Buscar elemento (un solo texto activo)
        private void txtNroSerie_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtNroSerie);
            txtActivoParaFiltro = txtNroSerie;
            ReiniciarTimerFiltro();
        }

        private void txtCodBarra_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtCodBarra);
            txtActivoParaFiltro = txtCodBarra;
            ReiniciarTimerFiltro();
        }

        private void txtPatrimonio_TextChanged(object sender, EventArgs e)
        {
            CambiarDisponibilidadDatos(txtPatrimonio);
            txtActivoParaFiltro = txtPatrimonio;
            ReiniciarTimerFiltro();
        }


        private void CambiarDisponibilidadDatos(Guna.UI2.WinForms.Guna2TextBox? activo)
        {
            if (activo != null)
            {
                if (activo != txtNroSerie)
                {
                    txtNroSerie.Enabled = false;
                }
                else
                {
                    txtNroSerie.Enabled = true;
                }

                if (activo != txtCodBarra)
                {
                    txtCodBarra.Enabled = false;
                }
                else
                {
                    txtCodBarra.Enabled = true;
                }

                if (activo != txtPatrimonio)
                {
                    txtPatrimonio.Enabled = false;
                }
                else
                {
                    txtPatrimonio.Enabled = true;
                }
            }
            else
            {
                txtNroSerie.Enabled = true;
                txtCodBarra.Enabled = true;
                txtPatrimonio.Enabled = true;
            }
        }

        private void ReiniciarTimerFiltro()
        {
            pnlDatosElemento.Visible = false;
            lblNinguno.Visible = false;
            lblBuscarElemento.Text = "Buscando elemento...";
            btnAgregarElemento.Enabled = false;
            tmrEsperarPausaTip2.Stop();
            tmrEsperarPausaTip2.Start();
        }

        private void tmrEsperarPausaTip2_Tick(object sender, EventArgs e)
        {
            tmrEsperarPausaTip2.Stop();

            if (txtActivoParaFiltro != null)
            {
                ProcesarFiltro(txtActivoParaFiltro);
            }
        }

        private void ProcesarFiltro(Guna.UI2.WinForms.Guna2TextBox activo)
        {
            if (string.IsNullOrWhiteSpace(activo.Text))
            {
                CambiarDisponibilidadDatos(null);
                lblBuscarElemento.Text = "Elemento encontrado:";
                lblNinguno.Visible = false;
                pnlDatosElemento.Visible = false;
                btnAgregarElemento.Enabled = false;
                _idElementoEncontrado = null;
                return;
            }

            string? TextoNroSerie = activo == txtNroSerie ? activo.Text : null;
            string? TextCodBarra = activo == txtCodBarra ? activo.Text : null;
            string? TextPatrimonio = activo == txtPatrimonio ? activo.Text : null;

            var elemento = prestamosCN.ObtenerElementoPorFiltro(TextoNroSerie, TextCodBarra, TextPatrimonio);

            if (elemento == null)
            {
                lblBuscarElemento.Text = "Elemento encontrado:";
                lblNinguno.Visible = true;
                pnlDatosElemento.Visible = false;
                btnAgregarElemento.Enabled = false;
                _idElementoEncontrado = null;
                return;
            }

            if (ElementosSeleccionados.Contains(elemento.IdElemento))
            {
                lblBuscarElemento.Text = "Elemento encontrado:";
                lblNinguno.Visible = true;
                pnlDatosElemento.Visible = false;
                btnAgregarElemento.Enabled = false;
                _idElementoEncontrado = null;
                return;
            }

            if (NotebooksDelCarrito != null && NotebooksDelCarrito.Contains(elemento.IdElemento))
            {
                lblBuscarElemento.Text = "Elemento encontrado:";
                lblNinguno.Visible = true;
                pnlDatosElemento.Visible = false;
                btnAgregarElemento.Enabled = false;
                _idElementoEncontrado = null;
                return;
            }

            lblBuscarElemento.Text = "Elemento encontrado:";
            pnlDatosElemento.Visible = true;
            lblNinguno.Visible = false;

            _idElementoEncontrado = elemento.IdElemento;

            lblNroSerie.Text = elemento.NumeroSerie;
            AplicarToolTip(lblNroSerie);

            lblCodBarra.Text = elemento.CodigoBarra;
            AplicarToolTip(lblCodBarra);

            lblPatrimonio.Text = elemento.Patrimonio;
            AplicarToolTip(lblPatrimonio);

            TipoElemento? tipoElemento = prestamosCN.ObtenerTipoElementoPorID(elemento.IdTipoElemento);
            lblTipoElemento.Text = tipoElemento?.ElementoTipo;
            AplicarToolTip(lblTipoElemento);

            Modelos? modelo = prestamosCN.ObtenerModeloPorID(elemento.IdModelo);
            lblModelo.Text = modelo?.NombreModelo ?? "Sin modelo";
            AplicarToolTip(lblModelo);

            if (elemento.IdTipoElemento == 1)
            {
                Notebooks? notebook = prestamosCN.ObtenerNotebookPorID(elemento.IdElemento);
                lblEquipo.Text = notebook?.Equipo ?? "Sin datos";
            }
            else
            {
                VariantesElemento? variante = prestamosCN.ObtenerVariantePorID(Convert.ToInt32(elemento.IdVarianteElemento));
                lblEquipo.Text = variante?.Variante ?? "Sin datos";
            }
            AplicarToolTip(lblEquipo);

            btnAgregarElemento.Enabled = true;
        }
        #endregion

        #region Agregar / eliminar elementos seleccionados
        private void btnAgregarElemento_Click(object sender, EventArgs e)
        {

            ElementosSeleccionados.Add(Convert.ToInt32(_idElementoEncontrado));

            dgvElementosDetalle.Rows.Add(
                lblTipoElemento.Text,
                lblEquipo.Text,
                lblNroSerie.Text,
                lblPatrimonio.Text,
                Convert.ToInt32(_idElementoEncontrado)
            );

            btnAgregarElemento.Enabled = false;

            txtNroSerie.Text = "";
            txtCodBarra.Text = "";
            txtPatrimonio.Text = "";
            pnlDatosElemento.Visible = false;
            lblNinguno.Visible = false;
            _idElementoEncontrado = null;
            
            CambiarDisponibilidadDatos(null);
            lblSeleccionados.Text = "Elementos seleccionados: " + ElementosSeleccionados.Count;

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
            lblSeleccionados.Text = "Elementos seleccionados: " + ElementosSeleccionados.Count;
            ActualizarEstadoBotonConfirmar();
        }
        #endregion

        private void btnConfirmarPrestamo_Click(object sender, EventArgs e)
        {
            if (LlevaCarrito)
            {
                Prestamos prestamos = new Prestamos
                {
                    IdPrestamo = _idPrestamo,
                    IdCurso = Convert.ToInt32(cmbCurso.SelectedValue),
                    IdUsuario = userActual.IdUsuario,
                    IdDocente = Convert.ToInt32(_idDocenteDelPrestamo),
                    IdEstadoPrestamo = 1,
                    IdCarrito = Convert.ToInt32(cmbCarros.SelectedValue),
                    FechaPrestamo = DateTime.Now
                };

                IEnumerable<int> notebooks = prestamosCN.ObtenerIDsElementosPorIdDisponibles(Convert.ToInt32(cmbCarros.SelectedValue));
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

            if (!hayDocente)
            {
                btnConfirmarPrestamo.Enabled = false;
                return;
            }

            if (LlevaCarrito)
            {
                btnConfirmarPrestamo.Enabled = true;
                return;
            }

            btnConfirmarPrestamo.Enabled = hayElementos;
        }

        private void AplicarToolTip(Label label)
        {
            string texto = string.IsNullOrWhiteSpace(label.Text) ? "" : label.Text;
            tltDatos.SetToolTip(label, texto);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            prestamosYDevolucionesUC.ActualizarDataGrid();
            formPrincipal.MostrarUserControl(prestamosYDevolucionesUC);
        }

        private void btnCarrito_Click(object sender, EventArgs e)
        {
            MostrarPanelCarrito(chkLlevarCarro.Checked);
            pnlCarrito.Visible = true;
            pnlElementos.Visible = false;
            dgvElementosDetalle.Visible = false;
        }

        private void btnElementos_Click(object sender, EventArgs e)
        {
            pnlCarrito.Visible = false;
            pnlElementos.Visible = true;
            dgvElementosDetalle.Visible = true;
            dgvNotebookDetalle.Visible = false;
        }
    }
}
