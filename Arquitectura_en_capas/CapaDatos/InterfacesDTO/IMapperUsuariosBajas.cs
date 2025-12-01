using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperUsuariosBajas
{
    public IEnumerable<UsuariosBajasDTO> GetAllDTO();
}
