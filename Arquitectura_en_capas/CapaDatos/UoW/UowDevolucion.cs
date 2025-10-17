using CapaDatos.Interfaces;
using CapaDatos.InterfaceUoW;
using CapaDatos.Repos;
using System.Data;

namespace CapaDatos.UoW;

public class UowDevolucion : UowBase, IUowDevolucion
{
    public IRepoDevolucion RepoDevolucion { get; }
    public IRepoPrestamos RepoPrestamos { get; }
    public IRepoUsuarios RepoUsuarios { get; }
    public IRepoElemento RepoElementos { get; }
    public IRepoDocentes RepoDocentes { get; }
    public IRepoEstadosPrestamo RepoEstadosPrestamo { get; }
    public IRepoDevolucionDetalle RepoDevolucionDetalle { get; }
    public IRepoPrestamoDetalle RepoPrestamoDetalle { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoHistorialCambio RepoHistorialCambio {get;}
    public IRepoHistorialElementos RepoHistorialElementos { get; }
    public IRepoHistorialCarrito RepoHistorialCarrito { get; }
    public IRepoHistorialNotebook RepoHistorialNotebook { get; }

    public UowDevolucion(IDbConnection conexion) : base(conexion)
    {
        RepoDevolucion = new RepoDevolucion(conexion, Transaction);
        RepoPrestamos = new RepoPrestamos(conexion, Transaction);
        RepoDevolucionDetalle = new RepoDevolucionDetalle(conexion, Transaction);
        RepoElementos = new RepoElemento(conexion, Transaction);
        RepoCarritos = new RepoCarritos(conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(conexion, Transaction);
        RepoHistorialElementos = new RepoHistorialElemento(conexion, Transaction);
        RepoHistorialCarrito = new RepoHistorialCarrito(conexion, Transaction);
        RepoHistorialNotebook = new RepoHistorialNotebook(conexion, Transaction);


        // REPO AUXILIARES / DE LECTURA
        RepoUsuarios = new RepoUsuarios(conexion);
        RepoDocentes = new RepoDocentes(conexion);
        RepoEstadosPrestamo = new RepoEstadosPrestamo(conexion);
        RepoPrestamoDetalle = new RepoPrestamoDetalle(conexion);
    }

    protected override void CambiarTransacion(IDbTransaction? transaction)
    {
        RepoDevolucion.SetTransaction(transaction);
        RepoPrestamos.SetTransaction(transaction);
        RepoDevolucionDetalle.SetTransaction(transaction);
        RepoElementos.SetTransaction(transaction);
        RepoCarritos.SetTransaction(transaction);
    }
}
