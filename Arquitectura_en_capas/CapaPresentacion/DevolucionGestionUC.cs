using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class DevolucionGestionUC : UserControl
    {
        private readonly PrestamosYDevolucionesUC prestamosYDevolucionesUC;
        private readonly FormPrincipal _formPrincipal;
        private readonly PrestamosCN prestamosCN;
        private Usuarios userActual;
        private readonly DevolucionCN devolucionCN;
        private int idPrestamoSeleccionado;

        private List<int> ElementosSeleccionados = new List<int>();
        private List<int> ElementosDevueltos = new List<int>();
        private int? idCarrito;

        public DevolucionGestionUC(
            PrestamosYDevolucionesUC prestamosYDevolucionesUC,
            FormPrincipal _formPrincipal,
            PrestamosCN prestamosCN,
            Usuarios userActual,
            DevolucionCN devolucionCN,
            int idPrestamoSeleccionado)
        {
            InitializeComponent();

            this.prestamosYDevolucionesUC = prestamosYDevolucionesUC;
            this._formPrincipal = _formPrincipal;
            this.prestamosCN = prestamosCN;
            this.userActual = userActual;
            this.devolucionCN = devolucionCN;
            this.idPrestamoSeleccionado = idPrestamoSeleccionado;

            // Evitar que la selección tape el color real
            dgvPrestamoDetalle.DefaultCellStyle.SelectionBackColor = dgvPrestamoDetalle.DefaultCellStyle.BackColor;
            dgvPrestamoDetalle.DefaultCellStyle.SelectionForeColor = dgvPrestamoDetalle.DefaultCellStyle.ForeColor;
        }

        private void DevolucionGestionUC_Load(object sender, EventArgs e)
        {
            IniciarTemporizador();
            CargarDatos();
        }

        private void dgvPrestamoDetalle_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPrestamoDetalle.IsCurrentCellDirty)
                dgvPrestamoDetalle.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void CargarDatos()
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            idCarrito = prestamo?.IdCarrito;

            var lista = prestamosCN.ObtenerPrestamoDetallePorId(idPrestamoSeleccionado, idCarrito);
            dgvPrestamoDetalle.DataSource = lista;

            Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);

            if (devolucion != null)
            {
                ElementosDevueltos = devolucionCN.ObtenerIDsElementosEnDev(Convert.ToInt32(devolucion?.IdDevolucion));

                Usuarios? usuariosL = prestamosCN.ObtenerUsuarioPorID(Convert.ToInt32(devolucion?.IdUsuario));

                lblEncargadoR.Text = usuariosL?.Nombre + " " + usuariosL?.Apellido;

                AplicarToolTip(lblEncargadoR);
            }

            // El grid se mantiene editable, pero controlamos manualmente
            dgvPrestamoDetalle.ReadOnly = false;

            // Solo permitir edición en Devuelto y ObservacionDevolucion
            foreach (DataGridViewColumn col in dgvPrestamoDetalle.Columns)
            {
                if (col.Name != "Devuelto" && col.Name != "ObservacionDevolucion")
                    col.ReadOnly = true;
            }

            Docentes? docentes = prestamosCN.ObtenerDocentePorID(Convert.ToInt32(prestamo?.IdDocente));

            lblDocente.Text = docentes?.Nombre + " " + docentes?.Apellido;

            AplicarToolTip(lblDocente);

            Curso? curso = prestamosCN.ObtenerCursoPorID(Convert.ToInt32(prestamo?.IdCurso));

            lblCurso.Text = curso?.NombreCurso;

            AplicarToolTip(lblCurso);

            Usuarios? usuarios = prestamosCN.ObtenerUsuarioPorID(Convert.ToInt32(prestamo?.IdUsuario));

            lblEncargadoL.Text = usuarios?.Nombre + " " + usuarios?.Apellido;

            AplicarToolTip(lblEncargadoL);

            lblFecha.Text = Convert.ToString(prestamo?.FechaPrestamo);

            AplicarToolTip(lblFecha);

            EstadosPrestamo? estadosPrestamo = prestamosCN.ObtenerEstadoPrestamoPorID(Convert.ToInt32(prestamo?.IdEstadoPrestamo));

            lblEstado.Text = estadosPrestamo?.EstadoPrestamo;

            AplicarToolTip(lblEstado);

            if (prestamo?.IdCarrito != null)
            {
                Carritos? carritos = prestamosCN.ObtenerCarritoPorID(Convert.ToInt32(prestamo.IdCarrito));

                lblCarrito.Text = carritos?.EquipoCarrito;

                AplicarToolTip(lblCarrito);
            }

            // Reconfigurar según estado
            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                bool devuelto = Convert.ToBoolean(row.Cells["Devuelto"].Value);

                if (devuelto)
                {
                    // No permitir tocar nada
                    row.Cells["Devuelto"].ReadOnly = true;
                    row.Cells["ObservacionDevolucion"].ReadOnly = false; // PERO el bloque real es en CellBeginEdit
                }
                else
                {
                    row.Cells["Devuelto"].ReadOnly = false;
                    row.Cells["ObservacionDevolucion"].ReadOnly = false;
                }
            }

            dgvPrestamoDetalle.Refresh();
            ActualizarEstadoBotones();
        }

        private void IniciarTemporizador()
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo == null) return;
            timerPrestamo.Start();
        }

        private void TimerPrestamo_Tick(object? sender, EventArgs e)
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo == null)
            {
                timerPrestamo.Stop();
                lblTiempoTranscurrido.Text = "00:00:00";
                return;
            }

            DateTime fechaReferencia;

            // Verificamos si ya se devolvió todo
            if (prestamo.IdEstadoPrestamo == 2) // 2 = devuelto completo
            {
                // Tomamos la fecha de devolución final
                Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);
                fechaReferencia = Convert.ToDateTime(devolucion?.FechaDevolucion);

                // Detenemos el timer, ya no necesitamos actualizar
                timerPrestamo.Stop();

                TimeSpan tiempoTranscurrido = fechaReferencia - prestamo.FechaPrestamo;

                int horas = (int)tiempoTranscurrido.TotalHours;
                int minutos = tiempoTranscurrido.Minutes;
                int segundos = tiempoTranscurrido.Seconds;

                lblTiempoTranscurrido.Text = $"{horas:D2}:{minutos:D2}:{segundos:D2}";
            }
            else
            {
                // Mientras no esté completo → mostrar tiempo desde fechaPrestamo hasta ahora
                fechaReferencia = DateTime.Now;

                TimeSpan tiempoTranscurrido = fechaReferencia - prestamo.FechaPrestamo;

                int horas = (int)tiempoTranscurrido.TotalHours;
                int minutos = tiempoTranscurrido.Minutes;
                int segundos = tiempoTranscurrido.Seconds;

                lblTiempoTranscurrido.Text = $"{horas:D2}:{minutos:D2}:{segundos:D2}";
            }

            
        }

        private void dgvPrestamoDetalle_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvPrestamoDetalle.Columns[e.ColumnIndex].Name;

            int idElem = Convert.ToInt32(
                dgvPrestamoDetalle.Rows[e.RowIndex].Cells["idElemento"].Value
            );

            // 🔒 BLOQUEO TOTAL: Si ya fue devuelto → no permitir tocar nada
            if (ElementosDevueltos.Contains(idElem))
            {
                e.Cancel = true;
                return;
            }

            // Si no fue devuelto, Observación es editable siempre (no hacemos nada especial)
        }

        // Pintar filas segun estado
        private void dgvPrestamoDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idElem = Convert.ToInt32(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["idElemento"].Value);

            if (ElementosDevueltos.Contains(idElem))
            {
                e.CellStyle.BackColor = Color.LightGreen;
                e.CellStyle.ForeColor = Color.Black;
            }
            else if (ElementosSeleccionados.Contains(idElem))
            {
                e.CellStyle.BackColor = Color.Gold;
                e.CellStyle.ForeColor = Color.Black;
            }
            else
            {
                e.CellStyle.BackColor = dgvPrestamoDetalle.DefaultCellStyle.BackColor;
                e.CellStyle.ForeColor = dgvPrestamoDetalle.DefaultCellStyle.ForeColor;
            }

            // Mantener color en selección
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }

        private void dgvPrestamoDetalle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvPrestamoDetalle.Columns[e.ColumnIndex].Name == "Devuelto")
                dgvPrestamoDetalle.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvPrestamoDetalle_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvPrestamoDetalle.Columns[e.ColumnIndex].Name != "Devuelto") return;

            var row = dgvPrestamoDetalle.Rows[e.RowIndex];
            int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
            bool isChecked = Convert.ToBoolean(row.Cells["Devuelto"].Value);

            if (isChecked)
            {
                if (!ElementosSeleccionados.Contains(idElem))
                    ElementosSeleccionados.Add(idElem);
            }
            else
            {
                ElementosSeleccionados.Remove(idElem);
            }

            dgvPrestamoDetalle.InvalidateRow(e.RowIndex);
            ActualizarEstadoBotones();
        }

        private void btnConfirmarDevolucion_Click(object sender, EventArgs e)
        {
            if (ElementosSeleccionados.Count == 0)
            {
                MessageBox.Show("No se seleccionaron elementos para devolver.");
                return;
            }

            // Recolectar observaciones
            List<string> observaciones = new();
            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                if (ElementosSeleccionados.Contains(idElem))
                {
                    string? obs = Convert.ToString(row.Cells["ObservacionDevolucion"].Value);
                    if (string.IsNullOrWhiteSpace(obs)) obs = "Sin observación";
                    observaciones.Add(obs);
                }
            }

            // ------------------------------
            // REGISTRO DE DEVOLUCIÓN
            // ------------------------------
            Devolucion? devExistente = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);

            if (devExistente == null)
            {
                Devolucion nueva = new Devolucion
                {
                    IdPrestamo = idPrestamoSeleccionado,
                    IdUsuario = userActual.IdUsuario,
                    FechaDevolucion = DateTime.Now
                };

                // Ya no se pasa idCarrito
                devolucionCN.CrearDevolucion(
                    nueva,
                    ElementosSeleccionados,
                    observaciones,
                    userActual.IdUsuario
                );
            }
            else
            {
                // Ya no se pasa idCarrito
                devolucionCN.CrearDevolucionParcial(
                    idPrestamoSeleccionado,
                    ElementosSeleccionados,
                    observaciones,
                    userActual.IdUsuario
                );
            }

            MessageBox.Show("Devolución registrada correctamente.");
            ElementosSeleccionados.Clear();
            CargarDatos();
        }

        // ------------------------------
        // DEVOLVER TODOS
        // ------------------------------
        private void btnDevolverTodos_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                if (ElementosDevueltos.Contains(idElem)) continue;

                row.Cells["Devuelto"].Value = true;

                if (!ElementosSeleccionados.Contains(idElem))
                    ElementosSeleccionados.Add(idElem);
            }

            dgvPrestamoDetalle.Refresh();
            ActualizarEstadoBotones();
        }

        // ------------------------------
        // DEVOLVER CARRO (solo marca elementos, no toca idCarrito)
        // ------------------------------
        private void btnDevolverCarrito_Click(object sender, EventArgs e)
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo?.IdCarrito == null)
            {
                MessageBox.Show("Este préstamo no está asociado a un carrito.");
                return;
            }

            var idsCarrito = prestamosCN.ObtenerIDsPorCarrito(prestamo.IdCarrito.Value).ToList();
            var idsPrestamo = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();
            var notebooks_carrito_en_prestamo = idsCarrito.Intersect(idsPrestamo).ToList();

            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                if (!notebooks_carrito_en_prestamo.Contains(idElem)) continue;
                if (ElementosDevueltos.Contains(idElem)) continue;

                row.Cells["Devuelto"].Value = true;
                if (!ElementosSeleccionados.Contains(idElem))
                    ElementosSeleccionados.Add(idElem);
            }

            dgvPrestamoDetalle.Refresh();
            ActualizarEstadoBotones();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados.Clear();
            CargarDatos();
        }

        private void AplicarToolTip(Label label)
        {
            string texto = string.IsNullOrWhiteSpace(label.Text) ? "" : label.Text;
            tltDatos.SetToolTip(label, texto);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            prestamosYDevolucionesUC.ActualizarDataGrid();
            _formPrincipal.MostrarUserControl(prestamosYDevolucionesUC);
        }

        private void ActualizarEstadoBotones()
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo == null) return;

            // Botón devolver todos
            // Solo habilitado si hay elementos no devueltos
            // Obtenemos todos los IDs de elementos prestados en este préstamo
            var idsPrestados = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();

            // Comparamos con los IDs ya devueltos
            bool hayElementosPendientes = idsPrestados.Except(ElementosDevueltos).Any();

            // Habilitamos o deshabilitamos el botón Devolver Todos
            btnDevolverTodo.Enabled = hayElementosPendientes;


            // Botón devolver carrito
            // Solo habilitado si hay un carrito asociado y todavía hay notebooks del carrito no devueltas
            bool tieneCarrito = prestamo.IdCarrito != null;
            bool faltanDelCarrito = false;

            if (tieneCarrito)
            {
                var idsCarrito = prestamosCN.ObtenerIDsPorCarrito(Convert.ToInt32(prestamo.IdCarrito)).ToList();
                var idsPrestamo = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();
                var notebooks_carrito_en_prestamo = idsCarrito.Intersect(idsPrestamo).ToList();
                faltanDelCarrito = notebooks_carrito_en_prestamo.Any(id => !ElementosDevueltos.Contains(id) && !ElementosSeleccionados.Contains(id));
            }
            btnDevolverCarro.Enabled = tieneCarrito && faltanDelCarrito;

            // Botón confirmar
            // Solo habilitado si hay al menos un elemento seleccionado
            btnConfirmar.Enabled = ElementosSeleccionados.Any();

            // Botón cancelar
            // Solo habilitado si hay al menos un elemento seleccionado
            btnCancelar.Enabled = ElementosSeleccionados.Any();

            // Si el préstamo está completo → bloquear todos
            if (prestamo.IdEstadoPrestamo == 2)
            {
                btnConfirmar.Enabled = false;
                btnDevolverCarro.Enabled = false;
                btnDevolverTodo.Enabled = false;
                btnCancelar.Enabled = false;
            }
        }

    }
}
