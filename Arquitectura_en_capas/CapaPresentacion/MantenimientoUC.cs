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
            //var elementos = mantenimientoCN.GetAllElementos();
            //dgvMantenimiento.DataSource = elementos.ToList();
        }

        private void btnHabilitar_Click(object sender, EventArgs e)
        {
            //mantenimientoCN.HabilitarElemento(txtNroSerie.Text);
            MostrarDatos();
        }

        private void dgvMantenimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvMantenimiento.Rows[e.RowIndex];

            txtNroSerie.Text = fila.Cells["NumeroSerie"].Value?.ToString();

            Elemento? elemento = repoElemento.GetByNumeroSerie(txtNroSerie.Text);
            cmbTipoElemento.SelectedIndex = elemento.IdTipoElemento - 1;
        }

        private void MantenimientoUC_Load(object sender, EventArgs e)
        {
            var tipos = tiposElementoCN.GetAllTipo();
            cmbTipoElemento.DataSource = tipos.ToList(); 
            cmbTipoElemento.ValueMember = "IdTipoElemento";
            cmbTipoElemento.DisplayMember = "ElementoTipo";
        }
    }
}
