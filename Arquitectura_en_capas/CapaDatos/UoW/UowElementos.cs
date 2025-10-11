using CapaDatos.Interfaces;
using CapaDatos.InterfaceUoW;
using CapaDatos.Repos;
using System.Data;

namespace CapaDatos.UoW;

public class UowElementos : UowBase, IUowElementos
{
    public IRepoElemento RepoElemento { get; }
    public IRepoHistorialCambio RepoHistorialCambio { get; }
    public IRepoHistorialElementos RepoHistorialElementos { get; }
    public IRepoUbicacion RepoUbicacion { get; }
    public IRepoModelo RepoModelo { get; }
    public IRepoVarianteElemento RepoVarianteElemento { get; }
    public IRepoEstadosMantenimiento RepoEstadosMantenimiento { get; }
    public IRepoTipoElemento RepoTipoElemento { get; }


    public UowElementos(IDbConnection conexion) : base(conexion)
    {
        RepoElemento = new RepoElemento(conexion, Transaction);
        RepoHistorialCambio = new RepoHistorialCambio(conexion, Transaction);
        RepoHistorialElementos = new RepoHistorialElemento(conexion, Transaction);
        RepoUbicacion = new RepoUbicacion(conexion, Transaction);
        RepoModelo = new RepoModelo(conexion, Transaction);
        RepoVarianteElemento = new RepoVarianteElemento(conexion, Transaction);
        RepoEstadosMantenimiento = new RepoEstadosMantenimiento(conexion, Transaction);
        RepoTipoElemento = new RepoTipoElemento(conexion, Transaction);
    }
}
