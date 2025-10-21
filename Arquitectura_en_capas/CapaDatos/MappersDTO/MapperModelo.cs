using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;


namespace CapaDatos.MappersDTO;

public class MapperModelo : RepoBase, IMapperModelo
{
    public MapperModelo(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<ModeloDTO> GetAll()
    {
        return Conexion.Query<Modelos, TipoElemento, ModeloDTO>(
            "select * from View_GetModeloDTO",
            (modelo, tipoElemento) => new ModeloDTO
            {
                IdModelo = modelo.IdModelo,
                Modelo = modelo.NombreModelo,
                Tipo = tipoElemento.ElementoTipo
            },
            splitOn: "IdModelo,ElementoTipo");
    }

    public IEnumerable<ModeloDTO> GetByTipo(int idTipoElemento)
    {
        string query = @"select 
                       m.idModelo,
                       m.modelo as 'NombreModelo',
                       t.elemento as 'ElementoTipo'
                       from modelo m
                       join tipoelemento t using (idTipoElemento)
                       where t.idTipoElemento = @unidTipoElemento";

        var parametros = new DynamicParameters();
        parametros.Add("unidTipoElemento", idTipoElemento);

        return Conexion.Query<Modelos, TipoElemento, ModeloDTO>(
            query,
            (modelo, tipoElemento) => new ModeloDTO
            {
                IdModelo = modelo.IdModelo,
                Modelo = modelo.NombreModelo,
                Tipo = tipoElemento.ElementoTipo
            },
            parametros,
            splitOn: "IdModelo,ElementoTipo");
    }
}
