using Core.Entities.Aggregates.Prestamos.Devolucion;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoDevolucionDetalle
{
    public void Insert(DevolucionDetalle devolucionDetalle);
    public IEnumerable<DevolucionDetalle> GetByDevolucion(int idDevolucion);
    public IEnumerable<DevolucionDetalle> GetAll();
    public DevolucionDetalle? Exists(int idDevolucion, int idElemento);
    public int CountByDevolucion(int idDevolucion);
    public List<int> GetIdsElementosByIdDevolucion(int idDevolucion);
    public void SetTransaction(IDbTransaction? transaction);
}
