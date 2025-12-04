using CapaDatos.Interfaces;

namespace CapaDatos.InterfaceUoW;

public interface IUowNotebooks
{
    public IRepoNotebooks RepoNotebooks { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialNotebook RepoHistorialNotebook { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoVarianteElemento RepoVarianteElemento { get; }
    public IRepoTipoElemento RepoTipoElemento { get; }
    public IRepoElemento RepoElemento { get; }
    public IRepoUsuarios RepoUsuarios { get; }
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
