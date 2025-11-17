using CapaDatos.InterfacesDTO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinForms; // <- importante para CartesianChart en WinForms
using LiveChartsCore.Measure;
using SkiaSharp; // para colores (SKColor)
using System.Data;
using LiveChartsCore.SkiaSharpView.Painting;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class Dashboard : UserControl
    {
        private readonly IMapperNotebooksPrestadas mapperNotebooksPrestadas;
        private readonly IMapperPrestamosActivos mapperPrestamosActivos;
        private CartesianChart cartesianChartNotebooks;
        private readonly IMapperRankingDocente mapperRankingDocente;
        private readonly NotebooksCN notebooksCN;

        public Dashboard(IMapperPrestamosActivos mapperPrestamosActivos, IMapperNotebooksPrestadas mapperNotebooksPrestadas, IMapperRankingDocente mapperRankingDocente, NotebooksCN notebooksCN)
        {
            InitializeComponent();
            this.mapperPrestamosActivos = mapperPrestamosActivos;
            this.mapperNotebooksPrestadas = mapperNotebooksPrestadas;
            this.mapperRankingDocente = mapperRankingDocente;
            this.notebooksCN = notebooksCN;

            if (mapperPrestamosActivos == null) throw new Exception("mapperPrestamosActivos es NULL");
            if (mapperNotebooksPrestadas == null) throw new Exception("mapperNotebooksPrestadas es NULL");
            if (mapperRankingDocente == null) throw new Exception("mapperRankingDocente es NULL");
            if (notebooksCN == null) throw new Exception("notebooksCN es NULL");
            InicializarCartesiano();
        }

        public void MostrarDatos()
        {
            //var elemento = mapperHistorialElemento.GetAllDTO();
            //dataGridView1.DataSource = elemento.ToList();
            dgvPrestamosActivos.DataSource = mapperPrestamosActivos.GetAllDTO().ToList();
            CargarGraficoNotebooksPorMes();
            CargarRankingDocentes();
            CargarConteos();
        }

        private void Dashboard_Load_1(object sender, EventArgs e)
        {
            MostrarDatos();

            dgvPrestamosActivos.Columns["IdPrestamo"].HeaderText = "ID";
            dgvPrestamosActivos.Columns["IdPrestamo"].Width = 40;
            dgvPrestamosActivos.Columns["IdPrestamo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrestamosActivos.Columns["Nombre"].Width = 85;
            dgvPrestamosActivos.Columns["Apellido"].Width = 85;
            dgvPrestamosActivos.Columns["Carrito"].Width = 120;
            dgvPrestamosActivos.Columns["Fecha"].Width = 115;
            dgvPrestamosActivos.Columns["Prestadas"].Width = 70;
            dgvPrestamosActivos.Columns["Prestadas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrestamosActivos.Columns["Devueltas"].Width = 70;
            dgvPrestamosActivos.Columns["Devueltas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        #region Grafico Lineal
        private void InicializarCartesiano()
        {
            cartesianChartNotebooks = new CartesianChart
            {
                Dock = DockStyle.Fill,
                LegendPosition = LegendPosition.Top,
                TooltipPosition = TooltipPosition.Auto,
                Padding = new Padding(6)
            };

            if (this.Controls.ContainsKey("pnlGrafico"))
            {
                var panel = this.Controls["pnlGrafico"] as Panel;
                panel?.Controls.Add(cartesianChartNotebooks);
            }
            else
            {
                cartesianChartNotebooks.Location = new Point(10, 80);
                cartesianChartNotebooks.Size = new Size(600, 280);
                this.Controls.Add(cartesianChartNotebooks);
                cartesianChartNotebooks.BringToFront();
            }

            cartesianChartNotebooks.BackgroundImage = null; 
        }

        private void CargarGraficoNotebooksPorMes()
        {
            var datos = mapperNotebooksPrestadas.GetAllDTO().ToList();

            if (datos == null || datos.Count == 0)
                return;

            var meses = datos.Select(d =>
            {
                if (DateTime.TryParse(d.Mes + "-01", out var dt))
                    return dt.ToString("MMM yyyy", new System.Globalization.CultureInfo("es-ES"));
                return d.Mes;
            }).ToArray();

            var cantidades = datos.Select(d => (double)d.CantidadNotebooks).ToList();

            cartesianChartNotebooks.Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "",
                    Values = cantidades,
                    LineSmoothness = 0,
                    Fill = null,
                    GeometrySize = 8,
                    AnimationsSpeed = TimeSpan.FromMilliseconds(700),
                    Stroke = new SolidColorPaint(new SKColor(59,130,246)) { StrokeThickness = 3 },
                    GeometryStroke = new SolidColorPaint(new SKColor(255,255,255)) { StrokeThickness = 2 },
                    GeometryFill = new SolidColorPaint(new SKColor(59,130,246))
                }
            };

            cartesianChartNotebooks.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = meses,
                    LabelsRotation = 0,
                    UnitWidth = 1
                }
            };

            cartesianChartNotebooks.YAxes = new Axis[]
            {
                new Axis
                {
                    Labeler = value => value.ToString("N0")
                }
            };

            cartesianChartNotebooks.Refresh();
        }
        #endregion

        public void CargarRankingDocentes()
        {
            var ranking = mapperRankingDocente.GetAllDTO();

            dgvRanking.DataSource = ranking;

            dgvRanking.Columns["IdDocente"].HeaderText = "ID";
            dgvRanking.Columns["IdDocente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRanking.Columns["PrestamosRecibidos"].HeaderText = "Total";

        }

        public void CargarConteos()
        {
            lblCantDisponibles.Text = notebooksCN.ObtenerCantidadPorEstados(1).ToString();
            lblCantPrestados.Text = notebooksCN.ObtenerCantidadPorEstados(2).ToString();
            lblCantMantenimiento.Text = notebooksCN.ObtenerCantidadPorEstados(3).ToString();
            lblCantRotas.Text = notebooksCN.ObtenerCantidadPorEstados(4).ToString();
            lblCantTotal.Text = notebooksCN.CantidadTotalNotebooks().ToString();
        }
    }
}
