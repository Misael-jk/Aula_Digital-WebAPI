using System.Data;

namespace CapaDatos;

public abstract class UowBase : IDisposable
{
    protected readonly IDbConnection Conexion;
    protected IDbTransaction? Transaction;
    private bool disposed;

    protected UowBase(IDbConnection conexion)
    {
        Conexion = conexion;

        if (Conexion.State != ConnectionState.Open)
            Conexion.Open();
    }

    public void BeginTransaction()
    {
        if (Transaction != null)
            throw new InvalidOperationException("Ya existe una transaccion activa.");

        Transaction = Conexion.BeginTransaction();
        CambiarTransacion(Transaction);
    }

    public void Commit()
    {
        if (Transaction == null)
            throw new InvalidOperationException("No hay una transaccion activa para confirmar.");

        Transaction.Commit();
        Transaction.Dispose();
        Transaction = null;
        CambiarTransacion(null);
    }

    public void Rollback()
    {
        if (Transaction == null)
            throw new InvalidOperationException("No hay una transaccion activa para revertir.");

        Transaction.Rollback();
        Transaction.Dispose();
        Transaction = null;
        CambiarTransacion(null);
    }

    protected abstract void CambiarTransacion(IDbTransaction? transaction);

    public void Dispose()
    {
        if (disposed) return;

        try
        {
            Transaction?.Dispose();
        }
        finally
        {
            Transaction = null;
            disposed = true;
        }
    }
}