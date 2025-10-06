using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;

namespace CapaNegocio;

public class NotebooksCN
{
    private readonly IRepoNotebooks repoNotebooks;
    private readonly IRepoCarritos repoCarritos;
    private readonly IRepoModelo repoModelo;
    private readonly IRepoUbicacion repoUbicacion;
    private readonly IMapperNotebooks mapperNotebook;

    public NotebooksCN(IRepoNotebooks repoNotebooks, IRepoCarritos repoCarritos, IRepoModelo repoModelo, IRepoUbicacion repoUbicacion, IMapperNotebooks mapperNotebooks)
    {
        this.repoNotebooks = repoNotebooks;
        this.repoCarritos = repoCarritos;
        this.repoModelo = repoModelo;
        this.repoUbicacion = repoUbicacion;
        this.mapperNotebook = mapperNotebooks;
    }

    #region READ
    public IEnumerable<NotebooksDTO> GetAll()
    {
        return mapperNotebook.GetAllDTO();
    }
    #endregion

    #region Alta
    public void CrearNotebook(Notebooks notebookNEW)
    {
        if (string.IsNullOrWhiteSpace(notebookNEW.NumeroSerie))
        {
            throw new Exception("La notebook debe tener número de serie.");
        }

        if (string.IsNullOrWhiteSpace(notebookNEW.CodigoBarra))
        {
            throw new Exception("La notebook debe tener código de barra.");
        }

        if (string.IsNullOrWhiteSpace(notebookNEW.Patrimonio))
        {
            throw new Exception("La notebook debe tener patrimonio.");
        }

        Notebooks? nroSerieHabilitado = repoNotebooks.GetByNumeroSerie(notebookNEW.NumeroSerie);

        if (nroSerieHabilitado != null)
        {
            if (nroSerieHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese numero de serie ya existe y está habilitado.");
            }
            else
            {
                throw new Exception("La notebook con ese numero de serie ya existe pero está deshabilitado, por favor habilitelo antes de crear uno nuevo.");
            }
        }

        Notebooks? codigoBarraHabilitado = repoNotebooks.GetByCodigoBarra(notebookNEW.CodigoBarra);

        if (codigoBarraHabilitado != null)
        {
            if (codigoBarraHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese codigo de barra ya existe y está habilitado.");
            }
            else
            {
                throw new Exception("La notebook con ese codigo ya existe pero está deshabilitado, por favor habilitelo antes de crear uno nuevo.");
            }
        }

        Notebooks? patrimonioHabilitado = repoNotebooks.GetByPatrimonio(notebookNEW.Patrimonio);

        if(patrimonioHabilitado != null)
        {
            if(patrimonioHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese patrimonio ya existe y esta habilitado");
            }
            else
            {
                throw new Exception("La notebook con ese patrimonio ya existe pero esta deshabilitado, por favor habilitelo antes de crear uno nuevo");
            }
        }

        if (notebookNEW.IdEstadoMantenimiento != 1)
        {
            throw new Exception("El estado del elemento debe ser 'Disponible' al momento de crearlo");
        }

        if(notebookNEW.IdTipoElemento != 1)
        {
            throw new Exception("El tipo de elemento debe ser 'Notebook'");
        }

        if (repoUbicacion.GetById(notebookNEW.IdUbicacion) == null)
        {
            throw new Exception("Ubicacion del elemento invalida");
        }

        if (repoModelo.GetById(notebookNEW.IdModelo) == null)
        {
            throw new Exception("Modelo del elemento invalida");
        }

        if (repoModelo.GetByTipo(notebookNEW.IdTipoElemento) == null)
        {
            throw new Exception("El modelo debe ser correspondiente al tipo de elemento");
        }


        notebookNEW.IdEstadoMantenimiento = 1;
        notebookNEW.IdCarrito = null;
        notebookNEW.PosicionCarrito = null;
        notebookNEW.Habilitado = true;
        notebookNEW.FechaBaja = null;

        repoNotebooks.Insert(notebookNEW);
    }
    #endregion

    #region Actualización
    public void ActualizarNotebook(Notebooks notebookNEW)
    {
        if(string.IsNullOrWhiteSpace(notebookNEW.NumeroSerie))
        {
            throw new Exception("La notebook debe tener número de serie.");
        }

        if (string.IsNullOrWhiteSpace(notebookNEW.CodigoBarra))
        {
            throw new Exception("La notebook debe tener código de barra.");
        }

        if (string.IsNullOrWhiteSpace(notebookNEW.Patrimonio))
        {
            throw new Exception("La notebook debe tener patrimonio.");
        }

        Notebooks? notebookOLD = repoNotebooks.GetById(notebookNEW.IdElemento);

        if (notebookOLD == null)
        {
            throw new Exception("El elemento no existe");
        }

        if (repoUbicacion.GetById(notebookNEW.IdUbicacion) == null)
        {
            throw new Exception("Ubicacion del elemento invalida");
        }

        if (repoModelo.GetById(notebookNEW.IdModelo) == null)
        {
            throw new Exception("Modelo del elemento invalida");
        }

        if (repoNotebooks.GetByPatrimonio(notebookNEW.Patrimonio) == null)
        {
            throw new Exception("El patrimonio no existe en otro elemento, por favor elija uno existente");
        }

        if (notebookOLD.IdTipoElemento != notebookNEW.IdTipoElemento)
        {
            throw new Exception("No se puede cambiar el tipo de elemento");
        }

        if (repoModelo.GetByTipo(notebookNEW.IdTipoElemento) == null)
        {
            throw new Exception("El modelo debe ser correspondiente al tipo de elemento");
        }

        Notebooks? patrimonioHabilitado = repoNotebooks.GetByPatrimonio(notebookNEW.Patrimonio);

        if (notebookOLD.Patrimonio != notebookNEW.Patrimonio && patrimonioHabilitado != null)
        {
            if (patrimonioHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro elemento con el mismo patrimonio.");
            }
            else
            {
                throw new Exception("El elemento ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }

        Notebooks? nroSerieHabilitado = repoNotebooks.GetByNumeroSerie(notebookNEW.NumeroSerie);

        if (notebookOLD.NumeroSerie != notebookNEW.NumeroSerie && nroSerieHabilitado != null)
        {
            if (nroSerieHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro elemento con el mismo numero de serie.");
            }
            else
            {
                throw new Exception("El elemento ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }

        Notebooks? codigoBarraHabilitado = repoNotebooks.GetByCodigoBarra(notebookNEW.CodigoBarra);

        if (notebookOLD.CodigoBarra != notebookNEW.CodigoBarra && codigoBarraHabilitado != null)
        {
            if (codigoBarraHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro elemento con el mismo código de barras.");
            }
            else
            {
                throw new Exception("El elemento ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }

        if (notebookNEW.IdEstadoMantenimiento == 2 && notebookOLD.IdEstadoMantenimiento != 2)
        {
            throw new Exception("No se puede cambiar el estado a 'En Prestamo' por que no se hiso un prestamo");
        }

        if (notebookNEW.IdEstadoMantenimiento != 2 && notebookOLD.IdEstadoMantenimiento == 2)
        {
            throw new Exception("No se puede cambiar el estado de un elemento en prestamo sin terminar su devolucion");
        }


        repoNotebooks.Update(notebookNEW);
    }
    #endregion
}
