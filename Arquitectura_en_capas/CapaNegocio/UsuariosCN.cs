using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class UsuariosCN
{
    private readonly IRepoUsuarios repoUsuarios;
    private readonly IRepoRoles repoRoles;
    private readonly IRepoHistorialCambio repoHistorialCambio;
    private readonly IMapperUsuarios mapperUsuarios;


    public UsuariosCN(IRepoUsuarios repoUsuarios, IRepoRoles repoRoles, IMapperUsuarios mapperUsuarios, IRepoHistorialCambio repoHistorialCambio)
    {
        this.repoUsuarios = repoUsuarios;
        this.repoRoles = repoRoles;
        this.mapperUsuarios = mapperUsuarios;
        this.repoHistorialCambio = repoHistorialCambio;
    }

    #region READ
    public IEnumerable<UsuariosDTO> ObtenerElementos()
    {
        return mapperUsuarios.GetAllDTO();
    }

    public Usuarios? ObtenerPorUsuario(string usuario)
    {
        return repoUsuarios.GetByUser(usuario);
    }

    public IEnumerable<Roles> ObtenerRoles()
    {
        return repoRoles.GetAll();
    }
    #endregion

    #region INSERT USUARIO
    public void CrearDocente(Usuarios usuariosNEW)
    {

        ValidarDatos(usuariosNEW);

        if (repoUsuarios.GetById(usuariosNEW.IdUsuario) != null)
        {
            throw new Exception("Ya existe un usuario con ese ID");
        }

        if(repoUsuarios.GetByUser(usuariosNEW.Usuario) != null)
        {
            throw new Exception("Ya existe un usuario con ese nombre de usuario");
        }

        if(repoRoles.GetById(usuariosNEW.IdRol) != null)
        {
            throw new Exception("El rol que eligio no existe");
        }


        repoUsuarios.Insert(usuariosNEW);
    }
    #endregion

    #region UPDATE USUARIO
    public void ActualizarUsuario(Usuarios usuariosNEW)
    {
        ValidarDatos(usuariosNEW);

        if (usuariosNEW.IdUsuario <= 0)
        {
            throw new Exception("El ID de usuario no es valido.");
        }

        Usuarios? usuariosOLD = repoUsuarios.GetById(usuariosNEW.IdUsuario);

        if (usuariosOLD == null)
        {
            throw new Exception("El usuario que eligio no esta registrado en el sistema");
        }

        if (usuariosOLD.Usuario != usuariosNEW.Usuario && repoUsuarios.GetByUser(usuariosNEW.Usuario) != null)
        {
            throw new Exception("Ya existe un usuario con ese nombre de usuario");
        }

        if (usuariosOLD.Email != usuariosNEW.Email && repoUsuarios.GetByEmail(usuariosNEW.Email) != null)
        {
            throw new Exception("Ya existe un usuario con ese email");
        }

        if (repoRoles.GetById(usuariosNEW.IdRol) == null)
        {
            throw new Exception("El rol que eligio no existe");
        }


        repoUsuarios.Update(usuariosNEW);

    }
    #endregion

    #region DELETE USUARIO
    public void EliminarUsuario(int idUsuario)
    {
        Usuarios? usuariosOLD = repoUsuarios.GetById(idUsuario);
        Roles? roles = repoRoles.GetById(usuariosOLD.IdRol);

        if (usuariosOLD == null)
        {
            throw new Exception("El usuario no existe");
        }

        if (roles?.Rol == "Administrador")
        {
            throw new Exception("No se puede eliminar un usuario con rol de administrador");
        }

        repoUsuarios.Delete(idUsuario);
    }
    #endregion

    #region LOGIN
    public Usuarios Login(string usuario, string password)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Usuario y contraseña son obligatorios");
        }

        Usuarios? user = repoUsuarios.GetByUserPass(usuario, password);

        if (user == null)
        {
            throw new Exception("Usuario o contraseña incorrectos");
        }

        return user; 
    }
    #endregion

    #region obtener por id
    public Usuarios? ObtenerID(int idUsuario)
    {
        return repoUsuarios.GetById(idUsuario);
    }
    #endregion

    #region Obtener por usuario
    public Usuarios? ObtenerUser(string User)
    {
        return repoUsuarios.GetByUser(User);
    }
    #endregion

    #region Obtener rol por ID
    public Roles? ObtenerRolPorID(int idRol)
    {
        return repoRoles.GetById(idRol);
    }
    #endregion

    #region Obtener ultima aportacion de usuario
    public HistorialCambios? ObtenerUltimaAportacion(int idUsuario)
    {
        return repoHistorialCambio.GetLastDateByUser(idUsuario);
    }
    #endregion

    #region VALIDACIONES
    private void ValidarDatos(Usuarios usuariosNEW)
    {
        if (string.IsNullOrWhiteSpace(usuariosNEW.Usuario))
        {
            throw new Exception("El nombre de usuario no puede estar vacio");
        }

        if (usuariosNEW.Usuario.Length > 40)
        {
            throw new Exception("El nombre de usuario no puede tener mas de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Usuario, @"^[A-Za-z0-9_-]+$"))
        {
            throw new Exception("El nombre de usuario solo puede contener letras, números, guiones y guiones bajos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Password))
        {
            throw new Exception("La contraseña no puede estar vacía");
        }

        if (usuariosNEW.Password.Length > 40)
        {
            throw new Exception("La contraseña no puede tener más de 40 caracteres");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (usuariosNEW.Nombre.Length > 40)
        {
            throw new Exception("El nombre no puede tener mas de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Nombre, @"^[A-Za-zÁÉÍÓÚáéíóúñÑ\s]+$"))
        {
            throw new Exception("El nombre contiene caracteres invalidos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
        }

        if (usuariosNEW.Apellido.Length > 40)
        {
            throw new Exception("El apellido no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Apellido, @"^[A-Za-zÁÉÍÓÚáéíóúñÑ\s]+$"))
        {
            throw new Exception("El apellido contiene caracteres invalidos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Email))
        {
            throw new Exception("El email es obligatorio");
        }

        if (usuariosNEW.Email.Length > 70)
        {
            throw new Exception("El email no puede tener más de 70 caracteres");
        }

        try
        {
            new MailAddress(usuariosNEW.Email);
        }
        catch
        {
            throw new Exception("Formato de email invalido");
        }

        if (repoUsuarios.GetByEmail(usuariosNEW.Email) != null)
        {
            throw new Exception("Ya existe un usuario con ese email");
        }
    }
    #endregion

}
