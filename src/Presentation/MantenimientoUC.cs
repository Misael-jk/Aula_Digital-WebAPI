using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaEntidad;
using CapaNegocio;
using Microsoft.VisualBasic.ApplicationServices;
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
        private readonly NotebookBajasCN notebookBajasCN;
        private readonly ElementosBajasCN elementosBajasCN;
        private readonly CarritosBajasCN carritosBajasCN;
        private Usuarios usuarioActual;
        private readonly CarritoUC carritoUC;
        private readonly NotebooksUC notebooksUC;
        private readonly ElementosUC elementosUC;
        private readonly CarritosCN carritosCN;
        private readonly NotebooksCN notebooksCN;
        private readonly ElementosCN elementosCN;
        private readonly FormPrincipal formPrincipal;

        private int _idActualElemento;
        private int _idActualNotebook;
        private int _idActualCarrito;
        private enum RecursoActual { Ninguno, Notebook, Carrito, Elemento }
        private RecursoActual recursoActual = RecursoActual.Ninguno;

        public MantenimientoUC(NotebookBajasCN notebookBajasCN, ElementosBajasCN elementosBajasCN, CarritosBajasCN carritosBajasCN, Usuarios user, FormPrincipal formPrincipal, CarritoUC carritoUC, NotebooksUC notebooksUC, ElementosUC elementosUC, CarritosCN carritosCN, NotebooksCN notebooksCN, ElementosCN elementosCN)
        {
            InitializeComponent();
            this.notebookBajasCN = notebookBajasCN;
            this.elementosBajasCN = elementosBajasCN;
            this.carritosBajasCN = carritosBajasCN;
            usuarioActual = user;
            this.formPrincipal = formPrincipal;
            this.carritoUC = carritoUC;
            this.notebooksUC = notebooksUC;
            this.elementosUC = elementosUC;
            this.carritosCN = carritosCN;
            this.notebooksCN = notebooksCN;
            this.elementosCN = elementosCN;
        }

        private void MantenimientoUC_Load(object sender, EventArgs e)
        {
            MostrarDatos();


        }

        public void MostrarDatos()
        {
            try
            {
                switch (recursoActual)
                {
                    case RecursoActual.Elemento:
                        dgvMantenimiento.DataSource = elementosBajasCN.GetAllElementos();
                        lblRecursoElegido.Text = "Elementos Deshabilitados";

                        #region DGV
                        dgvMantenimiento.Columns["IdElemento"].HeaderText = "ID";
                        dgvMantenimiento.Columns["IdElemento"].Width = 40;
                        dgvMantenimiento.Columns["IdElemento"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        #endregion
                        break;
                    case RecursoActual.Carrito:
                        dgvMantenimiento.DataSource = carritosBajasCN.GetAllDTO();
                        lblRecursoElegido.Text = "Carritos Deshabilitados";

                        #region DGV
                        dgvMantenimiento.Columns["IdCarrito"].HeaderText = "ID";
                        dgvMantenimiento.Columns["IdCarrito"].Width = 40;
                        dgvMantenimiento.Columns["IdCarrito"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvMantenimiento.Columns["NumeroSerieCarrito"].HeaderText = "Serie";
                        dgvMantenimiento.Columns["Capacidad"].Width = 70;
                        dgvMantenimiento.Columns["Capacidad"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        #endregion
                        break;
                    case RecursoActual.Notebook:
                        dgvMantenimiento.DataSource = notebookBajasCN.GetAllNotebooks();
                        lblRecursoElegido.Text = "Notebooks Deshabilitados";

                        #region DGV
                        dgvMantenimiento.Columns["IdNotebook"].HeaderText = "ID";
                        dgvMantenimiento.Columns["IdNotebook"].Width = 40;
                        dgvMantenimiento.Columns["IdNotebook"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvMantenimiento.Columns["PosicionCarrito"].HeaderText = "Posicion";
                        dgvMantenimiento.Columns["PosicionCarrito"].Width = 56;
                        dgvMantenimiento.Columns["PosicionCarrito"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        #endregion
                        break;
                    default:
                        dgvMantenimiento.DataSource = null;
                        lblRecursoElegido.Text = "Seleccione un recurso";
                        break;
                }

                ResetIdsSeleccion();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSetElemento_Click(object sender, EventArgs e)
        {
            recursoActual = RecursoActual.Elemento;
            MostrarDatos();
        }

        private void btnSetCarrito_Click(object sender, EventArgs e)
        {
            recursoActual = RecursoActual.Carrito;
            MostrarDatos();
        }

        private void btnSetNotebook_Click(object sender, EventArgs e)
        {
            recursoActual = RecursoActual.Notebook;
            MostrarDatos();
        }

        private void dgvMantenimiento_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvMantenimiento.Rows.Count == 0) return;

            switch (recursoActual)
            {
                case RecursoActual.Elemento:
                    _idActualElemento = Convert.ToInt32(dgvMantenimiento.Rows[e.RowIndex].Cells["IdElemento"].Value);
                    break;
                case RecursoActual.Carrito:
                    _idActualCarrito = Convert.ToInt32(dgvMantenimiento.Rows[e.RowIndex].Cells["IdCarrito"].Value);
                    break;
                case RecursoActual.Notebook:
                    _idActualNotebook = Convert.ToInt32(dgvMantenimiento.Rows[e.RowIndex].Cells["IdNotebook"].Value);
                    break;
            }
        }

        private void ResetIdsSeleccion()
        {
            _idActualElemento = 0;
            _idActualNotebook = 0;
            _idActualCarrito = 0;
        }

        private void btnHabilitacion_Click_1(object sender, EventArgs e)
        {
            switch (recursoActual)
            {
                case RecursoActual.Elemento:
                    if (_idActualElemento <= 0)
                    {
                        MessageBox.Show("Seleccione un elemento primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var GestionElemento = new ElementoGestionUC(formPrincipal, this, elementosCN, elementosBajasCN, _idActualElemento, usuarioActual);
                    formPrincipal.MostrarUserControl(GestionElemento);
                    break;

                case RecursoActual.Carrito:
                    if (_idActualCarrito <= 0)
                    {
                        MessageBox.Show("Seleccione un carrito primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var GestionCarrito = new CarritoGestionUC(formPrincipal, this, carritosCN, _idActualCarrito, usuarioActual, carritosBajasCN);
                    formPrincipal.MostrarUserControl(GestionCarrito);
                    break;

                case RecursoActual.Notebook:
                    if (_idActualNotebook <= 0)
                    {
                        MessageBox.Show("Seleccione una notebook primero.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var GestionNotebook = new NotebookGestionUC(formPrincipal, this, notebooksCN, _idActualNotebook, usuarioActual, notebookBajasCN);
                    formPrincipal.MostrarUserControl(GestionNotebook);
                    break;

                default:
                    MessageBox.Show("Seleccione un recurso para gestionar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
    }
}
