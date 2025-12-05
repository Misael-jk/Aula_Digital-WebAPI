using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Data;
using Dapper;
using Core.Entities.Catalogos;
using Core.Entities.Aggregates.Docentes;

namespace CapaDatos.MappersDTO;

public class MapperDocentes : RepoBase, IMapperDocentes
{
    public MapperDocentes(IDbConnection conexion, IDbTransaction? transaction = null)
    : base(conexion, transaction)
    {
    }

    public IEnumerable<DocentesDTO> GetAllDTO()
    {
        return Conexion.Query<Docentes, EstadosPrestamo, DocentesDTO>(
            "select * from View_GetDocenteDTO",
            (docente, estado) => new DocentesDTO
            {
                IdDocente = docente.IdDocente,
                Nombre = docente.Nombre,
                Apellido = docente.Apellido,
                Dni = docente.Dni,
                Email = docente.Email,
                EstadoPrestamo = estado.EstadoPrestamo
            },
            splitOn: "IdDocente,EstadoPrestamo");
    }
}
