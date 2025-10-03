using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
