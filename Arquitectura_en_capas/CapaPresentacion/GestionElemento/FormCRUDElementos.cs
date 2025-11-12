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
    public partial class FormCRUDElementos : Form
    {
        private readonly ElementosCN elementosCN;
        Action cargarDatos;
        private Usuarios usuarioActual;

        public FormCRUDElementos(ElementosCN elementosCN, Usuarios user, Action cargarElementos)
        {
            InitializeComponent();
            this.elementosCN = elementosCN;
            this.usuarioActual = user;
            this.cargarDatos = cargarElementos;
        }

        private void FormCRUDElementos_Load(object sender, EventArgs e)
        {
            cmbUbicacion.DataSource = elementosCN.ObtenerUbicaciones();
            cmbUbicacion.ValueMember = "IdUbicacion";
            cmbUbicacion.DisplayMember = "NombreUbicacion";

            if (cmbUbicacion.Items.Count > 0 && cmbUbicacion.SelectedValue != null)
            {
                int idubicacion = (int)cmbUbicacion.SelectedValue;
                cmbUbicacion.Tag = idubicacion;
            }

            CargarTipos();
            if (cmbTipoElemento.Items.Count > 0 && cmbTipoElemento.SelectedValue != null)
            {
                int idTipo = (int)cmbTipoElemento.SelectedValue;
                cmbTipoElemento.Tag = idTipo;
            }

            if (cmbTipoElemento.Items.Count > 0 && cmbTipoElemento.SelectedValue != null)
            {
                int idVariante = (int)cmbTipoElemento.SelectedValue;
                cmbVariante.Tag = idVariante;
            }
        }

        private void dgvTipoElemento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoElemento.SelectedValue is int selectedValue)
            {
                CargarVariantes(selectedValue);
                cmbTipoElemento.Tag = selectedValue;
            }
        }

        private void dgvVariante_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVariante.SelectedValue is int selectedValue)
            {
                CargarModelo(selectedValue);
                cmbVariante.Tag = selectedValue;
            }
        }

        private void cmbUbicacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVariante.SelectedValue is int selectedValue)
            {
                cmbUbicacion.Tag = selectedValue;
            }
        }

        private void CargarTipos()
        {
            cmbTipoElemento.DataSource = elementosCN.GetTiposByElemento();
            cmbTipoElemento.ValueMember = "IdTipoElemento";
            cmbTipoElemento.DisplayMember = "ElementoTipo";
        }

        private void CargarVariantes(int idTipoElemento)
        {
            cmbVariante.DataSource = elementosCN.ObtenerVariantesPorTipo(idTipoElemento);
            cmbVariante.ValueMember = "IdVarianteElemento";
            cmbVariante.DisplayMember = "Variante";
        }

        private void CargarModelo(int idVariante)
        {
            VariantesElemento? variantes = elementosCN.ObtenerVariantePorID(idVariante);

            if (variantes?.IdModelo != null)
            {
                Modelos? modelos = elementosCN.ObtenerModeloPorID(variantes.IdModelo);
                txtModelo.Text = modelos?.NombreModelo;
                txtModelo.Tag = modelos?.IdModelo;
            }
            else
            {
                txtModelo.Text = "Sin modelo";
                txtModelo.Tag = null;
            }
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCrearElemento_Click(object sender, EventArgs e)
        {
            Elemento elemento = new Elemento()
            {
                IdTipoElemento = Convert.ToInt32(cmbTipoElemento.Tag),
                NumeroSerie = txtNroSerie.Text,
                CodigoBarra = txtCodBarra.Text,
                Patrimonio = txtPatrimonio.Text,
                IdVarianteElemento = Convert.ToInt32(cmbVariante.Tag),
                IdUbicacion = Convert.ToInt32(cmbUbicacion.Tag),
                IdModelo = Convert.ToInt32(txtModelo.Tag),
                IdEstadoMantenimiento = 1,
                Habilitado = true,
                FechaBaja = null
            };

            elementosCN.CrearElemento(elemento, usuarioActual.IdUsuario);
            cargarDatos.Invoke();
            this.Close();
        }
    }
}
