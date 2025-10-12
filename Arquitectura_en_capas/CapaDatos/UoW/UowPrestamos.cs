using CapaDatos.Interfaces;
using CapaDatos.InterfaceUoW;
using System.Data;
using CapaDatos.Repos;

namespace CapaDatos.UoW;

public class UowPrestamos : UowBase, IUowPrestamos
{
    public IRepoPrestamos RepoPrestamos { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoElemento RepoElemento { get; }
    public IRepoDocentes RepoDocentes { get; }
    public IRepoPrestamoDetalle RepoPrestamoDetalle { get; }
    public IRepoUsuarios RepoUsuarios { get; }

    public UowPrestamos(IDbConnection conexion) : base(conexion)
    {
        RepoPrestamos = new RepoPrestamos(conexion, Transaction);
        RepoCarritos = new RepoCarritos(conexion, Transaction);
        RepoElemento = new RepoElemento(conexion, Transaction);
        RepoPrestamoDetalle = new RepoPrestamoDetalle(conexion, Transaction);


        // REPO DE LECTURA
        RepoDocentes = new RepoDocentes(conexion);
        RepoUsuarios = new RepoUsuarios(conexion);
    }

    protected override void CambiarTransacion(IDbTransaction? transaction)
    {
        RepoPrestamos.SetTransaction(transaction);
        RepoCarritos.SetTransaction(transaction);
        RepoElemento.SetTransaction(transaction);
        RepoPrestamoDetalle.SetTransaction(transaction);
    }
}
