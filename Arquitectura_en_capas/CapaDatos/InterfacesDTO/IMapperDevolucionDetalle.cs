using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperDevolucionDetalle
{
    public IEnumerable<DevolucionDetalleDTO> GetByIdDTO(int idDevolucion, int? idCarrito);
}
