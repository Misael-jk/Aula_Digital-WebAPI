using CapaDatos.InterfacesDTO;
using CapaDTOs;
using Core.Entities.Aggregates.Usuario;
using Core.Entities.Catalogos;
using Dapper;
using System.Data;

namespace CapaDatos.MappersDTO;

public class MapperUsuariosBajas : RepoBase, IMapperUsuariosBajas
{
    public MapperUsuariosBajas(IDbConnection conexion, IDbTransaction? transaction = null)
        : base(conexion, transaction)
    {
    }

    public IEnumerable<UsuariosBajasDTO> GetAllDTO()
    {
        return Conexion.Query<Usuarios, Roles, UsuariosBajasDTO>(
            "select * from View_GetUsuarioBajasDTO",
            (usuario, rol) => new UsuariosBajasDTO
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                FechaBaja = usuario.FechaBaja,
                Rol = rol.Rol
            },
            splitOn: "Rol"
        ).ToList();
    }
}
