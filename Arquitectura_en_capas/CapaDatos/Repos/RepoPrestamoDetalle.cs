using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoPrestamoDetalle : RepoBase, IRepoPrestamoDetalle
{
    public RepoPrestamoDetalle(IDbConnection conexion, IDbTransaction? transaction = null)
   : base(conexion, transaction)
    {
    }

    #region Insertar Detalle del Prestamo
    public void Insert(PrestamoDetalle prestamoDetalle)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidPrestamo", prestamoDetalle.IdPrestamo);
        parametros.Add("unidElemento", prestamoDetalle.IdElemento);

        try
        {
            Conexion.Execute("InsertPrestamoDetalle", parametros , transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al dar de alta el detalle del prestamo");
        }
    }
    #endregion

    #region Actualizar los detalles
    public void Update(PrestamoDetalle prestamoDetalle)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidPrestamo", prestamoDetalle.IdPrestamo);
        parametros.Add("unidElemento", prestamoDetalle.IdElemento);

        try
        {
            Conexion.Execute("UpdatePrestamoDetalle", parametros , transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar el Detalle del prestamo");
        }
    }
    #endregion

    #region Eliminar detalles
    public void Delete(int idPrestamo)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidPrestamo", idPrestamo);

        try
        {
            Conexion.Execute("DeletePrestamo", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al eliminar los detalles del prestamo");
        }
    }
    #endregion

    #region ver los datos del detalle
    public IEnumerable<PrestamoDetalle> GetAll()
    {
        string query = "select * from PrestamoDetalle";

        try
        {
            return Conexion.Query<PrestamoDetalle>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los datos del detalle");
        }
    }
    #endregion

    #region Obtener Detalle por prestamos y notebooks
    public IEnumerable<PrestamoDetalle> GetByPrestamo(int idPrestamo)
    {
        string query = "select * from PrestamoDetalle where idPrestamo = @idPrestamo";

        DynamicParameters parametros = new DynamicParameters();
        

        try
        {
            parametros.Add("unidPrestamo", idPrestamo);
            return Conexion.Query<PrestamoDetalle>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los detalles del prestamo");
        }
    }
    #endregion

    #region Obtener Detalle por elemento
    public Elemento? GetByElemento(int idElemento)
    {
        string query = "select idElemento from Elementos where idElemento = @unidElemento";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unidElemento", idElemento);
            return Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el elemento");
        }
    }
    #endregion

    #region Verificar si el elemento pertenece al prestamo
    public bool PerteneceAlPrestamo(int idPrestamo, int idElemento)
    {
        string query = "select idElemento from PrestamoDetalle where idPrestamo = @idPrestamo and idElemento = @idElemento";
        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidPrestamo", idPrestamo);
            parametros.Add("unidElemento", idElemento);

            Elemento? result = Conexion.QueryFirstOrDefault<Elemento>(query, parametros, transaction: Transaction);

            return result != null;
        }
        catch (Exception)
        {
            throw new Exception("Error al verificar si el elemento pertenece al prestamo");
        }
    }
    #endregion

    #region Obtener cantidad de elementos por prestamo
    public int GetCountByPrestamo(int idPrestamo)
    {
        string query = "select COUNT(*) from PrestamoDetalle where idPrestamo = @idPrestamo";
        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unidPrestamo", idPrestamo);
            return Conexion.ExecuteScalar<int>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener la cantidad de elementos del prestamo");
        }
    }
    #endregion

    #region Obtener Elementos pendientes por prestamo
    public IEnumerable<Elemento> GetElementosPendientes(int idPrestamo)
    {
        string query = @"
            SELECT e.*
            FROM Elementos e
            INNER JOIN PrestamoDetalle pd ON e.idElemento = pd.idElemento
            WHERE pd.idPrestamo = @idPrestamo
            AND e.idElemento NOT IN (
                SELECT dd.idElemento
                FROM DevolucionDetalle dd
                INNER JOIN Devoluciones d ON dd.idDevolucion = d.idDevolucion
                WHERE d.idPrestamo = @idPrestamo
            )";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidPrestamo", idPrestamo);
            return Conexion.Query<Elemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los elementos pendientes del prestamo");
        }
    }
    #endregion

    public List<int> GetIdsElementosByIdPrestamo(int idPrestamo)
    {
        string query = @"SELECT idElemento
                         FROM PrestamoDetalle
                         WHERE idPrestamo = @idPrestamo;";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("idPrestamo", idPrestamo);
            return Conexion.Query<int>(query, parametros, transaction: Transaction).ToList();
        }
        catch (Exception)
        {
            throw new Exception("Error al contar los detalles del prestamo");
        }
    }
}
