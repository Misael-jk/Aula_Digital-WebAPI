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
    public Elemento? ObtenerElementoPorID(int idElemento)
    {
        return uow.RepoElemento.GetById(idElemento);
    }

    public TipoElemento? ObtenerTipoElementoPorID(int idTipoElemento)
    {
        return uow.RepoTipoElemento.GetById(idTipoElemento);
    }

    public VariantesElemento? ObtenerVariantePorID(int idVariante)
    {
        return uow.RepoVarianteElemento.GetById(idVariante);
    }

    public void HabilitarElemento(int idElemento, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            Elemento? elemento = uow.RepoElemento.GetById(idElemento);

            if (elemento == null)
                throw new Exception("El elemento no existe.");

            if (elemento.Habilitado)
                throw new Exception("El elemento ya esta habilitado.");

            elemento.Habilitado = true;
            elemento.IdEstadoMantenimiento = 1;
            elemento.FechaBaja = null;

            uow.RepoElemento.Update(elemento);

            HistorialCambios historialCambios = new HistorialCambios
            {
                IdUsuario = idUsuario,
                IdTipoAccion = 2,
                Descripcion = $"Se habilito el elemento con numero de serie {elemento.NumeroSerie}",
                FechaCambio = DateTime.Now,
                Motivo = null
            };
            uow.RepoHistorialCambio.Insert(historialCambios);

            uow.RepoHistorialElementos.Insert(new HistorialElementos
            {
                IdHistorialCambio = historialCambios.IdHistorialCambio,
                IdElementos = idElemento
            });

            uow.Commit();
        }
        catch(Exception ex)
        {
            uow.Rollback();
            throw new Exception($"Error al habilitar el elemento: {ex.Message}");
        }
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
