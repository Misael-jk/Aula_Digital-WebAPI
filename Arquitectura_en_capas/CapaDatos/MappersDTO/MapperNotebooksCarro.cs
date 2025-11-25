using CapaDatos.InterfacesDTO;
using CapaDTOs;
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
    public class MapperNotebooksCarro : RepoBase, IMapperNotebooksCarro
    {
        public MapperNotebooksCarro(IDbConnection conexion, IDbTransaction? transaction = null) : base(conexion, transaction)
        {
        }

        public IEnumerable<NotebooksCarroDTO> GetAll(int idCarrito)
        {
            var parameters = new DynamicParameters();
            parameters.Add("unIdCarrito", idCarrito);

            return Conexion.Query<Notebooks, Modelos, NotebooksCarroDTO>(
            @"SELECT 
            n.equipo AS Equipo,
            e.numeroSerie AS NumeroSerie,
            n.posicionCarrito AS PosicionCarrito,
            m.modelo AS NombreModelo
            FROM Notebooks n
            INNER JOIN Elementos e ON n.idElemento = e.idElemento
            INNER JOIN TipoElemento te ON e.idTipoElemento = te.idTipoElemento
            LEFT JOIN Modelo m ON e.idModelo = m.idModelo
            WHERE e.idTipoElemento = 1
            AND e.idEstadoMantenimiento = 1
            AND n.idCarrito = @unIdCarrito;",
            (Notebooks, modelo) => new NotebooksCarroDTO
            {
                Equipo = Notebooks.Equipo,
                NumeroSerie = Notebooks.NumeroSerie,
                PosicionCarrito = Notebooks.PosicionCarrito,
                Modelo = modelo.NombreModelo,
            },
            parameters,
            splitOn: "Equipo,NombreModelo");
        }
    }
}
