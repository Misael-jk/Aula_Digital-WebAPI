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
        private int? idCarrito;
        private bool CarroYaDevuelto = false;
        private bool DevolverCarro = false;
        private int? idDevolcionSeleccionado;

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
            this.AutoScrollMinSize = new Size(0, 1000);
        }

        public void CargarDatos()
        {

            ElementosPrestados = prestamosCN.ObtenerIDsElementosPorIdPrestamo(idPrestamoSeleccionado);

            Prestamos? prestamos = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);

            idCarrito = prestamos?.IdCarrito;

            if (prestamos?.IdEstadoPrestamo == 2)
            {
                btnDevolverCarro.Visible = false;
                btnDevolverTodos.Visible = false;
                btnMarcarDevuelto.Visible = false;

                pnlElementosADevolver.Enabled = false;
            }

            if (idCarrito.HasValue)
            {
                Carritos? carritos = devolucionCN.ObtenerCarroPorID(idCarrito.Value);

                if (carritos?.IdEstadoMantenimiento == 1)
                {
                    btnDevolverCarro.Enabled = false;
                    CarroYaDevuelto = true;
                }
                else
                {
                    btnDevolverCarro.Enabled = true;
                    CarroYaDevuelto = false;
                }
            }
            else
            {
                btnDevolverCarro.Visible = false;
                CarroYaDevuelto = false;
            }

            dgvPrestamoDetalle.DataSource = prestamosCN.ObtenerPrestamoDetallePorId(idPrestamoSeleccionado, prestamos?.IdCarrito);

            Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);

            if (devolucion != null)
            {
                idDevolcionSeleccionado = devolucion?.IdDevolucion;
                ElementosDevueltos = devolucionCN.ObtenerIDsElementosEnDev(Convert.ToInt32(devolucion?.IdDevolucion));
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

            btnQuitarDevuelto.Enabled = true;
            btnMarcarDevuelto.Enabled = false;
            btnQuitarDevuelto.Enabled = false;
        }

        private void btnDevolverCarro_Click(object sender, EventArgs e)
        {
            NotebooksCarrito = prestamosCN.ObtenerIDsPrestadosPorCarrito(Convert.ToInt32(idCarrito)).ToList();

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

            if (idCarrito.HasValue)
            {
                if (CarroYaDevuelto == false)
                {
                    DevolverCarro = true;
                }
            }

            btnDevolverCarro.Enabled = false;
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

            if (idCarrito.HasValue)
            {
                if (CarroYaDevuelto == false)
                {
                    DevolverCarro = true;
                }
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
        }

        private void btnConfirmarDevolucion_Click(object sender, EventArgs e)
        {
            if (ElementosSeleccionados.Count == 0)
            {
                MessageBox.Show("No seleccionaste ningun elemento");
                return;
            }

            List<string> observaciones = new List<string>();

            foreach (DataGridViewRow row in dgvElementosPorConfirmacion.Rows)
            {
                if (!row.IsNewRow)
                {
                    string? obs = Convert.ToString(row.Cells["Observacion"].Value);

                    if (string.IsNullOrWhiteSpace(obs))
                        obs = "Sin observación";

                    observaciones.Add(obs);
                }
            }

            Devolucion? devolucionExistente = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);

            int totalPrestados = ElementosPrestados.Count;

            int totalDevueltosAhora = ElementosDevueltos.Count + ElementosSeleccionados.Count;

            if (devolucionExistente == null)
            {
                Devolucion nueva = new Devolucion
                {
                    IdPrestamo = idPrestamoSeleccionado,
                    IdUsuario = userActual.IdUsuario,
                    FechaDevolucion = DateTime.Now
                };

                if (totalDevueltosAhora == totalPrestados)
                {
                    devolucionCN.CrearDevolucion(nueva, ElementosSeleccionados, observaciones, userActual.IdUsuario, idCarrito);
                    CarroYaDevuelto = true;
                    MessageBox.Show("Devolución COMPLETA registrada correctamente.");
                }
                else
                {
                    if (idCarrito.HasValue)
                    {
                        if (DevolverCarro == true)
                        {
                            devolucionCN.CrearDevolucion(nueva, ElementosSeleccionados, observaciones, userActual.IdUsuario, idCarrito);
                            btnDevolverCarro.Enabled = false;
                            MessageBox.Show("Primera devolución PARCIAL registrada correctamente.");
                        }
                        else
                        {
                            devolucionCN.CrearDevolucion(nueva, ElementosSeleccionados, observaciones, userActual.IdUsuario, null);
                            MessageBox.Show("Primera devolución PARCIAL registrada correctamente.");
                        }
                    }
                    else
                    {
                        devolucionCN.CrearDevolucion(nueva, ElementosSeleccionados, observaciones, userActual.IdUsuario, null);
                        MessageBox.Show("Primera devolución PARCIAL registrada correctamente.");
                    }
                }
            }
            else
            {
                if (totalDevueltosAhora == totalPrestados)
                {
                    if (idCarrito.HasValue)
                    {
                        if (DevolverCarro == true)
                        {
                            devolucionCN.CrearDevolucionParcial(
                                idPrestamoSeleccionado,
                                ElementosSeleccionados,
                                observaciones,
                                userActual.IdUsuario,
                                idCarrito
                            );

                            btnDevolverCarro.Enabled = false;

                            MessageBox.Show("Se completó la devolución. Ahora es una devolución COMPLETA.");
                        }
                    }
                    else
                    {
                        devolucionCN.CrearDevolucionParcial(
                                idPrestamoSeleccionado,
                                ElementosSeleccionados,
                                observaciones,
                                userActual.IdUsuario,
                                null
                            );

                        MessageBox.Show("Se completó la devolución. Ahora es una devolución COMPLETA.");
                    }
                }
                else
                {
                    if (idCarrito.HasValue)
                    {
                        if (DevolverCarro == true)
                        {
                            devolucionCN.CrearDevolucionParcial(
                                idPrestamoSeleccionado,
                                ElementosSeleccionados,
                                observaciones,
                                userActual.IdUsuario,
                                idCarrito
                            );

                            MessageBox.Show("Devolución PARCIAL actualizada correctamente.");

                            btnDevolverCarro.Enabled = false;
                        }
                        else
                        {
                            devolucionCN.CrearDevolucionParcial(
                                idPrestamoSeleccionado,
                                ElementosSeleccionados,
                                observaciones,
                                userActual.IdUsuario,
                                null
                            );

                            MessageBox.Show("Devolución PARCIAL actualizada correctamente.");
                        }
                    }
                    else
                    {
                        devolucionCN.CrearDevolucionParcial(
                                idPrestamoSeleccionado,
                                ElementosSeleccionados,
                                observaciones,
                                userActual.IdUsuario,
                                null
                            );

                        MessageBox.Show("Devolución PARCIAL actualizada correctamente.");
                    }
                }

                dgvElementosPorConfirmacion.Rows.Clear();
            }

            dgvElementosPorConfirmacion.Rows.Clear();
            CargarDatos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

        }

        private void cmbListadoDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (cmbListadoDetalle.SelectedIndex)
            //{
            //    case 0:
            //        dgvPrestamoDetalle.DataSource = prestamosCN.ObtenerPrestamoDetallePorId(idPrestamoSeleccionado, idCarrito);
            //        dgvPrestamoDetalle.Visible = true;
            //        break;

            //    case 1:
            //        if (idDevolcionSeleccionado == null)
            //        {
            //            dgvPrestamoDetalle.Visible = false;
            //            break;
            //        }
            //        dgvPrestamoDetalle.DataSource = devolucionCN.ObtenerDevolucionDetallePorID(Convert.ToInt32(idDevolcionSeleccionado), idCarrito);
            //        dgvPrestamoDetalle.Visible = true;
            //        break;
            //}
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            dgvElementosPorConfirmacion.Rows.Clear();

            ElementosSeleccionados.Clear();

            if (DevolverCarro == true)
            {
                DevolverCarro = false;
            }

            CargarDatos();
        }

        private void dgvPrestamoDetalle_CellClick_1(object sender, DataGridViewCellEventArgs e)
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
    }
}
