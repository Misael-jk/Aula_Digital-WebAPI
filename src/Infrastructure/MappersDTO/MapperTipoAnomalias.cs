using CapaDatos.InterfacesDTO;
//using CapaDTOs;
//using CapaEntidad;
//using Dapper;
//using System.Data;

//namespace CapaDatos.MappersDTO;

//public class MapperTipoAnomalias : RepoBase, IMapperTipoAnomalias
//{
//    public MapperTipoAnomalias(IDbConnection dbConnection, IDbTransaction? transaction = null) : base(dbConnection, transaction)
//    {
//    }
//    public IEnumerable<TipoAnomaliasDTO> GetAllDTO()
//    {
//        return Conexion.Query<TipoAnomalias, TipoElemento, TipoAnomaliasDTO>(
//            "Select * from View_TipoAnomaliasDTO",
//            (tipoAnomalia, tipoElemento) => new TipoAnomaliasDTO
//            {
//                IdTipoAnomalia = tipoAnomalia.IdTipoAnomalia,
//                NombreTipoElemento = tipoElemento.ElementoTipo,
//                NombreAnomalia = tipoAnomalia.NombreAnomalia
//            },
//            splitOn: "IdTipoAnomalia,ElementoTipo");
//    }
//}
