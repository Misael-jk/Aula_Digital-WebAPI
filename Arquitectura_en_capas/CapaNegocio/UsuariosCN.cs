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
    private readonly IMapperUsuarios mapperUsuarios;


    public UsuariosCN(IRepoUsuarios repoUsuarios, IRepoRoles repoRoles, IMapperUsuarios mapperUsuarios)
    {
        this.repoUsuarios = repoUsuarios;
        this.mapperUsuarios = mapperUsuarios;
    }

    public IEnumerable<UsuariosDTO> ObtenerElementos()
    {
        return mapperUsuarios.GetAllDTO();
    }

    #region INSERT USUARIO
    public void CrearDocente(Usuarios usuariosNEW)
    {

        ValidarDatos(usuariosNEW);

        

        repoUsuarios.Insert(usuariosNEW);
    }
    #endregion

    #region UPDATE USUARIO
    public void ActualizarUsuario(Usuarios usuariosNEW)
    {
        ValidarEmail(usuariosNEW.Email);

        if (string.IsNullOrWhiteSpace(usuariosNEW.Usuario))
        {
            throw new Exception("El Usuario esta vacio");
        }
        if (string.IsNullOrWhiteSpace(usuariosNEW.Password))
        {
            throw new Exception("La contraseña esta vacia");
        }
        if (string.IsNullOrWhiteSpace(usuariosNEW.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }
        if (string.IsNullOrWhiteSpace(usuariosNEW.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
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
        repoUsuarios.Update(usuariosNEW);

    }
    #endregion

    #region DELETE USUARIO
    public void EliminarUsuario(int idUsuario)
    {
        Usuarios? usuariosOLD = repoUsuarios.GetById(idUsuario);

        if (usuariosOLD == null)
        {
            throw new Exception("El usuario no existe");
        }

        if (usuariosOLD.IdRol == 1)
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

    #region Validaciones Privadas
    private void ValidarDatos(Usuarios usuariosNEW)
    {
        if (string.IsNullOrWhiteSpace(usuariosNEW.Usuario))
        {
            throw new Exception("El Usuario esta vacio");
        }

        if (usuariosNEW.Usuario.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Usuario, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Password))
        {
            throw new Exception("La contraseña esta vacia");
        }

        if (usuariosNEW.Password.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Password, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (usuariosNEW.Nombre.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Nombre, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
        }

        if (usuariosNEW.Apellido.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Apellido, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (repoUsuarios.GetByEmail(usuariosNEW.Email) != null)
        {
            throw new Exception("Ya existe un docente ese email");
        }

        if (string.IsNullOrWhiteSpace(usuariosNEW.Email))
        {
            throw new Exception("No completo la casilla del email");
        }

        if (usuariosNEW.Email.Length >= 70)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(usuariosNEW.Email, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        try
        {
            MailAddress mail = new MailAddress(usuariosNEW.Email);
        }
        catch (FormatException)
        {
            throw new Exception("Email invalido, intente de nuevo");
        }
    }
    #endregion

}
