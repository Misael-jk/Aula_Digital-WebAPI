using Dapper;
using CapaDatos.Interfaces;
using System.Data;
using Core.Entities.Catalogos;

namespace CapaDatos.Repos;

public class RepoEstadosMantenimiento : RepoBase, IRepoEstadosMantenimiento
{
    public RepoEstadosMantenimiento(IDbConnection conexion, IDbTransaction? transaction = null)
    : base(conexion, transaction)
    {
    }

    #region Mostrar todos los estados de los elementos
    public IEnumerable<EstadosMantenimiento> GetAll()
    {
        string query = "select idEstadoMantenimiento, estadoMantenimiento as 'EstadoMantenimientoNombre' from EstadosMantenimiento";

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
        string query = "select idEstadoMantenimiento, estadoMantenimiento as 'EstadoMantenimientoNombre' from EstadosMantenimiento where idEstadoMantenimiento = @unidEstadoMantenimiento";

        DynamicParameters parameters = new DynamicParameters();
        try
        {
            parameters.Add("unidEstadoMantenimiento", idEstadoMantenimiento);

            return Conexion.QueryFirstOrDefault<EstadosMantenimiento>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al mostrar ese estado del Elemento" + ex.Message);
        }
    }
    #endregion

    #region Mostrar todos los estados para actualizaciones
    public IEnumerable<EstadosMantenimiento> GetAllForUpdates()
    {
        string query = "select idEstadoMantenimiento, estadoMantenimiento as 'EstadoMantenimientoNombre' from EstadosMantenimiento where idEstadoMantenimiento not in (2, 6)";
        try
        {
            return Conexion.Query<EstadosMantenimiento>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los estados de los Elementos para actualizaciones");
        }
    }
    #endregion

    public EstadosMantenimiento? GetByNombreEstado(string estado)
    {
        string query = "select idEstadoMantenimiento, estadoMantenimiento as 'EstadoMantenimientoNombre' from EstadosMantenimiento where estadoMantenimiento = @unEstadoMantenimiento";
        DynamicParameters parameters = new DynamicParameters();
        try
        {
            parameters.Add("unEstadoMantenimiento", estado);
            return Conexion.QueryFirstOrDefault<EstadosMantenimiento>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al mostrar ese estado del Elemento" + ex.Message);
        }
    }
}
