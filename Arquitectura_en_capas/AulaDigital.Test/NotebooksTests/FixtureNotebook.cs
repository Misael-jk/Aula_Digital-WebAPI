using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDTOs;
using CapaEntidad;
using CapaNegocio;
using Moq;

namespace AulaDigital.Test.NotebooksTests;

public class FixtureNotebook : IDisposable
{
    public Mock<IUowNotebooks> MockUow { get; }
    public Mock<IRepoNotebooks> RepoNotebooks { get; }
    public Mock<IRepoHistorialCambio> RepoHistorialCambio { get; }
    public Mock<IRepoHistorialNotebook> RepoHistorialNotebook { get; }
    public Mock<IRepoCarritos> RepoCarritos { get; }
    public Mock<IRepoModelo> RepoModelo { get; }
    public Mock<IRepoUbicacion> RepoUbicacion { get; }
    public Mock<IRepoEstadosMantenimiento> RepoEstadosMantenimiento { get; }
    public Mock<IRepoVarianteElemento> RepoVarianteElemento { get; }
    public Mock<IRepoTipoElemento> RepoTipoElemento { get; }
    public Mock<IRepoElemento> RepoElemento { get; }
    public NotebooksCN NotebooksCN { get; set; }

    public FixtureNotebook()
    {
        MockUow = new Mock<IUowNotebooks>();
        RepoNotebooks = new Mock<IRepoNotebooks>();
        RepoHistorialCambio = new Mock<IRepoHistorialCambio>();
        RepoHistorialNotebook = new Mock<IRepoHistorialNotebook>();
        RepoCarritos = new Mock<IRepoCarritos>();
        RepoModelo = new Mock<IRepoModelo>();
        RepoUbicacion = new Mock<IRepoUbicacion>();
        RepoEstadosMantenimiento = new Mock<IRepoEstadosMantenimiento>();
        RepoVarianteElemento = new Mock<IRepoVarianteElemento>();
        RepoTipoElemento = new Mock<IRepoTipoElemento>();
        RepoElemento = new Mock<IRepoElemento>();

        #region SetUp
        MockUow.Setup(r => r.RepoNotebooks).Returns(RepoNotebooks.Object);
        MockUow.Setup(r => r.RepoHistorialCambio).Returns(RepoHistorialCambio.Object);
        MockUow.Setup(r => r.RepoHistorialNotebook).Returns(RepoHistorialNotebook.Object);
        MockUow.Setup(r => r.RepoCarritos).Returns(RepoCarritos.Object);
        MockUow.Setup(r => r.RepoModelo).Returns(RepoModelo.Object);
        MockUow.Setup(r => r.RepoUbicacion).Returns(RepoUbicacion.Object);
        MockUow.Setup(r => r.RepoEstadosMantenimiento).Returns(RepoEstadosMantenimiento.Object);
        MockUow.Setup(r => r.RepoVarianteElemento).Returns(RepoVarianteElemento.Object);
        MockUow.Setup(r => r.RepoTipoElemento).Returns(RepoTipoElemento.Object);
        MockUow.Setup(r => r.RepoElemento).Returns(RepoElemento.Object);
        #endregion

        Mock<IMapperNotebooks> MapperNotebook = new Mock<IMapperNotebooks>();
        MapperNotebook.Setup(r => r.GetAllDTO()).Returns(new List<NotebooksDTO>());

        NotebooksCN = new NotebooksCN(MapperNotebook.Object, MockUow.Object);
    }

    #region Crear CARRITO
    public Carritos CreateCarrito(int id = 1)
    {
        return new Carritos
        {
            IdCarrito = id,
            EquipoCarrito = $"Equipo-{id}",
            NumeroSerieCarrito = $"SN-{id}",
            IdEstadoMantenimiento = 1,
            IdUbicacion = 1,
            IdModelo = 1,
            Habilitado = true,
            FechaBaja = null
        };
    }
    #endregion

    #region Crear NOTEBOOK
    public Notebooks CreateNotebook(int id = 1)
    {
        return new Notebooks
        {
            IdElemento = id,
            Equipo = $"Equipo-{id}",
            NumeroSerie = $"SN-{id}",
            CodigoBarra = $"CB-{id}",
            Patrimonio = $"P-{id}",
            IdEstadoMantenimiento = 1,
            IdUbicacion = 1,
            IdModelo = 1,
            IdVarianteElemento = null,
            IdTipoElemento = 1,
            Habilitado = true,
            FechaBaja = null,
            IdCarrito = 1,
            PosicionCarrito = 12
        };
    }
    #endregion
    public void Dispose()
    {
    }
}
