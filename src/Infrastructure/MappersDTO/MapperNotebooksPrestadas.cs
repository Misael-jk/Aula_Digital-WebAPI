using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperNotebooksPrestadas : RepoBase, IMapperNotebooksPrestadas
{
    public MapperNotebooksPrestadas(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<NotebooksPrestadasDTO> GetAllDTO()
    {
        string query = @"
                SELECT 
    DATE_FORMAT(fechaPrestamo, '%Y-%m') AS Mes,
    COUNT(*) AS CantidadNotebooks
FROM Prestamos
GROUP BY Mes
ORDER BY Mes;";

        return Conexion.Query<NotebooksPrestadasDTO>(query, transaction: Transaction).ToList();
    }
}
