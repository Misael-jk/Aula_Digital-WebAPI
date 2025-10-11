using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;
public class RepoRoles : RepoBase, IRepoRoles
{
    public RepoRoles(IDbConnection conexion, IDbTransaction? transaction = null)
        : base(conexion, transaction)
    {
    }

    public IEnumerable<Roles> GetAll()
    {
        string query = "select * from Rol";

        try
        {
            return Conexion.Query<Roles>(query);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los roles: " + ex.Message);
        }
    }

    public Roles? GetById(int idRol)
    {
        string query = "select * from Rol where IdRol = @unidRol";

        DynamicParameters parametros = new DynamicParameters();
        
        try
        {
            parametros.Add("@unidRol", idRol);
            return Conexion.QueryFirstOrDefault<Roles>(query, parametros);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener el rol por ID: " + ex.Message);
        }
    }
}
