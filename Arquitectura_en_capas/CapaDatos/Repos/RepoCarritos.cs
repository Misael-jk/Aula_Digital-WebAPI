using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoCarritos : RepoBase, IRepoCarritos
{
    public RepoCarritos(IDbConnection conexion)
   : base(conexion)
    { 
    }

    #region Alta Carrito
    public void Insert(Carritos carritos)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidCarrito", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("unequipo", carritos.Equipo);
        parametros.Add("unnumeroSerieCarrito", carritos.NumeroSerieCarrito);
        parametros.Add("unidEstadoMantenimiento", carritos.IdEstadoMantenimiento);
        parametros.Add("unidUbicacion", carritos.IdUbicacion);
        parametros.Add("unidModelo", carritos.IdModelo);
        parametros.Add("unhabilitado", carritos.Habilitado);
        parametros.Add("unfechaBaja", carritos.FechaBaja);

        try
        {
            Conexion.Execute("InsertCarrito", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un carrito");
        }
    }
    #endregion 

    #region Actualizar Carrito
    public void Update(Carritos carritos)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidCarrito", carritos.IdCarrito);
        parametros.Add("unequipo", carritos.Equipo);
        parametros.Add("unnumeroSerieCarrito", carritos.NumeroSerieCarrito);
        parametros.Add("unidEstadoMantenimiento", carritos.IdEstadoMantenimiento);
        parametros.Add("unidUbicacion", carritos.IdUbicacion);
        parametros.Add("unidModelo", carritos.IdModelo);
        parametros.Add("unhabilitado", carritos.Habilitado);
        parametros.Add("unfechaBaja", carritos.FechaBaja);

        try
        {
            Conexion.Execute("UpdateCarrito", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar un carrito");
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
            Conexion.Execute(query, parametros);
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
            Conexion.Execute("DeleteCarrito", parametros, commandType: CommandType.StoredProcedure);
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
        string query = "select * from Carritos where idCarrito = @unidCarrito";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("unidCarrito", idCarrito);
            return Conexion.QueryFirstOrDefault<Carritos>(query, parametros);
        }
        catch(Exception)
        {
            throw new Exception("Error al obtener el id del carrito");
        }
        
    }
    #endregion

    #region Obtener por Numero de Serie
    public Carritos? GetByNumeroSerie(string numeroSerie)
    {
        string query = "select * from Carritos where numeroSerieCarrito = @numeroSerie";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("numeroSerie", numeroSerie);
            return Conexion.QueryFirstOrDefault<Carritos>(query, parametros);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el numero de serie del carrito");
        }

    }
    #endregion

    public void UpdateDisponible(int idCarrito, int idEstadoMantenimiento)
    {
        string sql = @"update Carritos
                       idEstadoMantenimiento = @idEstadoMantenimiento
                       where idCarrito = @IdCarrito";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@idEstadoMantenimiento", idEstadoMantenimiento);
        parameters.Add("@IdCarritoo", idCarrito);

        Conexion.Execute(sql, parameters);
    }

    public bool GetDisponible(int idCarrito)
    {

        string sql = "select * from Carritos where idCarrito = @IdCarrito and idEstadoMantenimiento = 1 and habilitado = true limit 1;";

        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("@IdCarrito", idCarrito);

        int disponible = Conexion.ExecuteScalar<int>(sql, parameters);

        return disponible > 0;
    }

    public int GetCountByCarrito(int idCarrito)
    {
        string query = "select COUNT(*) from Elementos where idCarrito = @idCarrito";

        DynamicParameters parametros = new DynamicParameters();

        try
        {
            parametros.Add("idCarrito", idCarrito);
            int cantidad = Conexion.ExecuteScalar<int>(query, parametros);

            return cantidad;
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el numero de serie del carrito");
        }
    }
}
