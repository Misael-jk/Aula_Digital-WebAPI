using Dapper;
using CapaDatos.InterfacesDTO;
using System.Data;
using CapaDTOs;
using CapaEntidad;

namespace CapaDatos.MappersDTO;

public class MapperUsuarios : RepoBase, IMapperUsuarios
{
    public MapperUsuarios(IDbConnection conexion) 
        : base(conexion) 
    {
    }

    public IEnumerable<UsuariosDTO> GetAllDTO()
    {
        return Conexion.Query<Usuarios, Roles, UsuariosDTO>(
            "select * from View_GetUsuarioDTO",
            (usuario, rol) => new UsuariosDTO
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = rol.Rol,
                FotoPerfil = usuario.FotoPerfil
            },
            splitOn: "Rol"
        ).ToList();
    }

    public UsuariosDTO? GetById(int idUsuario)
    {
        var parametros = new DynamicParameters();
        parametros.Add("@idUsuario", idUsuario, DbType.Int32, ParameterDirection.Input);

        return Conexion.Query<Usuarios, Roles, UsuariosDTO>(
            "GetUsuarioByIdDTO",
            (usuario, rol) => new UsuariosDTO
            {
                IdUsuario = usuario.IdUsuario,
                Usuario = usuario.Usuario,
                Password = usuario.Password,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = rol.Rol,
                Email = usuario.Email,
                FotoPerfil = usuario.FotoPerfil
            },
            parametros,
            commandType: CommandType.StoredProcedure,
            splitOn: "Rol"
        ).FirstOrDefault();
    }
}
