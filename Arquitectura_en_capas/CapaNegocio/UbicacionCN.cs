using CapaDatos.Interfaces;
using CapaEntidad;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class UbicacionCN
{
    private readonly IRepoUbicacion repoUbicacion;

    public UbicacionCN(IRepoUbicacion repoUbicacion)
    {
        this.repoUbicacion = repoUbicacion;
    }

    #region CREATE UBICACION
    public void Insert(Ubicacion ubicacion)
    {
        if(string.IsNullOrEmpty(ubicacion.NombreUbicacion))
        {
            throw new Exception("La ubicacion no puede estar vacío o nulo");
        }

        if (ubicacion.NombreUbicacion.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(ubicacion.NombreUbicacion, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        if (repoUbicacion.GetByUbicacion(ubicacion.NombreUbicacion) != null)
        {
            throw new Exception("Ya existe una ubicacion con ese nombre, por favor elija uno nuevo");
        }
        repoUbicacion.Insert(ubicacion);
    }
    #endregion

    #region UPDATE UBICACION
    public void Update(Ubicacion ubicacionNEW)
    {
        if (string.IsNullOrEmpty(ubicacionNEW.NombreUbicacion))
        {
            throw new Exception("La ubicacion no puede estar vacío o nulo");
        }

        if (ubicacionNEW.NombreUbicacion.Length >= 40)
        {
            throw new Exception("El tipo de elemento no puede tener más de 40 caracteres");
        }

        if (!Regex.IsMatch(ubicacionNEW.NombreUbicacion, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El tipo del elemento contiene caracteres inválidos.");
        }

        Ubicacion? ubicacionOLD = repoUbicacion.GetById(ubicacionNEW.IdUbicacion);

        if (ubicacionOLD is null)
        {
            throw new Exception("El tipo de elemento no existe");
        }

        if (!ubicacionOLD.NombreUbicacion.Equals(ubicacionNEW.NombreUbicacion, StringComparison.OrdinalIgnoreCase))
        {
            Ubicacion? existente = repoUbicacion.GetByUbicacion(ubicacionNEW.NombreUbicacion);

            if (existente != null)
            {
                throw new Exception("Ya existe otro tipo de elemento con el mismo nombre.");
            }
        }

        repoUbicacion.Update(ubicacionNEW);
    }
    #endregion

    #region READ UBICACION
    public IEnumerable<Ubicacion> GetAll()
    {
        return repoUbicacion.GetAll();
    }

    public Ubicacion GetId(int idUbicacion)
    {
        Ubicacion? ubicacion = repoUbicacion.GetById(idUbicacion);
        if (ubicacion is null)
        {
            throw new Exception("No se encontro la ubicacion");
        }
        return ubicacion;
    }
    #endregion
}
