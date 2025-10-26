using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDTOs;
using CapaEntidad;

namespace CapaNegocio;

public class ElementosBajasCN
{
    private readonly IUowElementos uow;
    private readonly IMapperElementosBajas mapperElementosBajas;

    public ElementosBajasCN(IMapperElementosBajas mapperElementosBajas, IUowElementos uowElementos)
    {
        uow = uowElementos;
        this.mapperElementosBajas = mapperElementosBajas;
    }

    public IEnumerable<ElementoBajasDTO> GetAllElementos()
    {
        return mapperElementosBajas.GetAllDTO();
    }

    //public IEnumerable<ElementoBajasDTO> GetElementosByTipo(int idTipoElemento)
    //{
    //    return mapperElementosBajas.GetByTipo(idTipoElemento);
    //}

    //public IEnumerable<ElementoBajasDTO> GetElementosByEstado(int idEstadoElemento)
    //{
    //    return mapperElementosBajas.GetByEstado(idEstadoElemento);
    //}

    public void HabilitarElemento(int idElemento, int idUsuario)
    {

        Elemento? elemento = uow.RepoElemento.GetById(idElemento);

        if (elemento == null)
            throw new Exception("El elemento no existe.");

        if (elemento.Habilitado)
            throw new Exception("El elemento ya esta habilitado.");

        elemento.Habilitado = true;
        elemento.IdEstadoMantenimiento = 1;
        elemento.FechaBaja = null;

        uow.RepoElemento.Update(elemento);

    }

    #region DELETE ELEMENTO 
    public void EliminarElemento(int idElemento)
    {
        Elemento? elementoOLD = uow.RepoElemento.GetById(idElemento);

        if (elementoOLD == null)
        {
            throw new Exception("El elemento no existe.");
        }

        if (elementoOLD.IdEstadoMantenimiento == 2)
        {
            throw new Exception("No se puede eliminar un elemento que está en prestamo");
        }

        if (elementoOLD.Habilitado)
        {
            throw new Exception("El elemento debe estar deshabilitado antes de eliminarlo definitivamente.");
        }


        uow.RepoElemento.Delete(idElemento);
    }
    #endregion
}
