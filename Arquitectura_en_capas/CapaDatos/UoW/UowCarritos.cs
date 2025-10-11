using CapaDatos.Interfaces;
using CapaDatos.InterfaceUoW;
using CapaDatos.Repos;
using System.Data;

namespace CapaDatos.UoW;

public class UowCarritos : UowBase, IUowCarritos
{
    public IRepoCarritos RepoCarritos { get; }
    public IRepoHistorialCarrito RepoHistorialCarrito { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialNotebook RepoHistorialNotebooks { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoNotebooks RepoNotebooks { get; }

    public UowCarritos(IDbConnection conexion) : base(conexion)
    {
        RepoCarritos = new RepoCarritos(Conexion, Transaction);
        RepoHistorialCarrito = new RepoHistorialCarrito(Conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(Conexion, Transaction);
        RepoModelo = new RepoModelo(Conexion, Transaction);
        RepoUbicacion = new RepoUbicacion(Conexion, Transaction);
        RepoEstadosMantenimiento = new RepoEstadosMantenimiento(Conexion, Transaction);
        RepoNotebooks = new RepoNotebooks(Conexion, Transaction);
        RepoHistorialNotebooks = new RepoHistorialNotebook(Conexion, Transaction);
    }
}
