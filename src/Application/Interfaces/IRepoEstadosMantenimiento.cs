using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoEstadosMantenimiento
{
    public IEnumerable<EstadosMantenimiento> GetAll();
    public EstadosMantenimiento? GetById(int idEstadoMantenimiento);
    public IEnumerable<EstadosMantenimiento> GetAllForUpdates();
    public EstadosMantenimiento? GetByNombreEstado(string estado);
    public void SetTransaction(IDbTransaction? transaction);
}
