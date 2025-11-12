using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class DocentesCN
{
    private readonly IRepoDocentes _repoDocente;
    private readonly IMapperDocentes mapperDocentes;
    public DocentesCN(IRepoDocentes repoDocente, IMapperDocentes mapperDocentes)
    {
        _repoDocente = repoDocente;
        this.mapperDocentes = mapperDocentes;
    }

    #region READ DOCENTE
    public IEnumerable<DocentesDTO> MostrarDocente()
    {
        return mapperDocentes.GetAllDTO();
    }

    public Docentes? GetById(int idDocente)
    {
        return _repoDocente.GetById(idDocente);
    }

    public Docentes? ObtenerPorNombre(string nombre)
    {
        return _repoDocente.GetByNombre(nombre);
    }

    public IEnumerable<string> FiltroNombre(string nombre, int limit)
    {
        return _repoDocente.GetFiltroNombre(nombre, limit);
    }
    #endregion

    #region INSERT DOCENTE
    public void CrearDocente(Docentes docenteNEW)
    {
        //docenteNEW.Nombre = docenteNEW.Nombre.Trim();
        //docenteNEW.Apellido = docenteNEW.Apellido.Trim();
        //docenteNEW.Dni = docenteNEW.Dni.Trim();
        //docenteNEW.Email = docenteNEW.Email.Trim().ToLower();

        ValidarDocente(docenteNEW);

        if (_repoDocente.GetByDni(docenteNEW.Dni) is not null)
        {
            throw new Exception("Ya existe un docente ese dni");
        }

        if (_repoDocente.GetByEmail(docenteNEW.Email) != null)
        {
            throw new Exception("Ya existe un docente ese email");
        }

        _repoDocente.Insert(docenteNEW);
    }
    #endregion

    #region UPDATE DOCENTE
    public void ActualizarDocente(Docentes docenteNEW)
    {
        //docenteNEW.Nombre = docenteNEW.Nombre.Trim();
        //docenteNEW.Apellido = docenteNEW.Apellido.Trim();
        //docenteNEW.Dni = docenteNEW.Dni.Trim();
        //docenteNEW.Email = docenteNEW.Email.Trim().ToLower();

        ValidarDocente(docenteNEW);

        Docentes? docentesOLD = _repoDocente.GetById(docenteNEW.IdDocente);

        if (docentesOLD == null)
        {
            throw new Exception("El docente que eligio no esta registrado en el sistema");
        }

        if (docentesOLD.Dni != docenteNEW.Dni && _repoDocente.GetByDni(docenteNEW.Dni) != null)
        {
            throw new Exception("Ya existe otro docente con el mismo DNI");
        }

        if (docentesOLD.Email != docenteNEW.Email && _repoDocente.GetByEmail(docenteNEW.Email) != null)
        {
            throw new Exception("Ya existe otro docente con el mismo Email");
        }

        _repoDocente.Update(docenteNEW);
    }
    #endregion

    #region DESHABILITAR DOCENTE
    public void DeshabilitarDocente(int idDocente)
    {
        Docentes? docente = _repoDocente.GetById(idDocente);

        ValidarDocente(docente!);

        if (docente == null)
        {
            throw new Exception("No se encontro el docente");
        }

        if (_repoDocente.ExistsPrestamo(idDocente))
        {
            throw new Exception("No se puede deshabilitar, el docente porque tiene prestamos activos");
        }

        _repoDocente.Deshabilitar(idDocente, false);
    }
    #endregion

    #region DELETE DOCENTE
    public void EliminarDocente(int idDocente)
    {
        Docentes? docentesOLD = _repoDocente.GetById(idDocente);

        if (docentesOLD == null)
        {
            throw new Exception("No se encontro el docente");
        }

        _repoDocente.Delete(idDocente);
    }
    #endregion

    #region Validaciones 

    private void ValidarDocente(Docentes docentes)
    {
        if (string.IsNullOrWhiteSpace(docentes.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (docentes.Nombre.Length > 20)
        {
            throw new ValidationException("El nombre del docente no puede superar los 20 caracteres.");
        }

        if (!Regex.IsMatch(docentes.Nombre, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s\-]+$"))
        {
            throw new ValidationException("El nombre del docente contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(docentes.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
        }

        if (docentes.Apellido.Length > 20)
        {
            throw new ValidationException("El apellido del docente no puede superar los 20 caracteres.");
        }

        if (!Regex.IsMatch(docentes.Apellido, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s\-]+$"))
        {
            throw new ValidationException("El apellido del docente contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(docentes.Dni))
        {
            throw new Exception("No completo la casilla del DNI");
        }

        if (!Regex.IsMatch(docentes.Dni, @"^\d{8}$"))
        {
            throw new ValidationException("El DNI debe tener exactamente 8 dígitos numéricos.");
        }

        if (string.IsNullOrWhiteSpace(docentes.Email))
        {
            throw new Exception("No completo la casilla del email");
        }

        if (docentes.Email.Length > 70)
        {
            throw new ValidationException("El email del docente no puede superar los 70 caracteres.");
        }

        try
        {
            MailAddress mail = new MailAddress(docentes.Email);
        }
        catch (FormatException)
        {
            throw new Exception("Email invalido, intente de nuevo");
        }
    }
    #endregion
}
