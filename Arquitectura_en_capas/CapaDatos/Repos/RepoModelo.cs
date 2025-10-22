using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoModelo : RepoBase, IRepoModelo
{
    public RepoModelo(IDbConnection dbConnection, IDbTransaction? transaction = null) : base(dbConnection, transaction)
    {
    }

    #region INSERT MODELOS
    public void Insert(Modelos modelo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidModelo", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unnombre", modelo.NombreModelo);
        parameters.Add("unidTipoElemento", modelo.IdTipoElemento);
        try
        {
            Conexion.Execute("InsertModelo", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar un modelo");
        }
    }
    #endregion

    #region ACTUALIZAR MODELO
    public void Update(Modelos modelo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidModelo", modelo.IdModelo);
        parameters.Add("unnombre", modelo.NombreModelo);
        parameters.Add("unidTipoElemento", modelo.IdTipoElemento);
        try
        {
            Conexion.Execute("UpdateModelo", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar un modelo");
        }
    }
    #endregion

    #region OBTENER TODOS
    public IEnumerable<Modelos> GetAll()
    {
        string query = "select idModelo, idTipoElemento, modelo as 'NombreModelo' from Modelo";

        return Conexion.Query<Modelos>(query);
    }
    #endregion

    #region OBTENER POR ID
    public Modelos? GetById(int idModelo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidModelo", idModelo);

        string query = "select idModelo, idTipoElemento, modelo as 'NombreModelo' from Modelo where idModelo = @unidModelo";

        try
        {
            return Conexion.QueryFirstOrDefault<Modelos>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener un modelo por id");
        }
    }
    #endregion

    #region OBTENER POR TIPO
    public IEnumerable<Modelos> GetByTipo(int idTipoElemento)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidTipoElemento", idTipoElemento);

        string query = "select idModelo, idTipoElemento, modelo as 'NombreModelo' from Modelo where idTipoElemento = @unidTipoElemento";

        try
        {
            return Conexion.Query<Modelos>(query, parameters, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener los modelos por tipo de elemento");
        }
    }
    #endregion

    #region OBTENER POR NOMBRE
    public Modelos? GetByNombre(string nombreModelo)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unnombre", nombreModelo);

        string query = "select idModelo, idTipoElemento, modelo as 'NombreModelo' from Modelos where modelo = @unnombre";
        try
        {
            return Conexion.QueryFirstOrDefault<Modelos>(query, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener un modelo por nombre");
        }
    }
    #endregion
}
