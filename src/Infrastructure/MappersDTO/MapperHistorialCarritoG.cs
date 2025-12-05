using CapaDatos.InterfacesDTO;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Usuario;
using Core.Entities.Catalogos;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperHistorialCarritoG : RepoBase, IMapperHistorialCarritosG
{
    public MapperHistorialCarritoG(IDbConnection connection) : base(connection)
    {
    }

    public IEnumerable<HistorialCarritoGestionDTO> GetAllDTO(int idCarrito)
    {
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("unidCarrito", idCarrito);

        return Conexion.Query<HistorialCarritos, Usuarios, TipoAccion, HistorialCambios, HistorialCarritoGestionDTO>(
            @"select
		hc.idHistorialCambio as 'IdHistorialCambio',
		concat(u.nombre, ' ', u.apellido) AS Nombre,
		ta.accion as 'Accion',
		hc.fechaCambio
		from historialcarrito h 
		JOIN HistorialCambio hc ON h.idHistorialCambio = hc.idHistorialCambio
		JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
		JOIN Usuarios u ON hc.idUsuario = u.idUsuario
		join carritos c using (idCarrito)
		where c.idCarrito = @unidCarrito
		ORDER BY hc.fechaCambio DESC;",
              (HistorialCarrito, Usuarios, TipoAccion, HistorialCambios) => new HistorialCarritoGestionDTO
              {
                 IdHistorialCarrito = HistorialCarrito.IdHistorialCambio,
                 Usuario = Usuarios.Nombre,
                 AccionRealizada = TipoAccion.Accion,
                 FechaCambio = HistorialCambios.FechaCambio
              },
            parameters,
            splitOn: "IdHistorialCambio,Nombre,Accion,FechaCambio"
            );
    }
}
