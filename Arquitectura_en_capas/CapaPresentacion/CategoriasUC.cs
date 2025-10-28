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
    public partial class CategoriasUC : UserControl
    {
        private readonly TiposElementoCN tiposElementoCN;
        private readonly UbicacionCN ubicacionCN;
        private readonly ModeloCN modeloCN;
        private readonly VarianteElementoCN varianteElementoCN;

        public CategoriasUC(TiposElementoCN tiposElementoCN, UbicacionCN ubicacionCN, ModeloCN modeloCN, VarianteElementoCN varianteElementoCN)
        {
            InitializeComponent();
            this.tiposElementoCN = tiposElementoCN;
            this.ubicacionCN = ubicacionCN;
            this.modeloCN = modeloCN;
            this.varianteElementoCN = varianteElementoCN;
        }

        public void MostrarCategoria()
        {
            dgvTipoElemento.DataSource = tiposElementoCN.GetAllTipo();
            dgvUbicacion.DataSource = ubicacionCN.GetAll();
            dgvModelo.DataSource = modeloCN.ObtenerModelos();
        }

        private void CategoriasUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 2070);

            MostrarCategoria();
        }
    }
}
