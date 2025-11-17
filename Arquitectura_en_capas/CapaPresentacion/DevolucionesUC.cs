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
    public partial class DevolucionesUC : UserControl
    {
        private readonly PrestamosYDevolucionesUC prestamosYDevolucionesUC;
        private readonly FormPrincipal _formPrincipal;
        private readonly PrestamosCN prestamosCN;
        private Usuarios userActual;
        private readonly DevolucionCN devolucionCN;
        private int idPrestamoSeleccionado;
        private int? idCarrito = 0;

        private int _idElementoPorConfirmar = 0;
        private int _idElementoSeleccionado = 0;
        private string? _NroSerie = "";
        private string? _Patrimonio = "";
        private string? _TipoElemento = "";
        private string? _Equipo = "";
        private string? _Carrito = "";

        private List<int> ElementosSeleccionados = new List<int>();
        private List<int> ElementosDevueltos = new List<int>();
        private List<int> ElementosPrestados = new List<int>();
        private List<int> ElementosFaltantes = new List<int>();
        private List<int> NotebooksCarrito = new List<int>();
        public DevolucionesUC(PrestamosYDevolucionesUC prestamosYDevolucionesUC, FormPrincipal _formPrincipal, PrestamosCN prestamosCN, Usuarios userActual, DevolucionCN devolucionCN, int idPrestamoSeleccionado)
        {
            InitializeComponent();

            this.prestamosYDevolucionesUC = prestamosYDevolucionesUC;
            this._formPrincipal = _formPrincipal;
            this.prestamosCN = prestamosCN;
            this.userActual = userActual;
            this.devolucionCN = devolucionCN;
            this.idPrestamoSeleccionado = idPrestamoSeleccionado;
        }

        private void DevolucionesUC_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        public void CargarDatos()
        {
            ElementosPrestados = prestamosCN.ObtenerIDsElementosPorIdPrestamo(idPrestamoSeleccionado);

            Prestamos? prestamos = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);

            idCarrito = prestamos?.IdCarrito;

            if (idCarrito != null)
            {
                btnDevolverCarro.Enabled = true;
            }
            else
            {
                btnDevolverCarro.Enabled = false;
            }

            dgvPrestamoDetalle.DataSource = prestamosCN.ObtenerPrestamoDetallePorId(idPrestamoSeleccionado, prestamos?.IdCarrito);

            Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);

            if (devolucion != null)
            {
                dgvDevolucionDetalle.Visible = true;
                dgvElementosPorConfirmacion.Visible = false;

                dgvDevolucionDetalle.DataSource = devolucionCN.ObtenerDevolucionDetallePorID(devolucion.IdDevolucion, prestamos?.IdCarrito);

                ElementosDevueltos = devolucionCN.ObtenerIDsElementosEnDev(devolucion.IdDevolucion);
            }
            else
            {
                dgvDevolucionDetalle.Visible = false;
                dgvElementosPorConfirmacion.Visible = true;
            }
        }

        private void dgvPrestamoDetalle_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _idElementoSeleccionado = Convert.ToInt32(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["IdElemento"].Value);
            _TipoElemento = Convert.ToString(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["TipoElemento"].Value);
            _Equipo = Convert.ToString(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["Equipo"].Value);
            _Carrito = Convert.ToString(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["PosicionCarrito"].Value);
            _NroSerie = Convert.ToString(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["NumeroSerieElemento"].Value);
            _Patrimonio = Convert.ToString(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["Patrimonio"].Value);

            if (ElementosSeleccionados.Contains(_idElementoSeleccionado) || ElementosDevueltos.Contains(_idElementoSeleccionado))
            {
                btnMarcarDevuelto.Enabled = false;
                btnQuitarDevuelto.Enabled = false;
            }
            else
            {
                btnMarcarDevuelto.Enabled = true;
                btnQuitarDevuelto.Enabled = true;
            }
        }

        private void btnMarcarDevuelto_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados.Add(Convert.ToInt32(_idElementoSeleccionado));

            dgvElementosPorConfirmacion.Rows.Add(
                _idElementoSeleccionado,
                _TipoElemento,
                _Equipo,
                _Carrito,
                _NroSerie,
                _Patrimonio
            );

            btnMarcarDevuelto.Enabled = false;
            btnQuitarDevuelto.Enabled = false;
        }

        private void btnDevolverCarro_Click(object sender, EventArgs e)
        {

            NotebooksCarrito = prestamosCN.ObtenerIDsPorCarrito(Convert.ToInt32(idCarrito)).ToList();

            var faltanDevolver = NotebooksCarrito
                .Where(id =>
                    !ElementosDevueltos.Contains(id) &&
                    !ElementosSeleccionados.Contains(id)
                )
                .ToList();

            ElementosSeleccionados.AddRange(faltanDevolver);

            foreach (var id in faltanDevolver)
            {

                PrestamosDetalleDTO? elemento = prestamosCN.ObtenerElementoMapeadoPorID(id, idCarrito);

                dgvElementosPorConfirmacion.Rows.Add(
                    id,
                    elemento?.TipoElemento,
                    elemento?.Equipo,
                    elemento?.PosicionCarrito,
                    elemento?.NumeroSerieElemento,
                    elemento?.Patrimonio
                );
            }
        }

        private void btnDevolverTodos_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados = ElementosPrestados
            .Where(id => !ElementosDevueltos.Contains(id))
            .ToList();

            foreach (var id in ElementosSeleccionados)
            {

                PrestamosDetalleDTO? elemento = prestamosCN.ObtenerElementoMapeadoPorID(id, idCarrito);

                dgvElementosPorConfirmacion.Rows.Add(
                    id,
                    elemento?.TipoElemento,
                    elemento?.Equipo,
                    elemento?.PosicionCarrito,
                    elemento?.NumeroSerieElemento,
                    elemento?.Patrimonio
                );
            }

        }

        private void dgvElementosPorConfirmacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _idElementoPorConfirmar = Convert.ToInt32(dgvElementosPorConfirmacion.Rows[e.RowIndex].Cells["IdElemento"].Value);
        }

        private void btnQuitarDevuelto_Click(object sender, EventArgs e)
        {
            if (_idElementoPorConfirmar == 0)
            {
                return;
            }

            ElementosSeleccionados.Remove(_idElementoPorConfirmar);

            foreach (DataGridViewRow row in dgvElementosPorConfirmacion.Rows)
            {
                if (Convert.ToInt32(row.Cells["IdElemento"].Value) == _idElementoPorConfirmar)
                {
                    dgvElementosPorConfirmacion.Rows.Remove(row);
                    break;
                }
            }

            _idElementoPorConfirmar = 0;

            btnMarcarDevuelto.Enabled = false;
            btnQuitarDevuelto.Enabled = false;
        }

        private void btnConfirmarDevolucion_Click(object sender, EventArgs e)
        {

        }
    }
}
