using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperUsuarios
{
    public IEnumerable<UsuariosDTO> GetAllDTO();
    public UsuariosDTO? GetById(int idUsuario);
}
