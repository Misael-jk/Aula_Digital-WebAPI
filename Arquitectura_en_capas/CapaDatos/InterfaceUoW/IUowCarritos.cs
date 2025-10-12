using CapaDatos.Interfaces;
using System.Data;

namespace CapaDatos.InterfaceUoW;

public interface IUowCarritos : IDisposable
{
    public IRepoCarritos RepoCarritos { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialCarrito RepoHistorialCarrito { get; }
    public IRepoHistorialNotebook RepoHistorialNotebooks { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoNotebooks RepoNotebooks { get;  }
    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
