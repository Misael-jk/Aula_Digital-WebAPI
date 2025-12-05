using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;
using Core.Entities.Catalogos;

namespace CapaDatos.MappersDTO;

public class MapperElementosBajas : RepoBase, IMapperElementosBajas
{
    public MapperElementosBajas(IDbConnection conexion, IDbTransaction? transaction = null) 
        : base(conexion, transaction)
    {
    }

    public IEnumerable<ElementoBajasDTO> GetAllDTO()
    {
        return Conexion.Query<Elemento, VariantesElemento, TipoElemento, Modelos, Ubicacion, EstadosMantenimiento, ElementoBajasDTO>(
            "select * from View_GetElementosBajasDTO",
            (elemento, variante, tipo, modelos, ubicacion, estado) => new ElementoBajasDTO
            {
                IdElemento = elemento.IdElemento,
                NumeroSerie = elemento.NumeroSerie,
                CodigoBarra = elemento.CodigoBarra,
                Patrimonio = elemento.Patrimonio,
                FechaBaja = elemento.FechaBaja,
                Equipo = variante.Variante,
                TipoElemento = tipo.ElementoTipo,
                Modelo = modelos.NombreModelo,
                Ubicacion = ubicacion.NombreUbicacion,
                Estado = estado.EstadoMantenimientoNombre
            },
            splitOn: "IdElemento,Variante,ElementoTipo,NombreModelo,NombreUbicacion,EstadoMantenimientoNombre"
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
