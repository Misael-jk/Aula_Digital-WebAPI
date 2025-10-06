using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperHistorialNotebook : RepoBase, IMapperHistorialNotebook
{
    public MapperHistorialNotebook(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<HistorialNotebooksDTO> GetAllDTO()
    {
        return Conexion.Query<HistorialNotebooksDTO>(
            "select * from View_HistorialNotebookDTO"
        ).ToList();
    }
}
