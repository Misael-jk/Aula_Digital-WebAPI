using CapaDatos.Interfaces;

namespace CapaDatos.InterfaceUoW;

public interface IUowElementos
{
    public IRepoElemento RepoElemento { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialElementos RepoHistorialElementos { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoVarianteElemento RepoVarianteElemento { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoTipoElemento RepoTipoElemento { get; }
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
