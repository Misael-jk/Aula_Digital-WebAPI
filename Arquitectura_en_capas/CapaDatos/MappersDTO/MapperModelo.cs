using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;


namespace CapaDatos.MappersDTO;

public class MapperModelo : RepoBase, IMapperModelo
{
    public MapperModelo(IDbConnection conexion) : base(conexion)
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
}
