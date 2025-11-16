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
    public IRepoCursos RepoCursos { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialElementos RepoHistorialElementos { get; }
    public IRepoHistorialCarrito RepoHistorialCarrito { get; }
    public IRepoHistorialNotebook RepoHistorialNotebook { get; }
    public IRepoNotebooks RepoNotebooks { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoTipoElemento RepoTipoElemento { get; }
    public IRepoVarianteElemento RepoVarianteElemento { get; }

    public UowPrestamos(IDbConnection conexion) : base(conexion)
{
        // CON TRANSACCIÓN
        RepoPrestamos = new RepoPrestamos(conexion, Transaction);
        RepoCarritos = new RepoCarritos(conexion, Transaction);
        RepoElemento = new RepoElemento(conexion, Transaction);
        RepoPrestamoDetalle = new RepoPrestamoDetalle(conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(conexion, Transaction);
        RepoHistorialElementos = new RepoHistorialElemento(conexion, Transaction);
        RepoHistorialCarrito = new RepoHistorialCarrito(conexion, Transaction);
        RepoHistorialNotebook = new RepoHistorialNotebook(conexion, Transaction);
        RepoNotebooks = new RepoNotebooks(conexion, Transaction);

        // SOLO LECTURA
        RepoDocentes = new RepoDocentes(conexion, Transaction);
        RepoUsuarios = new RepoUsuarios(conexion, Transaction);
        RepoCursos = new RepoCursos(conexion, Transaction);
        RepoTipoElemento = new RepoTipoElemento(conexion, Transaction);
        RepoModelo = new RepoModelo(conexion, Transaction);
        RepoVarianteElemento = new RepoVarianteElemento(conexion, Transaction);
    }


    protected override void CambiarTransacion(IDbTransaction? transaction)
    {
        RepoPrestamos.SetTransaction(transaction);
        RepoCarritos.SetTransaction(transaction);
        RepoElemento.SetTransaction(transaction);
        RepoPrestamoDetalle.SetTransaction(transaction);
        RepoHistorialCambio.SetTransaction(transaction);
        RepoHistorialElementos.SetTransaction(transaction);
        RepoHistorialCarrito.SetTransaction(transaction);
        RepoHistorialNotebook.SetTransaction(transaction);
        RepoNotebooks.SetTransaction(transaction);
        RepoTipoElemento.SetTransaction(transaction);
        RepoModelo.SetTransaction(transaction);
        RepoVarianteElemento.SetTransaction(transaction);
        RepoDocentes.SetTransaction(transaction);
        RepoUsuarios.SetTransaction(transaction);
        RepoCursos.SetTransaction(transaction);
    }
}
