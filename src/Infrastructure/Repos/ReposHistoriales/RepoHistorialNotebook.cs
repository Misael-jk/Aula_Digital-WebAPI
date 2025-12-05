using Dapper;
using CapaDatos.Interfaces;
using System.Data;
using Core.Entities.Aggregates.Notebooks;

namespace CapaDatos.Repos;

public class RepoHistorialNotebook : RepoBase, IRepoHistorialNotebook
{
    public RepoHistorialNotebook(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public void Insert(HistorialNotebooks historialNotebooks)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidHistorialCambio", historialNotebooks.IdHistorialCambio);
        parameters.Add("unidElemento", historialNotebooks.IdNotebook);
        try
        {
            Conexion.Execute("InsertHistorialNotebook", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al insertar un historial de notebook" + ex.Message);
        }
    }

    public IEnumerable<HistorialNotebooks> GetAll(HistorialNotebooks historialNotebooks)
    {
        string query = "select * from HistorialNotebooks";
        return Conexion.Query<HistorialNotebooks>(query);
    }
}
