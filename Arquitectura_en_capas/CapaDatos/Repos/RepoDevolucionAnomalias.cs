//using Dapper;
//using CapaDatos.Interfaces;
//using CapaEntidad;
//using System.Data;

//namespace CapaDatos.Repos;

//public class RepoDevolucionAnomalias : RepoBase, IRepoDevolucionAnomalias
//{
//    public RepoDevolucionAnomalias(IDbConnection dbConnection, IDbTransaction? transaction = null) : base(dbConnection, transaction)
//    {
//    }
//    public void Insert(DevolucionAnomalias devolucionAnomalias)
//    {
//        DynamicParameters parameters = new DynamicParameters();

//        parameters.Add("unidDevolucion", devolucionAnomalias.IdDevolucion);
//        parameters.Add("unidTipoAnomalia", devolucionAnomalias.IdTipoAnomalia);
//        parameters.Add("uDescripcion", devolucionAnomalias.Descripcion);
//        try
//        {
//            Conexion.Execute("InsertDevolucionAnomalias", parameters, transaction: Transaction, commandType: CommandType.StoredProcedure);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al insertar la anomalia de la devolucion");
//        }
//    }

//    public IEnumerable<DevolucionAnomalias> GetByDevolucion(int idDevolucion)
//    {
//        DynamicParameters parameters = new DynamicParameters();
//        parameters.Add("unidDevolucion", idDevolucion);
//        string sql = "select * from DevolucionAnomalias where idDevolucion = @unidDevolucion";

//        try
//        {
//            return Conexion.Query<DevolucionAnomalias>(sql, parameters, transaction: Transaction);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al obtener las anomalias por devolucion");
//        }
//    }

//    public IEnumerable<DevolucionAnomalias> GetAll()
//    {
//        string sql = "select * from DevolucionAnomalias";
//        try
//        {
//            return Conexion.Query<DevolucionAnomalias>(sql, transaction: Transaction);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al obtener todas las anomalias de devoluciones");
//        }
//    }
//}
