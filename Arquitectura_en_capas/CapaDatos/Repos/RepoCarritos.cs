using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoCarritos : RepoBase, IRepoCarritos
{
    public RepoCarritos(IDbConnection conexion, IDbTransaction? transaction = null)
   : base(conexion, transaction)
    { 
    }
    
    // CRUD/GESTION

    #region Alta Carrito
    /// <summary>
    /// parametros.Add("unidCarrito", dbType: DbType.Int32, direction: ParameterDirection.Output); <br/>
    /// parametros.Add("unequipo", carritos.EquipoCarrito); <br/>
    /// parametros.Add("unnumeroSerieCarrito", carritos.NumeroSerieCarrito); <br/>
    /// parametros.Add("unidEstadoMantenimiento", carritos.IdEstadoMantenimiento); <br/>
    /// parametros.Add("unidUbicacion", carritos.IdUbicacion); <br/>
    /// parametros.Add("unidModelo", carritos.IdModelo); <br/>
    /// parametros.Add("unhabilitado", carritos.Habilitado); <br/>
    /// parametros.Add("unfechaBaja", carritos.FechaBaja); <br/>
    /// </summary>
    /// <param name="carritos"></param>
    /// <exception cref="Exception"></exception>
    public void Insert(Carritos carritos)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidCarrito", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("unequipo", carritos.EquipoCarrito);
        parametros.Add("unnumeroSerieCarrito", carritos.NumeroSerieCarrito);
        parametros.Add("unacapacidad", carritos.Capacidad);
        parametros.Add("unidEstadoMantenimiento", carritos.IdEstadoMantenimiento);
        parametros.Add("unidUbicacion", carritos.IdUbicacion);
        parametros.Add("unidModelo", carritos.IdModelo);
        parametros.Add("unhabilitado", carritos.Habilitado);
        parametros.Add("unafechaBaja", carritos.FechaBaja);

        try
        {
            Conexion.Execute("InsertCarrito", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);

            carritos.IdCarrito = parametros.Get<int>("unidCarrito");
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al insertar un carrito", ex);
        }
    }
    #endregion 

    #region Actualizar Carrito
    public void Update(Carritos carritos)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidCarrito", carritos.IdCarrito);
        parametros.Add("unequipo", carritos.EquipoCarrito);
        parametros.Add("unnumeroSerieCarrito", carritos.NumeroSerieCarrito);
        parametros.Add("unacapacidad", carritos.Capacidad);
        parametros.Add("unidEstadoMantenimiento", carritos.IdEstadoMantenimiento);
        parametros.Add("unidUbicacion", carritos.IdUbicacion);
        parametros.Add("unidModelo", carritos.IdModelo);
        parametros.Add("unhabilitado", carritos.Habilitado);
        parametros.Add("unafechaBaja", carritos.FechaBaja);

        try
        {
            Conexion.Execute("UpdateCarrito", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            throw new Exception("Hubo un error al actualizar un carrito", ex);
        }
    }
    #endregion

    #region Deshabilitar
    public void Deshabilitar(int idCarrito, bool habilitado)
    {
        string query = @"update Carritos
                         set habilitado = @unhabilitado, fechaBaja = @unafechaBaja
                         where idCarrito = @unidCarrito";

        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("unidCarrito", idCarrito);
        parametros.Add("unhabilitado", habilitado);
        parametros.Add("unfechaBaja", !habilitado ? DateTime.Now : null);

        try
        {
            Conexion.Execute(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al deshabilitar un carrito");
        }
    }
    #endregion

    #region Eliminar Carrito
    public void Delete(int idCarrito)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidCarrito", idCarrito);

        try
        {
            Conexion.Execute("DeleteCarrito", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
        }
        catch(Exception)
        {
            throw new Exception("Hubo un error al eliminar un carrito");
        }
    }
    #endregion

    #region Obtener todos los datos
    public IEnumerable<Carritos> GetAll()
    {
        string query = "select * from Carritos";
        try
        {

            return Conexion.Query<Carritos>(query);
        }
        catch (Exception)
        {
            throw new Exception("No se pudo Obtener los datos de los carritos");
        }
    }
    #endregion

    #region Obtener por Id
    public Carritos? GetById(int idCarrito)
    {
        string query = "select idCarrito, equipo as 'EquipoCarrito', capacidad as 'Capacidad', idModelo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, habilitado, fechaBaja from Carritos where idCarrito = @unidCarrito";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unidCarrito", idCarrito);
            return Conexion.QueryFirstOrDefault<Carritos>(query, parametros, transaction: Transaction);
        }
        catch(Exception)
        {
            throw new Exception("Error al obtener el id del carrito");
        }
        
    }
    #endregion



    // VALIDACIONES/CAPA DE NEGOCIO

    #region Obtener por Numero de Serie
    public Carritos? GetByNumeroSerie(string numeroSerie)
    {
        string query = "select idCarrito, equipo as 'EquipoCarrito', capacidad as 'Capacidad', idModelo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, habilitado, fechaBaja from Carritos where numeroSerieCarrito = @numeroSerie";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("numeroSerie", numeroSerie);
            return Conexion.QueryFirstOrDefault<Carritos>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el numero de serie del carrito");
        }

    }
    #endregion

    #region Obtener por el nombre del equipo
    public Carritos? GetByEquipo(string equipo)
    {
        string query = "select idCarrito, equipo as 'EquipoCarrito', capacidad as 'Capacidad', idModelo, numeroSerieCarrito, idEstadoMantenimiento, idUbicacion, habilitado, fechaBaja from Carritos where equipo = @equipo";
        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("equipo", equipo);
            return Conexion.QueryFirstOrDefault<Carritos>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el nombre del equipo del carrito");
        }
    }
    #endregion

    #region Cambiar Disponibilidad
    /// <summary>
    /// cambia el estado de un carrito: <br/>
    /// <br/>
    /// string sql = @"update Carritos <br/>
    /// idEstadoMantenimiento = @idEstadoMantenimiento <br/>
    /// where idCarrito = @IdCarrito";
    /// </summary>
    /// <param name="idCarrito">id del carrito</param>
    /// <param name="idEstadoMantenimiento">id del estado</param>
    public void UpdateDisponible(int idCarrito, int idEstadoMantenimiento)
    {
        string sql = @"update Carritos
                       set idEstadoMantenimiento = @idEstadoMantenimiento
                       where idCarrito = @IdCarrito";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@idEstadoMantenimiento", idEstadoMantenimiento);
        parameters.Add("@IdCarrito", idCarrito);

        Conexion.Execute(sql, parameters, transaction: Transaction);
    }
    #endregion

    #region Consultar la disponibilidad de un carrito
    /// <summary>
    /// consultar la disponibilidad de un carrito: <br/>
    /// select * <br/>
    /// from Carritos <br/>
    /// where idCarrito = @IdCarrito <br/>
    /// and idEstadoMantenimiento = 1 <br/>
    /// and habilitado = true limit 1 
    /// </summary> 
    /// <param name="idCarrito"></param>
    /// <returns>TRUE si el carrito y esta disponible; FALSE si no esta disponible</returns>
    public bool GetDisponible(int idCarrito)
    {
        string sql = "select count(*) from Carritos where idCarrito = @IdCarrito and idEstadoMantenimiento = 1 and habilitado = true;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@IdCarrito", idCarrito);

        int disponible = Conexion.ExecuteScalar<int>(sql, parameters, transaction: Transaction);

        return disponible > 0;
    }
    #endregion

    #region Consultar la cantidad de notebooks de un carrito
    /// <summary>
    /// Cantidad total de notebooks en un carrito: <br/>
    /// select COUNT(*) <br/>
    /// from Elementos <br/>
    /// where idCarrito = @idCarrito <br/>
    /// </summary>
    /// <param name="idCarrito"></param>
    /// <returns>Devuelve la cantidad de notebooks</returns>
    /// <exception cref="Exception">Error al obtener el numero de serie del carrito</exception>
    public int GetCountByCarrito(int idCarrito)
    {
        string query = "select COUNT(*) from Notebooks where idCarrito = @idCarrito";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("idCarrito", idCarrito);
            int cantidad = Conexion.ExecuteScalar<int>(query, parametros, transaction: Transaction);

            return cantidad;
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el numero de serie del carrito");
        }
    }
    #endregion

    public bool GetEstadoEnPrestamo(int idCarrito)
    {
        string sql = "select count(*) from Carritos where idCarrito = @IdCarrito and idEstadoMantenimiento = 2 and habilitado = true;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@IdCarrito", idCarrito);

        int disponible = Conexion.ExecuteScalar<int>(sql, parameters, transaction: Transaction);

        return disponible > 0;
    }
}
 