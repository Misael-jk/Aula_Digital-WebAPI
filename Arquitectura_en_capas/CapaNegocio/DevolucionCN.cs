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
    private readonly IMapperDevolucionDetalle mapperDevolucionDetalle;

    public DevolucionCN(IMapperDevoluciones mapperDevolucion, IUowDevolucion uow, IMapperDevolucionDetalle mapperDevolucionDetalle)
    {
        this.mapperDevolucion = mapperDevolucion;
        this.uow = uow;
        this.mapperDevolucionDetalle = mapperDevolucionDetalle;
    }

    public IEnumerable<DevolucionesDTO> ObtenerElementos()
    {
        return mapperDevolucion.GetAllDTO();
    }

    #region INSERT DEVOLUCION
    public void CrearDevolucion(Devolucion devolucionNEW, IEnumerable<int> idsElementos, IEnumerable<string>? Observaciones, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            if (uow.RepoDevolucion.GetByPrestamo(devolucionNEW.IdPrestamo) != null)
                throw new Exception("Ya existe una devolución asociada a este prestamo.");

            Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucionNEW.IdPrestamo);
            if (prestamo == null)
                throw new Exception("No existe el préstamo asociado.");

            uow.RepoDevolucion.Insert(devolucionNEW);

            InsertDevolucionDetalle(devolucionNEW, idsElementos, Observaciones, idUsuario);

            int totalPrestados = uow.RepoPrestamoDetalle.GetCountByPrestamo(prestamo.IdPrestamo);
            int totalDevueltos = uow.RepoDevolucionDetalle.CountByDevolucion(devolucionNEW.IdDevolucion);

            // estado del préstamo
            uow.RepoPrestamos.UpdateEstado(
                prestamo.IdPrestamo,
                totalPrestados == totalDevueltos ? 2 : 4
            );

            if (prestamo.IdCarrito != null)
            {
                int idCarrito = prestamo.IdCarrito.Value;

                // Obtener SOLO las notebooks que pertenecen al carrito y están en este préstamo
                var notebooksDelCarritoEnEstePrestamo =
                    uow.RepoPrestamoDetalle.GetIdsElementosPorPrestamoFiltrandoCarrito(prestamo.IdPrestamo, idCarrito);

                // Obtener cuáles siguen pendientes dentro de este préstamo
                var pendientes = notebooksDelCarritoEnEstePrestamo
                                    .Except(uow.RepoDevolucionDetalle.GetIdsElementosByIdDevolucion(devolucionNEW.IdDevolucion))
                                    .ToList();

                if (!pendientes.Any())
                {
                    uow.RepoCarritos.UpdateDisponible(idCarrito, 1);

                    HistorialCambios historial = new HistorialCambios
                    {
                        IdTipoAccion = 6,
                        IdUsuario = idUsuario,
                        FechaCambio = DateTime.Now,
                        Descripcion = $"El carrito {idCarrito} fue liberado."
                    };

                    uow.RepoHistorialCambio.Insert(historial);

                    uow.RepoHistorialCarrito.Insert(new HistorialCarritos
                    {
                        IdHistorialCambio = historial.IdHistorialCambio,
                        IdCarrito = idCarrito
                    });
                }
            }

            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new Exception("Hubo un error al crear la devolución: " + ex.Message);
        }
    }
    #endregion

    #region DEVOLUCION PARCIAL
    public void CrearDevolucionParcial(int idPrestamo, IEnumerable<int> idsElementos, IEnumerable<string>? Observaciones, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            Devolucion? devolucion = uow.RepoDevolucion.GetByPrestamo(idPrestamo);
            if (devolucion == null)
                throw new Exception("No existe una devolución asociada al préstamo.");

            Prestamos? prestamo = uow.RepoPrestamos.GetById(idPrestamo);
            if (prestamo == null)
                throw new Exception("No existe el préstamo asociado.");

            InsertDevolucionDetalle(devolucion, idsElementos, Observaciones, idUsuario);

            int totalPrestados = uow.RepoPrestamoDetalle.GetCountByPrestamo(prestamo.IdPrestamo);
            int totalDevueltos = uow.RepoDevolucionDetalle.CountByDevolucion(devolucion.IdDevolucion);

            // actualizar estado del préstamo
            uow.RepoPrestamos.UpdateEstado(
                idPrestamo,
                totalPrestados == totalDevueltos ? 2 : 4
            );

            // --------------------------------
            //       LÓGICA DE CARRITO
            // --------------------------------
            if (prestamo.IdCarrito != null)
            {
                int idCarrito = prestamo.IdCarrito.Value;

                // notebooks del carrito pertenecientes a este préstamo
                var notebooksDelCarritoEnEstePrestamo =
                    uow.RepoPrestamoDetalle.GetIdsElementosPorPrestamoFiltrandoCarrito(idPrestamo, idCarrito);

                // devueltas
                var devueltas = uow.RepoDevolucionDetalle.GetIdsElementosByIdDevolucion(devolucion.IdDevolucion);

                var pendientes = notebooksDelCarritoEnEstePrestamo.Except(devueltas).ToList();

                if (!pendientes.Any())
                {
                    uow.RepoCarritos.UpdateDisponible(idCarrito, 1);

                    HistorialCambios historial = new HistorialCambios
                    {
                        IdTipoAccion = 6,
                        IdUsuario = idUsuario,
                        FechaCambio = DateTime.Now,
                        Descripcion = $"El carrito {idCarrito} fue liberado."
                    };

                    uow.RepoHistorialCambio.Insert(historial);

                    uow.RepoHistorialCarrito.Insert(new HistorialCarritos
                    {
                        IdHistorialCambio = historial.IdHistorialCambio,
                        IdCarrito = idCarrito
                    });
                }
            }

            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }

    #endregion

    #region AUX INSERT DEVOLUCION DETALLE, HISTORICOS y ANOMALIAS
    private void InsertDevolucionDetalle(Devolucion devolucion, IEnumerable<int> idsElementos, IEnumerable<string>? Observaciones, int idUsuario)
    {
        if (idsElementos == null)
            throw new ArgumentNullException(nameof(idsElementos));

        int cont = 0;

        foreach (int idElemento in idsElementos)
        {
            string? obs = Observaciones?.ElementAtOrDefault(cont);

            DevolucionDetalle? devolucionDetalle = uow.RepoDevolucionDetalle.Exists(devolucion.IdDevolucion, idElemento);

            if (devolucionDetalle != null)
                throw new Exception($"El elemento {idElemento} ya fue devuelto previamente.");

            uow.RepoDevolucionDetalle.Insert(new DevolucionDetalle
            {
                IdDevolucion = devolucion.IdDevolucion,
                IdElemento = idElemento,
                Observaciones = obs
            });

            uow.RepoElementos.UpdateEstado(idElemento, 1);

            Elemento? elemento = uow.RepoElementos.GetById(idElemento);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 6,
                FechaCambio = DateTime.Now,
                IdUsuario = idUsuario,
                Descripcion = elemento?.IdTipoElemento == 1
                    ? $"El Elemento con el número de serie {elemento.NumeroSerie} fue devuelto"
                    : $"La notebook {idElemento} fue devuelta",
                Motivo = obs
            };

            uow.RepoHistorialCambio.Insert(historial);

            if (elemento != null && elemento.IdTipoElemento == 1)
            {
                uow.RepoHistorialNotebook.Insert(new HistorialNotebooks { IdHistorialCambio = historial.IdHistorialCambio, IdNotebook = idElemento });
            }
            else
            {
                uow.RepoHistorialElementos.Insert(new HistorialElementos { IdHistorialCambio = historial.IdHistorialCambio, IdElementos = idElemento });
            }

            cont++;
        }
    }

    #endregion

    #region Actualizar los estados de mantenimiento de los elementos devueltos
    //private void ActualizarEstadoPrestamoYDevolucion(int idPrestamo, int idDevolucion, bool huboAnomaliasEnEsteLote)
    //{
    //    int totalPrestados = uow.RepoPrestamoDetalle.GetCountByPrestamo(idPrestamo);
    //    int totalDevueltos = uow.RepoDevolucionDetalle.CountByDevolucion(idDevolucion); 

    //    bool hayAnomaliasGlobales = false;
    //    try
    //    {
    //        // Si tienes RepoDevolucionAnomalia, podrías hacer:
    //        var hayAnomaliasGlobales = uow.RepoDevolucionAnomalia.ExistsByDevolucion(idDevolucion);
    //        // Aquí lo dejamos en false para no depender obligatoriamente del repo.
    //        hayAnomaliasGlobales = false;
    //    }
    //    catch { hayAnomaliasGlobales = false; }

    //    bool tieneAnomalias = huboAnomaliasEnEsteLote || hayAnomaliasGlobales;

    //    if (totalPrestados == totalDevueltos)
    //    {
    //        uow.RepoPrestamos.UpdateEstado(idPrestamo, tieneAnomalias ? 4 : 3); // 4 = con anomalías, 3 = sin problemas

    //        uow.RepoDevolucion.SetParcial?.Invoke(idDevolucion, false); // si tu UoW/Repo implementa SetParcial
    //    }
    //    else
    //    {
    //        uow.RepoPrestamos.UpdateEstado(idPrestamo, 5); // 5 = Parcial (elige el id que uses)
    //        uow.RepoDevolucion.SetParcial?.Invoke(idDevolucion, true);
    //    }
    //}
    #endregion

    #region VALIDACION DE LA DEVOLUCION
    public void ValidarDevolucion(Devolucion devolucion, IEnumerable<int> idsElementos)
    {
        Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucion.IdPrestamo);

        #region DEVOLUCION
        if (devolucion == null)
        {
            throw new ArgumentNullException(nameof(devolucion));
        }
        #endregion

        #region USUARIO
        if (uow.RepoUsuarios.GetById(devolucion.IdUsuario) == null)
        {
            throw new Exception("El usuario no existe.");
        }
        #endregion

        #region ELEMENTOS
        if (idsElementos == null || !idsElementos.Any())
        {
            throw new Exception("Debe seleccionar al menos un elemento para devolver.");
        }

        foreach (int idElemento in idsElementos)
        {
            if (uow.RepoElementos.GetById(idElemento) == null)
            {
                throw new Exception($"El elemento {idElemento} no existe.");
            }

            if (uow.RepoElementos.GetDisponible(idElemento))
            {
                throw new Exception($"El elemento {idElemento} no debe estar disponible para poder devolverlo.");
            }

            if (!uow.RepoPrestamoDetalle.PerteneceAlPrestamo(devolucion.IdPrestamo, idElemento))
            {
                throw new Exception($"El elemento {idElemento} no pertenece a este préstamo.");
            }

            if (uow.RepoDevolucionDetalle.Exists(devolucion.IdDevolucion, idElemento) != null)
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

    #endregion

    #region OBTENER DEVOLUCION POR ID
    public Devolucion? ObtenerDevolucionPorIdPrestamo(int idPrestamo)
    {
        return uow.RepoDevolucion.GetByPrestamo(idPrestamo);
    }
    #endregion

    #region OBTENER DEVOLUCION DETALLE
    public IEnumerable<DevolucionDetalleDTO> ObtenerDevolucionDetallePorID(int idDevolucion, int? idCarrito)
    {
        return mapperDevolucionDetalle.GetByIdDTO(idDevolucion, idCarrito);
    }

    #region OBTENER LISTADO DE IDs DE ELEMENTOS EN DEV. DETALLE
    public List<int> ObtenerIDsElementosEnDev(int idDevolucion)
    {
        return uow.RepoDevolucionDetalle.GetIdsElementosByIdDevolucion(idDevolucion);
    }
    #endregion
    #endregion

    #region OBTENER CARRO
    public Carritos? ObtenerCarroPorID(int idCarrito)
    {
        return uow.RepoCarritos.GetById(idCarrito);
    }
    #endregion
}