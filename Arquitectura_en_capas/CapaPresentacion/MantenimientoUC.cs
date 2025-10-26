using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaEntidad;
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
using System.Xml.Linq;

namespace CapaPresentacion
{
    public partial class MantenimientoUC : UserControl
    {
        //private readonly MantenimientoCN mantenimientoCN;
        private readonly TiposElementoCN tiposElementoCN;
        private readonly IRepoElemento repoElemento;
        public MantenimientoUC(TiposElementoCN tiposElementoCN, IRepoElemento repoElemento)
        {
            InitializeComponent();
            //this.mantenimientoCN = mantenimientoCN;
            this.tiposElementoCN = tiposElementoCN;
            this.repoElemento = repoElemento;
        }

        public void MostrarDatos()
        {

        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            //mantenimientoCN.HabilitarElemento(txtNroSerie.Text);
            MostrarDatos();
        }

        private void dgvMantenimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void MantenimientoUC_Load(object sender, EventArgs e)
        {

        }
    }
}
