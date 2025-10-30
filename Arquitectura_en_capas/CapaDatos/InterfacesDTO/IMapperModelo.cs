using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperModelo
{
    public IEnumerable<ModeloDTO> GetAll();
    public IEnumerable<ModeloDTO> GetByTipo(int idTipo);
    public IEnumerable<ModeloDTO> GetByElementos();
}
