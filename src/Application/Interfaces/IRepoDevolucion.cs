using Core.Entities.Aggregates.Prestamos;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoDevolucion
{
    public void Insert(Devolucion devolucion);
    public Devolucion? GetById(int idDevolucion);
    public IEnumerable<Devolucion> GetAll();
    public Devolucion? GetByPrestamo(int idPrestamo);
    public void SetTransaction(IDbTransaction? transaction);
}
