using CapaDatos.Interfaces;
using CapaEntidad;
using CapaNegocio;
using CapaDTOs;
using System.Diagnostics;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;


namespace CapaPresentacion
{
    public partial class ElementosUC : UserControl
    {
        private readonly ElementosCN elementosCN;
        private readonly TiposElementoCN tiposElementoCN;
        private readonly ElementosBajasCN elementosBajasCN;
        private readonly Usuarios userVerificado;
        private readonly IRepoEstadosMantenimiento repoEstadosMantenimiento;
        private FormPrincipal formPrincipal;
        private int idElemento = 0;

        private readonly System.Windows.Forms.Timer Timer;
        private AutoCompleteStringCollection _acTipos;
        private AutoCompleteStringCollection _acModelos;
        private bool navegandoLista = false;
        private readonly int minima = 2;
        private List<string> SerieBarraPatrimonio = new List<string>();


        public ElementosUC(FormPrincipal formPrincipal, ElementosCN elementosCN, IRepoEstadosMantenimiento repoEstadosMantenimiento, TiposElementoCN tiposElementoCN, Usuarios userVerificado, ElementosBajasCN elementosBajasCN)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.tiposElementoCN = tiposElementoCN;
            this.elementosBajasCN = elementosBajasCN;
            this.repoEstadosMantenimiento = repoEstadosMantenimiento;
            this.userVerificado = userVerificado;

            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 300;
            Timer.Tick += Timer_Tick;

            _acTipos = new AutoCompleteStringCollection();
            _acModelos = new AutoCompleteStringCollection();

            this.formPrincipal = formPrincipal;
        }

