using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.MantenimientoDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperNotebookBajas : RepoBase, IMapperNotebookBajas
{
    public MapperNotebookBajas(IDbConnection connectionString) : base(connectionString)
    {
    }

    public IEnumerable<NotebooksBajasDTO> GetAll()
    {
        return Conexion.Query<Notebooks, Carritos, ElementosDTO, NotebooksBajasDTO>(
        "select * from View_GetNotebookBajasDTO",
        (notebook, carritos, elementos) => new NotebooksBajasDTO
        {
            IdNotebook = notebook.IdElemento,
            Equipo = notebook.Equipo,
            PosicionCarrito = notebook.PosicionCarrito ?? 0,
            NumeroSerieNotebook = notebook.NumeroSerie,
            CodigoBarra = notebook.CodigoBarra,
            Patrimonio = notebook.Patrimonio,
            FechaBaja = notebook.FechaBaja.Value,
            Carrito = carritos?.EquipoCarrito ?? "Sin Carrito",
            Estado = elementos.Estado,
            Modelo = elementos.Modelo,
            Ubicacion = elementos.Ubicacion
        },
        splitOn: "IdElemento,EquipoCarrito,Estado"
        ).ToList();
    }
}
