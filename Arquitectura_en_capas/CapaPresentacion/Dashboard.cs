using CapaDatos.MappersDTO;

namespace CapaPresentacion
{
    public partial class Dashboard : UserControl
    {
        private readonly MapperHistorialElemento mapperHistorialElemento;
        public Dashboard(MapperHistorialElemento mapperHistorialElemento)
        {
            InitializeComponent();
            this.mapperHistorialElemento = mapperHistorialElemento;

        }

        public void MostrarDatos()
        {
            //var elemento = mapperHistorialElemento.GetAllDTO();
            //dataGridView1.DataSource = elemento.ToList();
        }
    }
}
