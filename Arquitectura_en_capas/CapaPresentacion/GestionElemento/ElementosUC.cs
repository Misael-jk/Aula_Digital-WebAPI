using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaEntidad;
using CapaNegocio;
using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using CapaDTOs;
using System.Diagnostics;


namespace CapaPresentacion
{
    public partial class ElementosUC : UserControl
    {
        private readonly ElementosCN elementosCN;
        private readonly TiposElementoCN tiposElementoCN;
        private readonly ModeloCN modeloCN;
        private readonly Usuarios userVerificado;
        private readonly IRepoEstadosMantenimiento repoEstadosMantenimiento;
        private readonly IRepoElemento repoElemento;
        private int idElemento = 0;
        private int idVariante = 0;

        private readonly System.Windows.Forms.Timer Timer;

        private AutoCompleteStringCollection _acTipos;
        private AutoCompleteStringCollection _acModelos;
        private AutoCompleteStringCollection _acIdentificadores;
        private string ultimaQuery = string.Empty;
        private readonly int minima = 1;
        private bool navegandoSugerencias = false;
        private List<string> _cacheIdentificadores = new List<string>();

        public ElementosUC(ElementosCN elementosCN, IRepoEstadosMantenimiento repoEstadosMantenimiento, IRepoElemento repoElemento, TiposElementoCN tiposElementoCN, ModeloCN modeloCN, Usuarios userVerificado)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.tiposElementoCN = tiposElementoCN;
            this.modeloCN = modeloCN;
            this.repoEstadosMantenimiento = repoEstadosMantenimiento;
            this.repoElemento = repoElemento;
            this.userVerificado = userVerificado;

            Timer = new System.Windows.Forms.Timer();
            Timer.Interval = 1000;
            Timer.Tick += Timer_Tick;

            _acTipos = new AutoCompleteStringCollection();
            _acModelos = new AutoCompleteStringCollection();
            _acIdentificadores = new AutoCompleteStringCollection();
        }

        public void CargarElementos()
        {
            try
            {
                var elementos = elementosCN.ObtenerElementos();
                //dgvElementos.DataSource = elementos;
                dgvElementos_M.DataSource = elementos;
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
                    elementosFiltrados = elementosCN.GetAllByEstado(idEstado);
                }

                dgvElementos_M.DataSource = elementosFiltrados.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la lista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region LOAD
        private void ElementosUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1100);

            ApplyModernStyleCompact(dgvElementos);
            /*            txtTipoElemento.Leave += TxTTipoElemento_Leave; */// actualizar modelos al salir del txtTipo
                                                                            //txtModelo.TextChanged += TxtModelo_TextChanged;

            //btnBuscar.Click += BtnBuscar_Click;
            //btnBorrarFiltros.Click += btnBorrarFiltros_Click;

            //dgvElementos_M.CellFormatting += dgvElementos_M_CellFormatting;
            //dgvElementos_M.CellClick += dgvElementos_M_CellClick;


            txtSerieBarraPatrimonio.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtSerieBarraPatrimonio.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtSerieBarraPatrimonio.AutoCompleteCustomSource = _acIdentificadores;
            txtSerieBarraPatrimonio.Leave += (s, e) => Timer.Stop();
            txtSerieBarraPatrimonio.Leave += (s, e) => navegandoSugerencias = false;
            txtSerieBarraPatrimonio.LostFocus += (s, e) => navegandoSugerencias = false;

            txtTipoElemento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTipoElemento.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtTipoElemento.AutoCompleteCustomSource = _acTipos;

            txtModelo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtModelo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtModelo.AutoCompleteCustomSource = _acModelos;
            txtModelo.Leave -= (s, e) => { InferirTipoDesdeModelo(txtModelo.Text, lockIfFound: true); };
            txtModelo.Leave += (s, e) => { InferirTipoDesdeModelo(txtModelo.Text, lockIfFound: true); };
            txtModelo.Enter += (s, e) =>
            {
                if (_acModelos == null || _acModelos.Count == 0)
                    CargarTodosModelos();
            };


            cmbEstados.SelectedIndexChanged -= cmbEstados_SelectedIndexChanged;

