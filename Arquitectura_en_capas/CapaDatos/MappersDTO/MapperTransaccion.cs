using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;
namespace CapaDatos.MappersDTO;

public class MapperTransaccion : RepoBase, IMapperTransaccion
{
    public MapperTransaccion(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<TransaccionDTO> GetAllDTO()
    {
        return Conexion.Query<PrestamosDTO, DevolucionesDTO?, TransaccionDTO>(
            "GetPrestamoDevolucionDTO",
            (prestamo, devolucion) => new TransaccionDTO
            {
                IdPrestamo = prestamo.IdPrestamo,
                FechaPrestamo = prestamo.FechaPrestamo,
                NombreCurso = prestamo.NombreCurso ?? " - ",
                //ApellidoEncargados = prestamo.ApellidoEncargado ?? "Error",
                ApellidoDocentes = prestamo.ApellidoDocentes ?? " ERROR ",
                NumeroSerieCarrito = prestamo.NumeroSerieCarrito ?? "Sin Carrito",
                EstadoPrestamo = prestamo.EstadoPrestamo,
                IdDevolucion = prestamo.IdPrestamo,
                FechaDevolucion = devolucion?.FechaDevolucion,
                Observaciones = devolucion?.Observaciones ?? " ",
                EstadoDevolucion = devolucion?.EstadoPrestamo ?? " ",
                ApellidoDocente = devolucion?.ApellidoDocente,
                ApellidoEncargado = devolucion?.ApellidoEncargado
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "IdDevolucion"
        ).ToList();

    }
}
