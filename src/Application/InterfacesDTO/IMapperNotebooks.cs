using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperNotebooks
{
    public IEnumerable<NotebooksDTO> GetAllDTO();
    public IEnumerable<NotebooksDTO> GetAllByEstado(string estado);
    public IEnumerable<NotebooksDTO> GetNotebooksByCarritoDTO(string idCarrito);
    public IEnumerable<NotebooksDTO> GetByFiltros(string? text, int? idCarrito, string? equipo);
}
