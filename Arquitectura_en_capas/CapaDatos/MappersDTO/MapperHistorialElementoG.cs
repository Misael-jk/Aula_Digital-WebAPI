using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MappersDTO
{
    public class MapperHistorialElementoG : RepoBase, IMapperHistorialElementoG
    {
        public MapperHistorialElementoG(IDbConnection conexion, IDbTransaction? transaction = null)
        : base(conexion, transaction)
        {
        }

        public IEnumerable<HistorialElementoGestionDTO> GetAllDTO(int idElemento)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdElemento", idElemento);

            return Conexion.Query<HistorialElementos, Usuarios, TipoAccion, HistorialCambios, HistorialElementoGestionDTO>(
            @"SELECT 
            he.idHistorialCambio as 'IdHistorialCambio',
            concat(u.nombre, ' ', u.apellido) AS Nombre,
            ta.accion AS Accion,
            hc.fechaCambio 
            FROM HistorialElemento he
            JOIN HistorialCambio hc ON he.idHistorialCambio = hc.idHistorialCambio
            JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
            JOIN Usuarios u ON hc.idUsuario = u.idUsuario
            JOIN Elementos e ON he.idElemento = e.idElemento
            left join notebooks n on n.idElemento = e.idElemento
            where n.idElemento is null
            and e.idElemento = @unIdElemento
            ORDER BY hc.fechaCambio DESC;",

            (HistorialElemento, Usuarios, TipoAccion, HistorialCambios) => new HistorialElementoGestionDTO
            {
                IdHistorialElemento = HistorialElemento.IdHistorialCambio,
                Usuario = Usuarios.Nombre,
                AccionRealizada = TipoAccion.Accion,
                FechaCambio = HistorialCambios.FechaCambio
            },
            parameters,
            splitOn: "IdHistorialCambio,Nombre,Accion,FechaCambio"
            );
        }
    }
}
