using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoTipoElemento : RepoBase, IRepoTipoElemento
{
    public RepoTipoElemento( IDbConnection conexion, IDbTransaction? transaction = null) 
    : base(conexion, transaction)
    {
    }

    #region Insertar Tipo del Elemento
    public void Insert(TipoElemento tipoElemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidTipoElemento", tipoElemento.IdTipoElemento, dbType: DbType.Int32, direction: ParameterDirection.Output);
        parametros.Add("untipoElemento", tipoElemento.ElementoTipo);

        try
        {
            Conexion.Execute("InsertTipoElemento", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al dar de alta el tipo de Elemento");
        }
    }
    #endregion

    #region Actualizar el tipo del elemento
    public void Update(TipoElemento tipoElemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidTipoElemento", tipoElemento.IdTipoElemento);
        parametros.Add("untipoElemento", tipoElemento.ElementoTipo);

        try
        {
            Conexion.Execute("UpdateTipoElemento", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar el tipo de Elemento");
        }
    }
    #endregion

    #region Eliminar tipo del elemento
    public void Delete(int idTipoElemento)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidTipoElemento", idTipoElemento);

        try
        {
            Conexion.Execute("DeleteTipoElemento", parametros, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al eliminar el tipo del Elemento");
        }
    }
    #endregion

    #region ver los datos los tipos de los elementos
    public IEnumerable<TipoElemento> GetAll()
    {

        string query = "select IdTipoElemento, elemento AS 'ElementoTipo' from TipoElemento";

        try
        {
            return Conexion.Query<TipoElemento>(query, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener los datos de los tipos del elemento");
        }
    }
    #endregion

    #region Obtener id del Tipo elemento
    public TipoElemento? GetById(int idTipoElemento)
    {
        string query = "select IdTipoElemento, elemento as 'ElementoTipo' from TipoElemento where idTipoElemento = @unidTipoElemento";

        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("unidTipoElemento", idTipoElemento);

        try
        {
            return Conexion.QueryFirstOrDefault<TipoElemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el id Del tipo del Elemento");
        }
    }
    #endregion

    public TipoElemento? GetByTipo(string elementoTipo)
    {
        string query = "select IdTipoElemento, elemento as 'ElementoTipo' from TipoElemento where elemento = @elementoTipo";

        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("@elementoTipo", elementoTipo);

        try
        {
            return Conexion.QueryFirstOrDefault<TipoElemento>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el tipo del Elemento");
        }
    }
}
