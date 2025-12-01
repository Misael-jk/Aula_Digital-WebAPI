using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperHistorialElemento : RepoBase, IMapperHistorialElemento
{
    public MapperHistorialElemento(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }
    public IEnumerable<HistoralElementoDTO> GetAllDTO()
    {
        return Conexion.Query<HistoralElementoDTO>(
            "select * from View_HistorialElementoDTO"
        ).ToList();
    }
}
