using CapaDTOs;
using CapaDatos.InterfacesDTO;
using Dapper;
using System.Data;
using Core.Entities.Catalogos;
using Core.Entities.Aggregates.Usuario;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Docentes;
using Core.Entities.Aggregates.Prestamos;
namespace CapaDatos.MappersDTO;

public class MapperTransaccion : RepoBase, IMapperTransaccion
{
    public MapperTransaccion(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<TransaccionDTO> GetAllDTO()
    {
        var sql = @"
        SELECT 
            p.idPrestamo,
            p.fechaPrestamo,
            c.curso AS NombreCurso,
            CONCAT(d.nombre, ' ', d.apellido) AS Nombre,
            ep.estadoPrestamo AS EstadoPrestamo,
            ifnull(ca.equipo, 'No llevo Carrito') AS EquipoCarrito,
            CONCAT(uu.nombre, ' ', uu.apellido) AS Nombre,
            dv.idDevolucion,
            dv.fechaDevolucion,
            dv.observaciones AS Observaciones
            FROM Prestamos p
            INNER JOIN Docentes d ON p.idDocente = d.idDocente
            INNER JOIN Cursos c ON p.idCurso = c.idCurso
            INNER JOIN EstadosPrestamo ep ON p.idEstadoPrestamo = ep.idEstadoPrestamo
            LEFT JOIN Carritos ca ON p.idCarrito = ca.idCarrito
            LEFT JOIN Devoluciones dv ON p.idPrestamo = dv.idPrestamo
            LEFT JOIN Usuarios uu ON dv.idUsuarioDevolvio = uu.idUsuario
            ORDER BY p.idPrestamo ASC;";

        return Conexion.Query<Prestamos, Curso, Docentes, EstadosPrestamo, Carritos, Usuarios, Devolucion, TransaccionDTO>(
            sql,
            (prestamo, curso, docente, estadoPrestamo, carrito, usuario, devolucion) => new TransaccionDTO
            {
                IdPrestamo = prestamo.IdPrestamo,
                FechaPrestamo = prestamo.FechaPrestamo,
                NombreCurso = curso.NombreCurso ?? " - ",
                ApellidoDocentes = docente.Nombre ?? " ERROR ",
                EstadoPrestamo = estadoPrestamo.EstadoPrestamo,
                EquipoCarrito = carrito?.EquipoCarrito ?? "No llevo carrito",
                ApellidoEncargado = usuario?.Nombre,
                IdDevolucion = devolucion?.IdDevolucion,
                FechaDevolucion = devolucion?.FechaDevolucion,
                Observaciones = devolucion?.Observaciones ?? " "
            },
            splitOn: "idPrestamo,NombreCurso,Nombre,EstadoPrestamo,EquipoCarrito,Nombre,idDevolucion"
        ).ToList();
    }
}
