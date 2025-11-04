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
        private int IdModelo_Notebook = 0;
        private int IdModelo_Carrito = 0;
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
            dgvModelo_Elementos.DataSource = modeloCN.ObtenerModeloPorElementos();
            dgvVarianteElemento.DataSource = varianteElementoCN.MostrarDatos();
            dgvModelo_Notebook.DataSource = modeloCN.ObtenerModelosPorTipoElemento(1);
            dgvModelo_Carritos.DataSource = modeloCN.ObtenerModelosPorTipoElemento(2);


            cmbTipoElementoModelo.DataSource = tiposElementoCN.GetTiposByElemento();
            cmbTipoElementoModelo.ValueMember = "IdTipoElemento";
            cmbTipoElementoModelo.DisplayMember = "ElementoTipo";

            cmbTipoElementoVariante.DataSource = tiposElementoCN.GetTiposByElemento();
            cmbTipoElementoVariante.ValueMember = "IdTipoElemento";
            cmbTipoElementoVariante.DisplayMember = "ElementoTipo";

        }

        private void CategoriasUC_Load(object sender, EventArgs e)
        {
            this.AutoScroll = true;
            this.AutoScrollMinSize = new Size(0, 2070);

            MostrarCategoria();

            #region Variante Elemento
            cmbModeloVarianteActu.DataSource = modeloCN.ObtenerModeloPorElementos();
            cmbModeloVarianteActu.ValueMember = "IdModelo";
            cmbModeloVarianteActu.DisplayMember = "Modelo";

            cmbTipoElementoVarianteActu.DataSource = tiposElementoCN.GetTiposByElemento();
            cmbTipoElementoVarianteActu.ValueMember = "IdTipoElemento";
            cmbTipoElementoVarianteActu.DisplayMember = "ElementoTipo";



            cmbTipoElementoVariante.DataSource = tiposElementoCN.GetTiposByElemento();
            cmbTipoElementoVariante.ValueMember = "IdTipoElemento";
            cmbTipoElementoVariante.DisplayMember = "ElementoTipo";
            #endregion
        }

        // ELEMENTOS
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

        #region MODELOS
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
        #endregion

        #region UBICACIONES
        private void dgvUbicaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdUbicacion = Convert.ToInt32(dgvUbicaciones.Rows[e.RowIndex].Cells["IdUbicacion"].Value);
            Ubicacion? ubicacion = ubicacionCN.GetId(IdUbicacion);

            txtUbicacionActu.Text = ubicacion?.NombreUbicacion;
        }

        private void btnCrearUbicacion_Click(object sender, EventArgs e)
        {
            Ubicacion? ubicacion = new Ubicacion { NombreUbicacion = txtUbicacion.Text };

            ubicacionCN.Insert(ubicacion);
            MostrarCategoria();
        }

        private void btnActualizarUbicacion_Click(object sender, EventArgs e)
        {
            Ubicacion? ubicacion = new Ubicacion { IdUbicacion = IdUbicacion, NombreUbicacion = txtUbicacionActu.Text };

            ubicacionCN.Update(ubicacion);
            MostrarCategoria();
        }
        #endregion

        #region VARIANTE ELEMENTO
        private void dgvVarianteElemento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdVarianteElemento = Convert.ToInt32(dgvVarianteElemento.Rows[e.RowIndex].Cells["IdVarianteElemento"].Value);
            VariantesElemento? variante = varianteElementoCN.GetById(IdVarianteElemento);

            txtNombreVarianteActu.Text = variante?.Variante;
            cmbModeloVarianteActu.SelectedValue = (int)cmbModeloVarianteActu.SelectedValue;
            cmbTipoElementoVarianteActu.SelectedValue = (int)cmbTipoElementoVarianteActu.SelectedValue;
        }

        private void btnCrearVariante_Click(object sender, EventArgs e)
        {
            var variante = new VariantesElemento
            {
                Variante = txtNombreVariante.Text,
                IdModelo = (int)cmbModeloVariante.SelectedValue,
                IdTipoElemento = (int)cmbTipoElementoVariante.SelectedValue
            };

            varianteElementoCN.Insert(variante);
            MostrarCategoria();
        }

        private void btnActuVariante_Click(object sender, EventArgs e)
        {
            var variante = new VariantesElemento
            {
                IdVarianteElemento = IdVarianteElemento,
                Variante = txtNombreVarianteActu.Text,
                IdModelo = (int)cmbModeloVarianteActu.SelectedValue,
                IdTipoElemento = (int)cmbTipoElementoVarianteActu.SelectedValue
            };

            varianteElementoCN.Actualizar(variante);
            MostrarCategoria();
        }
        #endregion
        // -------------



        // NOTEBOOK
        #region MODELOS
        private void dgvModelo_Notebook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            IdModelo_Notebook = Convert.ToInt32(dgvModelo_Notebook.Rows[e.RowIndex].Cells["IdModelo"].Value);

            Modelos? modelo = modeloCN.ObtenerPorId(IdModelo_Notebook);

            txtNombreModeloNoteAct.Text = modelo?.NombreModelo;
        }

        private void btnCrearModeloNotebook1_Click(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                NombreModelo = txtNombreModeloNote.Text,
                IdTipoElemento = 1
            };

            modeloCN.CrearModelo(modelo);
            MostrarCategoria();
        }

        private void btnActuModeloNote_Click(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                IdModelo = IdModelo_Notebook,
                NombreModelo = txtNombreModeloNoteAct.Text,
                IdTipoElemento = 1
            };

            modeloCN.ActualizarModelo(modelo);
            MostrarCategoria();
        }
        #endregion
        // -------------



        // CARRITOS
        #region MODELOS
        private void dgvModelo_Carritos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            IdModelo_Carrito = Convert.ToInt32(dgvModelo_Carritos.Rows[e.RowIndex].Cells["IdModelo"].Value);
            Modelos? modelo = modeloCN.ObtenerPorId(IdModelo_Carrito);

            txtNombreModeloCarrAct.Text = modelo?.NombreModelo;
        }

        private void btnCrearModeloCarrito1_Click(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                NombreModelo = txtNombreModeloCarr.Text,
                IdTipoElemento = 2
            };

            modeloCN.CrearModelo(modelo);
            MostrarCategoria();
        }

        private void btnActuModeloCarr_Click_1(object sender, EventArgs e)
        {
            var modelo = new Modelos
            {
                IdModelo = IdModelo_Carrito,
                NombreModelo = txtNombreModeloCarrAct.Text,
                IdTipoElemento = 2
            };

            modeloCN.ActualizarModelo(modelo);
            MostrarCategoria();
        }
        #endregion


        private void cmbTipoElementoVariante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoElementoVariante.SelectedValue is int selectedValue)
            {

                cmbModeloVariante.DataSource = modeloCN.ObtenerModelosPorTipoElemento(selectedValue);
                cmbModeloVariante.ValueMember = "IdModelo";
                cmbModeloVariante.DisplayMember = "Modelo";
            }
            else
            {
                selectedValue = 0;
            }
        }

        private void cmbTipoElementoVarianteActu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbTipoElementoVarianteActu.SelectedValue is int selectedValue)
            {
                cmbModeloVarianteActu.DataSource = modeloCN.ObtenerModelosPorTipoElemento(selectedValue);
                cmbModeloVarianteActu.ValueMember = "IdModelo";
                cmbModeloVarianteActu.DisplayMember = "Modelo";
            }
        }
    }
}
