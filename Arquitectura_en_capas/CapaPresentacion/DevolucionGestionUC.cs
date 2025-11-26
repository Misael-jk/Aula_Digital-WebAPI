using CapaDTOs;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class DevolucionGestionUC : UserControl
    {
        private readonly FormPrincipal _formPrincipal;
        private readonly UserControl ucAnterior;
        private readonly PrestamosCN prestamosCN;
        private Usuarios userActual;
        private readonly DevolucionCN devolucionCN;
        private int idPrestamoSeleccionado;

        private List<int> ElementosSeleccionados = new List<int>();
        private List<int> ElementosDevueltos = new List<int>();
        private int? idCarrito;

        public DevolucionGestionUC(FormPrincipal _formPrincipal, PrestamosCN prestamosCN, Usuarios userActual, DevolucionCN devolucionCN, int idPrestamoSeleccionado, UserControl ucAnterior)
        {
            InitializeComponent();
            this._formPrincipal = _formPrincipal;
            this.prestamosCN = prestamosCN;
            this.userActual = userActual;
            this.devolucionCN = devolucionCN;
            this.idPrestamoSeleccionado = idPrestamoSeleccionado;
            this.ucAnterior = ucAnterior;

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
            {
                dgvPrestamoDetalle.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        #region CARGAR DATOS
        private void CargarDatos()
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            idCarrito = prestamo?.IdCarrito;

            IEnumerable<PrestamosDetalleDTO> lista = prestamosCN.ObtenerPrestamoDetallePorId(idPrestamoSeleccionado, idCarrito);
            dgvPrestamoDetalle.DataSource = lista;

            dgvPrestamoDetalle.ReadOnly = false;

            // Se aplica tooltip a los datos relacionados con la devolucion, tooltip a los labes
            if(prestamo is not null)
            ToolTip(prestamo);

            // Solo permitir edición en Devuelto y ObservacionDevolucion
            foreach (DataGridViewColumn col in dgvPrestamoDetalle.Columns)
            {
                if (col.Name != "Devuelto" && col.Name != "ObservacionDevolucion")
                    col.ReadOnly = true;
            }


            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                bool devuelto = Convert.ToBoolean(row.Cells["Devuelto"].Value);

                if (devuelto)
                {
                    row.Cells["Devuelto"].ReadOnly = true;
                    row.Cells["ObservacionDevolucion"].ReadOnly = false; 
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
        #endregion

        #region TOOLTIP
        private void ToolTip(Prestamos prestamo)
        {
            // Docentes
            Docentes? docentes = prestamosCN.ObtenerDocentePorID(Convert.ToInt32(prestamo?.IdDocente));
            lblDocente.Text = docentes?.Nombre + " " + docentes?.Apellido;
            AplicarToolTip(lblDocente);

            // Curso
            Curso? curso = prestamosCN.ObtenerCursoPorID(Convert.ToInt32(prestamo?.IdCurso));
            lblCurso.Text = curso?.NombreCurso;
            AplicarToolTip(lblCurso);

            // Usuarios
            Usuarios? usuarios = prestamosCN.ObtenerUsuarioPorID(Convert.ToInt32(prestamo?.IdUsuario));
            lblEncargadoL.Text = usuarios?.Nombre + " " + usuarios?.Apellido;
            AplicarToolTip(lblEncargadoL);

            // Fecha
            lblFecha.Text = Convert.ToString(prestamo?.FechaPrestamo);
            AplicarToolTip(lblFecha);

            // Estado
            EstadosPrestamo? estadosPrestamo = prestamosCN.ObtenerEstadoPrestamoPorID(Convert.ToInt32(prestamo?.IdEstadoPrestamo));
            lblEstado.Text = estadosPrestamo?.EstadoPrestamo;
            AplicarToolTip(lblEstado);

            // Carrito
            if (prestamo?.IdCarrito != null)
            {
                Carritos? carritos = prestamosCN.ObtenerCarritoPorID(Convert.ToInt32(prestamo.IdCarrito));
                lblCarrito.Text = carritos?.EquipoCarrito;
                AplicarToolTip(lblCarrito);
            }

            // Devolucion
            Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);
            if (devolucion != null)
            {
                ElementosDevueltos = devolucionCN.ObtenerIDsElementosEnDev(Convert.ToInt32(devolucion?.IdDevolucion));
                Usuarios? usuariosL = prestamosCN.ObtenerUsuarioPorID(Convert.ToInt32(devolucion?.IdUsuario));
                lblEncargadoR.Text = usuariosL?.Nombre + " " + usuariosL?.Apellido;
                AplicarToolTip(lblEncargadoR);
            }
        }

        private void AplicarToolTip(Label label)
        {
            string texto = string.IsNullOrWhiteSpace(label.Text) ? "" : label.Text;
            tltDatos.SetToolTip(label, texto);
        }
        #endregion

        #region TIMER
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

            /* 
             * Esta condicion verificamos que el prestamo este devuelto, para luego obtener la fecha con su
             * hora, minutos y segundos para luego finalizar el timer, en caso de que el prestamo siga en curso,
             * es decir, que sea != 2 va a seguir contando la fecha actual hasta que se realice la devolucion.
             */
            if (prestamo.IdEstadoPrestamo == 2) // 2 = devuelto completo
            {
                Devolucion? devolucion = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);
                fechaReferencia = Convert.ToDateTime(devolucion?.FechaDevolucion);

                timerPrestamo.Stop();

                TimeSpan tiempoTranscurrido = fechaReferencia - prestamo.FechaPrestamo;

                int horas = (int)tiempoTranscurrido.TotalHours;
                int minutos = tiempoTranscurrido.Minutes;
                int segundos = tiempoTranscurrido.Seconds;

                lblTiempoTranscurrido.Text = $"{horas:D2}:{minutos:D2}:{segundos:D2}";
            }
            else
            {
                fechaReferencia = DateTime.Now;

                TimeSpan tiempoTranscurrido = fechaReferencia - prestamo.FechaPrestamo;

                int horas = (int)tiempoTranscurrido.TotalHours;
                int minutos = tiempoTranscurrido.Minutes;
                int segundos = tiempoTranscurrido.Seconds;

                lblTiempoTranscurrido.Text = $"{horas:D2}:{minutos:D2}:{segundos:D2}";
            }
        }
        #endregion

        #region DATAGRIDVIEW EVENTS || FORMATTING Y EDITING
        private void dgvPrestamoDetalle_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgvPrestamoDetalle.Columns[e.ColumnIndex].Name;
            int idElem = Convert.ToInt32(dgvPrestamoDetalle.Rows[e.RowIndex].Cells["idElemento"].Value);

            // Verificamos que los elementos ya devueltos no se puedan editar
            if (ElementosDevueltos.Contains(idElem))
            {
                e.Cancel = true;
                return;
            }
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

            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }
        #endregion

        #region DATAGRIDVIEW EVENTOS || Modificacion en los valores de la celda
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
        #endregion

        #region BOTON DE CONFIRMACION DE DEVOLUCION
        private void btnConfirmarDevolucion_Click(object sender, EventArgs e)
        {
            if (ElementosSeleccionados.Count == 0)
            {
                MessageBox.Show("No se seleccionaron elementos para devolver.");
                return;
            }

            // Recorremos el DataGridView para obtener las observaciones sino hay se asigna "Sin observación"
            List<string> observaciones = new List<string>();

            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                if (ElementosSeleccionados.Contains(idElem))
                {
                    string? obs = Convert.ToString(row.Cells["ObservacionDevolucion"].Value);
                    if (string.IsNullOrWhiteSpace(obs))
                    {
                        obs = "Sin observación";
                    }
                    observaciones.Add(obs);
                }
            }

            // Se realiza una nueva devolucion o una parcial segun corresponda
            Devolucion? devExistente = devolucionCN.ObtenerDevolucionPorIdPrestamo(idPrestamoSeleccionado);
            if (devExistente == null)
            {
                Devolucion nueva = new Devolucion
                {
                    IdPrestamo = idPrestamoSeleccionado,
                    IdUsuario = userActual.IdUsuario,
                    FechaDevolucion = DateTime.Now
                };

                devolucionCN.CrearDevolucion(nueva, ElementosSeleccionados, observaciones, userActual.IdUsuario);
            }
            else
            {
                devolucionCN.CrearDevolucionParcial(idPrestamoSeleccionado, ElementosSeleccionados, observaciones, userActual.IdUsuario);
            }

            MessageBox.Show("Devolución registrada correctamente.");
            ElementosSeleccionados.Clear();
            CargarDatos();
        }
        #endregion

        #region BOTON PARA SELECCIONAR A TODOS LOS ELEMENTOS PARA DEVOLVER
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
        #endregion

        #region BOTON PARA SELECCIONAR SOLO LOS ELEMENTOS DEL CARRITO PARA DEVOLVER
        private void btnDevolverCarrito_Click(object sender, EventArgs e)
        {
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo?.IdCarrito == null)
            {
                MessageBox.Show("Este préstamo no está asociado a un carrito.");
                return;
            }

            List<int> idsCarrito = prestamosCN.ObtenerIDsPorCarrito(prestamo.IdCarrito.Value).ToList();
            List<int> idsPrestamo = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();
            // OJO: selecciona el id de las notebooks que estan tanto en el carrito como en el prestamo, no a los ids del carrito.
            List<int> notebooks_carrito_en_prestamo = idsCarrito.Intersect(idsPrestamo).ToList();

            foreach (DataGridViewRow row in dgvPrestamoDetalle.Rows)
            {
                int idElem = Convert.ToInt32(row.Cells["idElemento"].Value);
                if (!notebooks_carrito_en_prestamo.Contains(idElem)) continue;
                if (ElementosDevueltos.Contains(idElem)) continue;

                row.Cells["Devuelto"].Value = true;
                if (!ElementosSeleccionados.Contains(idElem))
                {
                    ElementosSeleccionados.Add(idElem);
                }
            }

            dgvPrestamoDetalle.Refresh();
            ActualizarEstadoBotones();
        }
        #endregion

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ElementosSeleccionados.Clear();
            CargarDatos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            if(ucAnterior is PrestamosYDevolucionesUC prestamosDevolucionesUC)
            {
                prestamosDevolucionesUC.ActualizarDataGrid();
            }
            else if(ucAnterior is Dashboard dashboard)
            {
                dashboard.MostrarDatos();
            }

            _formPrincipal.MostrarUserControl(ucAnterior);
        }

        private void ActualizarEstadoBotones()
        {
            #region Habilitamos o Deshabilitamos el BOTON DE SELECIONAR A TODOS LOS ELEMENTOS
            Prestamos? prestamo = prestamosCN.ObtenerPrestamoPorID(idPrestamoSeleccionado);
            if (prestamo == null) return;

            /*
             * se obtienen los ids de los elementos prestados en el prestamo seleccionado, para luego verificar
             * con Except() si hay elementos que no estan en la lista de ElementosDevueltos, si hay al menos uno
             * devuelve true sino false.
             */
            List<int> idsPrestados = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();
            bool hayElementosPendientes = idsPrestados.Except(ElementosDevueltos).Any();

            // Habilitamos o deshabilitamos el boton Devolver Todos
            btnDevolverTodo.Enabled = hayElementosPendientes;
            #endregion

            #region Habilitamos o Deshabilitamos el BOTON DE SELECIONAR SOLO LOS ELEMENTOS DEL CARRITO
            /*
             * El boton se habilita si:
             * 1. El prestamo actual esta asociado a un carrito, es decir que el IdCarrito no es null
             * 2. Todavia existen elementos que estaban en ese carrito Y en el prestamo
             */
            bool tieneCarrito = prestamo.IdCarrito != null;
            bool faltanDelCarrito = false;

            if (tieneCarrito)
            {
                List<int> idsCarrito = prestamosCN.ObtenerIDsPorCarrito(Convert.ToInt32(prestamo.IdCarrito)).ToList();
                List<int> idsPrestamo = prestamosCN.ObtenerIDsElementosPorPrestamo(idPrestamoSeleccionado).ToList();
                List<int> notebooks_carrito_en_prestamo = idsCarrito.Intersect(idsPrestamo).ToList();
                // Verifica si ALGUNO de esos elementos sigue sin ser devuelto o seleccionado
                faltanDelCarrito = notebooks_carrito_en_prestamo.Any(id => !ElementosDevueltos.Contains(id) && !ElementosSeleccionados.Contains(id));
            }
            // Habilita el boton solo si hay un carrito asociado Y faltan elementos por gestionar de ese carrito.
            btnDevolverCarro.Enabled = tieneCarrito && faltanDelCarrito;
            #endregion

            // Boton de confirmar, solo se habilita si hay al menos un elemento seleccionado
            btnConfirmar.Enabled = ElementosSeleccionados.Any();

            // Boton de cancelar, solo habilitado si hay al menos un elemento seleccionado
            btnCancelar.Enabled = ElementosSeleccionados.Any();

            // si el prestamo esta completo se deshabilita todo
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
