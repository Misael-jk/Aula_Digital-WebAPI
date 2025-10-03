using CapaDTOs;

namespace CapaDatos.InterfacesDTO
{
    public interface IMapperDocentes
    {
        public IEnumerable<DocentesDTO> GetAllDTO();
    }
}
