using CapaDatos.InterfacesDTO;

namespace CapaPresentacion
{
    public partial class InventarioUC : UserControl
    {
        private readonly IMapperInventario mapperInventario;
        public InventarioUC(IMapperInventario mapperInventario)
        {
            InitializeComponent();

            this.mapperInventario = mapperInventario;
        }

        public void MostrarInventario()
        {
            dgvInventario.DataSource = mapperInventario.GetAllDTO();
        }

        private void InventarioUC_Load(object sender, EventArgs e)
        {
            MostrarInventario();

            dgvInventario.Columns["Observacion"].Visible = false;
            dgvInventario.Columns["Motivo"].Visible = false;
        }

        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0) return;


            string? motivo = dgvInventario.Rows[e.RowIndex].Cells["Motivo"].Value.ToString();
            string? descripcion = dgvInventario.Rows[e.RowIndex].Cells["Observacion"].Value.ToString();

            txtMotivo.Text = motivo ?? "Sin Motivo";
            txtDescripcion.Text = descripcion ?? "No hay descripcion";
        }
    }
}
