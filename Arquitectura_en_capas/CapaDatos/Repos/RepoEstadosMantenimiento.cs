using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoEstadosMantenimiento : RepoBase, IRepoEstadosMantenimiento
{
    public RepoEstadosMantenimiento(IDbConnection conexion)
    : base(conexion)
    {
    }

    #region Mostrar todos los estados de los elementos
    public IEnumerable<EstadosMantenimiento> GetAll()
    {
        string query = "select * from EstadosMantenimiento";

        try
        {
            return Conexion.Query<EstadosMantenimiento>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los estados de los Elementos");
        }
    }
    #endregion

    #region Mostrar por id estados
    public EstadosMantenimiento? GetById(int idEstadoMantenimiento)
    {
        string query = "select * from EstadosMantenimiento where idEstadoMantenimiento = @idEstadoMantenimiento";

        DynamicParameters parameters = new DynamicParameters();
        try
        {
            parameters.Add("unidEstadoMantenimiento", idEstadoMantenimiento);

            return Conexion.QueryFirstOrDefault<EstadosMantenimiento>(query, parameters);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al mostrar ese estado del Elemento" + ex.Message);
        }
    }
    #endregion

}
