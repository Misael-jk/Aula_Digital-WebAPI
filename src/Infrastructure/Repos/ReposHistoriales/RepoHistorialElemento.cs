using Dapper;
using CapaDatos.Interfaces;
using System.Data;
using Core.Entities.Aggregates.Elementos;

namespace CapaDatos.Repos;

public class RepoHistorialElemento : RepoBase, IRepoHistorialElementos
{
    public RepoHistorialElemento(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public void Insert(HistorialElementos historialElementos)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidHistorialCambio", historialElementos.IdHistorialCambio);
        parameters.Add("unidElemento", historialElementos.IdElementos);

        try
        {
            Conexion.Execute("InsertHistorialElemento", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un historial de elemento");
        }
    }

    public IEnumerable<HistorialElementos> GetAll(HistorialElementos historialElementos)
    {
        string query = "select * from HistorialElementos";
        return Conexion.Query<HistorialElementos>(query);
    }
}
