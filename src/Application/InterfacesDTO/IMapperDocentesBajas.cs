using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperDocentesBajas
{
    public IEnumerable<DocentesBajasDTO> GetAllDTO();
}
