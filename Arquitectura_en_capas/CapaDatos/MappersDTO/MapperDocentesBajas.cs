using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperDocentesBajas : RepoBase, IMapperDocentesBajas
{
    public MapperDocentesBajas(IDbConnection conexion, IDbTransaction? transaction = null)
        : base(conexion, transaction)
    {
    }

    public IEnumerable<DocentesBajasDTO> GetAllDTO()
    {
        string query = @"select d.IdDocente, d.Dni, d.Nombre, d.Apellido, d.Email, d.Habilitado, d.FechaBaja
                       from Docentes d 
                       where d.Habilitado = 0";
        try
        {
            return Conexion.Query<DocentesBajasDTO>(query);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los docentes dados de baja", ex);
        }
    }
}
