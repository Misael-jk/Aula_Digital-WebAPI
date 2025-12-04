using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoHistorialElementos
{
    public void Insert(HistorialElementos historialElementos);
    public IEnumerable<HistorialElementos> GetAll(HistorialElementos historialElementos);
    public void SetTransaction(IDbTransaction? transaction);
}
