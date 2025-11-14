using CapaDTOs.AuditoriaDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperHistorialNotebookG
{
    public IEnumerable<HistorialNotebookGestionDTO> GetAllDTO(int idNotebook); 
}
