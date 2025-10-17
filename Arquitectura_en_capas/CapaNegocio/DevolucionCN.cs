using CapaEntidad;
using System.Data;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaDatos.InterfaceUoW;

namespace CapaNegocio;

public class DevolucionCN
{
    private readonly IUowDevolucion uow;
    private readonly IMapperDevoluciones mapperDevolucion;

    public DevolucionCN(IMapperDevoluciones mapperDevolucion, IUowDevolucion uow)
    {
        this.mapperDevolucion = mapperDevolucion;
        this.uow = uow;
    }

    public IEnumerable<DevolucionesDTO> ObtenerElementos()
    {
        return mapperDevolucion.GetAllDTO();
    }

    #region INSERT DEVOLUCION
    public void CrearDevolucion(Devolucion devolucionNEW, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadosElemento, IEnumerable<string>? Observaciones)
    {
        try
        {
            uow.BeginTransaction();

            ValidarDevolucion(devolucionNEW, idsElementos, idsEstadosElemento);

            Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucionNEW.IdPrestamo);

            uow.RepoDevolucion.Insert(devolucionNEW);

            int cont = 0;
            foreach (int idElemento in idsElementos)
            {
                int estadoElemento = idsEstadosElemento.ElementAt(cont);
                string? obs = Observaciones?.ElementAtOrDefault(cont);

                uow.RepoDevolucionDetalle.Insert(new DevolucionDetalle
                {
                    IdDevolucion = devolucionNEW.IdDevolucion,
                    IdElemento = idElemento,
                    Observaciones = obs
                });

                uow.RepoElementos.UpdateEstado(idElemento, estadoElemento);

                Elemento? elemento = uow.RepoElementos.GetById(idElemento);

                HistorialCambios Historial = new HistorialCambios
                {
                    IdTipoAccion = 3,
                    FechaCambio = DateTime.Now,
                    IdUsuario = devolucionNEW.IdUsuario,
                    Descripcion = elemento.IdTipoElemento == 1 ?
                        $"El Elemento {idElemento} fue Devuelto" :
                        $"La notebook {idElemento} fue devuelta",
                    Motivo = obs
                };

                uow.RepoHistorialCambio.Insert(Historial);

                if (elemento.IdTipoElemento == 1)
                {
                    uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
                    {
                        IdHistorialCambio = Historial.IdHistorialCambio,
                        IdNotebook = idElemento
                    });
                }
                else
                {
                    uow.RepoHistorialElementos.Insert(new HistorialElementos
                    {
                        IdHistorialCambio = Historial.IdHistorialCambio,
                        IdElementos = idElemento
                    });
                }

                cont++;
            }

            int totalPrestados = uow.RepoPrestamoDetalle.GetCountByPrestamo(prestamo.IdPrestamo);
            int totalDevueltos = uow.RepoDevolucionDetalle.CountByDevolucion(devolucionNEW.IdDevolucion);

            if (totalPrestados == totalDevueltos)
            {
                uow.RepoPrestamos.UpdateEstado(prestamo.IdPrestamo, 3); //Devuelto Sin Problemas
            }
            else
            {
                uow.RepoPrestamos.UpdateEstado(prestamo.IdPrestamo, 4); //Devuelto Con Anomalias
            }

            if (prestamo.IdCarrito.HasValue)
            {
                IEnumerable<Elemento> carritoLibre = uow.RepoPrestamoDetalle.GetElementosPendientes(prestamo.IdPrestamo);

                if (carritoLibre.Any())
                {
                    uow.RepoCarritos.UpdateDisponible(prestamo.IdCarrito.Value, 1);


                    HistorialCambios? historial = new HistorialCambios
                    {
                        IdTipoAccion = 2,
                        IdUsuario = devolucionNEW.IdUsuario,
                        FechaCambio = DateTime.Now,
                        Descripcion = $"El carrito {prestamo.IdCarrito.Value} fue liberado.",
                        Motivo = null
                    };

                    uow.RepoHistorialCambio.Insert(historial);

                    uow.RepoHistorialCarrito.Insert(new HistorialCarritos
                    {
                        IdHistorialCambio = historial.IdHistorialCambio,
                        IdCarrito = prestamo.IdCarrito.Value
                    });
                }
            }
            uow.Commit();

        }
        catch (Exception ex)
        { 
            uow.Rollback();
            throw new Exception("Hubo un error al crear la devolucion: " + ex.Message);
        }
    }
    #endregion

    public void ValidarDevolucion(Devolucion devolucion, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadoMantenimiento)
    {
        Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucion.IdPrestamo);

        #region DEVOLUCION
        if (uow.RepoDevolucion.GetByPrestamo(devolucion.IdPrestamo) != null)
        {
            throw new Exception("El prestamo ya fue devuelto");
        }
        #endregion

        #region PRESTAMO
        if (prestamo == null)
        {
            throw new Exception("El prestamo no existe");
        }
        #endregion

        #region USUARIOS
        if (uow.RepoUsuarios.GetById(devolucion.IdUsuario) == null)
        {
            throw new Exception("El usuario no existe");
        }
        #endregion

        #region ESTADOS
        if (idsEstadoMantenimiento == null || idsEstadoMantenimiento.Count() < idsElementos.Count())
        {
            throw new Exception("Debe indicar el estado para cada elemento devuelto.");
        }
        #endregion

        #region ELEMENTOS
        if (idsElementos == null || !idsElementos.Any())
        {
            throw new Exception("Debe seleccionar al menos un elemento para devolver.");
        }

        foreach (int idElemento in idsElementos)
        {
            if(uow.RepoElementos.GetById(idElemento) == null)
            {
                throw new Exception($"El elemento {idElemento} no existe.");
            }

            if (uow.RepoElementos.GetDisponible(idElemento))
            {
                throw new Exception($"El elemento {idElemento} no debe estar disponible.");
            }

            if (!uow.RepoPrestamoDetalle.PerteneceAlPrestamo(devolucion.IdPrestamo, idElemento))
            {
                throw new Exception($"El elemento {idElemento} no pertenece a este prestamo.");
            }

            if (uow.RepoDevolucionDetalle.Exists(devolucion.IdPrestamo, idElemento))
            {
                throw new Exception($"El elemento {idElemento} ya fue devuelto previamente.");
            }

            if (idsElementos.Count(x => x == idElemento) > 1)
            {
                throw new Exception($"El elemento {idElemento} está repetido en la lista de devolución.");
            }
        }

        #endregion
    }
}
