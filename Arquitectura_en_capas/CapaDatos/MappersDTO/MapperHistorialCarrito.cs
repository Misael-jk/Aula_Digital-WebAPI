using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperHistorialCarrito : RepoBase, IMapperHistorialCarrito
{
    public MapperHistorialCarrito(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<HistorialCarritosDTO> GetAllDTO()
    {
        return Conexion.Query<HistorialCarritosDTO>(
            "select * from View_HistorialCarritoDTO"
        ).ToList();
    }
}
