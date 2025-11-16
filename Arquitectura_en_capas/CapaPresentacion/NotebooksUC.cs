using CapaDTOs;
using CapaEntidad;
using CapaNegocio;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;
using System.Data;
using System.Text.RegularExpressions;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        private readonly NotebooksCN notebooksCN;
        private readonly CarritosCN carritosCN;
        private readonly NotebookBajasCN notebookBajasCN;
        private FormPrincipal _formPrincipal;
        private Usuarios usuarioActual;
        private int IdActual = 0;

        private readonly System.Windows.Forms.Timer Timer;
        private bool navegandoLista = false;
        private AutoCompleteStringCollection _EquipoNotebook;
        private AutoCompleteStringCollection _EquipoCarrito;
        private List<string> _listaIdentificadoresCache = new List<string>();
        private readonly int minima = 2;

        public NotebooksUC(NotebooksCN notebooksCN, Usuarios user, CarritosCN carritosCN, FormPrincipal formPrincipal, NotebookBajasCN notebookBajasCN)
        {
            InitializeComponent();

            this.notebooksCN = notebooksCN;
            this._formPrincipal = formPrincipal;
            this.usuarioActual = user;
            this.carritosCN = carritosCN;
            this.notebookBajasCN = notebookBajasCN;

            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 300;
            Timer.Tick += Timer_Tick;

            _EquipoCarrito = new AutoCompleteStringCollection();
            _EquipoNotebook = new AutoCompleteStringCollection();
        }

        #region Actualizar DATAGRIDVIEWS
        public void ActualizarDataGrid(int idEstado)
        {
            try
            {
                IEnumerable<NotebooksDTO> notebooksFiltrados;

                if (idEstado == 0)
                {
                    notebooksFiltrados = notebooksCN.GetAll();
                }
                else
                {
                    var estado = notebooksCN.ObtenerEstadoMantenimientoPorID(idEstado);
                    notebooksFiltrados = notebooksCN.ObtenerPorEstados(estado?.EstadoMantenimientoNombre);
                }

                dgvNotebooks_M.DataSource = notebooksFiltrados.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la lista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void MostrarNotebooks()
        {
            dgvNotebooks_M.DataSource = notebooksCN.GetAll();
            CargarGraficoCarritos();
            CargarGrafico();
            CargarGraficoEstados();
        }
        #endregion

        #region LOAD
        private void NotebooksUC_Load(object sender, EventArgs e)
        {

            lstSugerencias.MouseMove += (s, ev) => navegandoLista = true;
            lstSugerencias.MouseLeave += (s, ev) => navegandoLista = false;

            txtSerieBarraPatrimonio.LostFocus += (s, e) =>
            {
                if (!lstSugerencias.Focused)
                    lstSugerencias.Visible = false;
            };

            SetModoInicial();
            RenovarIdentificadores();
            CargarEstados();

            //ActualizarDataGrid(0);
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1157);

            cmbUbicacion_RemoveCarrito.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion_RemoveCarrito.ValueMember = "IdUbicacion";
            cmbUbicacion_RemoveCarrito.DisplayMember = "NombreUbicacion";

            CargarGraficoEstados();
            CargarGraficoCarritos();

            txtCarrito.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtCarrito.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtEquipo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtEquipo.AutoCompleteSource = AutoCompleteSource.CustomSource;

            CargarAutoCompleteEquipoNotebook();
            CargarAutoCompleteEquipoCarrito();
        }
        #endregion

        #region CARGAR AUTOCOMPLETES + IDENTIFICADORES (CACHE)
        private void CargarAutoCompleteEquipoNotebook()
        {
            _EquipoNotebook.Clear();
            IEnumerable<string> equipoNotebook = notebooksCN.ObtenerEquiposDeNotebooks();

            foreach (var Ac in equipoNotebook)
            {
                _EquipoNotebook.Add(Ac);
            }

            txtEquipo.AutoCompleteCustomSource = _EquipoNotebook;
        }

        private void CargarAutoCompleteEquipoCarrito()
        {
            _EquipoCarrito.Clear();
            IEnumerable<string> equipoCarrito = notebooksCN.ObtenerEquiposDeCarritos();

            foreach (var Ac in equipoCarrito)
            {
                _EquipoCarrito.Add(Ac);
            }

            txtCarrito.AutoCompleteCustomSource = _EquipoCarrito;
        }

        private void CargarEstados()
        {
            var estados = notebooksCN.ListarEstadoMantenimiento().ToList();
            estados.Insert(0, new EstadosMantenimiento { IdEstadoMantenimiento = 0, EstadoMantenimientoNombre = "Todos" });

            cmbEstados.DataSource = estados;
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";
            cmbEstados.SelectedIndex = 0;
        }

        private void RenovarIdentificadores()
        {
            try
            {
                var desdeBD = notebooksCN.ObtenerSerieBarrasPatrimonio("", 5000);
                _listaIdentificadoresCache = desdeBD.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error RenovarIdentificadores: " + ex);
            }
        }
        #endregion

        #region Modos
        private void SetModoInicial()
        {
            txtEquipo.Enabled = true;
            txtSerieBarraPatrimonio.Enabled = true;
            txtCarrito.Enabled = true;
            cmbEstados.Enabled = true;
        }

        private void SetModoBusquedaPorEquipo(bool activo)
        {
            txtEquipo.Enabled = true;
            txtSerieBarraPatrimonio.Enabled = !activo;
            txtCarrito.Enabled = !activo;
            cmbEstados.Enabled = true;
        }

        private void SetModoBusquedaPorIdentificador(bool activo)
        {
            txtSerieBarraPatrimonio.Enabled = true;
            txtEquipo.Enabled = !activo;
            txtCarrito.Enabled = !activo;
            cmbEstados.Enabled = true;
        }

        private void SetModoBusquedaPorCarrito(bool activo)
        {
            txtCarrito.Enabled = true;
            txtEquipo.Enabled = !activo;
            txtSerieBarraPatrimonio.Enabled = !activo;
            cmbEstados.Enabled = true;
        }
        #endregion

        #region Eventos TextChanged + Timer
        private void txtEquipo_TextChanged(object sender, EventArgs e)
        {
            if (!txtEquipo.Focused) return;

            string txt = txtEquipo.Text ?? "";
            bool hayTexto = !string.IsNullOrWhiteSpace(txt);

            SetModoBusquedaPorEquipo(hayTexto);

            //if (!hayTexto || txt.Length < minima)  Se comenta estos 2 metodos ya que no es necesario que el timer se ejecute cuando se navega por las sugerencias
            //{
            //    Timer.Stop();
            //    return;
            //}

            //Timer.Stop();
            //Timer.Start();
        }

        private void txtModelo_TextChanged(object sender, EventArgs e)
        {
            if (!txtCarrito.Focused) return;

            string txt = txtCarrito.Text ?? "";
            bool hayTexto = !string.IsNullOrWhiteSpace(txt);

            SetModoBusquedaPorCarrito(hayTexto);

            //if (!hayTexto || txt.Length < minima)
            //{
            //    Timer.Stop();
            //    return;
            //}

            //Timer.Stop();
            //Timer.Start();
        }

        private void txtSerieBarraPatrimonio_TextChanged(object sender, EventArgs e)
        {
            if (!txtSerieBarraPatrimonio.Focused) return;

            string txt = txtSerieBarraPatrimonio.Text ?? "";
            bool hayTexto = !string.IsNullOrWhiteSpace(txt);

            SetModoBusquedaPorIdentificador(hayTexto);

            if (!hayTexto || txt.Length < minima)
            {
                Timer.Stop();
                lstSugerencias.Visible = false;
                return;
            }

            Timer.Stop();
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();

            if (navegandoLista) return;

            if (!string.IsNullOrWhiteSpace(txtEquipo.Text) && txtEquipo.Text.Trim().Length >= minima)
            {
                EjecutarBusqueda();
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtSerieBarraPatrimonio.Text) && txtSerieBarraPatrimonio.Text.Trim().Length >= minima)
            {
                MostrarSugerenciasIdentificadores(txtSerieBarraPatrimonio.Text.Trim());
                return;
            }

            if (!string.IsNullOrWhiteSpace(txtCarrito.Text) && txtCarrito.Text.Trim().Length >= minima)
            {
                EjecutarBusqueda();
                return;
            }
        }
        #endregion

        private void MostrarSugerenciasIdentificadores(string q)
        {
            try
            {
                var sugerencias = _listaIdentificadoresCache
                    .Where(s => !string.IsNullOrEmpty(s) && s.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Take(15)
                    .ToArray();

                if (sugerencias.Length == 0)
                {
                    lstSugerencias.Visible = false;
                    return;
                }

                lstSugerencias.BeginUpdate();
                lstSugerencias.Items.Clear();
                lstSugerencias.Items.AddRange(sugerencias);
                lstSugerencias.EndUpdate();

                lstSugerencias.Width = txtSerieBarraPatrimonio.Width;
                lstSugerencias.Location = new Point(txtSerieBarraPatrimonio.Left, txtSerieBarraPatrimonio.Bottom);
                lstSugerencias.Visible = true;
                navegandoLista = false;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR En las sugerencias");
            }
        }

        private void txtSerieBarraPatrimonio_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugerencias.Visible) return;

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (lstSugerencias.SelectedIndex < lstSugerencias.Items.Count - 1)
                    lstSugerencias.SelectedIndex++;
                else
                    lstSugerencias.SelectedIndex = 0;
                navegandoLista = true;
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (lstSugerencias.SelectedIndex > 0)
                    lstSugerencias.SelectedIndex--;
                else
                    lstSugerencias.SelectedIndex = lstSugerencias.Items.Count - 1;
                navegandoLista = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (lstSugerencias.SelectedItem != null)
                    SeleccionarSugerencia(lstSugerencias.SelectedItem.ToString());
                else
                    EjecutarBusqueda();
                lstSugerencias.Visible = false;
                navegandoLista = false;
                return;
            }

            navegandoLista = false;
            Timer.Stop();
            Timer.Start();
        }

        private void LstSugerencias_Click(object sender, EventArgs e)
        {
            if (lstSugerencias.SelectedItem != null)
                SeleccionarSugerencia(lstSugerencias.SelectedItem.ToString());
        }

        private void SeleccionarSugerencia(string valor)
        {
            if (valor == null) return;
            txtSerieBarraPatrimonio.Text = valor;
            txtSerieBarraPatrimonio.SelectionStart = txtSerieBarraPatrimonio.Text.Length;
            lstSugerencias.Items.Clear();
            EjecutarBusqueda();
            lstSugerencias.Visible = false;
        }

        #region Botones
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            EjecutarBusqueda();
            navegandoLista = false;
        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e)
        {
            txtEquipo.Clear();
            txtSerieBarraPatrimonio.Clear();
            txtCarrito.Clear();
            lstSugerencias.Items.Clear();
            lstSugerencias.Visible = false;
            RenovarIdentificadores();
            SetModoInicial();
            ActualizarDataGrid(0);
        }
        #endregion

        private void EjecutarBusqueda()
        {
            string equipoQ = string.IsNullOrWhiteSpace(txtEquipo.Text) ? null : txtEquipo.Text.Trim();
            string identificadorQ = string.IsNullOrWhiteSpace(txtSerieBarraPatrimonio.Text) ? null : txtSerieBarraPatrimonio.Text.Trim();
            string carritoQ = string.IsNullOrWhiteSpace(txtCarrito.Text) ? null : txtCarrito.Text.Trim();
            int estadoId = cmbEstados.SelectedValue != null ? Convert.ToInt32(cmbEstados.SelectedValue) : 0;


            if (!string.IsNullOrWhiteSpace(equipoQ))
            {
                var resultados = notebooksCN.ObtenerPorFiltros(null, null, equipoQ);
                dgvNotebooks_M.DataSource = resultados.ToList();
                return;
            }

            if (!string.IsNullOrWhiteSpace(identificadorQ))
            {
                var resultados = notebooksCN.ObtenerPorFiltros(identificadorQ, null, null);
                dgvNotebooks_M.DataSource = resultados.ToList();
                return;
            }

            if (!string.IsNullOrWhiteSpace(carritoQ))
            {
                var carrito = notebooksCN.ObtenerCarritoPorEquipo(carritoQ);
                var resultados = notebooksCN.ObtenerPorFiltros(null ,carrito?.IdCarrito ,null);
                dgvNotebooks_M.DataSource = resultados.ToList();
                return;
            }

            if (estadoId == 0)
                MostrarNotebooks();
            else
                ActualizarDataGrid(estadoId);
        }

        private void dtgNotebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;

            IdActual = Convert.ToInt32(dgvNotebooks_M.Rows[e.RowIndex].Cells["IdNotebook"].Value);

            Notebooks? notebook = notebooksCN.ObtenerNotebookPorID(IdActual);

            #region Agregar cellClick para agregar al carrito
            txtEquipo_AddCarrito.Text = notebook?.Equipo;
            #endregion

            #region Quitar CellClick para quitar del carrito
            txtEquipo_RemoveCarrito.Text = notebook?.Equipo;
            txtCarrito_RemoveCarrito.Text = notebook?.IdCarrito.HasValue == true ? notebooksCN.ObtenerCarritoPorID(notebook.IdCarrito.Value)?.EquipoCarrito : "Sin Carrito";
            #endregion
        }

        //ActualizarDataGrid();
        //CargarGrafico();
        //CargarGraficoEstados();
        //CargarGraficoCarritos();

        private void btnCrearNotebook_M_Click(object sender, EventArgs e)
        {
            var Notebook = new FormCRUDNotebook(notebooksCN, MostrarNotebooks, CargarGrafico, usuarioActual);
            Notebook.ShowDialog();
        }
        private void btnAgragarAlCarrito_Click(object sender, EventArgs e)
        {
            Carritos? carrito = carritosCN.ObtenerCarritoPorEquipo(txtCarrito_AddCarrito.Text);
            int posicion = Convert.ToInt32(txtPosicion.Text);

            if (carrito == null)
            {
                MessageBox.Show("El carrito no existe.");
                return;
            }

            carritosCN.AddNotebook(carrito.IdCarrito, posicion, IdActual, usuarioActual.IdUsuario);
            ActualizarDataGrid(0);
        }

        private void btnRemoveCarrito_Click(object sender, EventArgs e)
        {
            Carritos? carrito = carritosCN.ObtenerCarritoPorEquipo(txtCarrito_RemoveCarrito.Text);
            if (carrito == null)
            {
                MessageBox.Show("El carrito no existe.");
                return;
            }

            int idUbicacion = cmbUbicacion_RemoveCarrito.SelectedValue != null ? (int)cmbUbicacion_RemoveCarrito.SelectedValue : 0;

            carritosCN.RemoveNotebook(carrito.IdCarrito, IdActual, usuarioActual.IdUsuario, idUbicacion);

            ActualizarDataGrid(0);
        }

        #region GRAFICOS
        private void CargarGrafico()
        {
            List<(string Modelo, int Cantidad)> datos = notebooksCN.GetCantidadPorModelo();

            if (datos == null || datos.Count == 0) return;

            var series = new List<PieSeries<int>>();
            foreach (var d in datos)
            {
                series.Add(new PieSeries<int>
                {
                    Values = new int[] { d.Cantidad },
                    Name = d.Modelo,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 14
                });
            }

            PieChart pieChart = new PieChart
            {
                Dock = DockStyle.Fill,
                Series = series.ToArray()
            };

            pnlGraficoPie.Controls.Clear();
            pnlGraficoPie.Controls.Add(pieChart);
        }

        private void CargarGraficoEstados()
        {
            var datos = notebooksCN.GetCantidadEstado();

            if (datos == null || datos.Count == 0) return;

            var series = new List<PieSeries<int>>();

            foreach (var d in datos)
            {
                SKColor color = d.Estado switch
                {
                    "Disponible" => SKColors.GreenYellow,
                    "Prestado" => SKColors.Red,
                    "En mantenimiento" => SKColors.Yellow,
                    _ => SKColors.Gray
                };

                series.Add(new PieSeries<int>
                {
                    Values = new int[] { d.Cantidad },
                    Name = d.Estado,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 14,
                    Fill = new SolidColorPaint(color),
                });
            }

            var pieChart = new PieChart
            {
                Dock = DockStyle.Fill,
                Series = series.ToArray(),
            };

            pnlGraficoPie.Controls.Clear();
            pnlGraficoPie.Controls.Add(pieChart);
        }

        private void CargarGraficoCarritos()
        {
            var datos = notebooksCN.GetCantidadNotebooksEnCarritos();

            if (datos == null || datos.Count == 0) return;

            var datosOrdenados = datos
                .Select(d => new
                {
                    Original = d,
                    KeyNumber = ExtractTrailingNumber(d.Equipo),
                    KeyString = d.Equipo
                })
                .OrderBy(x => x.KeyNumber.HasValue ? 0 : 1)
                .ThenBy(x => x.KeyNumber ?? int.MaxValue)
                .ThenBy(x => x.KeyString)
                .Select(x => x.Original)
                .ToList();

            var labels = datosOrdenados.Select(d => d.Equipo).ToArray();
            var values = datosOrdenados.Select(d => d.Cantidad == 0 ? 0.1 : (double)d.Cantidad).ToArray();

            var series = new ColumnSeries<double>
            {
                Values = values,
                DataLabelsPaint = new SolidColorPaint(new SKColor(60, 60, 60)),
                DataLabelsSize = 15,
                MaxBarWidth = 18,
                Padding = 3,
                Stroke = null,
                Fill = new SolidColorPaint(new SKColor(70, 130, 180)),
                DataLabelsFormatter = point =>
                {
                    int real = (int)datosOrdenados[point.Index].Cantidad;
                    return real.ToString();
                }
            };

            var chart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                Series = new ISeries[] { series },

                XAxes = new Axis[]
                {
            new Axis
            {
                Labels = labels,
                TextSize = 0,
                //Padding = new LiveChartsCore.Drawing.Padding { Left = 5, Right = 5, Top = 5, Bottom = 5 },
                //NameTextSize = 13,
                //NamePaint = new SolidColorPaint(new SKColor(80, 80, 80)),
                //LabelsRotation = 20,
                ForceStepToMin = false
            }
                },

                YAxes = new Axis[]
                {
            new Axis
            {
                Labeler = value => value.ToString("N0"),
                NameTextSize = 0,
                NamePaint = new SolidColorPaint(new SKColor(80, 80, 80)),
                TextSize = 12
            }
                },

                LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden,
            };

            pnlGraficoCarritos.Controls.Clear();
            pnlGraficoCarritos.Controls.Add(chart);
        }

        /// <summary>
        /// Intenta identificar un numero entero del label. 
        /// Devuelve int? con el número encontrado (primero que aparezca), o null si no hay números.
        /// Maneja "1", "Carrito 1", "Equipo-01", "C-12A", etc.
        /// </summary>
        private int? ExtractTrailingNumber(string label)
        {
            if (string.IsNullOrWhiteSpace(label)) return null;

            var m = Regex.Match(label, @"\d+");
            if (!m.Success) return null;

            if (int.TryParse(m.Value, out int n))
                return n;

            return null;
        }
        #endregion

        private void btnGestionarNotebook_M_Click(object sender, EventArgs e)
        {
            var detalleUC = new NotebookGestionUC(_formPrincipal, this, notebooksCN, IdActual, usuarioActual, notebookBajasCN);
            _formPrincipal.MostrarUserControl(detalleUC);
        }

        private void btnVerEstadoGrafico_Click(object sender, EventArgs e)
        {
            CargarGraficoEstados();
        }

        private void btnVerModeloGrafico_Click(object sender, EventArgs e)
        {
            CargarGrafico();
        }

        private void cmbEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstados.SelectedValue == null) return;

            try
            {
                int idEstado = Convert.ToInt32(cmbEstados.SelectedValue);
                ActualizarDataGrid(idEstado);
            }
            catch { }
        }
    }
}
