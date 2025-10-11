using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperElementosBajas : RepoBase, IMapperElementosBajas
{
    public MapperElementosBajas(IDbConnection conexion, IDbTransaction? transaction = null) 
        : base(conexion, transaction)
    {
    }

    public IEnumerable<ElementoBajasDTO> GetAllDTO()
    {
        return Conexion.Query<Elemento, TipoElemento, Modelos, Ubicacion, EstadosMantenimiento, ElementoBajasDTO>(
            "GetElementosMantenimientoDTO",
            (elemento, tipo, modelos, ubicacion, estado) => new ElementoBajasDTO
            {
                IdElemento = elemento.IdElemento,
                TipoElemento = tipo.ElementoTipo,
                NumeroSerie = elemento.NumeroSerie,
                CodigoBarra = elemento.CodigoBarra,
                Patrimonio = elemento.Patrimonio,
                Estado = estado.EstadoMantenimientoNombre,
                Modelo = modelos.NombreModelo,
                Ubicacion = ubicacion.NombreUbicacion,
                Disponible = elemento.Habilitado,
                FechaBaja = elemento.FechaBaja
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "numeroSerie,ElementoTipo,NombreModelo,NombreUbicacion,EstadoMantenimientoNombre"
        ).ToList();
    }

    //public IEnumerable<ElementoBajasDTO> GetByTipo(int idTipoElemento)
    //{
    //    DynamicParameters parameters = new DynamicParameters();

    //    parameters.Add("unidTipoElemento", idTipoElemento);

    //    return Conexion.Query<Elemento, TipoElemento, EstadosMantenimiento, ElementoBajasDTO>(
    //        "GetElementosMantenimientoByTipoDTO",
    //        (elemento, tipo, estado) => new ElementoBajasDTO
    //        {
    //            IdElemento = elemento.IdElemento,
    //            TipoElemento = tipo.ElementoTipo,
    //            NumeroSerie = elemento.numeroSerie,
    //            Estado = estado.EstadoMantenimientoNombre,
    //            Disponible = elemento.Habilitado,
    //            FechaBaja = elemento.FechaBaja
    //        },
    //        parameters,
    //        commandType: CommandType.StoredProcedure,
    //        splitOn: "numeroSerie,ElementoTipo,EstadoMantenimientoNombre"
    //    ).ToList();
    //}

    //public IEnumerable<ElementoBajasDTO> GetByEstado(int idEstadoElemento)
    //{
    //    DynamicParameters parameters = new DynamicParameters();

    //    parameters.Add("unidEstadoElemento", idEstadoElemento);

    //    return Conexion.Query<Elemento, TipoElemento, EstadosMantenimiento, ElementoBajasDTO>(
    //        "GetElementosMantenimientoByEstadoDTO",
    //        (elemento, tipo, estado) => new ElementoBajasDTO
    //        {
    //            IdElemento = elemento.IdElemento,
    //            TipoElemento = tipo.ElementoTipo,
    //            NumeroSerie = elemento.numeroSerie,
    //            Estado = estado.EstadoMantenimientoNombre,
    //            Disponible = elemento.Habilitado,
    //            FechaBaja = elemento.FechaBaja
    //        },
    //        parameters,
    //        commandType: CommandType.StoredProcedure,
    //        splitOn: "numeroSerie,ElementoTipo,EstadoMantenimientoNombre"
    //    ).ToList();
    //}
}
