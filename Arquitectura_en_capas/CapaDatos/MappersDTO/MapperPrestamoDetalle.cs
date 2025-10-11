using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperPrestamoDetalle : RepoBase, IMapperPrestamoDetalle
{
    public MapperPrestamoDetalle(IDbConnection conexion, IDbTransaction? transaction = null) 
        : base(conexion, transaction)
    {
    }

    public IEnumerable<PrestamosDetalleDTO> GetAllDTO()
    {
        return Conexion.Query<Elemento, TipoElemento, Carritos, PrestamosDetalleDTO>(
            "GetPrestamoDetalleDTO",
            (elemento, tipo, carrito) => new PrestamosDetalleDTO
            {
                NumeroSerieElemento = elemento.NumeroSerie,
                TipoElemento = tipo.ElementoTipo,
                NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin Carrito"
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "numeroSerie,ElementoTipo, NumeroSerieCarrito"
        ).ToList();
    }

    //public PrestamosDetalleDTO? GetByIdDTO(int idPrestamo)
    //{
    //    DynamicParameters parameters = new DynamicParameters();
    //    parameters.Add("@idPrestamo", idPrestamo, dbType: DbType.Int32, ParameterDirection.Input);

    //    return Conexion.Query<Elemento, TipoElemento, Carritos, PrestamosDetalleDTO>(
    //        "GetPrestamosDetalleById",
    //        (elemento, tipo, carrito) => new PrestamosDetalleDTO
    //        {
    //            NumeroSerieElemento = elemento.numeroSerie,
    //            TipoElemento = tipo.ElementoTipo,
    //            NumeroSerieCarrito = carrito?.NumeroSerieCarrito
    //        },
    //        parameters,
    //        splitOn: "numeroSerie, ElementoTipo, NumeroSerieCarrito"
    //    ).FirstOrDefault();
    //}

    public IEnumerable<PrestamosDetalleDTO> GetByPrestamoDTO(int idPrestamo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@idPrestamo", idPrestamo, dbType: DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Elemento, TipoElemento, Carritos, PrestamosDetalleDTO>(
            "GetPrestamosDetalleByPrestamos",
            (elemento, tipo, carrito) => new PrestamosDetalleDTO
            {
                NumeroSerieElemento = elemento.NumeroSerie,
                TipoElemento = tipo.ElementoTipo,
                NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin Carrito"
            },
            parameters,
            splitOn: "numeroSerie, ElementoTipo, NumeroSerieCarrito"
        ).ToList();
    }
}
