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
        private readonly IRepoTipoElemento repoTipoElemento;
        private readonly IRepoUbicacion repoUbicacion;
        private readonly ModeloCN modeloCN = null!;
        public CategoriasUC(IRepoTipoElemento repoTipoElemento, IRepoUbicacion repoUbicacion, ModeloCN modeloCN)
        {
            InitializeComponent();
            this.repoTipoElemento = repoTipoElemento;
            this.repoUbicacion = repoUbicacion;
            this.modeloCN = modeloCN;
        }

        public void MostrarCategoria()
        {
            dgvTipoElemento.DataSource = repoTipoElemento.GetAll();
            dgvUbicacion.DataSource = repoUbicacion.GetAll();
            dgvModelo.DataSource = modeloCN.ObtenerModelos();
        }

        private void CategoriasUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 1253);

            MostrarCategoria();
        }
    }
}
