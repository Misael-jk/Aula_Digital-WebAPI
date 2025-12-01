using System.Data;
using CapaDatos.Interfaces;
using CapaEntidad;
using Dapper;

namespace CapaDatos.Repos;

public class RepoHistorialCambio : RepoBase, IRepoHistorialCambio
{
    public RepoHistorialCambio(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public void Insert(HistorialCambios historialCambio)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidHistorialCambio", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unidTipoAccion", historialCambio.IdTipoAccion);
        parameters.Add("unidUsuario", historialCambio.IdUsuario);
        parameters.Add("unfechaCambio", historialCambio.FechaCambio);
        parameters.Add("unadescripcion", historialCambio.Descripcion);
        parameters.Add("unmotivo", historialCambio.Motivo);

        try
        {
            Conexion.Execute("InsertHistorialCambio", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);

            historialCambio.IdHistorialCambio = parameters.Get<int>("unidHistorialCambio");
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al insertar un historial de cambio: " + ex.Message, ex);
        }
    }

    public IEnumerable<HistorialCambios> GetAll()
    {
        string query = "select * from HistorialCambios";

        return Conexion.Query<HistorialCambios>(query);
    }

    public HistorialCambios? GetById(int idHistorialCambio)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidHistorialCambio", idHistorialCambio);

        string query = "select * from HistorialCambio where idHistorialCambio = @unidHistorialCambio";

        try
        {
            return Conexion.QueryFirstOrDefault<HistorialCambios>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener un historial de cambio por id");
        }
    }

    public IEnumerable<HistorialCambios> GetByAccion(int idTipoAccion)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidTipoAccion", idTipoAccion);

        string query = "select * from HistorialCambios where idTipoAccion = @unidTipoAccion";

        try
        {
            return Conexion.Query<HistorialCambios>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener los historiales de cambio por accion");
        }
    }

    #region Obtener fecha de ultima modificacion de una notebook
    public HistorialCambios? GetUltimateDateByIdNotebook(int idNotebook)
    {
        string query = @"select max(fechaCambio) as 'fechaCambio'
                        from historialcambio h 
                        join historialnotebook h2 on h.idHistorialCambio = h2.idHistorialCambio
                        where h2.idElemento = @unidNotebook;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidNotebook", idNotebook);

        try
        {
            return Conexion.QueryFirstOrDefault<HistorialCambios>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la ultima fecha de modificacion de notebook");
        }
    }
    #endregion

    #region Obtener fecha de ultima modificacion de un carrito
    public HistorialCambios? GetUltimateDateByIdCarrito(int idCarrito)
    {
        string query = @"select max(fechaCambio) as 'FechaCambio'
                        from historialcambio h
                        join historialcarrito h2 on h.idHistorialCambio = h2.idHistorialCambio
                        where h2.idCarrito = @unidCarrito;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidCarrito", idCarrito);

        try
        {
            return Conexion.QueryFirstOrDefault<HistorialCambios>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la ultima fecha de modificacion de carrito");
        }
    }
    #endregion

    #region Obtener fecha de ultima aportacion de un Uusario
    public string? GetLastDateByUser(int idUsuario)
    {
        string query = @"select max(fechaCambio) as 'FechaCambio'
                        from historialcambio h 
                        where h.idUsuario = @unidUsuario;";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidUsuario", idUsuario);

        try
        {
            return Conexion.QueryFirstOrDefault<string>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la ultima fecha de aporte de un usuario");
        }
    }
    #endregion

    #region Cantidad de Acciones por usuario y tipo accion
    public int CantidadAccionByUser(int idUsuario, int idTipoAccion)
    {
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("unidUsuario", idUsuario);
        parameters.Add("unidTipoAccion", idTipoAccion);

        string query = @"SELECT COUNT(*) AS CantidadCambios
                         FROM HistorialCambio hc
                         JOIN Usuarios u ON hc.idUsuario = u.idUsuario
                         JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
                         where u.idUsuario = @unidUsuario
                         and ta.idTipoAccion = @unidTipoAccion;";

        try
        {
            return Conexion.ExecuteScalar<int>(query, parameters, Transaction);
        }
        catch(Exception ex)
        {
            throw new Exception("No se pudo obtener la cantidad de dicha accion"+ ex);
        }
    }
    #endregion

    #region Cantidad de Acciones de Prestamos por Usuarios
    public int CantidadPrestamosByUser(int idUsuario)
    {
        string query = @"select count(*)
                        from prestamos p 
                        where p.idUsuarioRecibio = @unidUsuario";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidUsuario", idUsuario);

        try 
        {
            return Conexion.QueryFirstOrDefault<int>(query, parameters, transaction: Transaction);
        }
        catch(Exception)
        {
            throw new Exception("Error al obtener la cantidad de acciones de Prestamos");
        }
    }
    #endregion

    public int CantidadDevolucionByUser(int idUsuario)
    {
        string query = @"select count(d.idDevolucion)
                         from Devoluciones d 
                         join Prestamos p on p.idPrestamo = d.idPrestamo
                         where p.idUsuarioRecibio = @unidUsuario";

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidUsuario", idUsuario);

        try
        {
            return Conexion.QueryFirstOrDefault<int>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener la cantidad de acciones de Devolucion");
        }
    }
}
