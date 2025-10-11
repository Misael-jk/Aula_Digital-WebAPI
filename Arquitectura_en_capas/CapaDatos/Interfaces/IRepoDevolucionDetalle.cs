using CapaEntidad;

namespace CapaDatos.Interfaces;

public interface IRepoDevolucionDetalle
{
    public void Insert(DevolucionDetalle devolucionDetalle);
    public IEnumerable<DevolucionDetalle> GetByDevolucion(int idDevolucion);
    public IEnumerable<DevolucionDetalle> GetAll();
}
