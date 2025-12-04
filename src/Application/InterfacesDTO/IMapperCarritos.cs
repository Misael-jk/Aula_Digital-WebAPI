using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperCarritos
{
    public IEnumerable<CarritosDTO> GetAllDTO();
    public IEnumerable<CarritosDTO> GetAllByEstado(string estado);
}
