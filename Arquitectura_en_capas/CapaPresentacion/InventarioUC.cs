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

        private void InventarioUC_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.DataSource = mapperInventario.GetAllDTO();
        }
    }
}
