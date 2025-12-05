using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using Core.Entities.Aggregates.Usuario;

namespace CapaNegocio;

public class UsuariosBajasCN
{
    private readonly IRepoUsuarios repoUsuarios;
    private readonly IMapperUsuariosBajas _mapperUsuariosBajas;

    public UsuariosBajasCN(IMapperUsuariosBajas mapperUsuariosBajas, IRepoUsuarios repoUsuarios)
    {
        _mapperUsuariosBajas = mapperUsuariosBajas;
        this.repoUsuarios = repoUsuarios;
    }

    public IEnumerable<UsuariosBajasDTO> GetAllDTO()
    {
        return _mapperUsuariosBajas.GetAllDTO();
    }

    public void HabilitarUsuario(int idUsuario)
    {
        Usuarios? usuarios = repoUsuarios.GetById(idUsuario);

        if (usuarios == null)
        {
            throw new Exception("El elemento no existe.");
        }

        if (usuarios.Habilitado)
        {
            throw new Exception("El elemento ya esta habilitado.");
        }

        usuarios.Habilitado = true;
        usuarios.FechaBaja = null;

        repoUsuarios.Update(usuarios);
    }
}
