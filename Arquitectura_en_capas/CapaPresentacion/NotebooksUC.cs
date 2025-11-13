using CapaDatos.InterfacesDTO;
using CapaEntidad;
using CapaNegocio;
using Guna.UI2.WinForms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace CapaPresentacion
{
    public partial class NotebooksUC : UserControl
    {
        private readonly NotebooksCN notebooksCN;
        private readonly CarritosCN carritosCN;
        private FormPrincipal _formPrincipal;
        private Usuarios usuarioActual;
        private int IdActual = 0;

        public NotebooksUC(NotebooksCN notebooksCN, Usuarios user, CarritosCN carritosCN, FormPrincipal formPrincipal)
        {
            InitializeComponent();

            this.notebooksCN = notebooksCN;
            this._formPrincipal = formPrincipal;
            this.usuarioActual = user;
            this.carritosCN = carritosCN;
        }
        public void ActualizarDataGrid()
        {
            dgvNotebooks_M.DataSource = notebooksCN.GetAll();

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

        private void btnActualizarNotebook_Click(object sender, EventArgs e)
        {
            //Notebooks? notebooks = notebooksCN.ObtenerNotebookPorID(IdActual);

            //Notebooks? notebook = new Notebooks
            //{
            //    IdElemento = IdActual,
            //    Equipo = txtEquipo.Text,
            //    NumeroSerie = txtNroSerie.Text,
            //    CodigoBarra = txtCodBarra.Text,
            //    Patrimonio = txtPatrimonio.Text,
            //    IdModelo = (int)cmbModelo.SelectedValue,
            //    IdUbicacion = (int)cmbUbicacion.SelectedValue,
            //    IdEstadoMantenimiento = Convert.ToInt32(lblEstado.Tag),
            //    IdTipoElemento = 1,
            //    IdVarianteElemento = null,
            //    IdCarrito = notebooks?.IdCarrito,
            //    PosicionCarrito = notebooks?.PosicionCarrito,
            //    Habilitado = true,
            //    FechaBaja = null
            //};

            //notebooksCN.ActualizarNotebook(notebook, usuarioActual.IdUsuario);
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

        private void btnGestionarNotebook_M_Click(object sender, EventArgs e)
        {
            var detalleUC = new NotebookGestionUC(_formPrincipal, this, notebooksCN, IdActual, usuarioActual);
            _formPrincipal.MostrarUserControl(detalleUC);
        }
    }
}
