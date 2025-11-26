

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
        #region Conexion
        private readonly IDbConnection connection;
        #endregion

        #region Variables de User Control
        private Dashboard dashboard;
        private ElementosUC elementosUC;
        private CarritoUC carritoUC;
        private DocentesUC docentesUC;
        private CategoriasUC categoriasUC;
        private UsuariosUC usuariosUC;
        private MantenimientoUC mantenimientoUC;
        private InventarioUC inventarioUC;
        private NotebooksUC notebooksUC;
        private UsuarioGestionuc usuarioGestion;
        private PrestamosYDevolucionesUC prestamosYDevolucionesUC;
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
        private readonly IRepoRoles repoRoles;
        #endregion

        #region Elementos
        private readonly IRepoElemento repoElementos;
        private readonly IRepoTipoElemento repoTipoElemento;
        private readonly IRepoHistorialElementos repoHistorialElementos;
        private readonly IRepoVarianteElemento repoVarianteElemento;
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
        private readonly IMapperCarritosBajas mapperCarritosBajas;
        private readonly IMapperVarianteElemento mapperVarianteElemento;
        private readonly IMapperDocentesBajas mapperDocentesBajas;
        private readonly IMapperUsuariosBajas mapperUsuariosBajas;
        private readonly IMapperNotebookBajas mapperNotebookBajas;
        private readonly IMapperPrestamosActivos mapperPrestamosActivos;
        private readonly IMapperHistorialElementoG mapperHistorialElementoG;
        private readonly IMapperTransaccion mapperTransaccion;
        private readonly IMapperNotebooksCarro mapperNotebooksCarro;
        private readonly IMapperHistorialNotebookG mapperHistorialNotebookG;
        private readonly IMapperHistorialCarritosG mapperHistorialCarritosG;
        private readonly IMapperPrestamoDetalle mapperPrestamoDetalle;
        private readonly IMapperDevolucionDetalle mapperDevolucionDetalle;
        private readonly IMapperNotebooksPrestadas mapperNotebooksPrestadas;
        private readonly IMapperRankingDocente mapperRankingDocente;
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
        private readonly CarritosBajasCN carritosBajasCN;
        private readonly ElementosBajasCN elementosBajasCN;
        private readonly VarianteElementoCN varianteElementoCN;
        private readonly UbicacionCN ubicacionCN;
        private readonly DocentesBajasCN docentesBajasCN;
        private readonly NotebookBajasCN notebookBajasCN;
        private readonly UsuariosBajasCN usuariosBajasCN;
        #endregion

        private Usuarios userVerificado;
        private Roles rolUserVerficado;
        private System.Windows.Forms.UserControl ucActual;

        public FormPrincipal(IDbConnection conexion, Usuarios userVerificado, Roles rolUserVerificado)
        {
            InitializeComponent();


            this.userVerificado = userVerificado;
            this.rolUserVerficado = rolUserVerificado;

            this.connection = conexion;

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
            repoVarianteElemento = new RepoVarianteElemento(conexion);
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
            mapperCarritosBajas = new MapperCarritosBajas(conexion);
            mapperVarianteElemento = new MapperVarianteElemento(conexion);
            mapperDocentesBajas = new MapperDocentesBajas(conexion);
            mapperUsuariosBajas = new MapperUsuariosBajas(conexion);
            mapperNotebookBajas = new MapperNotebookBajas(conexion);
            mapperPrestamosActivos = new MapperPrestamosActivos(conexion);
            mapperHistorialElementoG = new MapperHistorialElementoG(conexion);
            mapperTransaccion = new MapperTransaccion(conexion);
            mapperNotebooksCarro = new MapperNotebooksCarro(conexion);
            mapperHistorialNotebookG = new MapperHistorialNotebookG(conexion);
            mapperHistorialCarritosG = new MapperHistorialCarritoG(conexion);
            mapperPrestamoDetalle = new MapperPrestamoDetalle(conexion);
            mapperDevolucionDetalle = new MapperDevolucionDetalle(conexion);
            mapperNotebooksPrestadas = new MapperNotebooksPrestadas(conexion);
            mapperRankingDocente = new MapperRankingDocentes(conexion);

            elementoCN = new ElementosCN(mapperElementos, uowElementos, mapperHistorialElementoG);
            carritosCN = new CarritosCN(mapperCarritos, uowCarritos, mapperHistorialCarritosG);
            docentesCN = new DocentesCN(repoDocentes, mapperDocentes);
            prestamosCN = new PrestamosCN(mapperPrestamos, uowPrestamos, mapperNotebooksCarro, mapperPrestamoDetalle);
            tiposElementoCN = new TiposElementoCN(repoTipoElemento);
            usuariosCN = new UsuariosCN(repoUsuarios, repoRoles, mapperUsuarios, repoHistorialCambio);
            modeloCN = new ModeloCN(repoModelo, mapperModelos, repoTipoElemento);
            devolucionCN = new DevolucionCN(mapperDevoluciones, uowDevolucion, mapperDevolucionDetalle);
            notebooksCN = new NotebooksCN(mapperNotebooks, uowNotebooks, mapperHistorialNotebookG);
            carritosBajasCN = new CarritosBajasCN(mapperCarritosBajas, uowCarritos);
            elementosBajasCN = new ElementosBajasCN(mapperElementosBajas, uowElementos);
            varianteElementoCN = new VarianteElementoCN(repoVarianteElemento, repoTipoElemento, repoModelo, mapperVarianteElemento);
            ubicacionCN = new UbicacionCN(repoUbicacion);
            docentesBajasCN = new DocentesBajasCN(mapperDocentesBajas, repoDocentes);
            usuariosBajasCN = new UsuariosBajasCN(mapperUsuariosBajas, repoUsuarios);
            notebookBajasCN = new NotebookBajasCN(mapperNotebookBajas, uowNotebooks);

            Dashboard();
        }

        #region Eventos del Formulario

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            CargarDatosDelUsuarioActual();
        }

        public void CargarDatosDelUsuarioActual()
        {

            Usuarios? usuarios = usuariosCN.ObtenerID(userVerificado.IdUsuario);

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

            if (usuarios?.FotoPerfil == null)
            {
                btnVerPerfil.BackgroundImage = Properties.Resources.Perfil_default;
            }

            if (usuarios?.FotoPerfil == "fotoperfil1")
            {
                btnVerPerfil.BackgroundImage = Properties.Resources.fotoperfil1;
            }

            if (usuarios?.FotoPerfil == "fotoperfil2")
            {
                btnVerPerfil.BackgroundImage = Properties.Resources.fotoperfil2;
            }

            if (usuarios?.FotoPerfil == "fotoperfil3")
            {
                btnVerPerfil.BackgroundImage = Properties.Resources.fotoperfil3;
            }

            if (usuarios?.FotoPerfil == "fotoperfil4")
            {
                btnVerPerfil.BackgroundImage = Properties.Resources.fotoperfil4;
            }

            lblUsuario.Text = usuarios?.Nombre + " " + usuarios?.Apellido;
            lblRol.Text = rolUserVerficado.Rol;

            pnlContenedor.AutoScroll = true;
        }


        private void BtnCerrar1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnInfoUser_Click(object sender, EventArgs e)
        {
            AbrirUsuarioGestion(userVerificado.IdUsuario);
        }

        public void AbrirUsuarioGestion(int idUsuarioSeleccionado)
        {
            pnlGestionUsuario.Visible = true;

            if (usuarioGestion == null)
                usuarioGestion = new UsuarioGestionuc(usuariosCN, usuariosBajasCN, idUsuarioSeleccionado, repoHistorialCambio, userVerificado);


            if (!pnlGestionUsuario.Controls.Contains(usuarioGestion))
            {
                pnlGestionUsuario.Controls.Add(usuarioGestion);
                usuarioGestion.Dock = DockStyle.Fill;
            }

            usuarioGestion.ActualizarUC(idUsuarioSeleccionado);
        }

        #endregion

        #region Métodos Auxiliares

        public void MostrarUserControl(System.Windows.Forms.UserControl uc)
        {
            if (!pnlContenedor.Controls.Contains(uc))
            {
                pnlContenedor.Controls.Add(uc);
                uc.Dock = DockStyle.Fill;
            }
            MostrarSolo(uc);
        }

        private void CambiarNombrePort(string nombre)
        {
            lblPort.Text = nombre;
        }

        private void MostrarSolo(System.Windows.Forms.UserControl uc)
        {
            if (ucActual == uc)
            {
                DesplazarScrollArriba(uc);
                return;
            }

            foreach (System.Windows.Forms.Control c in pnlContenedor.Controls)
                c.Visible = c == uc;

            uc.BringToFront();
            pnlContenedor.PerformLayout();

            ucActual = uc;
        }

        private async void DesplazarScrollArriba(System.Windows.Forms.UserControl uc)
        {
            if (!uc.AutoScroll) return;

            for (int i = uc.VerticalScroll.Value; i > 0; i -= 50)
            {
                uc.VerticalScroll.Value = Math.Max(0, i);
                uc.PerformLayout();
                await Task.Delay(10);
            }

            uc.AutoScrollPosition = new Point(0, 0);
        }

        #endregion

        #region Botones Principales

        public void Dashboard()
        {
            CerrarGestionUsuario();

            if (dashboard == null)
                dashboard = new Dashboard(mapperPrestamosActivos, mapperNotebooksPrestadas, mapperRankingDocente, notebooksCN, this, userVerificado, prestamosCN, devolucionCN);

            CambiarNombrePort(BtnDashboard.Text);
            pnlContenedor.Controls.Clear();
            if (!pnlContenedor.Controls.Contains(dashboard))
            {
                pnlContenedor.Controls.Add(dashboard);
                dashboard.Dock = DockStyle.Fill;
            }

            MostrarSolo(dashboard);

            dashboard.MostrarDatos();

        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard();
        }

        private void BtnElementos_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (elementosUC == null)
                elementosUC = new ElementosUC(this, elementoCN, repoEstadosMantenimiento, tiposElementoCN, userVerificado, elementosBajasCN);

            CambiarNombrePort(BtnElementos.Text);

            if (!pnlContenedor.Controls.Contains(elementosUC))
            {
                pnlContenedor.Controls.Add(elementosUC);
                elementosUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(elementosUC);

            try
            {
                elementosUC.CargarElementos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en ElementosUC: " + ex.Message);
            }
        }

        private void BtnCarritos_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (carritoUC == null)
                carritoUC = new CarritoUC(carritosCN, userVerificado, carritosBajasCN, this);

            CambiarNombrePort(BtnCarritos.Text);

            pnlContenedor.Controls.Clear();

            carritoUC.MostrarDatos();
            carritoUC.RenovarDatos();
            if (!pnlContenedor.Controls.Contains(carritoUC))
            {
                pnlContenedor.Controls.Add(carritoUC);
                carritoUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(carritoUC);
        }

        private void BtnDocentes_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (docentesUC == null)
                docentesUC = new DocentesUC(docentesCN, docentesBajasCN);

            CambiarNombrePort(BtnDocentes.Text);

            if (!pnlContenedor.Controls.Contains(docentesUC))
            {
                pnlContenedor.Controls.Add(docentesUC);
                docentesUC.Dock = DockStyle.Fill;
            }

            docentesUC.MostrarDocentes();

            MostrarSolo(docentesUC);
        }

        private void BtnCategoria_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (categoriasUC == null)
                categoriasUC = new CategoriasUC(tiposElementoCN, ubicacionCN, modeloCN, varianteElementoCN);

            CambiarNombrePort(BtnCategoria.Text);

            if (!pnlContenedor.Controls.Contains(categoriasUC))
            {
                pnlContenedor.Controls.Add(categoriasUC);
                categoriasUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(categoriasUC);

            try
            {
                categoriasUC.MostrarCategoria();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en CategoriasUC: " + ex.Message);
            }
        }

        private void BtnUsuario_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (usuariosUC == null)
                usuariosUC = new UsuariosUC(usuariosCN, usuariosBajasCN, this);

            CambiarNombrePort(BtnUsuario.Text);

            if (!pnlContenedor.Controls.Contains(usuariosUC))
            {
                pnlContenedor.Controls.Add(usuariosUC);
                usuariosUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(usuariosUC);

            try
            {
                usuariosUC.MostrarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en UsuariosUC: " + ex.Message);
            }
        }

        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (mantenimientoUC == null)
                mantenimientoUC = new MantenimientoUC(notebookBajasCN, elementosBajasCN, carritosBajasCN, userVerificado, this, carritoUC, notebooksUC, elementosUC, carritosCN, notebooksCN, elementoCN);

            //cmsMantenimiento.Show(btnMantenimiento, new Point(0, btnMantenimiento.Height));

            CambiarNombrePort(btnMantenimiento.Text);

            if (!pnlContenedor.Controls.Contains(mantenimientoUC))
            {
                pnlContenedor.Controls.Add(mantenimientoUC);
                mantenimientoUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(mantenimientoUC);

            try
            {
                mantenimientoUC.MostrarDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en MantenimientoUC: " + ex.Message);
            }
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (inventarioUC == null)
                inventarioUC = new InventarioUC(mapperInventario);

            CambiarNombrePort(btnInventario.Text);

            if (!pnlContenedor.Controls.Contains(inventarioUC))
            {
                pnlContenedor.Controls.Add(inventarioUC);
                inventarioUC.Dock = DockStyle.Fill;
            }

            MostrarSolo(inventarioUC);

            try
            {
                inventarioUC.MostrarInventario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en InventarioUC: " + ex.Message);
            }
        }

        private void btnNotebooks_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (notebooksUC == null)
                notebooksUC = new NotebooksUC(notebooksCN, userVerificado, carritosCN, this, notebookBajasCN);

            CambiarNombrePort(btnNotebooks.Text);

            if (!pnlContenedor.Controls.Contains(notebooksUC))
            {
                pnlContenedor.Controls.Add(notebooksUC);
                notebooksUC.Dock = DockStyle.Fill;
            }

            notebooksUC.ActualizarDataGrid(0);


            MostrarSolo(notebooksUC);

            try
            {
                notebooksUC.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en NotebooksUC: " + ex.Message);
            }
        }

        private void btnPrestAndDevo_Click(object sender, EventArgs e)
        {
            CerrarGestionUsuario();

            if (prestamosYDevolucionesUC == null)
                prestamosYDevolucionesUC = new PrestamosYDevolucionesUC(mapperTransaccion, this, prestamosCN, devolucionCN, userVerificado);

            CambiarNombrePort(btnPrestAndDevo.Text);

            if (!pnlContenedor.Controls.Contains(prestamosYDevolucionesUC))
            {
                pnlContenedor.Controls.Add(prestamosYDevolucionesUC);
                prestamosYDevolucionesUC.Dock = DockStyle.Fill;
            }

            prestamosYDevolucionesUC.ActualizarDataGrid();

            MostrarSolo(prestamosYDevolucionesUC);

            try
            {
                prestamosYDevolucionesUC.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos en NotebooksUC: " + ex.Message);
            }
        }
        #endregion

        public void CerrarGestionUsuario()
        {
            pnlGestionUsuario.Visible = false;
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            LoginState loginState = new LoginState(connection);
            loginState.Show();
            this.Close();
        }
    }
}