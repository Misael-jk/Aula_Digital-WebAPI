using CapaEntidad;

namespace CapaDatos.Interfaces;

public interface IRepoTipoAccion
{
    public IEnumerable<TipoAccion> GetAll();
    public TipoAccion? GetById(int idTipoAccion);
}
