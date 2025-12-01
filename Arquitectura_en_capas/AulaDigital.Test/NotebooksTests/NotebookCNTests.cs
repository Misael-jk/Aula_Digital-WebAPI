using CapaEntidad;
using Moq;

namespace AulaDigital.Test.NotebooksTests;

public class NotebookCNTests : IClassFixture<FixtureNotebook>
{
    private readonly FixtureNotebook fixture;

    public NotebookCNTests(FixtureNotebook fixture)
    {
        this.fixture = fixture;
    }

    #region INSERT NOTEBOOK Transaccion
    [Fact]
    public void InsertNotebook()
    {
        int idUsuario = 2;
        Notebooks? nuevo = fixture.CreateNotebook(0);
        Carritos? carrito = fixture.CreateCarrito(1);

        fixture.RepoNotebooks.Setup(r => r.GetById(nuevo.IdElemento)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByEquipo(nuevo.Equipo)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerie)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByCodigoBarra(nuevo.CodigoBarra)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByPatrimonio(nuevo.Patrimonio)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetNotebookByPosicion(nuevo.IdCarrito, nuevo.PosicionCarrito.Value)).Returns((Notebooks?)null);

        fixture.RepoCarritos.Setup(r => r.GetById(carrito.IdCarrito)).Returns(carrito);
        fixture.RepoCarritos.Setup(r => r.GetDisponible(carrito.IdCarrito)).Returns(true);

        fixture.RepoEstadosMantenimiento.Setup(r => r.GetById(nuevo.IdEstadoMantenimiento))
            .Returns(new EstadosMantenimiento
            {
                IdEstadoMantenimiento = 1,
                EstadoMantenimientoNombre = "Disponible"
            });

        fixture.RepoUbicacion.Setup(r => r.GetById(nuevo.IdUbicacion))
            .Returns(new Ubicacion
            {
                IdUbicacion = 1,
                NombreUbicacion = "Espacio Digital"
            });

        fixture.RepoModelo.Setup(r => r.GetById(nuevo.IdModelo))
            .Returns(new Modelos
            {
                IdModelo = 1,
                IdTipoElemento = 1,
                NombreModelo = "HP 3014"
            });

        fixture.RepoTipoElemento.Setup(r => r.GetById(nuevo.IdTipoElemento))
            .Returns(new TipoElemento
            {
                IdTipoElemento = 1,
                ElementoTipo = "Cable"
            });

        fixture.NotebooksCN.CrearNotebook(nuevo, idUsuario);

        fixture.RepoNotebooks.Verify(r => r.Insert(It.Is<Notebooks>(e =>
            e.Equipo == nuevo.Equipo &&
            e.NumeroSerie == nuevo.NumeroSerie &&
            e.CodigoBarra == nuevo.CodigoBarra &&
            e.Patrimonio == nuevo.Patrimonio &&
            e.IdModelo == nuevo.IdModelo &&
            e.IdTipoElemento == nuevo.IdTipoElemento &&
            e.IdCarrito == nuevo.IdCarrito &&
            e.PosicionCarrito == nuevo.PosicionCarrito &&
            e.IdVarianteElemento == null
        )), Times.Once);

        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }
    #endregion

    #region INSERT NOTEBOOK Transaccion
    [Fact]
    public void UpdateNotebook()
    {
        int idUsuario = 2;
        Notebooks? exists = fixture.CreateNotebook(1);
        Carritos? carrito = fixture.CreateCarrito(1);

        fixture.RepoNotebooks.Setup(r => r.GetById(exists.IdElemento)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByEquipo(exists.Equipo)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByNumeroSerie(exists.NumeroSerie)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByCodigoBarra(exists.CodigoBarra)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetByPatrimonio(exists.Patrimonio)).Returns((Notebooks?)null);
        fixture.RepoNotebooks.Setup(r => r.GetNotebookByPosicion(exists.IdCarrito, exists.PosicionCarrito.Value)).Returns((Notebooks?)null);

        fixture.RepoCarritos.Setup(r => r.GetById(carrito.IdCarrito)).Returns(carrito);
        fixture.RepoCarritos.Setup(r => r.GetDisponible(carrito.IdCarrito)).Returns(true);

        fixture.RepoEstadosMantenimiento.Setup(r => r.GetById(exists.IdEstadoMantenimiento))
            .Returns(new EstadosMantenimiento
            {
                IdEstadoMantenimiento = 1,
                EstadoMantenimientoNombre = "Disponible"
            });

        fixture.RepoUbicacion.Setup(r => r.GetById(exists.IdUbicacion))
            .Returns(new Ubicacion
            {
                IdUbicacion = 1,
                NombreUbicacion = "Espacio Digital"
            });

        fixture.RepoModelo.Setup(r => r.GetById(exists.IdModelo))
            .Returns(new Modelos
            {
                IdModelo = 1,
                IdTipoElemento = 1,
                NombreModelo = "HP 3014"
            });

        //fixture.RepoTipoElemento.Setup(r => r.GetById(exists.IdTipoElemento)).Returns((TipoElemento?)null);

        fixture.NotebooksCN.ActualizarNotebook(exists, idUsuario);

        fixture.RepoNotebooks.Verify(r => r.Update(It.Is<Notebooks>(e =>
            e.Equipo == exists.Equipo &&
            e.NumeroSerie == exists.NumeroSerie &&
            e.CodigoBarra == exists.CodigoBarra &&
            e.Patrimonio == exists.Patrimonio &&
            e.IdModelo == exists.IdModelo &&
            e.IdCarrito == exists.IdCarrito &&
            e.PosicionCarrito == exists.PosicionCarrito &&
            e.IdVarianteElemento == null
        )), Times.Once);

        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }
    #endregion
}
