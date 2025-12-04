using CapaDTOs.AuditoriaDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperHistorialCarrito
{
    public IEnumerable<HistorialCarritosDTO> GetAllDTO();
}
