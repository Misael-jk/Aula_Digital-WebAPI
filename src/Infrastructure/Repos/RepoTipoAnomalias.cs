//using Dapper;
//using CapaDatos.Interfaces;
//using CapaEntidad;
//using System.Data;

//namespace CapaDatos.Repos.ReposHistoriales;

//public class RepoTipoAnomalias : RepoBase, IRepoTipoAnomalias
//{
//    public RepoTipoAnomalias(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
//    {
//    }

//    public void Insert(TipoAnomalias tipoAnomalias)
//    {
//        DynamicParameters parametros = new DynamicParameters();

//        parametros.Add("unidTipoAnomalia", tipoAnomalias.IdTipoAnomalia, dbType: DbType.Int32, direction: ParameterDirection.Output);

//        parametros.Add("uDescripcion", tipoAnomalias.Descripcion);
//        parametros.Add("unidTipoElemento", tipoAnomalias.IdTipoElemento);
//        try
//        {
//            Conexion.Execute("InsertTipoAnomalias", parametros, commandType: CommandType.StoredProcedure);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al dar de alta el tipo de anomalia");
//        }
//    }

//    public void Update(TipoAnomalias tipoAnomalias)
//    {
//        DynamicParameters parametros = new DynamicParameters();
//        parametros.Add("unidTipoAnomalia", tipoAnomalias.IdTipoAnomalia);
//        parametros.Add("uDescripcion", tipoAnomalias.Descripcion);
//        parametros.Add("unidTipoElemento", tipoAnomalias.IdTipoElemento);

//        try
//        {
//            Conexion.Execute("UpdateTipoAnomalias", parametros, commandType: CommandType.StoredProcedure);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al actualizar el tipo de anomalia");
//        }
//    }

//    public IEnumerable<TipoAnomalias> GetByTipoElemento(int idTipoElemento)
//    {
//        string query = "select * from TipoAnomalias where idTipoElemento = @unidTipoElemento";

//        DynamicParameters parametros = new DynamicParameters();

//        parametros.Add("unidTipoElemento", idTipoElemento);
//        try
//        {
//            return Conexion.Query<TipoAnomalias>(query, parametros);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al obtener los tipos de anomalias por tipo de elemento");
//        }
//    }

//    public IEnumerable<TipoAnomalias> GetAll()
//    {
//        string query = "select * from TipoAnomalias";
//        try
//        {
//            return Conexion.Query<TipoAnomalias>(query);
//        }
//        catch (Exception)
//        {
//            throw new Exception("Hubo un error al obtener todos los tipos de anomalias");
//        }
//    }
//}
