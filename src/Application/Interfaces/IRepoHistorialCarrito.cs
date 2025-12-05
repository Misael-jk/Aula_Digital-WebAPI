using Core.Entities.Aggregates.Carritos;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoHistorialCarrito
{
    public void Insert(HistorialCarritos historialCarritos);
    public IEnumerable<HistorialCarritos> GetAll(HistorialCarritos historialCarritos);
    public void SetTransaction(IDbTransaction? transaction);
}
