using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
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
            "GetUsuariosBajasDTO",
            (usuario, rol) => new UsuariosBajasDTO
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = rol.Rol,
                Habilitado = usuario.Habilitado,
                FechaBaja = usuario.FechaBaja
            },
            commandType: CommandType.StoredProcedure,
            splitOn: "NombreUsuario,RolNombre"
        ).ToList();
    }
}
