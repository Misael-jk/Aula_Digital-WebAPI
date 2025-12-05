using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Notebooks;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperNotebooks : RepoBase, IMapperNotebooks
{
    public MapperNotebooks(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
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
                PosicionCarrito = notebook.PosicionCarrito ?? 0,
                NumeroSerieNotebook = notebook.NumeroSerie,
                CodigoBarra = notebook.CodigoBarra,
                Patrimonio = notebook.Patrimonio,
                Carrito = carritos?.EquipoCarrito ?? "Sin Carrito",
                Estado = elementos.Estado,
                Modelo = elementos.Modelo,
                Ubicacion = elementos.Ubicacion
            },
            splitOn: "IdElemento,EquipoCarrito,Estado"
        ).ToList();
    }

    public IEnumerable<NotebooksDTO> GetNotebooksByCarritoDTO(string carrito)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@carrito", carrito);

        return Conexion.Query<Notebooks, Carritos, ElementosDTO, NotebooksDTO>(
            "select * from View_GetNotebookDTO vg where vg.EquipoCarrito = @carrito",
            (notebook, carritos, elementos) => new NotebooksDTO
            {
                IdNotebook = notebook.IdElemento,
                Equipo = notebook.Equipo,
                PosicionCarrito = notebook.PosicionCarrito ?? 0,
                NumeroSerieNotebook = notebook.NumeroSerie,
                CodigoBarra = notebook.CodigoBarra,
                Patrimonio = notebook.Patrimonio,
                Carrito = carritos?.EquipoCarrito ?? "Sin Carrito",
                Estado = elementos.Estado,
                Modelo = elementos.Modelo,
                Ubicacion = elementos.Ubicacion
            },
            parametros,
            splitOn: "IdElemento,EquipoCarrito,Estado"
        ).ToList();
    }

    public IEnumerable<NotebooksDTO> GetAllByEstado(string estado)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@estado", estado);

        return Conexion.Query<Notebooks, Carritos, ElementosDTO, NotebooksDTO>(
            "select * from View_GetNotebookDTO vg where vg.Estado = @estado",

            (notebook, carritos, elementos) => new NotebooksDTO
            {
                IdNotebook = notebook.IdElemento,
                Equipo = notebook.Equipo,
                PosicionCarrito = notebook.PosicionCarrito ?? 0,
                NumeroSerieNotebook = notebook.NumeroSerie,
                CodigoBarra = notebook.CodigoBarra,
                Patrimonio = notebook.Patrimonio,
                Carrito = carritos?.EquipoCarrito ?? "Sin Carrito",
                Estado = elementos.Estado,
                Modelo = elementos.Modelo,
                Ubicacion = elementos.Ubicacion
            },
            parametros,
            splitOn: "IdElemento,EquipoCarrito,Estado"
        ).ToList();
    }

    public IEnumerable<NotebooksDTO> GetByFiltros(string? text, int? idCarrito, string? equipo)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@untext", text);
        parametros.Add("@unidCarrito", idCarrito);
        parametros.Add("@unequipo", equipo);

        return Conexion.Query<Notebooks, Carritos, ElementosDTO, NotebooksDTO>(
            "SP_GetNotebooksDTO",
            (notebook, carritos, elementos) => new NotebooksDTO
            {
                IdNotebook = notebook.IdElemento,
                Equipo = notebook.Equipo,
                PosicionCarrito = notebook.PosicionCarrito ?? 0,
                NumeroSerieNotebook = notebook.NumeroSerie,
                CodigoBarra = notebook.CodigoBarra,
                Patrimonio = notebook.Patrimonio,
                Carrito = carritos?.EquipoCarrito ?? "Sin Carrito",
                Estado = elementos.Estado,
                Modelo = elementos.Modelo,
                Ubicacion = elementos.Ubicacion
            },
            parametros,
            splitOn: "IdElemento,EquipoCarrito,Estado",
            commandType: CommandType.StoredProcedure
        ).ToList();
    }
}
