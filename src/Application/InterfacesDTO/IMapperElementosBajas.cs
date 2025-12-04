using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperElementosBajas
{
    public IEnumerable<ElementoBajasDTO> GetAllDTO();
    //public IEnumerable<ElementoBajasDTO> GetByTipo(int idTipoElemento);
    //public IEnumerable<ElementoBajasDTO> GetByEstado(int idEstadoMantenimiento);
}
