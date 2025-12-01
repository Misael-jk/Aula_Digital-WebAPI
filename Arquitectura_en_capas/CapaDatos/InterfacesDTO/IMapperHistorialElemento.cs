using CapaDTOs.AuditoriaDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperHistorialElemento
{
    public IEnumerable<HistoralElementoDTO> GetAllDTO();
}
