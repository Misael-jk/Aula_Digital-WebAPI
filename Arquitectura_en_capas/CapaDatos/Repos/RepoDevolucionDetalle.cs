using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.Repos;

public class RepoDevolucionDetalle : RepoBase, IRepoDevolucionDetalle
{
    public RepoDevolucionDetalle(IDbConnection conexion, IDbTransaction? transaction = null)
        : base(conexion, transaction)
    {
    }

    #region Insertar Detalle de la devolucion
    public void Insert(DevolucionDetalle devolucionDetalle)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDevolucion", devolucionDetalle.IdDevolucion);
        parametros.Add("unidElemento", devolucionDetalle.IdElemento);
        parametros.Add("unaObservacion", devolucionDetalle.Observaciones);

        try
        {
            Conexion.Execute("InsertDevolucionDetalle", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al dar de alta el detalle de la devolucion");
        }
    }
    #endregion

    //#region Actualizar los detalles
    //public void Update(DevolucionDetalle devolucionDetalle)
    //{
    //    DynamicParameters parametros = new DynamicParameters();

    //    parametros.Add("unidDevolucion", devolucionDetalle.IdDevolucion);
    //    parametros.Add("unidElemento", devolucionDetalle.IdElemento);
    //    parametros.Add("unidEstadoMantenimiento", devolucionDetalle.IdEstadoMantenimiento);
    //    parametros.Add("unaObservacion", devolucionDetalle.Observaciones);

    //    try
    //    {
    //        Conexion.Execute("UpdateDevolucionDetalle", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
    //    }
    //    catch (Exception)
    //    {
    //        throw new Exception("Hubo un error al actualizar el Detalle de la devolucion");
    //    }
    //}
    //#endregion

    #region ver los datos del detalle
    public IEnumerable<DevolucionDetalle> GetAll()
    {
        string query = "select * from DevolucionDetalle";

        try
        {
            return Conexion.Query<DevolucionDetalle>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los datos del detalle de la devolucion");
        }
    }
    #endregion

    #region Obtener Detalle por prestamos y notebooks
    public IEnumerable<DevolucionDetalle> GetByDevolucion(int idDevolucion)
    {
        string query = "select * from DevolucionDetalle where idDevolucion = @idDevolucion";

        DynamicParameters parametros = new DynamicParameters();


        try
        {
            parametros.Add("unidDevolucion", idDevolucion);
            return Conexion.Query<DevolucionDetalle>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los detalles de la devolucion");
        }
    }
    #endregion

    #region Verificar existencia del detalle
    public bool Exists(int idDevolucion, int idElemento)
    {
        string query = "select count(1) from DevolucionDetalle where idDevolucion = @idDevolucion and idElemento = @idElemento";
        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidDevolucion", idDevolucion);
            parametros.Add("unidElemento", idElemento);

            return Conexion.ExecuteScalar<bool>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al verificar la existencia del detalle de la devolucion");
        }
    }
    #endregion

    #region Obtener cantidad de detalles por devolucion
    public int CountByDevolucion(int idDevolucion)
    {
        string query = "select count(*) from DevolucionDetalle where idDevolucion = @idDevolucion";
        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidDevolucion", idDevolucion);
            return Conexion.ExecuteScalar<int>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al contar los detalles de la devolucion");
        }
    }
    #endregion
}
