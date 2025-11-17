using CapaDTOs;
using CapaDatos.InterfacesDTO;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperPrestamoDetalle : RepoBase, IMapperPrestamoDetalle
{
    public MapperPrestamoDetalle(IDbConnection conexion, IDbTransaction? transaction = null) 
        : base(conexion, transaction)
    {
    }

    public IEnumerable<PrestamosDetalleDTO> GetByIdDTO(int idPrestamo, int? idCarrito)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idPrestamo", idPrestamo);
        parameters.Add("@idCarrito", idCarrito);

        var sql = @"SELECT
                    e.idElemento,
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
                    END AS EquipoCarrito
                    FROM PrestamoDetalle pd
                    INNER JOIN Elementos e 
                    ON pd.idElemento = e.idElemento
                    INNER JOIN TipoElemento te 
                    ON e.idTipoElemento = te.idTipoElemento
                    LEFT JOIN VariantesElemento ve 
                    ON e.idVariante = ve.idVariante
                    LEFT JOIN Notebooks nb 
                    ON e.idElemento = nb.idElemento
                    LEFT JOIN Carritos ca
                    ON nb.idCarrito = ca.idCarrito
                    WHERE pd.idPrestamo = @idPrestamo
                    ORDER BY e.idElemento;";

        return Conexion.Query<Elemento, TipoElemento, Notebooks, Carritos, PrestamosDetalleDTO>(
            sql,
            (elemento, tipo, notebook, carrito) => new PrestamosDetalleDTO
            {
                idElemento = elemento.IdElemento,
                NumeroSerieElemento = elemento.NumeroSerie,
                Patrimonio = elemento.Patrimonio,
                TipoElemento = tipo.ElementoTipo,
                Equipo = notebook.Equipo,
                PosicionCarrito = carrito.EquipoCarrito,
            },
            parameters,
            splitOn: "IdElemento,ElementoTipo,Equipo,EquipoCarrito"
        ).ToList();
    }

    public PrestamosDetalleDTO? GetElementoById(int idElemento, int? idCarrito)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@idElemento", idElemento);
        parameters.Add("@idCarrito", idCarrito);

        var sql = @"SELECT
                e.idElemento,
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
                END AS EquipoCarrito
                FROM Elementos e
                INNER JOIN TipoElemento te ON e.idTipoElemento = te.idTipoElemento
                LEFT JOIN VariantesElemento ve ON e.idVariante = ve.idVariante
                LEFT JOIN Notebooks nb ON e.idElemento = nb.idElemento
                LEFT JOIN Carritos ca ON nb.idCarrito = ca.idCarrito
                WHERE e.idElemento = @idElemento
                ORDER BY e.idElemento;";

        return Conexion.Query<Elemento, TipoElemento, Notebooks, Carritos, PrestamosDetalleDTO>(
            sql,
            (elemento, tipo, notebook, carrito) => new PrestamosDetalleDTO
            {
                idElemento = elemento.IdElemento,
                NumeroSerieElemento = elemento.NumeroSerie,
                Patrimonio = elemento.Patrimonio,
                TipoElemento = tipo.ElementoTipo,
                Equipo = notebook?.Equipo ?? "-",
                PosicionCarrito = carrito?.EquipoCarrito ?? "-"
            },
            parameters,
            splitOn: "IdElemento,ElementoTipo,Equipo,EquipoCarrito"
        ).FirstOrDefault();
    }

}
