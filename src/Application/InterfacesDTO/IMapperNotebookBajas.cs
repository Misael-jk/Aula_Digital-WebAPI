using CapaDTOs.MantenimientoDTO;

namespace CapaDatos.InterfacesDTO;

public interface IMapperNotebookBajas
{
    public IEnumerable<NotebooksBajasDTO> GetAll();
}
