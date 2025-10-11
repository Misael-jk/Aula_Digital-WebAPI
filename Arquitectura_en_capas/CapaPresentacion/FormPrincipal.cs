using CapaDatos.Interfaces;
using CapaDatos.MappersDTO;
using CapaNegocio;
using CapaDatos.Repos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDatos.UoW;

namespace CapaPresentacion
{
    public partial class FormPrincipal : Form
    {
        #region Variables de User Control
        private Dashboard dashboard;
        private ElementosUC elementosUC;
        private CarritoUC carritoUC;
        private DocentesUC docentesUC;
        private CategoriasUC categoriasUC;
        private UsuariosUC usuariosUC;
        private MantenimientoUC mantenimientoUC;
        private HistorialUC historialUC;
        private InventarioUC inventarioUC;
        private NotebooksUC notebooksUC;
        #endregion

        #region Variables UoW(UnitOfWork) - Repositorios

        #region Carritos
        private readonly IRepoCarritos repoCarritos;
        private readonly IRepoHistorialCambio repoHistorialCambio;
        private readonly IRepoHistorialCarrito repoHistorialCarrito;
        private readonly IRepoUbicacion repoUbicacion;
        private readonly IRepoModelo repoModelo;
        private readonly IRepoNotebooks repoNotebooks;
        private readonly IRepoEstadosMantenimiento repoEstadosMantenimiento;
        #endregion

        #region Elementos
        private readonly IRepoElemento repoElementos;
        private readonly IRepoTipoElemento repoTipoElemento;
        private readonly IRepoHistorialElementos repoHistorialElementos;
        #endregion

        #region Notebook
        private readonly IRepoHistorialNotebook repoHistorialNotebook;
        #endregion

        #region Prestamo
        private readonly IRepoPrestamos repoPrestamos;
        private readonly IRepoPrestamoDetalle repoPrestamoDetalle;
        private readonly IRepoUsuarios repoUsuarios;
        private readonly IRepoDocentes repoDocentes;
        #endregion

        #region Devolucion
        private readonly IRepoEstadosPrestamo repoEstadosPrestamo;
        private readonly IRepoDevolucionDetalle repoDevolucionDetalle;
        private readonly IRepoDevolucion repoDevolucion;
        #endregion


        private readonly IUowCarritos uowCarritos;
        private readonly IUowElementos uowElementos;
        private readonly IUowNotebooks uowNotebooks;
        private readonly IUowPrestamos uowPrestamos;
        private readonly IUowDevolucion uowDevolucion;
        
        private readonly IRepoRoles repoRoles;
        //private readonly IRepoDocentes repoDocentes;
        //private readonly IRepoModelo repoModelo;
        //private readonly IRepoUbicacion repoUbicacion;
        //private readonly IRepoTipoElemento repoTipoElemento;
        //private readonly IRepoUsuarios repoUsuarios;
        #endregion

        #region Variables Mapper Interfaces
        private readonly IMapperElementos mapperElementos;
        private readonly IMapperPrestamos mapperPrestamos;
        private readonly IMapperDevoluciones mapperDevoluciones;
        private readonly IMapperUsuarios mapperUsuarios;
        private readonly IMapperElementosBajas mapperElementosBajas;
        private readonly IMapperCarritos mapperCarritos;
        private readonly IMapperModelo mapperModelos;
        private readonly IMapperDocentes mapperDocentes;
        private readonly IMapperNotebooks mapperNotebooks;
        private readonly IMapperInventario mapperInventario;
        private readonly MapperHistorialElemento mapperHistorialElemento;
        private readonly IMapperHistorialNotebook mapperHistorialNotebook;
        private readonly IMapperHistorialCarrito mapperHistorialCarrito;
        #endregion

        #region Variables Capa Negocio
        private readonly ElementosCN elementoCN;
        private readonly CarritosCN carritosCN;
        private readonly DocentesCN docentesCN;
        private readonly PrestamosCN prestamosCN;
        private readonly TiposElementoCN tiposElementoCN;
        private readonly UsuariosCN usuariosCN;
        private readonly ModeloCN modeloCN;
        //private readonly MantenimientoCN mantenimientoCN;
        private readonly DevolucionCN devolucionCN;
        private readonly NotebooksCN notebooksCN;
        #endregion

        private Usuarios userVerificado;
        private Roles rolUserVerficado;

