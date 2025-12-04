using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperInventario
{
    public IEnumerable<InventarioDTO> GetAllDTO();
}
