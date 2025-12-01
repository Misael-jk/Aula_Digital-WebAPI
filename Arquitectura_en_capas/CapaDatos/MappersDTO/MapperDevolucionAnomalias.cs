//using CapaDatos.InterfacesDTO;
//using CapaDTOs;
//using CapaEntidad;
//using Dapper;
//using System.Data;

//namespace CapaDatos.MappersDTO;

//public class MapperDevolucionAnomalias : RepoBase, IMapperDevolucionAnomalias
//{
//    public MapperDevolucionAnomalias(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
//    {
//    }

//    public IEnumerable<DevolucionAnomaliasDTO> GetAllDTO()
//    {
//        return Conexion.Query<DevolucionAnomaliasDTO>(
//            "GetDevolucionAnomaliasDTO",
//            commandType: CommandType.StoredProcedure
//        ).ToList();
//    }
//}
