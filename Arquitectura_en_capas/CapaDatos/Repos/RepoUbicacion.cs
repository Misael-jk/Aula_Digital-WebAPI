using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoUbicacion : RepoBase, IRepoUbicacion
{
    public RepoUbicacion(IDbConnection conexion) : base(conexion)
    {
    }

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

    public IEnumerable<Ubicacion> GetAll()
    {
        string query = "select idUbicacion, ubicacion as 'NombreUbicacion' from Ubicacion";

        return Conexion.Query<Ubicacion>(query);
    }
    public Ubicacion? GetIdByUbicacion(string ubicacion)
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
}
