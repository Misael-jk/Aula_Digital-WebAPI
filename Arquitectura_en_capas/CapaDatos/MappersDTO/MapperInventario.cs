using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperInventario : RepoBase, IMapperInventario
{
    public MapperInventario(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<InventarioDTO> GetAllDTO()
    {
        return Conexion.Query<InventarioDTO>(
        "select * from View_Inventario"
    ).ToList();
    }
}
