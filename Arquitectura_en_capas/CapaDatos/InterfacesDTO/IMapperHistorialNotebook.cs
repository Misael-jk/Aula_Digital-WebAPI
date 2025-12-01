using CapaDTOs.AuditoriaDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperHistorialNotebook
{
    public IEnumerable<HistorialNotebooksDTO> GetAllDTO();
}
