using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperVarianteElemento : RepoBase, IMapperVarianteElemento
{
    public MapperVarianteElemento(IDbConnection conexion) : base(conexion)
    {
    }

    public IEnumerable<VarianteElementoDTO> GetAllDTO()
    {
        return Conexion.Query<VariantesElemento, TipoElemento, Modelos, VarianteElementoDTO>(
            "select * from View_GetVarianteElementoDTO",
            (variante, tipo, modelo) => new VarianteElementoDTO
            {
                IdVarianteElmento = variante.IdVarianteElemento,
                Equipo = variante.Variante,
                TipoElemento = tipo.ElementoTipo,
                Modelo = modelo.NombreModelo
            },
            splitOn: "IdVarianteElemento, ElementoTipo, NombreModelo");
            
    }
}