            var estados = repoEstadosMantenimiento.GetAll().ToList();


            estados.Insert(0, new EstadosMantenimiento { IdEstadoMantenimiento = 0, EstadoMantenimientoNombre = "Todos" });

            cmbEstados.DataSource = estados;
            cmbEstados.ValueMember = "IdEstadoMantenimiento";
            cmbEstados.DisplayMember = "EstadoMantenimientoNombre";
            cmbEstados.SelectedIndex = 0;

            cmbEstados.SelectedIndexChanged += cmbEstados_SelectedIndexChanged;


            // -------------------- TIPOS --------------------
            var tipos = tiposElementoCN.GetTiposByElemento();

            cmbTipoElemento.DataSource = tipos;
            cmbTipoElemento.ValueMember = "IdTipoElemento";
            cmbTipoElemento.DisplayMember = "ElementoTipo";

            cmbUbicaciones.DataSource = elementosCN.ObtenerUbicaciones();
            cmbUbicaciones.ValueMember = "IdUbicacion";
            cmbUbicaciones.DisplayMember = "NombreUbicacion";

            CargarAutoCompleteTipos();
            RenovarIdentificadores();
            SetModoInicial();
        }
        #endregion

        #region AUTOCOMPLETES
        private void CargarAutoCompleteTipos()
        {
            IEnumerable<string> tipos = tiposElementoCN.GetNombreTipos();
            _acTipos.Clear();
            _acTipos.AddRange(tipos.ToArray());

            txtTipoElemento.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtTipoElemento.AutoCompleteSource = AutoCompleteSource.CustomSource;
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

            foreach (var m in modelos)
                _acModelos.Add(m);

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
            // Si el usuario escribe en txtBuscar, deshabilitar txtTipo y txtModelo
            txtSerieBarraPatrimonio.Enabled = true;
            txtTipoElemento.Enabled = !activo;
            txtModelo.Enabled = !activo;
        }

        private void SetModoBusquedaPorTipoModelo(bool activo)
        {
            // Si el usuario usa tipo/modelo, deshabilitar txtBuscar
            txtTipoElemento.Enabled = true;
            txtModelo.Enabled = true;
            txtSerieBarraPatrimonio.Enabled = !activo;
        }

        private void RenovarIdentificadores()
        {
            try
            {
                var desdeBD = elementosCN.ObtenerSerieBarrasPatrimonio("", 5000) ?? Enumerable.Empty<string>();
                _cacheIdentificadores = desdeBD.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                _acIdentificadores.Clear();
                _acIdentificadores.AddRange(_cacheIdentificadores.Take(0).ToArray());
                txtSerieBarraPatrimonio.AutoCompleteCustomSource = _acIdentificadores;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error RenovarIdentificadores: " + ex);
            }
        }

        #endregion

        #region EVENTOS TEXT CHANGED

        private void txtSerieBarraPatrimonio_TextChanged_1(object sender, EventArgs e)
        {
            if (!txtSerieBarraPatrimonio.Focused) return;

            string texto = txtSerieBarraPatrimonio.Text ?? string.Empty;

            bool hayTexto = !string.IsNullOrWhiteSpace(texto);
            SetModoBusquedaPorTexto(hayTexto);

            if (!hayTexto || texto.Length < minima)
            {
                Timer.Stop();

                if (!hayTexto)
                {
                    ultimaQuery = string.Empty;
                    _acIdentificadores.Clear();
                }
                return;
            }

            Timer.Stop();
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (navegandoSugerencias) return;
            Timer.Stop();

            string q = txtSerieBarraPatrimonio.Text?.Trim() ?? string.Empty;
            if (q.Length < minima)
            {
                _acIdentificadores.Clear();
                txtSerieBarraPatrimonio.AutoCompleteCustomSource = _acIdentificadores;
                return;
            }
            if (string.Equals(q, ultimaQuery, StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                var sugerencias = _cacheIdentificadores
                    .Where(s => s != null && s.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Take(15)
                    .ToArray();

                var actuales = _acIdentificadores.Cast<string>().ToArray();
                bool iguales = actuales.Length == sugerencias.Length &&
                               actuales.Zip(sugerencias, (a, b) => string.Equals(a?.Trim(), b?.Trim(), StringComparison.OrdinalIgnoreCase)).All(x => x);

                if (!iguales)
                {
                    _acIdentificadores.Clear();
                    _acIdentificadores.AddRange(sugerencias);
                    txtSerieBarraPatrimonio.AutoCompleteCustomSource = _acIdentificadores;
                }

                ultimaQuery = q;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error Timer_Tick (filtrar cache): " + ex);
            }
        }


        private void txtSerieBarraPatrimonio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                EjecutarBusqueda();
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                navegandoSugerencias = true;
                Timer.Stop();
            }
        }

        private void txtTipoElemento_TextChanged(object sender, EventArgs e)
        {
            bool hayTexto = !string.IsNullOrWhiteSpace(txtTipoElemento.Text);
            SetModoBusquedaPorTipoModelo(hayTexto);

            CargarModeloParaTipo(txtTipoElemento.Text);
        }

        private void txtTipoElemento_Leave(object sender, EventArgs e)
        {
        }

        private void txtModelo_TextChanged(object sender, EventArgs e)
        {
        }

        private void InferirTipoDesdeModelo(string modeloNombre, bool lockIfFound = false)
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

            if (lockIfFound)
                txtTipoElemento.Enabled = false;
        }


        private void txtModelo_Validated(object sender, EventArgs e)
        {
            var texto = txtModelo.Text?.Trim();
            if (string.IsNullOrEmpty(texto))
            {
                txtTipoElemento.Enabled = true;
                return;
            }

            bool existe = _acModelos.Cast<string>()
                         .Any(m => string.Equals(m.Trim(), texto, StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                InferirTipoDesdeModeloExacto(texto, lockIfFound: true);
            }
            else
            {
                txtTipoElemento.Enabled = true;
            }
        }

        private void InferirTipoDesdeModeloExacto(string modeloNombre, bool lockIfFound = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(modeloNombre))
                {
                    if (lockIfFound) txtTipoElemento.Enabled = true;
                    return;
                }

                var modelo = elementosCN.ObtenerModelosPorNombre(modeloNombre);
                
                if (modelo != null)
                {
                    string tipoNombre = tiposElementoCN.ObtenerTipoPorNombre(modelo.IdTipoElemento);
                    txtTipoElemento.Text = tipoNombre;
                    if (lockIfFound) txtTipoElemento.Enabled = false;
                }
                else
                {
                    if (lockIfFound) txtTipoElemento.Enabled = true;
                }
            }
            catch
            {
                if (lockIfFound) txtTipoElemento.Enabled = true;
            }
        }
        #endregion


        #region BOTONES FILTRO

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            EjecutarBusqueda();
        }

        private void btnBorrarFiltros_Click(object sender, EventArgs e)
        {
            txtSerieBarraPatrimonio.Clear();
            txtTipoElemento.Clear();
            txtModelo.Clear();

            _acIdentificadores.Clear();
            _acModelos.Clear();
            ultimaQuery = string.Empty;
            txtSerieBarraPatrimonio.AutoCompleteCustomSource = _acIdentificadores;
            txtModelo.AutoCompleteCustomSource = _acModelos;

            SetModoInicial();
            CargarElementos();
        }
        #endregion


        // ----------------- LÓGICA DE BÚSQUEDA -----------------
        private void EjecutarBusqueda()
        {
            string q = string.IsNullOrWhiteSpace(txtSerieBarraPatrimonio.Text) ? null : txtSerieBarraPatrimonio.Text.Trim();
            string tipoTexto = string.IsNullOrWhiteSpace(txtTipoElemento.Text) ? null : txtTipoElemento.Text.Trim();
            string modeloTexto = string.IsNullOrWhiteSpace(txtModelo.Text) ? null : txtModelo.Text.Trim();


            if (!string.IsNullOrWhiteSpace(q))
            {
                var resultados = elementosCN.BuscarElementos(q, null, null);
                dgvElementos_M.DataSource = resultados.ToList();
                return;
            }

            int? idTipo = null;
            int? idModelo = null;

            if (!string.IsNullOrWhiteSpace(tipoTexto))
            {
                var tipo = tiposElementoCN.ObtenerPorNombre(tipoTexto);
                if (tipo != null) idTipo = tipo.IdTipoElemento;
            }

            if (!string.IsNullOrWhiteSpace(modeloTexto))
            {
                var modelo = elementosCN.ObtenerModelosPorNombre(modeloTexto);
                if (modelo != null) idModelo = modelo.IdModelo;
                // si modelo tiene idTipo, setear idTipo si no lo tenías
                if (idModelo.HasValue && !idTipo.HasValue)
                {
                    var mod = elementosCN.ObtenerModeloPorID(idModelo.Value);
                    if (mod != null) idTipo = mod.IdTipoElemento;
                }
            }

            var lista = elementosCN.BuscarElementos(null, idTipo, idModelo);
            dgvElementos_M.DataSource = lista.ToList();
        }

        private void dgvElementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var fila = dgvElementos.Rows[e.RowIndex];

            idElemento = Convert.ToInt32(fila.Cells["IdElemento"].Value);
            var elemento = repoElemento.GetById(idElemento);

            txtNroSerie.Text = elemento?.NumeroSerie;
            txtCodBarra.Text = elemento?.CodigoBarra;
            txtPatrimonio.Text = elemento?.Patrimonio;
            cmbTipoElemento.SelectedValue = elemento.IdTipoElemento;
            cmbVarianteElementos.SelectedValue = elemento.IdVarianteElemento.HasValue ? elemento.IdVarianteElemento.Value : -1;

            cmbUbicaciones.SelectedIndex = elemento.IdUbicacion;
        }

        private void btnCrearElemento_Click(object sender, EventArgs e)
        {
            var CrearElemento = new FormCRUDElementos(elementosCN, userVerificado, CargarElementos);
            CrearElemento.ShowDialog();
        }

        private void btnActualizarElemento_Click(object sender, EventArgs e)
        {
            VariantesElemento? variante = elementosCN.ObtenerVariantePorID(idVariante);
            Elemento? elementoOLD = elementosCN.ObtenerPorId(idElemento);

            var elemento = new Elemento
            {
                IdElemento = idElemento,
                IdTipoElemento = (int)cmbTipoElemento.SelectedValue,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdVarianteElemento = idVariante,
                IdUbicacion = (int)cmbUbicaciones.SelectedValue,
                IdModelo = variante.IdModelo,
                IdEstadoMantenimiento = 1,
                Habilitado = true,
                FechaBaja = null
            };

            elementosCN.ActualizarElemento(elemento, userVerificado.IdUsuario);
            CargarElementos();
        }

        private void btnDeshabilitar_Click(object sender, EventArgs e)
        {
            //elementosCN.DeshabilitarElemento(txtNroSerie.Text);
            CargarElementos();
        }

        private void ApplyModernStyleCompact(Guna2DataGridView dgv)
        {
            // Tamaño y comportamiento general
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = Color.FromArgb(215, 230, 215);
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersHeight = 38;
            dgv.RowTemplate.Height = 34;

            // Header verde brillante
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(67, 160, 71);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Celdas base
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 45);
            dgv.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f);
            dgv.DefaultCellStyle.Padding = new Padding(6, 3, 6, 3);

            // Alternancia
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);

            // Selección verde suave
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 200);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 30, 35);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 230, 200);

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            EnableDoubleBuffering(dgv);

        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            Type dgvType = dgv.GetType();
            System.Reflection.PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (pi != null)
            {
                pi.SetValue(dgv, true, null);
            }
        }

        private void dgvElementos_M_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvElementos_M.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string? estado = e.Value.ToString();

                if (estado == "En mantenimiento")
                {
                    e.CellStyle.BackColor = Color.FromArgb(255, 150, 150);
                    e.CellStyle.ForeColor = Color.Black;
                }
                else if (estado == "Prestamo")
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

        private void cmbEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstados.SelectedValue == null) return;

            try
            {
                int idEstado = Convert.ToInt32(cmbEstados.SelectedValue);
                ActualizarDataGrid(idEstado);
            }
            catch
            {
            }
        }

    }
}
