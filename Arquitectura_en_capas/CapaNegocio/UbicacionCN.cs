using CapaDatos.Interfaces;
using CapaDatos.Repos;
using CapaEntidad;

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

        if(repoUbicacion.GetIdByUbicacion(ubicacion.NombreUbicacion) != null)
        {
            throw new Exception("Ya existe una ubicacion con ese nombre, por favor elija uno nuevo");
        }
        repoUbicacion.Insert(ubicacion);
    }
    #endregion

    #region UPDATE UBICACION
    public void Update(Ubicacion ubicacionNEW)
    {
        if (string.IsNullOrWhiteSpace(ubicacionNEW.NombreUbicacion))
        {
            throw new Exception("La ubicacion no puede estar vacío o nulo");
        }

        Ubicacion? ubicacionOLD = repoUbicacion.GetById(ubicacionNEW.IdUbicacion);

        if (ubicacionOLD is null)
        {
            throw new Exception("El tipo de elemento no existe");
        }

        if (ubicacionOLD.NombreUbicacion != ubicacionNEW.NombreUbicacion && repoUbicacion.GetIdByUbicacion(ubicacionNEW.NombreUbicacion) != null)
        {
            throw new Exception("Ya existe otro tipo de elemento con el mismo nombre");
        }

        repoUbicacion.Update(ubicacionNEW);
    }
    #endregion

    #region READ UBICACION
    public IEnumerable<Ubicacion> GetAll()
    {
        return repoUbicacion.GetAll();
    }
    #endregion
}
