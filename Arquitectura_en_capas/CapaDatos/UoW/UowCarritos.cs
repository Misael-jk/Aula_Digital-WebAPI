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
    public IRepoUsuarios RepoUsuarios { get; }

    public UowCarritos(IDbConnection conexion) : base(conexion)
    {
        RepoCarritos = new RepoCarritos(Conexion, Transaction);
        RepoHistorialCarrito = new RepoHistorialCarrito(Conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(Conexion, Transaction);
        RepoHistorialNotebooks = new RepoHistorialNotebook(Conexion, Transaction);
        RepoNotebooks = new RepoNotebooks(Conexion, Transaction);

        // REPOS AUXILIARES / LECTURA

        RepoModelo = new RepoModelo(Conexion, Transaction);
        RepoUbicacion = new RepoUbicacion(Conexion, Transaction);
        RepoEstadosMantenimiento = new RepoEstadosMantenimiento(Conexion, Transaction);
        RepoUsuarios = new RepoUsuarios(Conexion, Transaction);

    }

    protected override void CambiarTransacion(IDbTransaction? transaction)
    {
        RepoCarritos.SetTransaction(transaction);
        RepoHistorialCambio.SetTransaction(transaction);
        RepoHistorialCarrito.SetTransaction(transaction);
        RepoHistorialNotebooks.SetTransaction(transaction);
        RepoNotebooks.SetTransaction(transaction);

        RepoModelo.SetTransaction(transaction);
        RepoUbicacion.SetTransaction(transaction);
        RepoEstadosMantenimiento.SetTransaction(transaction);
        RepoUsuarios.SetTransaction(transaction);
    }
}
