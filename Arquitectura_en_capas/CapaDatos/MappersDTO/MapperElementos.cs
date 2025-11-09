using CapaEntidad;
using CapaDTOs;
using CapaDatos.InterfacesDTO;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperElementos : RepoBase, IMapperElementos
{
    public MapperElementos(IDbConnection conexion, IDbTransaction? transaction = null)
    : base(conexion, transaction)
    {
    }

    #region consulta join con splitOn para mostrar en la UI
    public IEnumerable<ElementosDTO> GetAllDTO()
    {
        return Conexion.Query<Elemento, VariantesElemento, TipoElemento, EstadosMantenimiento, Modelos, Ubicacion, ElementosDTO>(
            "select * from View_GetElementosDTO",
            (elemento, variante, tipo, estado, modelo, ubicacion) => new ElementosDTO
            {
                IdElemento = elemento.IdElemento,
                NumeroSerie = elemento.NumeroSerie,
                CodigoBarra = elemento.CodigoBarra,
                Patrimonio = elemento.Patrimonio,
                Equipo = variante.Variante,
                TipoElemento = tipo.ElementoTipo,
                Estado = estado.EstadoMantenimientoNombre,
                Modelo = modelo.NombreModelo,
                Ubicacion = ubicacion.NombreUbicacion
            },
            splitOn: "IdElemento,Variante,ElementoTipo,EstadoMantenimientoNombre,NombreModelo,NombreUbicacion"
        ).ToList();
    }
    #endregion

    #region Obtener por Id Elemetos
    public IEnumerable<ElementosDTO> GetByEstado(int idEstado)
    {
        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("@unidEstadoMantenimiento", idEstado, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Elemento, VariantesElemento, TipoElemento, EstadosMantenimiento, Modelos, Ubicacion, ElementosDTO>(
            @"select 
        e.idElemento as 'IdElemento',
        e.numeroSerie as 'NumeroSerie',
        e.codigoBarra as 'CodigoBarra',
        e.patrimonio as 'Patrimonio',
        v.subtipo as 'Variante',
        t.elemento as 'ElementoTipo',
        ee.estadoMantenimiento as 'EstadoMantenimientoNombre',
        m.modelo as 'NombreModelo',
        u.ubicacion as 'NombreUbicacion'
    from Elementos e
    join modelo m using (idModelo)
    join varianteselemento v using (idVariante)
    join tipoElemento t on t.idTipoElemento = m.idTipoElemento
    join EstadosMantenimiento ee using(idEstadoMantenimiento)
    join Ubicacion u using (idUbicacion)
    where e.habilitado = 1
    and ee.idEstadoMantenimiento = @unidEstadoMantenimiento
    and t.elemento not in ('Notebook');",
            (elemento, variante, tipo, estado, modelo, ubicacion) => new ElementosDTO
            {
                IdElemento = elemento.IdElemento,
                NumeroSerie = elemento.NumeroSerie,
                CodigoBarra = elemento.CodigoBarra,
                Patrimonio = elemento.Patrimonio,
                Equipo = variante.Variante,
                TipoElemento = tipo.ElementoTipo,
                Estado = estado.EstadoMantenimientoNombre,
                Modelo = modelo.NombreModelo,
                Ubicacion = ubicacion.NombreUbicacion
            },
            parametros,
            splitOn: "IdElemento,Variante,ElementoTipo,EstadoMantenimientoNombre,NombreModelo,NombreUbicacion"
        ).ToList();

    }

    #endregion

    public IEnumerable<ElementosDTO> GetFiltrosDTO(string? text, int? tipo, int? modelo)
    {
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("untext", text);
        parameters.Add("unidTipo", tipo);
        parameters.Add("unidModelo", modelo);

        try
        {
            return Conexion.Query<ElementosDTO>("SP_GetElementosDTO", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al filtrar los elementos");
        }
    }

    //#region mostrar datos por carrito
    //public IEnumerable<ElementosDTO> GetByCarritoDTO(int idCarrito)
    //{
    //    DynamicParameters parametros = new DynamicParameters();
    //    parametros.Add("@idCarrito", idCarrito, DbType.Int32, ParameterDirection.Input);

    //    return Conexion.Query<Elemento, TipoElemento, EstadosMantenimiento, Carritos, ElementosDTO>(
    //        "GetElementoByCarritoDTO",
    //        (elemento, tipo, estado, carrito) => new ElementosDTO
    //        {
    //            IdElemento = elemento.IdElemento,
    //            NumeroSerie = elemento.numeroSerie,
    //            CodigoBarra = elemento.codigoBarra,
    //            TipoElemento = tipo.ElementoTipo,
    //            Estado = estado.EstadoMantenimientoNombre,
    //            Carrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
    //            PosicionCarrito = elemento?.PosicionCarrito
    //        },
    //        parametros,
    //        commandType: CommandType.StoredProcedure,
    //        splitOn: "ElementoTipo,EstadoMantenimientoNombre,NumeroSerieCarrito"
    //    ).ToList();

    //}
    //#endregion

    //#region Mostrar datos por el tipo de elemento
    //public IEnumerable<ElementosDTO> GetByTipoDTO(int idTipoElemento)
    //{
    //    DynamicParameters parametros = new DynamicParameters();
    //    parametros.Add("@idTipoElemento", idTipoElemento, DbType.Int32, ParameterDirection.Input);

    //    return Conexion.Query<Elemento, TipoElemento, EstadosMantenimiento, Carritos, ElementosDTO>(
    //        "GetElementosByTipoDTO",
    //        (elemento, tipo, estado, carrito) => new ElementosDTO
    //        {
    //            IdElemento = elemento.IdElemento,
    //            NumeroSerie = elemento.numeroSerie,
    //            CodigoBarra = elemento.codigoBarra,
    //            TipoElemento = tipo.ElementoTipo,
    //            Estado = estado.EstadoMantenimientoNombre,
    //            Carrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
    //            PosicionCarrito = elemento?.PosicionCarrito
    //        },
    //        parametros,
    //        commandType: CommandType.StoredProcedure,
    //        splitOn: "ElementoTipo,EstadoMantenimientoNombre,NumeroSerieCarrito"
    //    ).ToList();
    //}
    //#endregion

    //#region Mostrar datos por el codigo de barra
    //public ElementosDTO? GetByCodigoBarraDTO(string codigoBarra)
    //{
    //    DynamicParameters parametros = new DynamicParameters();
    //    parametros.Add("@codigoBarra", codigoBarra, DbType.String, ParameterDirection.Input);

    //    return Conexion.Query<Elemento, TipoElemento, EstadosMantenimiento, Carritos, ElementosDTO>(
    //        "GetElementosByCodigoBarraDTO",
    //        (elemento, tipo, estado, carrito) => new ElementosDTO
    //        {
    //            IdElemento = elemento.IdElemento,
    //            NumeroSerie = elemento.numeroSerie,
    //            CodigoBarra = elemento.codigoBarra,
    //            TipoElemento = tipo.ElementoTipo,
    //            Estado = estado.EstadoMantenimientoNombre,
    //            Carrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
    //            PosicionCarrito = elemento?.PosicionCarrito
    //        },
    //        parametros,
    //        commandType: CommandType.StoredProcedure,
    //        splitOn: "ElementoTipo,EstadoMantenimientoNombre,NumeroSerieCarrito"
    //    ).FirstOrDefault();
    //}
    //#endregion
}