        public FormPrincipal(IDbConnection conexion, Usuarios userVerificado, Roles rolUserVerificado)
        {
            InitializeComponent();

            this.userVerificado = userVerificado;
            this.rolUserVerficado = rolUserVerificado;

            #region Carritos
            repoCarritos = new RepoCarritos(conexion);
            repoEstadosMantenimiento = new RepoEstadosMantenimiento(conexion);
            repoUbicacion = new RepoUbicacion(conexion);
            repoModelo = new RepoModelo(conexion);
            repoNotebooks = new RepoNotebooks(conexion);
            repoHistorialCambio = new RepoHistorialCambio(conexion);
            repoHistorialCarrito = new RepoHistorialCarrito(conexion);
            #endregion

            #region Elementos
            repoElementos = new RepoElemento(conexion);
            repoTipoElemento = new RepoTipoElemento(conexion);
            repoHistorialElementos = new RepoHistorialElemento(conexion);
            #endregion

            #region Notebook
            repoHistorialNotebook = new RepoHistorialNotebook(conexion);
            #endregion

            #region Prestamos
            repoPrestamos = new RepoPrestamos(conexion);
            repoPrestamoDetalle = new RepoPrestamoDetalle(conexion);
            repoUsuarios = new RepoUsuarios(conexion);
            repoDocentes = new RepoDocentes(conexion);
            #endregion

            #region Devolucion
            repoDevolucionDetalle = new RepoDevolucionDetalle(conexion);
            repoEstadosPrestamo = new RepoEstadosPrestamo(conexion);
            repoDevolucion = new RepoDevolucion(conexion);
            #endregion

            uowCarritos = new UowCarritos(conexion);
            uowElementos = new UowElementos(conexion);
            uowNotebooks = new UowNotebooks(conexion);
            uowPrestamos = new UowPrestamos(conexion);
            uowDevolucion = new UowDevolucion(conexion);
            
            repoRoles = new RepoRoles(conexion);
            //repoDocentes = new RepoDocentes(conexion);
            //repoUsuarios = new RepoUsuarios(conexion);
            //repoTipoElemento = new RepoTipoElemento(conexion);
            //repoModelo = new RepoModelo(conexion);
          

            mapperElementos = new MapperElementos(conexion);
            mapperPrestamos = new MapperPrestamos(conexion);
            mapperDevoluciones = new MapperDevoluciones(conexion);
            mapperUsuarios = new MapperUsuarios(conexion);
            mapperElementosBajas = new MapperElementosBajas(conexion);
            mapperHistorialElemento = new MapperHistorialElemento(conexion);
            mapperHistorialNotebook = new MapperHistorialNotebook(conexion);
            mapperHistorialCarrito = new MapperHistorialCarrito(conexion);
            mapperCarritos = new MapperCarrritos(conexion);
            mapperModelos = new MapperModelo(conexion);
            mapperDocentes = new MapperDocentes(conexion);
            mapperNotebooks = new MapperNotebooks(conexion);
            mapperInventario = new MapperInventario(conexion);

            elementoCN = new ElementosCN(mapperElementos, uowElementos);
            carritosCN = new CarritosCN(mapperCarritos, uowCarritos);
            docentesCN = new DocentesCN(repoDocentes, mapperDocentes);
            prestamosCN = new PrestamosCN(mapperPrestamos, uowPrestamos);
            tiposElementoCN = new TiposElementoCN(repoTipoElemento);
            usuariosCN = new UsuariosCN(repoUsuarios, repoRoles, mapperUsuarios);
            modeloCN = new ModeloCN(repoModelo, mapperModelos);
            devolucionCN = new DevolucionCN(mapperDevoluciones, uowDevolucion);
            //mantenimientoCN = new MantenimientoCN(repoElementoMantenimiento, mapperElementoMantenimiento, repoHistorialElemento);
            notebooksCN = new NotebooksCN(mapperNotebooks, uowNotebooks);
        }


        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            if (rolUserVerficado.Rol == "Administrador")
            {
                btnMantenimiento.Visible = true;
                BtnUsuario.Visible = true;
            }
            else
            {
                btnMantenimiento.Visible = false;
                BtnUsuario.Visible = false;

            }

            lblUsuario.Text = userVerificado.Nombre + " " + userVerificado.Apellido;
            lblRol.Text = rolUserVerficado.Rol;
        }

        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void CambiarNombrePort(string nombre)
        {
            lblPort.Text = nombre;
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            historialUC = new HistorialUC(mapperHistorialElemento, mapperHistorialNotebook, mapperHistorialCarrito);
            CambiarNombrePort(BtnDashboard.Text);

            if (!pnlContenedor.Controls.Contains(historialUC))
            {
                pnlContenedor.Controls.Add(historialUC);
            }

            historialUC.Visible = true;
            historialUC.BringToFront();
        }