        #region LOAD
        private void ElementosUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1157);


            // AutoCompletes
            txtTipoElemento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTipoElemento.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtModelo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtModelo.AutoCompleteSource = AutoCompleteSource.CustomSource;

            // El ListBox
            lstSugerencias.BringToFront();
            lstSugerencias.Height = 120;
            lstSugerencias.Location = new Point(txtSerieBarraPatrimonio.Left, txtSerieBarraPatrimonio.Bottom + 3);

            // Eventos 
            lstSugerencias.Click += LstSugerencias_Click;
            lstSugerencias.MouseMove += (s, ev) => navegandoLista = true;
            lstSugerencias.MouseLeave += (s, ev) => navegandoLista = false;
            txtModelo.Leave += (s, e) => { InferirTipoDesdeModelo(txtModelo.Text, true); };
            txtModelo.Enter += (s, e) =>
            {
                if (_acModelos == null || _acModelos.Count == 0)
                    CargarTodosModelos();
            };
            txtSerieBarraPatrimonio.LostFocus += (s, e) =>
            {
                if (!lstSugerencias.Focused)
                {
                    lstSugerencias.Visible = false;
                }
            };

            // Cargar comboBox Estado
            var estados = repoEstadosMantenimiento.GetAll().ToList();
            estados.Insert(0, new EstadosMantenimiento { IdEstadoMantenimiento = 0, EstadoMantenimientoNombre = "Todos" });
            cmbEstados.DataSource = estados;
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";
            cmbEstados.SelectedIndex = 0;

            CargarAutoCompleteTipos();
            RenovarIdentificadores();
            SetModoInicial();

            if (dgvElementos_M.Columns.Contains("IdElemento"))
            {
                dgvElementos_M.Columns["IdElemento"].HeaderText = "ID";
                dgvElementos_M.Columns["IdElemento"].Width = 40;
                dgvElementos_M.Columns["IdElemento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dgvElementos_M.Columns.Contains("Equipo"))
            {
                dgvElementos_M.Columns["Equipo"].Width = 165;
            }
        }
        #endregion

        #region Cargar Elemento
        public void CargarElementos()
        {
            try
            {
                var elementos = elementosCN.ObtenerElementos();
                dgvElementos_M.DataSource = elementos;
                CargarGrafico();
                ResumenDeLosEstados();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los elementos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ActualizarDataGrid(int idEstado)
        {
            try
            {
                IEnumerable<ElementosDTO> elementosFiltrados;

                if (idEstado == 0)
                {
                    elementosFiltrados = elementosCN.ObtenerElementos();
                }
                else
                {
                    EstadosMantenimiento? estado = elementosCN.ObtenerEstadoMantenimientoPorID(idEstado);
                    elementosFiltrados = elementosCN.GetAllByEstado(estado?.EstadoMantenimientoNombre);
                }

                dgvElementos_M.DataSource = elementosFiltrados.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la lista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region AUTOCOMPLETES
        private void CargarAutoCompleteTipos()
        {
            IEnumerable<string> tipos = tiposElementoCN.GetNombreTipos();
            _acTipos.Clear();
            _acTipos.AddRange(tipos.ToArray());
            txtTipoElemento.AutoCompleteCustomSource = _acTipos;
        }

        private void CargarModeloParaTipo(string tipo)
        {
            _acModelos.Clear();

            IEnumerable<string> modelos;

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                modelos = elementosCN.ObtenerModelosPorNombreTipo(tipo);
            }
            else
            {
                modelos = elementosCN.ObtenerModelo();
            }

            //foreach (var m in modelos)
            //    _acModelos.Add(m);

            _acModelos.AddRange(modelos.ToArray());

            txtModelo.AutoCompleteCustomSource = _acModelos;
        }

        private void CargarTodosModelos()
        {
            CargarModeloParaTipo(null);
        }

        #endregion

        #region MODOS DE BÚSQUEDA
        private void SetModoInicial()
        {
            txtSerieBarraPatrimonio.Enabled = true;
            txtTipoElemento.Enabled = true;
            txtModelo.Enabled = true;
        }

        private void SetModoBusquedaPorTexto(bool activo)
        {
            txtSerieBarraPatrimonio.Enabled = true;
            txtTipoElemento.Enabled = !activo;
            txtModelo.Enabled = !activo;
        }

        private void SetModoBusquedaPorTipoModelo(bool activo)
        {
            txtTipoElemento.Enabled = true;
            txtModelo.Enabled = true;
            txtSerieBarraPatrimonio.Enabled = !activo;
        }

        private void RenovarIdentificadores()
        {
            try
            {
                var desdeBD = elementosCN.ObtenerSerieBarrasPatrimonio("", 5000);
                SerieBarraPatrimonio = desdeBD.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error RenovarIdentificadores: " + ex);
            }
        }

        #endregion

        #region EVENTOS TEXT CHANGED y KEYDOWN
        private void txtSerieBarraPatrimonio_TextChanged_1(object sender, EventArgs e)
        {
            if (!txtSerieBarraPatrimonio.Focused) return;

            string texto = txtSerieBarraPatrimonio.Text;
            bool hayTexto = !string.IsNullOrWhiteSpace(texto);

            SetModoBusquedaPorTexto(hayTexto);

            if (!hayTexto || texto.Length < minima)
            {
                Timer.Stop();
                lstSugerencias.Visible = false;
                return;
            }

            Timer.Stop();
            Timer.Start();
        }

        private void txtSerieBarraPatrimonio_KeyDown(object sender, KeyEventArgs e)
        {
            if (!lstSugerencias.Visible) return;

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                int count = lstSugerencias.Items.Count;

                if (count == 0) return;

                if (lstSugerencias.SelectedIndex == -1)
                {
                    lstSugerencias.SelectedIndex = e.KeyCode == Keys.Down ? 0 : count - 1;
                }
                else
                {
                    int idx = lstSugerencias.SelectedIndex + (e.KeyCode == Keys.Down ? 1 : -1);
                    if (idx < 0) idx = count - 1;
                    if (idx >= count) idx = 0;
                    lstSugerencias.SelectedIndex = idx;
                }

                navegandoLista = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (lstSugerencias.SelectedItem != null)
                {
                    SeleccionarSugerencia(lstSugerencias.SelectedItem.ToString());
                }
                else
                {
                    EjecutarBusqueda();
                }

                navegandoLista = false;
                lstSugerencias.Visible = false;
            }

            navegandoLista = false;
            Timer.Stop();
            Timer.Start();
        }
        #endregion

        #region TIMER 
        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();

            if (navegandoLista) return;

            string textoNEW = txtSerieBarraPatrimonio.Text?.Trim() ?? string.Empty;

            if (textoNEW.Length < minima)
            {
                lstSugerencias.Visible = false;
                return;
            }

            try
            {
                string[] sugerencias = SerieBarraPatrimonio
                    .Where(s => !string.IsNullOrEmpty(s) && s.Contains(textoNEW, StringComparison.OrdinalIgnoreCase))
                    .Take(15)
                    .ToArray();

                if (sugerencias.Length == 0)
                {
                    lstSugerencias.Visible = false;
                    return;
                }


                navegandoLista = false;
                lstSugerencias.BeginUpdate();
                lstSugerencias.Items.Clear();
                lstSugerencias.Items.AddRange(sugerencias);
                lstSugerencias.EndUpdate();

                lstSugerencias.Width = txtSerieBarraPatrimonio.Width;
                lstSugerencias.Location = new Point(txtSerieBarraPatrimonio.Left, txtSerieBarraPatrimonio.Bottom);

                lstSugerencias.Visible = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Timer_Tick (llenar listbox): " + ex);
            }
        }
        #endregion

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

        private void txtTipoElemento_TextChanged(object sender, EventArgs e)
        {
            bool hayTexto = !string.IsNullOrWhiteSpace(txtTipoElemento.Text);
            SetModoBusquedaPorTipoModelo(hayTexto);

            CargarModeloParaTipo(txtTipoElemento.Text);
        }

        private void InferirTipoDesdeModelo(string modeloNombre, bool BloquearTipo = false)
        {
            if (string.IsNullOrWhiteSpace(modeloNombre))
            {
                txtTipoElemento.Enabled = true;
                return;
            }

            var modelo = elementosCN.ObtenerModelosPorNombre(modeloNombre);
            if (modelo == null)
            {
                txtTipoElemento.Enabled = true;
                return;
            }

            var tipo = tiposElementoCN.GetById(modelo.IdTipoElemento);
            if (tipo == null)
            {
                txtTipoElemento.Enabled = true;
                return;
            }

            txtTipoElemento.Text = tipo.ElementoTipo;

            if (BloquearTipo)
                txtTipoElemento.Enabled = false;
        }

        #region BOTONES FILTRO

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            EjecutarBusqueda();
            navegandoLista = false;
        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e)
        {
            txtSerieBarraPatrimonio.Clear();
            txtTipoElemento.Clear();
            txtModelo.Clear();
            SerieBarraPatrimonio.Clear();
            lstSugerencias.Items.Clear();
            lstSugerencias.Visible = false;
            RenovarIdentificadores();
            _acModelos.Clear();
            txtModelo.AutoCompleteCustomSource = _acModelos;
            SetModoInicial();
            CargarElementos();
        }
        #endregion

        private void EjecutarBusqueda()
        {
            string q = string.IsNullOrWhiteSpace(txtSerieBarraPatrimonio.Text) ? null : txtSerieBarraPatrimonio.Text.Trim();
            string tipoTexto = string.IsNullOrWhiteSpace(txtTipoElemento.Text) ? null : txtTipoElemento.Text.Trim();
            string modeloTexto = string.IsNullOrWhiteSpace(txtModelo.Text) ? null : txtModelo.Text.Trim();


            if (!string.IsNullOrWhiteSpace(q))
            {
                IEnumerable<ElementosDTO> resultados = elementosCN.BuscarElementos(q, null, null);
                dgvElementos_M.DataSource = resultados.ToList();

                return;
            }

            int? idTipo = null;
            int? idModelo = null;

            if (!string.IsNullOrWhiteSpace(tipoTexto))
            {
                TipoElemento? tipo = tiposElementoCN.ObtenerPorNombre(tipoTexto);
                if (tipo != null)
                {
                    idTipo = tipo.IdTipoElemento;
                }
            }

            if (!string.IsNullOrWhiteSpace(modeloTexto))
            {
                Modelos? modelo = elementosCN.ObtenerModelosPorNombre(modeloTexto);
                if (modelo != null)
                {
                    idModelo = modelo.IdModelo;
                }

                if (idModelo.HasValue && !idTipo.HasValue)
                {
                    Modelos? mod = elementosCN.ObtenerModeloPorID(idModelo.Value);
                    if (mod != null)
                    {
                        idTipo = mod.IdTipoElemento;
                    }
                }
            }

            IEnumerable<ElementosDTO> lista = elementosCN.BuscarElementos(null, idTipo, idModelo);
            dgvElementos_M.DataSource = lista.ToList();
        }

        private void dgvElementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvElementos_M.Rows[e.RowIndex];

            idElemento = Convert.ToInt32(fila.Cells["IdElemento"].Value);
        }

        private void dgvElementos_M_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvElementos_M.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string? estado = e.Value.ToString();

                if (estado == "En Reparacion")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 150, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Prestado")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 230, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    return;
                }

                e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
        }

        private void btnCrearElemento_M_Click(object sender, EventArgs e)
        {
            var CrearElemento = new FormCRUDElementos(elementosCN, userVerificado, CargarElementos);
            CrearElemento.ShowDialog();
        }

        private void cmbEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstados.SelectedValue == null) return;

            try
            {
                int idEstado = Convert.ToInt32(cmbEstados.SelectedValue);
                ActualizarDataGrid(idEstado);
            }
            catch (Exception)
            {
            }
        }

        private void btnGestionarElemento_M_Click(object sender, EventArgs e)
        {
            var detalleUC = new ElementoGestionUC(formPrincipal, this, elementosCN, elementosBajasCN, idElemento, userVerificado);
            formPrincipal.MostrarUserControl(detalleUC);
        }
        
        private void CargarGrafico()
        {
            List<(string Nombre, int Cantidad)> datos = elementosCN.ObtenerElementosPorTipo();

            if (datos == null || datos.Count == 0) return;

            List<PieSeries<int>> series = new List<PieSeries<int>>();

            foreach (var d in datos)
            {
                series.Add(new PieSeries<int>
                {
                    Values = new int[] { d.Cantidad },
                    Name = d.Nombre,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsSize = 14
                });
            }

            var pieChart = new PieChart
            {
                Dock = DockStyle.Fill,
                Series = series.ToArray()
            };

            pnlGraficoPie.Controls.Clear();
            pnlGraficoPie.Controls.Add(pieChart);
        }

        private void ResumenDeLosEstados()
        {
            lblCantDisponibles.Text = elementosCN.SumarElementosPorEstado(1).ToString();
            lblCantEnPrestamo.Text = elementosCN.SumarElementosPorEstado(2).ToString();
            lblCantEnReparacion.Text = elementosCN.SumarElementosPorEstado(3).ToString();
            lblCantParaReparar.Text = elementosCN.SumarElementosPorEstado(4).ToString();
            lblCantFaltantes.Text = elementosCN.SumarElementosPorEstado(5).ToString();
            lblCantTotal.Text = elementosCN.TotalElementos().ToString();
        }
    }
}