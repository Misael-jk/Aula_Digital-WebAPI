using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperRankingDocentes : RepoBase, IMapperRankingDocente
{
    public MapperRankingDocentes(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<RankingDocentesDTO> GetAllDTO()
    {
        string query = @"
                        SELECT 
    d.idDocente AS IdDocente,
    Concat(d.nombre, ' ', d.apellido) as Nombre,
    COUNT(p.idPrestamo) AS PrestamosRecibidos
FROM Prestamos p
JOIN docentes d ON d.idDocente = p.idDocente 
GROUP BY p.idDocente 
ORDER BY PrestamosRecibidos DESC
LIMIT 5;";
        return Conexion.Query<RankingDocentesDTO>(query, transaction: Transaction).ToList();
    }
}
