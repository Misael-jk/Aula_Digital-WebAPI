using Moq;
using CapaNegocio;
using CapaDatos.InterfaceUoW;
using CapaDatos.Interfaces;
using CapaEntidad;
using CapaDTOs;
using CapaDatos.InterfacesDTO;

namespace AulaDigital.Tests.Unit.CarritosTests;

public class CarritosFixture : IDisposable
{
    #region Variables Repos para Carritos
    public Mock<IUowCarritos> MockUow { get; }
    public Mock<IRepoCarritos> MockRepoCarritos { get; }
    public Mock<IRepoHistorialCambio> MockRepoHistorialCambio { get; }
    public Mock<IRepoHistorialCarrito> MockRepoHistorialCarrito { get; }
    public Mock<IRepoHistorialNotebook> MockRepoHistorialNotebook { get; }
    public Mock<IRepoModelo> MockRepoModelo { get; }
    public Mock<IRepoUbicacion> MockRepoUbicacion { get; }
    public Mock<IRepoEstadosMantenimiento> MockRepoEstadosMantenimiento { get; }
    public Mock<IRepoNotebooks> MockRepoNotebooks { get; }
    public CarritosCN Service { get; }
    #endregion

    public CarritosFixture()
    {
        #region Instanciacion Mock
        MockUow = new Mock<IUowCarritos>();
        MockRepoCarritos = new Mock<IRepoCarritos>();
        MockRepoHistorialCambio = new Mock<IRepoHistorialCambio>();
        MockRepoHistorialCarrito = new Mock<IRepoHistorialCarrito>();
        MockRepoHistorialNotebook = new Mock<IRepoHistorialNotebook>();
        MockRepoModelo = new Mock<IRepoModelo>();
        MockRepoUbicacion = new Mock<IRepoUbicacion>();
        MockRepoEstadosMantenimiento = new Mock<IRepoEstadosMantenimiento>();
        MockRepoNotebooks = new Mock<IRepoNotebooks>();
        #endregion

        #region SETUP
        MockUow.Setup(u => u.RepoCarritos).Returns(MockRepoCarritos.Object);
        MockUow.Setup(u => u.RepoHistorialCambio).Returns(MockRepoHistorialCambio.Object);
        MockUow.Setup(u => u.RepoHistorialCarrito).Returns(MockRepoHistorialCarrito.Object);
        MockUow.Setup(u => u.RepoHistorialNotebooks).Returns(MockRepoHistorialNotebook.Object);
        MockUow.Setup(u => u.RepoModelo).Returns(MockRepoModelo.Object);
        MockUow.Setup(u => u.RepoUbicacion).Returns(MockRepoUbicacion.Object);
        MockUow.Setup(u => u.RepoEstadosMantenimiento).Returns(MockRepoEstadosMantenimiento.Object);
        MockUow.Setup(u => u.RepoNotebooks).Returns(MockRepoNotebooks.Object);
        #endregion

        Mock<IMapperCarritos> mockMapper = new Mock<IMapperCarritos>();
        mockMapper.Setup(m => m.GetAllDTO()).Returns(new List<CarritosDTO>());


        Service = new CarritosCN(mockMapper.Object, MockUow.Object);
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
            IdCarrito = null,
            PosicionCarrito = null
        };
    }
    #endregion

    public void Dispose()
    {
    }
}
