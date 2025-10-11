using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoTipoAccion : RepoBase, IRepoTipoAccion
{
    public RepoTipoAccion(IDbConnection dbConnection, IDbTransaction? transaction = null) : base(dbConnection, transaction)
    {
    }

    public IEnumerable<TipoAccion> GetAll()
    {
        string sql = "select * from TipoAccion";

        try
        {
            return Conexion.Query<TipoAccion>(sql);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener los tipos de accion");
        }
    }
    public TipoAccion? GetById(int idTipoAccion)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidTipoAccion", idTipoAccion);

        string sql = "select * from TipoAccion where idTipoAccion = @unidTipoAccion";

        try
        {
            return Conexion.QueryFirstOrDefault<TipoAccion>(sql, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener el tipo de accion por id");
        }
    }
}
