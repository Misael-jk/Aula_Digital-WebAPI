using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using CapaNegocio;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Guna.UI2.WinForms;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        private readonly NotebooksCN notebooksCN;
        private readonly CarritosCN carritosCN;
        private Usuarios usuarioActual;
        private int IdActual = 0;

        public NotebooksUC(NotebooksCN notebooksCN, Usuarios user, CarritosCN carritosCN)
        {
            InitializeComponent();

            this.notebooksCN = notebooksCN;
            this.usuarioActual = user;
            this.carritosCN = carritosCN;
        }
        public void ActualizarDataGrid()
        {
            ApplyModernStyleCompact(dtgNotebook);

            dtgNotebook.DataSource = notebooksCN.GetAll();

            cmbModelo.DataSource = notebooksCN.ListarModelosPorTipo(1);
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "NombreModelo";

            cmbUbicacion.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";

            CargarGraficoCarritos();
        }

        private void NotebooksUC_Load(object sender, EventArgs e)
        {
            ActualizarDataGrid();
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1300);

            cmbUbicacion_RemoveCarrito.DataSource = notebooksCN.ListarUbicaciones();
            cmbUbicacion_RemoveCarrito.ValueMember = "IdUbicacion";
            cmbUbicacion_RemoveCarrito.DisplayMember = "NombreUbicacion";

            CargarGrafico();
            CargarGraficoEstados();
            CargarGraficoCarritos();
        }

        private void btnAgregarNotebook_Click(object sender, EventArgs e)
        {
            //Action ActualizarGrid = ActualizarDataGrid; No es necesario crear esta variable temporal, ya que puedo pasar el metodo directamente

            var Notebook = new FormCRUDNotebook(notebooksCN, ActualizarDataGrid, CargarGrafico, usuarioActual);
            Notebook.ShowDialog();
        }

        private void dtgNotebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblCarroAsignado.Text = "Carrito Asignado: ";
            lblCasillero.Text = "Casillero: ";
            lblIDNotebook.Text = "ID: ";
            lblFechaModificacion.Text = "Fecha de modificacion: ";

            if (e.RowIndex < 0) return;

            IdActual = Convert.ToInt32(dtgNotebook.Rows[e.RowIndex].Cells["IdNotebook"].Value);

            Notebooks? notebook = notebooksCN.ObtenerNotebookPorID(IdActual);

            lblIDNotebook.Text += notebook?.IdElemento.ToString();
            txtEquipo.Text = notebook?.Equipo;
            txtNroSerie.Text = notebook?.NumeroSerie;
            txtCodBarra.Text = notebook?.CodigoBarra;
            txtPatrimonio.Text = notebook?.Patrimonio;
            cmbModelo.SelectedValue = notebook?.IdModelo;
            cmbUbicacion.SelectedValue = notebook?.IdUbicacion;

            EstadosMantenimiento? estadosMantenimiento = notebooksCN.ObtenerEstadoMantenimientoPorID(notebook.IdEstadoMantenimiento);

            lblEstado.Text = estadosMantenimiento?.EstadoMantenimientoNombre;
            lblEstado.Tag = estadosMantenimiento?.IdEstadoMantenimiento;

            if (estadosMantenimiento?.IdEstadoMantenimiento == 1)
            {
                ptbEstado.Image = Properties.Resources.disponibleIcon;
            }
            else
            {
                ptbEstado.Image = Properties.Resources.prestadoIcon;
            }

            HistorialCambios? historialCambios = notebooksCN.ObtenerUltimaFechaDeModiciacionPorID(IdActual);

            lblFechaModificacion.Text += historialCambios?.FechaCambio;
            /* 
             * Corregi el problema del metodo ObtenerCarritoPorID ya que esta propiedad no admite null por eso debemos usar
             * el .HasValue para identificar si tiene o no carrito antes de invocar el metodo, si ves arriba de este sms tambien
             * hay otro metodo ObtenerEstadoMantenimientoPorID y seguro pienses que tambien necesita esto, pero a diferencia de esto
             * el metodo de arriba si apuntas el cursor ahi veras que dice "puede ser NULL aqui" lo que no pasa nada si no hacemos 
             * validaciones lo de abajo decia que un tipo que acepta valores nullos recibe un valor null, por eso ahi que verificar eso.
             */
            if (notebook?.IdCarrito.HasValue == true)
            {
                Carritos? carritos = notebooksCN.ObtenerCarritoPorID(notebook.IdCarrito.Value);

                if (carritos != null)
                {
                    lblCarroAsignado.Text += carritos.EquipoCarrito;
                    lblCasillero.Text += notebook.PosicionCarrito.ToString();
                }
                else
                {
                    lblCarroAsignado.Text += "Sin Carrito";
                    lblCasillero.Text += "-";
                }
            }
            else
            {
                lblCarroAsignado.Text += "Sin Carrito";
                lblCasillero.Text += "-";
            }

            #region Agregar cellClick para agregar al carrito
            txtEquipo_AddCarrito.Text = notebook?.Equipo;
            #endregion

            #region Quitar CellClick para quitar del carrito
            txtEquipo_RemoveCarrito.Text = notebook?.Equipo;
            txtCarrito_RemoveCarrito.Text = notebook?.IdCarrito.HasValue == true ? notebooksCN.ObtenerCarritoPorID(notebook.IdCarrito.Value)?.EquipoCarrito : "Sin Carrito";
            #endregion
        }

        private void btnActualizarNotebook_Click(object sender, EventArgs e)
        {
            Notebooks? notebooks = notebooksCN.ObtenerNotebookPorID(IdActual);

            Notebooks? notebook = new Notebooks
            {
                IdElemento = IdActual,
                Equipo = txtEquipo.Text,
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdModelo = (int)cmbModelo.SelectedValue,
                IdUbicacion = (int)cmbUbicacion.SelectedValue,
                IdEstadoMantenimiento = Convert.ToInt32(lblEstado.Tag),  
                IdTipoElemento = 1,
                IdVarianteElemento = null,
                IdCarrito = notebooks?.IdCarrito,
                PosicionCarrito = notebooks?.PosicionCarrito,
                Habilitado = true,
                FechaBaja = null
            };

            notebooksCN.ActualizarNotebook(notebook, usuarioActual.IdUsuario);
            ActualizarDataGrid();
            CargarGrafico();
            CargarGraficoEstados();
            CargarGraficoCarritos();
        }

        private void btnDeshabiliar_Click(object sender, EventArgs e)
        {
            notebooksCN.DeshabilitarNotebook(IdActual, usuarioActual.IdUsuario, 3);

            ActualizarDataGrid();
            CargarGrafico();
            CargarGraficoEstados();
            CargarGraficoCarritos();
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
            ActualizarDataGrid();
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

            ActualizarDataGrid();
        }

        private void CargarGrafico()
        {
            var datos = notebooksCN.GetCantidadPorModelo();

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

            var pieChart = new PieChart
            {
                Dock = DockStyle.Fill,
                Series = series.ToArray()
            };

            pnlGrafico.Controls.Clear();
            pnlGrafico.Controls.Add(pieChart);
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

            pnlGraficoEstados.Controls.Clear();
            pnlGraficoEstados.Controls.Add(pieChart);
        }



    private void CargarGraficoCarritos()
    {
        var datos = notebooksCN.GetCantidadNotebooksEnCarritos();

        if (datos == null || datos.Count == 0) return;

        var labels = datos.Select(d => d.Equipo).ToArray();
        var values = datos.Select(d => (double)d.Cantidad).ToArray();

        var series = new ColumnSeries<double>
        {
            Values = values,
            DataLabelsPaint = new SolidColorPaint(new SKColor(60, 60, 60)),
            DataLabelsSize = 11,
            MaxBarWidth = 45,     // ✅ barras más parejas
            Padding = 5,          // ✅ espacio entre barras
            Stroke = null,        // ✅ quita bordes de barras
            Fill = new SolidColorPaint(new SKColor(70, 130, 180)) // ✅ color suave estilo dashboard
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
                TextSize = 12,
                Padding = new LiveChartsCore.Drawing.Padding { Left = 5, Right = 5, Top = 5, Bottom = 5 },
                NameTextSize = 13,
                NamePaint = new SolidColorPaint(new SKColor(80, 80, 80))
            }
            },

            YAxes = new Axis[]
            {
            new Axis
            {
                Labeler = value => value.ToString("N0"),
                NameTextSize = 13,
                NamePaint = new SolidColorPaint(new SKColor(80, 80, 80)),
                TextSize = 12
            }
            },

            LegendPosition = LiveChartsCore.Measure.LegendPosition.Hidden 
        };

        pnlGraficoCarritos.Controls.Clear();
        pnlGraficoCarritos.Controls.Add(chart);
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
    }
}
