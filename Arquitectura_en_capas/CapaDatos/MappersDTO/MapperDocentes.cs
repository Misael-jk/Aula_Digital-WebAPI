using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using System.Data;
using Dapper;

namespace CapaDatos.MappersDTO;

public class MapperDocentes : RepoBase, IMapperDocentes
{
    public MapperDocentes(IDbConnection conexion)
    : base(conexion)
    {
    }

    public IEnumerable<DocentesDTO> GetAllDTO()
    {
        string query = "select * from View_GetDocenteDTO";

        try
        {
            return Conexion.Query<DocentesDTO>(query);
        }
        catch
        {
            throw new Exception("Hubo un error al obtener los docentes");
        }
    }
}
