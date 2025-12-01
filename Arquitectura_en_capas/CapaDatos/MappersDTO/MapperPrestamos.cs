using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperPrestamos : RepoBase, IMapperPrestamos
{
    public MapperPrestamos(IDbConnection conexion, IDbTransaction? transaction = null) 
        : base(conexion, transaction) 
    {
    }

    public IEnumerable<PrestamosDTO> GetAllDTO()
    {
        return Conexion.Query<Prestamos, Curso, Docentes, Usuarios, Carritos, EstadosPrestamo, PrestamosDTO>(
        "GetPrestamosDTO",
        (prestamo, curso, docente, usuario, carrito, estado) => new PrestamosDTO
        {
            IdPrestamo = prestamo.IdPrestamo,
            FechaPrestamo = prestamo.FechaPrestamo,
            NombreCurso = curso?.NombreCurso ?? " - ",
            ApellidoDocentes = docente.Apellido ?? " ERROR ",
            NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin Carrito",
            EstadoPrestamo = estado.EstadoPrestamo
        },
        commandType: CommandType.StoredProcedure,
        splitOn: "NombreCurso,Apellido,Apellido,NumeroSerieCarrito,EstadoPrestamo"
        ).ToList();
    }

    public PrestamosDTO? GetByIdDTO(int idPrestamo)
    {
        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("@idPrestamo", idPrestamo, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Prestamos, Curso, Docentes, Usuarios, Carritos, EstadosPrestamo, PrestamosDTO>(
        "GetPrestamoByIdDTO",
        (prestamo, curso, docente, usuario, carrito, estado) => new PrestamosDTO
            {
                IdPrestamo = prestamo.IdPrestamo,
                NombreCurso = curso?.NombreCurso ?? " - ",
                ApellidoDocentes = docente.Apellido,
                NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
                FechaPrestamo = prestamo.FechaPrestamo,
                EstadoPrestamo = estado.EstadoPrestamo
            },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "NombreCurso, Apellido, Apellido, NumeroSerieCarrito,EstadoPrestamo"
        ).FirstOrDefault();
    }

    public IEnumerable<PrestamosDTO> GetByDocenteDTO(int idDocente)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@idDocente", idDocente, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Prestamos, Curso, Docentes, Usuarios, Carritos, EstadosPrestamo, PrestamosDTO>(
        "GetPrestamosByDocenteDTO",
        (prestamo, curso, docente, usuario, carrito, estado) => new PrestamosDTO
            {
                IdPrestamo = prestamo.IdPrestamo,
                NombreCurso = curso?.NombreCurso ?? " - ",
                ApellidoDocentes = docente.Apellido,
                NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
                FechaPrestamo = prestamo.FechaPrestamo,
                EstadoPrestamo = estado.EstadoPrestamo
            },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "NombreCurso, Apellido, Apellido, NumeroSerieCarrito,EstadoPrestamo"
        ).ToList();
    }

    public IEnumerable<PrestamosDTO> GetByPaginas(int limit, int offset)
    {
        DynamicParameters parametros = new DynamicParameters();
        parametros.Add("@limit", limit, DbType.Int32, ParameterDirection.Input);
        parametros.Add("@offset", offset, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Prestamos, Curso, Docentes, Usuarios, Carritos, EstadosPrestamo, PrestamosDTO>(
        "GetPrestamosDTOByPaginas",
        (prestamo, curso, docente, usuario, carrito, estado) => new PrestamosDTO
        {
            IdPrestamo = prestamo.IdPrestamo,
            NombreCurso = curso?.NombreCurso ?? " - ",
            ApellidoDocentes = docente.Apellido,
            NumeroSerieCarrito = carrito?.NumeroSerieCarrito ?? "Sin carrito",
            FechaPrestamo = prestamo.FechaPrestamo,
            EstadoPrestamo = estado.EstadoPrestamo
        },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "NombreCurso, Apellido, Apellido, NumeroSerieCarrito,EstadoPrestamo"
        ).ToList();
    }
}
