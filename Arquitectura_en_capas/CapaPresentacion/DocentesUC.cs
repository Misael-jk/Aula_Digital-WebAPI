using CapaDatos.Interfaces;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class DocentesUC : UserControl
    {
        private readonly DocentesCN docentesCN = null!;
        public DocentesUC(DocentesCN docentesCN)
        {
            InitializeComponent();
            this.docentesCN = docentesCN;
        }

        public void MostrarDocentes()
        {
            dgvDocentes.DataSource = docentesCN.MostrarDocente();
        }
    }
}
