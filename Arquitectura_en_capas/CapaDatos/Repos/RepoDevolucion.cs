using Dapper;
using CapaDatos.Interfaces;
using CapaEntidad;
using System.Data;

namespace CapaDatos.Repos;

public class RepoDevolucion : RepoBase, IRepoDevolucion
{
    public RepoDevolucion(IDbConnection conexion, IDbTransaction? transaction = null) 
    : base(conexion, transaction)
    {
    }

    #region Alta Devolucion
    public void Insert(Devolucion devolucion)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("unidDevolucion", dbType: DbType.Int32, direction: ParameterDirection.Output);

        parametros.Add("unidPrestamo", devolucion.IdPrestamo);
        parametros.Add("unidEncargado", devolucion.IdUsuario);
        parametros.Add("unidEstdoPrestamo", devolucion.IdEstadoPrestamo);
        parametros.Add("unafechaDevolucion", devolucion.FechaDevolucion);
        parametros.Add("unaobservaciones", devolucion.Observaciones);

        try
        {
            Conexion.Execute("InsertDevolucion", parametros, transaction: Transaction, commandType: CommandType.StoredProcedure);
            devolucion.IdDevolucion = parametros.Get<int>("unidDevolucion");
        }
        catch (Exception)
        {
            throw new Exception("Hubo un error al insertar una devolucion");
        }
    }
    #endregion

    #region Obtener todas las devoluciones
    public IEnumerable<Devolucion> GetAll()
    {
        string query = "select * from Devoluciones";

        try
        {
            return Conexion.Query<Devolucion>(query);
        }
        catch (Exception)
        {
            throw new Exception("Error al ver los datos de las devoluciones");
        }
    }
    #endregion

    #region obtener por Id
    public Devolucion? GetById(int iddevolucion)
    {
        string query = "select * from Devoluciones where idDevolucion = unidDevolucion";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidDevolucion", iddevolucion);
            return Conexion.QueryFirstOrDefault<Devolucion>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el id de la devolucion");
        }
    }
    #endregion

    #region obtener por Prestamo
    public IEnumerable<Devolucion> GetByPrestamo(int idPrestamo)
    {
        string query = "select * from Devoluciones where idPrestamo = unidPrestamo";

        DynamicParameters parametros = new DynamicParameters();
        try
        {
            parametros.Add("unidPrestamo", idPrestamo);
            return Conexion.Query<Devolucion>(query, parametros, transaction: Transaction);
        }
        catch (Exception)
        {
            throw new Exception("Error al obtener el id del prestamo en Devoluciones");
        }
    }
    #endregion
}
