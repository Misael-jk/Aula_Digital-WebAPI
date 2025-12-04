using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperDevoluciones
{
    public IEnumerable<DevolucionesDTO> GetAllDTO();
    public DevolucionesDTO? GetByIdDTO(int idPrestamo);
    public IEnumerable<DevolucionesDTO> GetByDocenteDTO(int idDocente);
}
