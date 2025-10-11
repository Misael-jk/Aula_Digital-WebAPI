using System.Data;

namespace CapaDatos;

public abstract class RepoBase
{
    protected readonly IDbConnection Conexion;
    protected readonly IDbTransaction? Transaction;

    protected RepoBase(IDbConnection conexion, IDbTransaction? transaction = null)
    {
        Conexion = conexion;
        Transaction = transaction;
    }
}

