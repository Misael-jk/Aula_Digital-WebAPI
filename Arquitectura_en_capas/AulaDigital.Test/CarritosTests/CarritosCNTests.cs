using System;
using Xunit;
using Moq;
using AulaDigital.Tests.Unit.CarritosTests;
using CapaEntidad;

namespace AulaDigital.Tests.Unit;

public class CarritosCNTests : IClassFixture<CarritosFixture>
{
    private readonly CarritosFixture fixture;

    public CarritosCNTests(CarritosFixture fixture)
    {
        this.fixture = fixture;
    }

    #region VALIDACION DE TRANSACCION AL INSERTAR
    [Fact]
    public void ValidarTransaccionInsert()
    {
        // Arrange
        Carritos? nuevo = fixture.CreateCarrito(0);
        int idUsuario = 5;

        fixture.MockRepoCarritos.Setup(r => r.GetById(nuevo.IdCarrito)).Returns((Carritos?)null);
        fixture.MockRepoCarritos.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerieCarrito)).Returns((Carritos?)null);
        fixture.MockRepoCarritos.Setup(r => r.GetByEquipo(nuevo.EquipoCarrito)).Returns((Carritos?)null);

        fixture.MockRepoUbicacion.Setup(r => r.GetById(nuevo.IdUbicacion))
            .Returns(new Ubicacion 
            { 
              IdUbicacion = 1, 
              NombreUbicacion = "Deposito" 
            });

        fixture.MockRepoEstadosMantenimiento.Setup(r => r.GetById(nuevo.IdEstadoMantenimiento))
            .Returns(new EstadosMantenimiento 
            { 
                IdEstadoMantenimiento = 1, 
                EstadoMantenimientoNombre = "Disponible" 
            });

        fixture.MockRepoModelo.Setup(r => r.GetById(nuevo.IdModelo ?? 0))
            .Returns(new Modelos
            {
                IdModelo = 1,
                IdTipoElemento = 1, 
                NombreModelo = "ENOVA"
            });

        fixture.MockRepoModelo.Setup(r => r.GetByTipo(nuevo.IdModelo ?? 0))
            .Returns(new List<Modelos>
            {
                new Modelos { IdModelo = 1, IdTipoElemento = 1, NombreModelo = "ENOVA" },
                new Modelos { IdModelo = 2, IdTipoElemento = 1, NombreModelo = "Carrito chico" }
            });

        // Act
        fixture.Service.CrearCarrito(nuevo, idUsuario);

        // Assert
        fixture.MockUow.Verify(u => u.BeginTransaction(), Times.Once);
        fixture.MockRepoCarritos.Verify(r => r.Insert(It.Is<Carritos>(c => 
            c.IdCarrito == nuevo.IdCarrito &&
            c.EquipoCarrito == nuevo.EquipoCarrito &&
            c.NumeroSerieCarrito == nuevo.NumeroSerieCarrito &&
            c.IdEstadoMantenimiento == nuevo.IdEstadoMantenimiento &&
            c.IdUbicacion == nuevo.IdUbicacion &&
            c.IdUbicacion == nuevo.IdUbicacion)), Times.Once);
        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }
    #endregion

    #region VALIDAR ID CARRITO
    [Fact]
    public void ValidarIdInsert()
    {
        Carritos? nuevo = fixture.CreateCarrito(1); 
        int idUsuario = 5;

        fixture.MockRepoCarritos.Setup(r => r.GetById(nuevo.IdCarrito)).Returns(nuevo);

        Assert.Throws<Exception>(() => fixture.Service.CrearCarrito(nuevo, idUsuario));


        fixture.MockRepoCarritos.Verify(r => r.Insert(It.IsAny<Carritos>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region VALIDAR NUMERO SERIE
    [Fact]
    public void ValidarNumeroSerieInsert()
    {
        // Arrange
        Carritos? nuevo = fixture.CreateCarrito(0);
        Carritos? existente = fixture.CreateCarrito(2);

        fixture.MockRepoCarritos.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerieCarrito)).Returns(existente);

        // Act 
        Assert.Throws<Exception>(() => fixture.Service.CrearCarrito(nuevo, 1));

        // Assert
        fixture.MockRepoCarritos.Verify(r => r.Insert(It.IsAny<Carritos>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region VALIDAR EQUIPO
    [Fact]
    public void ValidarEquipoInsert()
    {
        // Arrange
        Carritos? nuevo = fixture.CreateCarrito(0);
        Carritos? existente = fixture.CreateCarrito(2);
        fixture.MockRepoCarritos.Setup(r => r.GetByEquipo(nuevo.EquipoCarrito)).Returns(existente);

        // Act 
        Assert.Throws<Exception>(() => fixture.Service.CrearCarrito(nuevo, 1));

        // Assert
        fixture.MockRepoCarritos.Verify(r => r.Insert(It.IsAny<Carritos>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion

    #region VALIDAR MODELO
    [Fact]
    public void ValidarModeloInsert()
    {
        // Arrange
        Carritos nuevo = fixture.CreateCarrito(1);
        int idUsuario = 5;

        Modelos modelo = new Modelos { IdModelo = 1, IdTipoElemento = 2, NombreModelo = "HP 250" };

        List<Modelos> modelosDelTipoCarrito = new List<Modelos>
        {
            new Modelos { IdModelo = 2, IdTipoElemento = 1, NombreModelo = "ENOVA" },
            new Modelos { IdModelo = 3, IdTipoElemento = 1, NombreModelo = "Carrito chico" }
        };

        fixture.MockRepoModelo.Setup(r => r.GetById(1)).Returns(modelo);
        fixture.MockRepoModelo.Setup(r => r.GetByTipo(modelo.IdTipoElemento)).Returns(modelosDelTipoCarrito);

        fixture.MockRepoCarritos.Setup(r => r.GetById(nuevo.IdCarrito)).Returns((Carritos?)null);
        fixture.MockRepoUbicacion.Setup(r => r.GetById(nuevo.IdUbicacion)).Returns(new Ubicacion { IdUbicacion = 1, NombreUbicacion = "Deposito" });
        fixture.MockRepoEstadosMantenimiento.Setup(r => r.GetById(nuevo.IdEstadoMantenimiento)).Returns(new EstadosMantenimiento { IdEstadoMantenimiento = 1, EstadoMantenimientoNombre = "Disponible" });
        fixture.MockRepoCarritos.Setup(r => r.GetByNumeroSerie(nuevo.NumeroSerieCarrito)).Returns((Carritos?)null);
        fixture.MockRepoCarritos.Setup(r => r.GetByEquipo(nuevo.EquipoCarrito)).Returns((Carritos?)null);

        // Act 
        var ex = Assert.Throws<Exception>(() => fixture.Service.CrearCarrito(nuevo, idUsuario));
        Assert.Equal("El modelo seleccionado no corresponde a un carrito.", ex.Message);

        fixture.MockRepoCarritos.Verify(r => r.Insert(It.IsAny<Carritos>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }

    #endregion




    #region VALIDAR ID UPDATE
    [Fact]
    public void ValidarIdUpdate()
    {
        // Arrange
        Carritos carrito = fixture.CreateCarrito(10);

        fixture.MockRepoCarritos.Setup(r => r.GetById(carrito.IdCarrito)).Returns((Carritos?)null);

        var ex = Assert.Throws<Exception>(() => fixture.Service.ActualizarCarrito(carrito, 1));
        Assert.Equal("El carrito que intenta actualizar no existe.", ex.Message);

        fixture.MockRepoCarritos.Verify(r => r.Update(It.IsAny<Carritos>()), Times.Never);
        fixture.MockUow.Verify(u => u.Commit(), Times.Never);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Once);
    }
    #endregion


    [Fact]
    public void DeshabilitarCarrito_SiNoDisponible_LanzaExcepcionYRollback()
    {
        // Arrange
        var carrito = fixture.CreateCarrito(3);
        int idEstado = 4;
        int idUsuario = 7;

        fixture.MockRepoCarritos.Setup(r => r.GetById(carrito.IdCarrito)).Returns(carrito);
        fixture.MockRepoEstadosMantenimiento.Setup(r => r.GetById(idEstado)).Returns(new EstadosMantenimiento { IdEstadoMantenimiento = idEstado, EstadoMantenimientoNombre = "Fuera"});
        fixture.MockRepoUbicacion.Setup(r => r.GetById(carrito.IdUbicacion)).Returns(new Ubicacion { IdUbicacion = 1, NombreUbicacion = "Disponible" });
        fixture.MockRepoModelo.Setup(r => r.GetById(carrito.IdModelo ?? 0)).Returns(new Modelos { IdModelo = 1, NombreModelo = "Enova" });
        fixture.MockRepoCarritos.Setup(r => r.GetDisponible(carrito.IdCarrito)).Returns(false);

        // Act 
        fixture.Service.DeshabilitarCarrito(carrito.IdCarrito, idEstado, idUsuario);

        
        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
        fixture.MockUow.Verify(u => u.Rollback(), Times.Never);
    }

    [Fact]
    public void EliminarCarrito_SiNoExiste_LanzaExcepcion()
    {
        // Arrange
        int id = 99;
        fixture.MockRepoCarritos.Setup(r => r.GetById(id)).Returns((Carritos?)null);

        // Act & Assert
        Assert.Throws<Exception>(() => fixture.Service.EliminarCarrito(id));
        fixture.MockRepoCarritos.Verify(r => r.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void AddNotebook_TodoValido_ActualizaYCommit()
    {
        // Arrange
        var carrito = fixture.CreateCarrito(5);
        var notebook = fixture.CreateNotebook(12);
        int posicion = 1;
        int idUsuario = 2;

        fixture.MockRepoCarritos.Setup(r => r.GetById(carrito.IdCarrito)).Returns(carrito);
        fixture.MockRepoCarritos.Setup(r => r.GetDisponible(carrito.IdCarrito)).Returns(true);
        fixture.MockRepoNotebooks.Setup(r => r.GetById(notebook.IdElemento)).Returns(notebook);
        fixture.MockRepoNotebooks.Setup(r => r.GetDisponible(notebook.IdElemento)).Returns(true);
        fixture.MockRepoNotebooks.Setup(r => r.DuplicatePosition(carrito.IdCarrito, posicion)).Returns(false);
        fixture.MockRepoCarritos.Setup(r => r.GetCountByCarrito(carrito.IdCarrito)).Returns(0);

        // Act
        fixture.Service.AddNotebook(carrito.IdCarrito, posicion, notebook.IdElemento, idUsuario);

        // Assert
        fixture.MockRepoNotebooks.Verify(r => r.Update(It.Is<Notebooks>(n => n.IdCarrito == carrito.IdCarrito && n.PosicionCarrito == posicion)), Times.Once);
        fixture.MockUow.Verify(u => u.Commit(), Times.Once);
    }
}
