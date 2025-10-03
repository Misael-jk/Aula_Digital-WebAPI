using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using System.Net.Mail;

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

    #region Mostrar Docentes
    public IEnumerable<DocentesDTO> MostrarDocente()
    {
        return mapperDocentes.GetAllDTO();
    }
    #endregion

    #region INSERT DOCENTE
    public void CrearDocente(Docentes docenteNEW)
    {
        ValidarDni(docenteNEW.Dni);

        ValidarEmail(docenteNEW.Email);

        if (string.IsNullOrWhiteSpace(docenteNEW.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(docenteNEW.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
        }

        if (_repoDocente.GetByDni(docenteNEW.Dni) != null)
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
        ValidarDni(docenteNEW.Dni);

        ValidarEmail(docenteNEW.Email);

        if (string.IsNullOrWhiteSpace(docenteNEW.Nombre))
        {
            throw new Exception("El nombre es obligatorio");
        }

        if (string.IsNullOrWhiteSpace(docenteNEW.Apellido))
        {
            throw new Exception("El apellido es obligatorio");
        }

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



    #region Validaciones Privadas

    private void ValidarDni(string dni)
    {
        if(string.IsNullOrWhiteSpace(dni))
        {
            throw new Exception("No completo la casilla del DNI");
        }

        if (dni.Length != 8)
        {
            throw new Exception("El DNI debe tener exactamente 8 dígitos");
        }
    }

    private void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new Exception("No completo la casilla del email");
        }

        try
        {
            MailAddress mail = new MailAddress(email);
        }
        catch (FormatException)
        {
            throw new Exception("Email invalido, intente de nuevo");
        }
    }
    #endregion
}
