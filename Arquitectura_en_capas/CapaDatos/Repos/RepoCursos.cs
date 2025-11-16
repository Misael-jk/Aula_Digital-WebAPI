using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoCursos : RepoBase, IRepoCursos
{
    public RepoCursos (IDbConnection conexion, IDbTransaction? transaction = null) 
    : base (conexion, transaction)
    {
    }

    #region Mostrar todos los cursos
    public IEnumerable<Curso> GetAll()
    {
        string query = "select idCurso AS IdCurso, curso AS NombreCurso from Cursos";

        try
        {
            return Conexion.Query<Curso>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los cursos");
        }
    }
    #endregion

    #region Mostrar por id los cursos
    public Curso? GetById(int idCurso)
    {
        string query = "select idCurso AS IdCurso, curso AS NombreCurso from Cursos where idCurso = @unidCurso";

        DynamicParameters parameters = new DynamicParameters();
        try
        {
            parameters.Add("unidCurso", idCurso);

            return Conexion.QueryFirstOrDefault<Curso>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al mostrar ese curso" + ex.Message);
        }
    }
    #endregion
}
