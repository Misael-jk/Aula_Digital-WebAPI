using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperPrestamos
{
    IEnumerable<PrestamosDTO> GetAllDTO();
    PrestamosDTO? GetByIdDTO(int idPrestamo);
    IEnumerable<PrestamosDTO> GetByDocenteDTO(int idDocente);
    IEnumerable<PrestamosDTO> GetByPaginas(int limit, int offset);
}
