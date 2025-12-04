using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoEstadosPrestamo
{
    public IEnumerable<EstadosPrestamo> GetAll();
    public EstadosPrestamo? GetById(int idEstadosPrestamo);
    public void SetTransaction(IDbTransaction? transaction);
}
