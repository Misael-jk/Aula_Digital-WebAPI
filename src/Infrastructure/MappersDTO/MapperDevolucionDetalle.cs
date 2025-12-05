using CapaEntidad;
using Dapper;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Data;
using Core.Entities.Catalogos;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Notebooks;
using Core.Entities.Aggregates.Prestamos;

namespace CapaDatos.MappersDTO;

public class MapperDevolucionDetalle : RepoBase, IMapperDevolucionDetalle
{
    public MapperDevolucionDetalle(IDbConnection conexion, IDbTransaction? transaction = null)
    : base(conexion, transaction)
    {
    }

    public IEnumerable<DevolucionDetalleDTO> GetByIdDTO(int idDevolucion, int? idCarrito)
    {
        var parameters = new DynamicParameters();
        parameters.Add("idDevolucion", idDevolucion);
        parameters.Add("idCarrito", idCarrito);

        var sql = @"SELECT
                    dd.idElemento,
                        e.numeroSerie,
                        e.patrimonio,
                        te.elemento AS ElementoTipo,
                        CASE 
                            WHEN e.idTipoElemento = 1 THEN nb.equipo
                            ELSE ve.subtipo
                        END AS Equipo,
                        CASE 
                            WHEN e.idTipoElemento = 1 
                                 AND nb.idCarrito = @idCarrito THEN 
                                CONCAT(ca.equipo, ' - Posición ', nb.posicionCarrito)
                            ELSE '-'
                        END AS EquipoCarrito,
                    d.fechaDevolucion
                    FROM DevolucionDetalle dd
                    INNER JOIN Elementos e ON dd.idElemento = e.idElemento
                    INNER JOIN TipoElemento te ON e.idTipoElemento = te.idTipoElemento
                    LEFT JOIN VariantesElemento ve ON e.idVariante = ve.idVariante
                    LEFT JOIN Notebooks nb ON e.idElemento = nb.idElemento
                    LEFT JOIN Carritos ca ON nb.idCarrito = ca.idCarrito
                    INNER JOIN Devoluciones d ON dd.idDevolucion = d.idDevolucion
                    WHERE dd.idDevolucion = @idDevolucion
                    ORDER BY dd.idElemento;";

        return Conexion.Query<Elemento, TipoElemento, Notebooks, Carritos, Devolucion, DevolucionDetalleDTO>(
            sql,
            (elemento, tipo, notebook, carrito, devolucion) => new DevolucionDetalleDTO
            {
                idElemento = elemento.IdElemento,
                NumeroSerieElemento = elemento.NumeroSerie,
                Patrimonio = elemento.Patrimonio,
                TipoElemento = tipo.ElementoTipo,
                Equipo = notebook?.Equipo ?? "",
                PosicionCarrito = carrito?.EquipoCarrito ?? "Sin carrito",
                FechaDevolucion = devolucion.FechaDevolucion,
            },
            parameters,
            splitOn: "IdElemento,ElementoTipo,Equipo,EquipoCarrito,fechaDevolucion"
        ).ToList();
    }
}
