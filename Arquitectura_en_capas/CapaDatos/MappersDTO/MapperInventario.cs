using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperInventario : RepoBase, IMapperInventario
{
    public MapperInventario(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<InventarioDTO> GetAllDTO()
    {
        return Conexion.Query<InventarioDTO>(
        "select * from View_InventarioDTO"
    ).ToList();
    }
}
