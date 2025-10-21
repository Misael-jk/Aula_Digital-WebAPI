using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoEstadosMantenimiento
{
    public IEnumerable<EstadosMantenimiento> GetAll();
    public EstadosMantenimiento? GetById(int idEstadoMantenimiento);
    public void SetTransaction(IDbTransaction? transaction);
}
