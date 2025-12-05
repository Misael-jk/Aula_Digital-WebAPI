using Core.Entities.Aggregates.Notebooks;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoHistorialNotebook
{
    public void Insert(HistorialNotebooks historialNotebooks);
    public IEnumerable<HistorialNotebooks> GetAll(HistorialNotebooks historialNotebooks);
    public void SetTransaction(IDbTransaction? transaction);
}
