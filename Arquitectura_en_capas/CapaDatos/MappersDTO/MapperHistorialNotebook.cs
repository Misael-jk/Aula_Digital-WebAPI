using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperHistorialNotebook : RepoBase, IMapperHistorialNotebook
{
    public MapperHistorialNotebook(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<HistorialNotebooksDTO> GetAllDTO()
    {
        return Conexion.Query<HistorialNotebooks, HistorialCambios, TipoAccion, NotebooksDTO, UsuariosDTO, HistorialNotebooksDTO>(
            "GetHistorialNotebooksDTO",
            (historial, cambios, accion, notebook, usuario) => new HistorialNotebooksDTO
            {
                IdHistorialNotebook = historial.IdHistorialCambio,
                NumeroSerie = notebook.NumeroSerieNotebook,
                CodigoBarra = notebook.CodigoBarra,
                Modelo = notebook.Modelo,
                Carrito = notebook.Carrito,
                PosicionCarrito = notebook.PosicionCarrito,
                EstadoMantenimiento = notebook.Estado,
                Descripcion = cambios?.Descripcion,
                FechaCambio = cambios.FechaCambio,
                AccionRealizada = accion.Accion,
                Usuario = usuario.Apellido
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "FechaCambio,Accion,NombreNotebook,Apellido"
        ).ToList();
    }
}
