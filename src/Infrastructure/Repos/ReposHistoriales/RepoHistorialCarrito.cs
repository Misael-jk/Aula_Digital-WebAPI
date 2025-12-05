using Dapper;
using CapaDatos.Interfaces;
using System.Data;
using Core.Entities.Aggregates.Carritos;

namespace CapaDatos.Repos;

public class RepoHistorialCarrito : RepoBase, IRepoHistorialCarrito
{
    public RepoHistorialCarrito(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public void Insert(HistorialCarritos historialCarritos)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidHistorialCambio", historialCarritos.IdHistorialCambio);
        parameters.Add("unidCarrito", historialCarritos.IdCarrito);
        try
        {
            Conexion.Execute("InsertHistorialCarrito", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un historial de carrito");
        }
    }

    public IEnumerable<HistorialCarritos> GetAll(HistorialCarritos historialCarritos)
    {
        string query = "select * from HistorialCarritos";
        return Conexion.Query<HistorialCarritos>(query);
    }
}
