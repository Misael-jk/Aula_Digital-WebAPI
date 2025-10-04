using CapaNegocio;

namespace CapaPresentacion
{
    public partial class InventarioUC : UserControl
    {
        private readonly ElementosCN elementosCN;
        public InventarioUC(ElementosCN elementosCN)
        {
            InitializeComponent();

            this.elementosCN = elementosCN;
        }

        private void InventarioUC_Load(object sender, EventArgs e)
        {

        }
    }
}
