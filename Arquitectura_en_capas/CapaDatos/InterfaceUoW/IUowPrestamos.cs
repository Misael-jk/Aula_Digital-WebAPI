using CapaDatos.Interfaces;

namespace CapaDatos.InterfaceUoW;

public interface IUowPrestamos
{
    public IRepoPrestamos RepoPrestamos { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoElemento RepoElemento { get; }
    public IRepoDocentes RepoDocentes { get; }
    public IRepoPrestamoDetalle RepoPrestamoDetalle { get; }
    public IRepoUsuarios RepoUsuarios { get; }
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
