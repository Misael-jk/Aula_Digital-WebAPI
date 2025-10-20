using CapaDatos.Interfaces;

namespace CapaDatos.InterfaceUoW;

public interface IUowDevolucion
{
    public IRepoDevolucion RepoDevolucion { get; }
    public IRepoPrestamos RepoPrestamos { get; }
    public IRepoUsuarios RepoUsuarios {get; }
    public IRepoElemento RepoElementos { get; }
    public IRepoDocentes RepoDocentes { get; }
    public IRepoEstadosPrestamo RepoEstadosPrestamo { get; }
    public IRepoDevolucionDetalle RepoDevolucionDetalle { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialElementos RepoHistorialElementos { get; }
    public IRepoHistorialCarrito RepoHistorialCarrito { get; }
    public IRepoHistorialNotebook RepoHistorialNotebook { get; }
    public IRepoPrestamoDetalle RepoPrestamoDetalle { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    //public IRepoDevolucionAnomalias RepoDevolucionAnomalias { get; }
    //public IRepoTipoAnomalias RepoTipoElemento { get; }
    public IRepoTipoElemento RepoTipoElemento { get; }

    public void BeginTransaction();
    public void Commit();
    public void Rollback();
}
