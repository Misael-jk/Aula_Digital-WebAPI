using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperNotebooksPrestadas
{
    public IEnumerable<NotebooksPrestadasDTO> GetAllDTO();
}
