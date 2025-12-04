using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperPrestamosActivos
{
    public IEnumerable<PrestamosActivosDTO> GetAllDTO();
}