        private void BtnElementos_Click(object sender, EventArgs e)
        {
            elementosUC = new ElementosUC(elementoCN, repoEstadosMantenimiento, repoElementos, tiposElementoCN);
            CambiarNombrePort(BtnElementos.Text);

            if (!pnlContenedor.Controls.Contains(elementosUC))
            {
                pnlContenedor.Controls.Add(elementosUC);
            }

            elementosUC.Visible = true;
            elementosUC.BringToFront();

            try
            {
                elementosUC.CargarElementos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en TipoElementoUC: " + ex.Message);
            }
        }

        private void BtnCarritos_Click(object sender, EventArgs e)
        {
            carritoUC = new CarritoUC(carritosCN, repoNotebooks, repoEstadosMantenimiento, userVerificado, repoUbicacion);
            CambiarNombrePort(BtnCarritos.Text);

            if (!pnlContenedor.Controls.Contains(carritoUC))
            {
                pnlContenedor.Controls.Add(carritoUC);
            }

            carritoUC.Visible = true;
            carritoUC.BringToFront();
        }

        private void BtnDocentes_Click(object sender, EventArgs e)
        {
            docentesUC = new DocentesUC(docentesCN);
            CambiarNombrePort(BtnDocentes.Text);

            if (!pnlContenedor.Controls.Contains(docentesUC))
            {
                pnlContenedor.Controls.Add(docentesUC);
            }

            docentesUC.Visible = true;
            docentesUC.BringToFront();

            try
            {
                docentesUC.MostrarDocentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en TipoElementoUC: " + ex.Message);
            }
        }

        private void BtnCategoria_Click(object sender, EventArgs e)
        {
            categoriasUC = new CategoriasUC(repoTipoElemento, repoUbicacion, modeloCN);
            CambiarNombrePort(BtnCategoria.Text);

            if (!pnlContenedor.Controls.Contains(categoriasUC))
            {
                pnlContenedor.Controls.Add(categoriasUC);
            }

            categoriasUC.Visible = true;
            categoriasUC.BringToFront();

            try
            {
                categoriasUC.MostrarCategoria();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en TipoElementoUC: " + ex.Message);
            }
        }

        private void BtnUsuario_Click(object sender, EventArgs e)
        {
            usuariosUC = new UsuariosUC(usuariosCN, repoRoles, repoUsuarios);
            CambiarNombrePort(BtnUsuario.Text);

            if (!pnlContenedor.Controls.Contains(usuariosUC))
            {
                pnlContenedor.Controls.Add(usuariosUC);
            }

            usuariosUC.Visible = true;
            usuariosUC.BringToFront();

            try
            {
                usuariosUC.MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en TipoElementoUC: " + ex.Message);
            }
        }

        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            cmsMantenimiento.Show(btnMantenimiento, new Point(0, btnMantenimiento.Height));

            mantenimientoUC = new MantenimientoUC(tiposElementoCN, repoElementos);
            CambiarNombrePort(btnMantenimiento.Text);

            if (!pnlContenedor.Controls.Contains(mantenimientoUC))
            {
                pnlContenedor.Controls.Add(mantenimientoUC);
            }

            mantenimientoUC.Visible = true;
            mantenimientoUC.BringToFront();

            try
            {
                mantenimientoUC.MostrarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en TipoElementoUC: " + ex.Message);
            }
        }

        private void btnPrestAndDevo_Click(object sender, EventArgs e)
        {

        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            inventarioUC = new InventarioUC(mapperInventario);
            CambiarNombrePort(btnInventario.Text);

            if (!pnlContenedor.Controls.Contains(inventarioUC))
            {
                pnlContenedor.Controls.Add(inventarioUC);
            }

            inventarioUC.Visible = true;

            try
            {
                inventarioUC.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en Historial: " + ex.Message);
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnInfoadminadminUser_Click(object sender, EventArgs e)
        {
            var entrar = new InfoUsuario();
            entrar.Show();
        }

        private void btnNotebooks_Click(object sender, EventArgs e)
        {
            notebooksUC = new NotebooksUC(notebooksCN);
            CambiarNombrePort(btnInventario.Text);

            if (!pnlContenedor.Controls.Contains(notebooksUC))
            {
                pnlContenedor.Controls.Add(notebooksUC);
            }

            notebooksUC.Visible = true;

            try
            {
                notebooksUC.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en Notebook: " + ex.Message);
            }
        }
    }
}
