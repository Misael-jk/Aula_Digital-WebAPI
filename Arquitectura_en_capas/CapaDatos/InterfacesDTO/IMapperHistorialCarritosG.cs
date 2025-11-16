using CapaDTOs.AuditoriaDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperHistorialCarritosG
{
    public IEnumerable<HistorialCarritoGestionDTO> GetAllDTO(int idCarrito);
}
