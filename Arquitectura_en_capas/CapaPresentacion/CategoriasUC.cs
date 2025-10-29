using CapaDatos.Interfaces;
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

namespace CapaPresentacion
{
    public partial class CategoriasUC : UserControl
    {
        private readonly TiposElementoCN tiposElementoCN;
        private readonly UbicacionCN ubicacionCN;
        private readonly ModeloCN modeloCN;
        private readonly VarianteElementoCN varianteElementoCN;
        private int IdTipoElemento = 0;
        private int IdUbicacion = 0;
        private int IdModelo_Elementos = 0;
        private int IdVarianteElemento = 0;

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
            dgvUbicaciones.DataSource = ubicacionCN.GetAll();
            dgvModelo_Elementos.DataSource = modeloCN.ObtenerModelos();
            dgvVarianteElemento.DataSource = varianteElementoCN.MostrarDatos();


            cmbTipoElementoModelo.DataSource = tiposElementoCN.GetAllTipo();
            cmbTipoElementoModelo.ValueMember = "IdTipoElemento";
            cmbTipoElementoModelo.DisplayMember = "ElementoTipo";
        }

        private void CategoriasUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 2070);

            MostrarCategoria();
        }

        #region TIPO ELEMENTO
        private void dgvTipoElemento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdTipoElemento = Convert.ToInt32(dgvTipoElemento.Rows[e.RowIndex].Cells["IdTipoElemento"].Value);

            TipoElemento? tipo = tiposElementoCN.GetById(IdTipoElemento);

            txtNombreTipoElementoActu.Text = tipo?.ElementoTipo;
        }

        private void btnCrearTipo_Click(object sender, EventArgs e)
        {
            var tipo = new TipoElemento { ElementoTipo = txtNombreTipoElemento.Text };

            tiposElementoCN.InsertarTipoElemento(tipo);
            MostrarCategoria();
        }

        private void btnActuTipo_Click(object sender, EventArgs e)
        {
            var tipo = new TipoElemento { IdTipoElemento = IdTipoElemento, ElementoTipo = txtNombreTipoElementoActu.Text };

            tiposElementoCN.ActualizarTipoElemento(tipo);
            MostrarCategoria();
        }
        #endregion

        #region UBICACION
        #endregion

        private void dgvModelo_Elementos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdModelo_Elementos = Convert.ToInt32(dgvModelo_Elementos.Rows[e.RowIndex].Cells["IdModelo"].Value);

            Modelos? modelo = modeloCN.ObtenerPorId(IdModelo_Elementos);

            txtNombreModeloActu.Text = modelo?.NombreModelo;
        }

        private void btnCrearModelo_Click(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                NombreModelo = txtNombreModelo.Text,
                IdTipoElemento = (int)cmbTipoElementoModelo.SelectedValue
            };

            modeloCN.CrearModelo(modelo);
            MostrarCategoria();
        }

        private void btnActuModelo_Click(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                IdModelo = IdModelo_Elementos,
                NombreModelo = txtNombreModeloActu.Text,
                IdTipoElemento = (int)cmbTipoElementoModelo.SelectedValue
            };

            modeloCN.ActualizarModelo(modelo);
            MostrarCategoria();
        }
    }
}
