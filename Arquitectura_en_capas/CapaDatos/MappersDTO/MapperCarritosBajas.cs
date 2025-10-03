using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperCarritosBajas : RepoBase, IMapperCarritosBajas
{
    public MapperCarritosBajas(IDbConnection conexion)
        : base(conexion)
    {
    }

    public IEnumerable<CarritosBajasDTO> GetAllDTO()
    {
        return Conexion.Query<Carritos, Ubicacion, Modelos, EstadosMantenimiento, CarritosBajasDTO>(
            "GetCarritosBajasDTO",
            (carrito, ubicacion, modelo, estado) => new CarritosBajasDTO
            {
                IdCarrito = carrito.IdCarrito,
                Equipo = carrito.Equipo,
                NumeroSerieCarrito = carrito.NumeroSerieCarrito,
                EstadoMantenimiento = estado.EstadoMantenimientoNombre,
                Ubicacion = ubicacion.NombreUbicacion,
                Modelo = modelo.NombreModelo,
                FechaBaja = carrito.FechaBaja
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "NumeroSerieCarrito,NombreUbicacion,NombreModelo,EstadoMantenimientoNombre"
        ).ToList();
    }
}
