using Moq;
using CapaNegocio;
using CapaDatos.InterfaceUoW;
using CapaDatos.Interfaces;
using CapaEntidad;
using CapaDTOs;
using CapaDatos.InterfacesDTO;

namespace AulaDigital.Test.ElementosTests;

public class FixtureElementos : IDisposable
{
    #region VARIABLES REPOS PARA ELEMENTOS
    public Mock<IRepoElemento> RepoElemento { get; }
    public Mock<IRepoHistorialCambio> RepoHistorialCambio { get; }
    public Mock<IRepoHistorialElementos> RepoHistorialElementos { get; }
    public Mock<IRepoUbicacion> RepoUbicacion { get; }
    public Mock<IRepoModelo> RepoModelo { get; }
    public Mock<IRepoVarianteElemento> RepoVarianteElemento { get; }
    public Mock<IRepoEstadosMantenimiento> RepoEstadosMantenimiento { get; }
    public Mock<IRepoTipoElemento> RepoTipoElemento { get; }
    public Mock<IUowElementos> MockUow { get; }
    public ElementosCN Service { get; }
    #endregion

    public FixtureElementos()
    {
        #region Instanciacion Mock
        RepoElemento = new Mock<IRepoElemento>();
        RepoHistorialCambio = new Mock<IRepoHistorialCambio>();
        RepoHistorialElementos = new Mock<IRepoHistorialElementos>();
        RepoUbicacion = new Mock<IRepoUbicacion>();
        RepoModelo = new Mock<IRepoModelo>();
        RepoVarianteElemento = new Mock<IRepoVarianteElemento>();
        RepoEstadosMantenimiento = new Mock<IRepoEstadosMantenimiento>();
        RepoTipoElemento = new Mock<IRepoTipoElemento>();
        MockUow = new Mock<IUowElementos>();
        #endregion

        MockUow.Setup(u => u.RepoElemento).Returns(RepoElemento.Object);
        MockUow.Setup(u => u.RepoHistorialCambio).Returns(RepoHistorialCambio.Object);
        MockUow.Setup(u => u.RepoHistorialElementos).Returns(RepoHistorialElementos.Object);
        MockUow.Setup(u => u.RepoUbicacion).Returns(RepoUbicacion.Object);
        MockUow.Setup(u => u.RepoModelo).Returns(RepoModelo.Object);
        MockUow.Setup(u => u.RepoVarianteElemento).Returns(RepoVarianteElemento.Object);
        MockUow.Setup(u => u.RepoEstadosMantenimiento).Returns(RepoEstadosMantenimiento.Object);
        MockUow.Setup(u => u.RepoTipoElemento).Returns(RepoTipoElemento.Object);

        Mock<IMapperElementos> mockMapper = new Mock<IMapperElementos>();
        mockMapper.Setup(m => m.GetAllDTO()).Returns(new List<ElementosDTO>());

        Service = new ElementosCN(mockMapper.Object, MockUow.Object);
    }

    #region Crear Elemento
    public Elemento CreateElemento(int id = 1)
    {
        return new Elemento
        {
            IdElemento = id,
            IdVarianteElemento = 1,
            NumeroSerie = $"SN-{id}",
            CodigoBarra = $"CB-{id}",
            Patrimonio = $"P-{id}",
            IdEstadoMantenimiento = 1,
            IdUbicacion = 1,
            IdModelo = 1,
            IdTipoElemento = 1,
            Habilitado = true,
            FechaBaja = null
        };
    }
    #endregion

    public void Dispose()
    {
    }
}
