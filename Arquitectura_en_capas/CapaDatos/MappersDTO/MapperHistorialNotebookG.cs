using CapaDatos.InterfacesDTO;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.MappersDTO;

public class MapperHistorialNotebookG : RepoBase, IMapperHistorialNotebookG
{
    public MapperHistorialNotebookG(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
    {
    }

    public IEnumerable<HistorialNotebookGestionDTO> GetAllDTO(int idNotebook)
    {
        var parameters = new DynamicParameters();
        parameters.Add("unidNotebook", idNotebook);

        return Conexion.Query<HistorialNotebooks, Usuarios, TipoAccion, HistorialCambios, HistorialNotebookGestionDTO>(
        @"SELECT  
            hn.idHistorialCambio as 'IdHistorialCambio',
            concat(u.nombre, ' ', u.apellido) AS Nombre,
            ta.accion AS Accion,
            hc.fechaCambio
            FROM historialnotebook hn
            JOIN HistorialCambio hc ON hn.idHistorialCambio = hc.idHistorialCambio
            JOIN TipoAccion ta ON hc.idTipoAccion = ta.idTipoAccion
            JOIN Usuarios u ON hc.idUsuario = u.idUsuario
            JOIN Elementos e ON hn.idElemento = e.idElemento
            join tipoelemento t on t.idTipoElemento = e.idTipoElemento
            join notebooks n on e.idElemento = n.idElemento
            where t.elemento in ('Notebook')
            and n.idElemento = @unidNotebook
            ORDER BY hc.fechaCambio DESC;",

        (historialNotebook, Usuarios, TipoAccion, HistorialCambios) => new HistorialNotebookGestionDTO
        {
            IdHistorialNotebook = historialNotebook.IdHistorialCambio,
            Usuario = Usuarios.Nombre,
            AccionRealizada = TipoAccion.Accion,
            FechaCambio = HistorialCambios.FechaCambio
        },
        parameters,
        splitOn: "IdHistorialCambio,Nombre,Accion,FechaCambio"
        );
    }
}
