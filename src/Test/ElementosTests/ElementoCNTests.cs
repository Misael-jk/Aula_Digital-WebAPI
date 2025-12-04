using CapaEntidad;
using Moq;

namespace AulaDigital.Test.ElementosTests;

public class ElementoCNTests : IClassFixture<FixtureElementos>
{
    private readonly FixtureElementos fixture;
    public ElementoCNTests(FixtureElementos fixture)
    {
        this.fixture = fixture;
    }

    #region Insert ID Elemento
    [Fact]
    public void InsertElemento()
    {
        int idUsuario = 2;
        Elemento? nuevo = fixture.CreateElemento(0);

        fixture.RepoElemento.Setup(r => r.GetById(nuevo.IdElemento)).Returns((Elemento?)null);

        Assert.Throws<Exception>(() => fixture.Service.CrearElemento(nuevo, idUsuario));

        fixture.RepoElemento.Verify(r => r.Insert(It.IsAny<Elemento>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region Validar Insert TRANSACCION
    [Fact]
    public void ValidarInsert()
    {
        int idUsuario = 2;
        Elemento? nuevo = fixture.CreateElemento(0);

        fixture.RepoElemento.Setup(r => r.GetById(nuevo.IdElemento)).Returns((Elemento?)null);
        fixture.RepoElemento.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerie)).Returns((Elemento?)null);
        fixture.RepoElemento.Setup(r => r.GetByCodigoBarra(nuevo.CodigoBarra)).Returns((Elemento?)null);
        fixture.RepoElemento.Setup(r => r.GetByPatrimonio(nuevo.Patrimonio)).Returns((Elemento?)null);

        fixture.RepoVarianteElemento.Setup(r => r.GetById(nuevo.IdVarianteElemento ?? 0))
            .Returns(new VariantesElemento
            {
                IdVarianteElemento = nuevo.IdVarianteElemento ?? 0,
                IdModelo = nuevo.IdModelo,
                IdTipoElemento = nuevo.IdTipoElemento,
                Variante = "Cable HDMI"
            });

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
                NombreUbicacion = "Armario A"
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

        fixture.Service.CrearElemento(nuevo, idUsuario);


        fixture.RepoElemento.Verify(r => r.Insert(It.Is<Elemento>(e =>
            e.NumeroSerie == nuevo.NumeroSerie &&
            e.CodigoBarra == nuevo.CodigoBarra &&
            e.Patrimonio == nuevo.Patrimonio &&
            e.IdModelo == nuevo.IdModelo &&
            e.IdTipoElemento == nuevo.IdTipoElemento
        )), Times.Once);

        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }
    #endregion

    #region VALIDAR NUMERO SERIE
    [Fact]
    public void ValidarNumeroSerieInsert()
    {
        Elemento? nuevo = fixture.CreateElemento(0);
        Elemento? existente = fixture.CreateElemento(2);

        fixture.RepoElemento.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerie)).Returns(existente);

        var ex= Assert.Throws<Exception>(() => fixture.Service.CrearElemento(nuevo, 1));
        Assert.Equal("\"El elemento ya existe con ese numero de serie y está habilitado.", ex.Message);

        fixture.RepoElemento.Verify(r => r.Insert(It.IsAny<Elemento>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region VALIDAR Codigo BARRA
    [Fact]
    public void ValidarCodigoBArraINsert()
    {
        Elemento? nuevo = fixture.CreateElemento(0);
        Elemento? existente = fixture.CreateElemento(2);

        fixture.RepoElemento.Setup(r => r.GetByCodigoBarra(nuevo.CodigoBarra)).Returns(existente);

        var ex = Assert.Throws<Exception>(() => fixture.Service.CrearElemento(nuevo, 1));
        Assert.Equal("El elemento ya existe con ese codigo de barra y está habilitado.", ex.Message);

        fixture.RepoElemento.Verify(r => r.Insert(It.IsAny<Elemento>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region VALIDAR PATRIMONIO 
    [Fact]
    public void ValidarPatrimonioInsert()
    {
        Elemento? nuevo = fixture.CreateElemento(0);
        Elemento? existente = fixture.CreateElemento(2);

        fixture.RepoElemento.Setup(r => r.GetByPatrimonio(nuevo.Patrimonio)).Returns(existente);

        var ex = Assert.Throws<Exception>(() => fixture.Service.CrearElemento(nuevo, 1));
        Assert.Equal("El elemento ya existe con ese patrimonio y está habilitado.", ex.Message);

        fixture.RepoElemento.Verify(r => r.Insert(It.IsAny<Elemento>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region Validar Update Transaccion
    [Fact]
    public void ValidarUpdate()
    {
        int idUsuario = 2;
        Elemento? existente = fixture.CreateElemento(1);

        fixture.RepoElemento.Setup(r => r.GetById(existente.IdElemento)).Returns(existente);
        fixture.RepoElemento.Setup(r => r.GetByNumeroSerie(existente.NumeroSerie)).Returns(existente);
        fixture.RepoElemento.Setup(r => r.GetByCodigoBarra(existente.CodigoBarra)).Returns(existente);
        fixture.RepoElemento.Setup(r => r.GetByPatrimonio(existente.Patrimonio)).Returns(existente);

        fixture.RepoVarianteElemento.Setup(r => r.GetById(existente.IdVarianteElemento ?? 0))
            .Returns(new VariantesElemento
            {
                IdVarianteElemento = existente.IdVarianteElemento ?? 0,
                IdModelo = existente.IdModelo,
                IdTipoElemento = existente.IdTipoElemento,
                Variante = "Cable HDMI"
            });

        fixture.RepoEstadosMantenimiento.Setup(r => r.GetById(existente.IdEstadoMantenimiento))
            .Returns(new EstadosMantenimiento
            {
                IdEstadoMantenimiento = 1,
                EstadoMantenimientoNombre = "Disponible"
            });

        fixture.RepoUbicacion.Setup(r => r.GetById(existente.IdUbicacion))
            .Returns(new Ubicacion
            {
                IdUbicacion = 1,
                NombreUbicacion = "Armario A"
            });

        fixture.RepoModelo.Setup(r => r.GetById(existente.IdModelo))
            .Returns(new Modelos
            {
                IdModelo = 1,
                IdTipoElemento = 1,
                NombreModelo = "HP 3014"
            });

        fixture.RepoTipoElemento.Setup(r => r.GetById(existente.IdTipoElemento))
            .Returns(new TipoElemento
            {
                IdTipoElemento = 1,
                ElementoTipo = "Cable"
            });

        fixture.Service.ActualizarElemento(existente, idUsuario);

        fixture.RepoElemento.Verify(r => r.Update(It.Is<Elemento>(e =>
            e.IdElemento == existente.IdElemento &&
            e.NumeroSerie == existente.NumeroSerie &&
            e.CodigoBarra == existente.CodigoBarra &&
            e.Patrimonio == existente.Patrimonio
        )), Times.Once);
        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }
    #endregion
}
