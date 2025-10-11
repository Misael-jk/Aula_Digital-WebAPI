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
    public IRepoCarritos RepoCarritos { get; }

    public UowDevolucion(IDbConnection conexion) : base(conexion)
    {
        RepoDevolucion = new RepoDevolucion(conexion, Transaction);
        RepoPrestamos = new RepoPrestamos(conexion, Transaction);
        RepoUsuarios = new RepoUsuarios(conexion, Transaction);
        RepoElementos = new RepoElemento(conexion, Transaction);
        RepoDocentes = new RepoDocentes(conexion, Transaction);
        RepoEstadosPrestamo = new RepoEstadosPrestamo(conexion, Transaction);
        RepoDevolucionDetalle = new RepoDevolucionDetalle(conexion, Transaction);
        RepoCarritos = new RepoCarritos(conexion, Transaction);
    }
}
