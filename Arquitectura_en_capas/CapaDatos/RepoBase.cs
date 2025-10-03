using System.Data;

namespace CapaDatos;

public abstract class RepoBase
{
    protected readonly IDbConnection Conexion;

    protected RepoBase(IDbConnection conexion)
    {
        Conexion = conexion;
    }
}

