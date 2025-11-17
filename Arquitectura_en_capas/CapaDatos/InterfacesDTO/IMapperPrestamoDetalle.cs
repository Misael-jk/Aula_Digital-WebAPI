using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperPrestamoDetalle
{
    public IEnumerable<PrestamosDetalleDTO> GetByIdDTO(int idPrestamo, int? idCarrito);
    public PrestamosDetalleDTO? GetElementoById(int idElemento, int? idCarrito);

}
