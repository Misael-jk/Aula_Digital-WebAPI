using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoUbicacion : RepoBase, IRepoUbicacion
{
    public RepoUbicacion(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    #region ALTA UBICACION
    public void Insert(Ubicacion ubicacion)
    {
        DynamicParameters parameters = new DynamicParameters();

        parameters.Add("unidUbicacion", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("unaubicacion", ubicacion.NombreUbicacion);

        try
        {
            Conexion.Execute("InsertUbicacion", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar una ubicacion");
        }
    }
    #endregion

    #region UPDATE UBICACION
    public void Update(Ubicacion ubicacion)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidUbicacion", ubicacion.IdUbicacion);
        parameters.Add("unaubicacion", ubicacion.NombreUbicacion);
        try
        {
            Conexion.Execute("UpdateUbicacion", parameters, commandType: CommandType.StoredProcedure);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al actualizar una ubicacion");
        }
    }
    #endregion

    #region OBTENER TODO
    public IEnumerable<Ubicacion> GetAll()
    {
        string query = "select idUbicacion, ubicacion as 'NombreUbicacion' from Ubicacion";

        return Conexion.Query<Ubicacion>(query);
    }
    #endregion

    #region OBTENER POR UBICACION
    public Ubicacion? GetByUbicacion(string ubicacion)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unaUbicacion", ubicacion);

        string sql = "select * from Ubicacion where ubicacion = @unaubicacion";

        try
        {
            return Conexion.QueryFirstOrDefault<Ubicacion>(sql, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la ubicacion por nombre");
        }
    }
    #endregion

    #region OBTENER POR ID
    public Ubicacion? GetById(int idUbicacion)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidUbicacion", idUbicacion);

        try
        {
            string sql = "select * from Ubicacion where idUbicacion = @unidUbicacion";
            return Conexion.QueryFirstOrDefault<Ubicacion>(sql, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener la ubicacion por Id");
        }
    }
    #endregion

    #region OBTENER POR TIPO
    public IEnumerable<Ubicacion> GetByTipo(int idTipoElemento)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidTipoElemento", idTipoElemento);

        string sql = @"select *' 
                       from Ubicacion u
                       where idTipoElemento = @unidTipoElemento";
        try
        {
            return Conexion.Query<Ubicacion>(sql, parameters);
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al obtener las ubicaciones por tipo de elemento");
        }
    }
    #endregion
}
