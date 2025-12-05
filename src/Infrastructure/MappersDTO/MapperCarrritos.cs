using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Catalogos;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperCarrritos : RepoBase, IMapperCarritos
{
    public MapperCarrritos(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<CarritosDTO> GetAllDTO()
    {
        return Conexion.Query<Carritos, EstadosMantenimiento, Ubicacion, Modelos, CarritosDTO>(
        "select * from View_GetCarritoDTO",
        (carrito, estadoMantenimiento, ubicacion, modelo) => new CarritosDTO
        {
            IdCarrito = carrito.IdCarrito,
            Equipo = carrito.EquipoCarrito,
            NumeroSerieCarrito = carrito.NumeroSerieCarrito,
            Capacidad = carrito.Capacidad,
            EstadoMantenimiento = estadoMantenimiento.EstadoMantenimientoNombre,
            UbicacionActual = ubicacion.NombreUbicacion,
            Modelo = modelo.NombreModelo,
        },
        splitOn: "IdCarrito,EstadoMantenimientoNombre,NombreUbicacion,NombreModelo");
    }

    public IEnumerable<CarritosDTO> GetAllByEstado(string estado)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unestado", estado);

        return Conexion.Query<Carritos, EstadosMantenimiento, Ubicacion, Modelos, CarritosDTO>(
"select * from View_GetCarritoDTO where e.estadoMantenimiento = @unestado",
(carrito, estadoMantenimiento, ubicacion, modelo) => new CarritosDTO
{
    IdCarrito = carrito.IdCarrito,
    Equipo = carrito.EquipoCarrito,
    NumeroSerieCarrito = carrito.NumeroSerieCarrito,
    Capacidad = carrito.Capacidad,
    EstadoMantenimiento = estadoMantenimiento.EstadoMantenimientoNombre,
    UbicacionActual = ubicacion.NombreUbicacion,
    Modelo = modelo.NombreModelo,
},
parameters,
splitOn: "IdCarrito,EstadoMantenimientoNombre,NombreUbicacion,NombreModelo");
    }

    
}
