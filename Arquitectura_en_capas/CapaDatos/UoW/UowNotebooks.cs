using CapaDatos.Interfaces;
using CapaDatos.Repos;
using System.Data;
using CapaDatos.InterfaceUoW;

namespace CapaDatos.UoW;

public class UowNotebooks : UowBase, IUowNotebooks
{
    public IRepoNotebooks RepoNotebooks { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialNotebook RepoHistorialNotebook { get; }
    public IRepoCarritos RepoCarritos { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoVarianteElemento RepoVarianteElemento {get;}
    public IRepoTipoElemento RepoTipoElemento { get; }
    public IRepoElemento RepoElemento { get; }

    public UowNotebooks(IDbConnection conexion) : base(conexion)
    {
        RepoNotebooks = new RepoNotebooks(conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(conexion, Transaction);
        RepoHistorialNotebook = new RepoHistorialNotebook(conexion, Transaction);
        RepoCarritos = new RepoCarritos(conexion, Transaction);
        RepoElemento = new RepoElemento(conexion, Transaction);


        //REPO DE LECTURA

        RepoModelo = new RepoModelo(conexion);
        RepoUbicacion = new RepoUbicacion(conexion);
        RepoEstadosMantenimiento = new RepoEstadosMantenimiento(conexion);
        RepoVarianteElemento = new RepoVarianteElemento(conexion);
        RepoTipoElemento = new RepoTipoElemento(conexion);   
    }

    protected override void CambiarTransacion(IDbTransaction? transaction)
    {
        RepoNotebooks.SetTransaction(transaction);
        RepoHistorialCambio.SetTransaction(transaction);
        RepoHistorialNotebook.SetTransaction(transaction);
        RepoCarritos.SetTransaction(transaction);
        RepoElemento.SetTransaction(transaction);
    }
}
