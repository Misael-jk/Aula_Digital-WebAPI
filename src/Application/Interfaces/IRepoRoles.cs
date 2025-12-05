using Core.Entities.Catalogos;

namespace CapaDatos.Interfaces;

public interface IRepoRoles
{
    public IEnumerable<Roles> GetAll();
    public Roles? GetById(int idRol);
}
