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
    public void CrearDevolucion(Devolucion devolucionNEW, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadosElemento, IEnumerable<string>? Observaciones /*, Dictionary<int, List<int>>? anomaliasPorElemento = null*/)
    {
        try
        {
            uow.BeginTransaction();

            ValidarDevolucion(devolucionNEW, idsElementos, idsEstadosElemento);

            if (uow.RepoDevolucion.GetByPrestamo(devolucionNEW.IdPrestamo) != null)
            {
                throw new Exception("Ya existe una devolución asociada a este prestamo.");
            }

            Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucionNEW.IdPrestamo);

            uow.RepoDevolucion.Insert(devolucionNEW);

            /*bool tuvoAnomalias = */InsertDevolucionDetalle(devolucionNEW, idsElementos, idsEstadosElemento, Observaciones /*,anomaliasPorElemento*/);

            //ActualizarEstadoPrestamoYDevolucion(prestamo.IdPrestamo, devolucionNEW.IdDevolucion, tuvoAnomalias);

            // ___________________
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
            //__________________

            if (prestamo.IdCarrito.HasValue)
            {
                IEnumerable<Elemento> carritoLibre = uow.RepoPrestamoDetalle.GetElementosPendientes(prestamo.IdPrestamo);

                if (!carritoLibre.Any())
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

    #region DEVOLUCION PARCIAL
    public void CrearDevolucionParcial(int idPrestamo, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadoMantenimiento, IEnumerable<string>? Observaciones /*, Dictionary<int, List<int>>? anomaliasPorElemento = null*/)
    {
        try
        {
            uow.BeginTransaction();

            Devolucion? devolucion = uow.RepoDevolucion.GetByPrestamo(idPrestamo);

            if (devolucion == null)
            {
                throw new Exception("No existe una devolución asociada al prestamo. Debe crearla primero.");
            }

            Prestamos? prestamo = uow.RepoPrestamos.GetById(idPrestamo);

            if(prestamo == null)
            {
                throw new Exception("No existe el prestamo asociado a la devolucion.");
            }

            ValidarDevolucion(devolucion, idsElementos, idsEstadoMantenimiento);

            /*bool tuvoAnomalias = */InsertDevolucionDetalle(devolucion, idsElementos, idsEstadoMantenimiento, Observaciones);

            //ActualizarEstadoPrestamoYDevolucion(prestamo.IdPrestamo, devolucion.IdDevolucion, tuvoAnomalias);

            if (prestamo.IdCarrito.HasValue)
            {
                IEnumerable<Elemento> carritoLibre = uow.RepoPrestamoDetalle.GetElementosPendientes(prestamo.IdPrestamo);

                if (carritoLibre.Any())
                {
                    uow.RepoCarritos.UpdateDisponible(prestamo.IdCarrito.Value, 1);


                    HistorialCambios? historial = new HistorialCambios
                    {
                        IdTipoAccion = 2,
                        IdUsuario = devolucion.IdUsuario,
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
        catch
        {
            uow.Rollback();
            throw;
        }
    }
    #endregion

    #region AUX INSERT DEVOLUCION DETALLE, HISTORICOS y ANOMALIAS
    private /*bool */void InsertDevolucionDetalle(Devolucion devolucionNEW, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadoMantenimiento, IEnumerable<string>? Observaciones/*, Dictionary<int, List<int>>? anomaliasPorElemento = null; */ )
    {
        if (idsElementos == null)
        {
            throw new ArgumentNullException(nameof(idsElementos));
        }

        if (idsEstadoMantenimiento == null)
        {
            throw new ArgumentNullException(nameof(idsEstadoMantenimiento));
        }

        if (idsElementos.Count() != idsEstadoMantenimiento.Count())
        {
            throw new Exception("La cantidad de elementos y de estados debe coincidir.");
        }

        //bool huboAnomaliasEnEsteLote = false;
        int cont = 0;
        
        foreach (int idElemento in idsElementos)
        {
            int estadoElemento = idsEstadoMantenimiento.ElementAt(cont);
            string? obs = Observaciones?.ElementAtOrDefault(cont);

            bool yaDevuelto = uow.RepoDevolucionDetalle.Exists(devolucionNEW.IdDevolucion, idElemento);
            if (yaDevuelto)
            {
                throw new Exception($"El elemento {idElemento} ya fue devuelto previamente para este préstamo.");
            }

            uow.RepoDevolucionDetalle.Insert(new DevolucionDetalle
            {
                IdDevolucion = devolucionNEW.IdDevolucion,
                IdElemento = idElemento,
                Observaciones = obs
            });

            //if (anomaliasPorElemento != null && anomaliasPorElemento.TryGetValue(idElemento, out var listaAnomalia) && listaAnomalia.Any())
            //{
            //    huboAnomaliasEnEsteLote = true;

            //    foreach (var idTipoAnomalia in listaAnomalia)
            //    {
            //        uow.RepoDevolucionAnomalia.Insert(new DevolucionAnomalia
            //        {
            //            IdDevolucion = devolucionNEW.IdDevolucion,
            //            IdElemento = idElemento,
            //            IdTipoAnomalia = idTipoAnomalia,
            //            Descripcion = obs
            //        });
            //    }
            //}

            uow.RepoElementos.UpdateEstado(idElemento, estadoElemento);

            Elemento? elemento = uow.RepoElementos.GetById(idElemento);

            HistorialCambios Historial = new HistorialCambios
            {
                IdTipoAccion = 3,
                FechaCambio = DateTime.Now,
                IdUsuario = devolucionNEW.IdUsuario,
                Descripcion = elemento?.IdTipoElemento == 1 ?
                    $"El Elemento {idElemento} fue Devuelto" :
                    $"La notebook {idElemento} fue devuelta",
                Motivo = obs
            };

            uow.RepoHistorialCambio.Insert(Historial);

            if (elemento != null && elemento?.IdTipoElemento == 1)
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
        //return huboAnomaliasEnEsteLote;
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
    public void ValidarDevolucion(Devolucion devolucion, IEnumerable<int> idsElementos, IEnumerable<int> idsEstadoMantenimiento)
    {
        Prestamos? prestamo = uow.RepoPrestamos.GetById(devolucion.IdPrestamo);

        #region DEVOLUCION
        if (devolucion == null)
        {
            throw new ArgumentNullException(nameof(devolucion));
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

        foreach (var idEstado in idsEstadoMantenimiento.Distinct())
        {
            if (uow.RepoEstadosMantenimiento.GetById(idEstado) == null)
            {
                throw new Exception($"El estado de mantenimiento {idEstado} no existe.");
            }
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
                throw new Exception($"El elemento {idElemento} no debe estar disponible.");
            }

            if (!uow.RepoPrestamoDetalle.PerteneceAlPrestamo(devolucion.IdPrestamo, idElemento))
            {
                throw new Exception($"El elemento {idElemento} no pertenece a este prestamo.");
            }

            if (uow.RepoDevolucionDetalle.Exists(devolucion.IdDevolucion, idElemento))
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
}
