using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperDevoluciones : RepoBase, IMapperDevoluciones
{
    public MapperDevoluciones(IDbConnection conexion, IDbTransaction? transaction = null)
      : base(conexion, transaction)
    {
    }

    #region Obtener todas las devoluciones
    public IEnumerable<DevolucionesDTO> GetAllDTO()
    {
        return Conexion.Query<Devolucion, Prestamos, Docentes, Usuarios, EstadosPrestamo, DevolucionesDTO>(
    "GetDevolucionesDTO",
    (devolucion, prestamo, docente, usuario, estado) => new DevolucionesDTO
    {
        IdDevolucion = devolucion.IdDevolucion,
        FechaDevolucion = devolucion.FechaDevolucion,
        Observaciones = devolucion?.Observaciones ?? "-",
        FechaPrestamo = prestamo.FechaPrestamo,
        EstadoPrestamo = estado.EstadoPrestamo,
        ApellidoDocente = docente.Apellido,
        ApellidoEncargado = usuario.Apellido
    },
    commandType: CommandType.StoredProcedure,
    splitOn: "FechaPrestamo,Apellido,Apellido,EstadoPrestamo"
).ToList();
    }
    #endregion

    #region Obtener por Id de prestamo
    public DevolucionesDTO? GetByIdDTO(int idPrestamo)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("@idPrestamo", idPrestamo, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Devolucion, Prestamos, Docentes, Usuarios, EstadosPrestamo, DevolucionesDTO>(
            "GetDevolucionByPrestamoDTO",
            (devolucion, prestamos, docente, encargado, estado) => new DevolucionesDTO
            {
                IdDevolucion = devolucion.IdDevolucion,
                ApellidoDocente = docente.Apellido,
                ApellidoEncargado = encargado.Apellido,
                FechaPrestamo = prestamos.FechaPrestamo,
                FechaDevolucion = devolucion.FechaDevolucion,
                EstadoPrestamo = estado.EstadoPrestamo,
                Observaciones = devolucion?.Observaciones ?? "-"    
            },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "Apellido,Apellido,EstadoPrestamo"
        ).FirstOrDefault();
    }
    #endregion

    #region Obtener por Id de docente
    public IEnumerable<DevolucionesDTO> GetByDocenteDTO(int idDocente)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("@idDocente", idDocente, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Devolucion, Prestamos, Docentes, Usuarios, EstadosPrestamo, DevolucionesDTO>(
            "GetDevolucionesByDocenteDTO",
            (devolucion, prestamos, docente, encargado, estado) => new DevolucionesDTO
            {
                IdDevolucion = devolucion.IdDevolucion,
                ApellidoDocente = docente.Apellido,
                ApellidoEncargado = encargado.Apellido,
                FechaPrestamo = prestamos.FechaPrestamo,
                FechaDevolucion = devolucion.FechaDevolucion,
                EstadoPrestamo = estado.EstadoPrestamo,
                Observaciones = devolucion?.Observaciones ?? "-"
            },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "Apellido, Apellido, FechaPrestamo,EstadoPrestamo"
        ).ToList();
    }
    #endregion
}