using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperPrestamosActivos : RepoBase, IMapperPrestamosActivos
{
    public MapperPrestamosActivos(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<PrestamosActivosDTO> GetAllDTO()
    {
        return Conexion.Query<PrestamosActivosDTO>(
            "select * from View_GetPrestamosActivosDTO").ToList();
    }
}
