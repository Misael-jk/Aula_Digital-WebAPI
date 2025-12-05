using Dapper;
using CapaDatos.Interfaces;
using System.Data;
using Core.Entities.Catalogos;

namespace CapaDatos.Repos;

public class RepoEstadosPrestamo : RepoBase, IRepoEstadosPrestamo
{
    public RepoEstadosPrestamo(IDbConnection conexion, IDbTransaction? transaction = null)
    : base(conexion, transaction)
    {
    }

    #region Obtener todo los estados de los Prestamos
    public IEnumerable<EstadosPrestamo> GetAll()
    {
        string query = "select * from EstadosPrestamo";

        try
        {
            return Conexion.Query<EstadosPrestamo>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los datos de los estados del prestamo");
        }
    }
    #endregion

    #region Obtener por Id los estados de los prestamos
    public EstadosPrestamo? GetById(int idEstadoPrestamo)
    {
        string query = "select * from EstadosPrestamo where idEstadoPrestamo = @unidEstadoPrestamo";

        DynamicParameters parameters = new DynamicParameters();
        try
        {
            parameters.Add("unidEstadoPrestamo", idEstadoPrestamo);

            return Conexion.QueryFirstOrDefault<EstadosPrestamo>(query, parameters, transaction: Transaction);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al mostrar ese estado del Prestamo" + ex.Message);
        }
    }
    #endregion
}
