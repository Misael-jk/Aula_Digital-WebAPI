using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperVarianteElemento
{
    public IEnumerable<VarianteElementoDTO> GetAllDTO();
}
