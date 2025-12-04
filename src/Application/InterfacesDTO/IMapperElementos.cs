using CapaDTOs;
namespace CapaDatos.InterfacesDTO;

public interface IMapperElementos
{
    public IEnumerable<ElementosDTO> GetAllDTO();
    public IEnumerable<ElementosDTO> GetByEstado(int idEstado);
    public IEnumerable<ElementosDTO> GetAllByEstado(string estado);
    public IEnumerable<ElementosDTO> GetFiltrosDTO(string? text, int? tipo, int? modelo);
    //ElementosDTO? GetByIdDTO(int idElemento);
    //IEnumerable<ElementosDTO> GetByCarritoDTO(int idCarrito);
    //IEnumerable<ElementosDTO> GetByTipoDTO(int idTipoElemento);
    //ElementosDTO? GetByCodigoBarraDTO(string codigoBarra);
}
