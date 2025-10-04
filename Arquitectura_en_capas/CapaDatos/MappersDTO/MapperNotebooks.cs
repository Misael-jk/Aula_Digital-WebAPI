using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperNotebooks : RepoBase, IMapperNotebooks
{
    public MapperNotebooks(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<NotebooksDTO> GetAllDTO()
    {
        return Conexion.Query<Notebooks, Carritos, ElementosDTO, NotebooksDTO>(
            "select * from View_GetNotebookDTO",
            (notebook, carritos, elementos) => new NotebooksDTO
            {
                IdNotebook = notebook.IdElemento,
                Equipo = notebook.Equipo,
                Carrito = carritos.Equipo,
                PosicionCarrito = notebook.PosicionCarrito,
                NumeroSerieNotebook = notebook.NumeroSerie,
                CodigoBarra = notebook.CodigoBarra,
                Patrimonio = notebook.Patrimonio,
                Estado = elementos.Estado,
                Modelo = elementos.Modelo
            },
            splitOn: "Equipo,Carrito,Estado"
        ).ToList();
    }
}
