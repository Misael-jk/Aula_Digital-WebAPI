using CapaEntidad;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Transactions;
using CapaDatos.InterfaceUoW;

namespace CapaNegocio;

public class PrestamosCN
{
    private readonly IUowPrestamos uow;
    private readonly IMapperPrestamos mapperPrestamos;

    public PrestamosCN(IMapperPrestamos mapperPrestamos, IUowPrestamos uow)
    {
        this.uow = uow;
        this.mapperPrestamos = mapperPrestamos;
    }

    #region READ PRESTAMO
    public IEnumerable<PrestamosDTO> ObtenerTodo()
    {
        return mapperPrestamos.GetAllDTO();
    }
    #endregion

    #region CREATE PRESTAMO
    public void CrearPrestamo(Prestamos prestamo, IEnumerable<int> idsElementos, int? idCarrito)
    {
        try
        {
            uow.BeginTransaction();

            ValidarPrestamos(prestamo, idsElementos, idCarrito);

            uow.RepoPrestamos.Insert(prestamo);

            foreach (int idElemento in idsElementos)
            {
                uow.RepoPrestamoDetalle.Insert(new PrestamoDetalle
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    IdElemento = idElemento
                });

                uow.RepoElemento.UpdateEstado(idElemento, 2);

                Elemento? elemento = uow.RepoElemento.GetById(idElemento);

                HistorialCambios? historial = new HistorialCambios
                {
                    IdTipoAccion = 5, 
                    FechaCambio = DateTime.Now,
                    IdUsuario = prestamo.IdUsuario,
                    Descripcion = elemento.IdTipoElemento == 1
                        ? $"La notebook {idElemento} fue prestada."
                        : $"El elemento con numero de serie: {elemento.NumeroSerie} fue prestado.",
                    Motivo = null
                };

                uow.RepoHistorialCambio.Insert(historial);

                if (elemento.IdTipoElemento == 1)
                {
                    uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
                    {
                        IdHistorialCambio = historial.IdHistorialCambio,
                        IdNotebook = idElemento
                    });
                }
                else
                {
                    uow.RepoHistorialElementos.Insert(new HistorialElementos
                    {
                        IdHistorialCambio = historial.IdHistorialCambio,
                        IdElementos = idElemento
                    });
                }
            }

            if (idCarrito.HasValue)
            {
                prestamo.IdCarrito = idCarrito.Value;
                uow.RepoCarritos.UpdateDisponible(idCarrito.Value, 2);

                HistorialCambios historial = new HistorialCambios
                {
                    IdTipoAccion = 5,
                    FechaCambio = DateTime.Now,
                    IdUsuario = prestamo.IdUsuario,
                    Descripcion = $"Carrito {idCarrito} fue Prestamo",
                    Motivo = null
                };

                uow.RepoHistorialCambio.Insert(historial);

                uow.RepoHistorialCarrito.Insert(new HistorialCarritos
                {
                    IdHistorialCambio = historial.IdHistorialCambio,
                    IdCarrito = idCarrito.Value
                });
            }
            else
            {
                prestamo.IdCarrito = null;
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

    #region UPDATE PRESTAMO
    public void ActualizarPrestamo(Prestamos prestamo, IEnumerable<int> nuevosIdsElementos, int? nuevoIdCarrito)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            if (uow.RepoDocentes.GetById(prestamo.IdDocente) == null)
            {
                throw new Exception("El docente no existe");
            }

            if (uow.RepoUsuarios.GetById(prestamo.IdUsuario) == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (nuevosIdsElementos == null || !nuevosIdsElementos.Any())
            {
                throw new Exception("Debe prestar al menos un elemento.");
            }

            if (nuevoIdCarrito.HasValue)
            {
                if (uow.RepoCarritos.GetById(nuevoIdCarrito.Value) == null)
                {
                    throw new Exception("El carrito no existe");
                }

                if (uow.RepoCarritos.GetCountByCarrito(nuevoIdCarrito.Value) < 25)
                {
                    throw new Exception("El carrito debe tener al menos 25 elementos para ser prestado");
                }

                if (!uow.RepoCarritos.GetDisponible(nuevoIdCarrito.Value))
                {
                    throw new Exception("El carrito no esta disponible");
                }
                prestamo.IdCarrito = nuevoIdCarrito.Value;

                uow.RepoCarritos.UpdateDisponible(nuevoIdCarrito.Value, 2);
            }


            foreach (int idElemento in nuevosIdsElementos)
            {
                if (!uow.RepoElemento.GetDisponible(idElemento))
                {
                    throw new Exception($"El elemento {idElemento} no esta disponible");
                }
            }

            uow.RepoPrestamos.Update(prestamo);

            uow.RepoPrestamoDetalle.Delete(prestamo.IdPrestamo);

            foreach (int idElemento in nuevosIdsElementos)
            {
                uow.RepoPrestamoDetalle.Insert(new PrestamoDetalle
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    IdElemento = idElemento,
                });

                uow.RepoElemento.UpdateEstado(idElemento, 2);

            }

            scope.Complete();
        }
    }
    #endregion

    #region Eliminar PRESTAMO
    public void EliminarPrestamo(int idPrestamo)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            Prestamos? prestamo = uow.RepoPrestamos.GetById(idPrestamo);

            if (prestamo == null)
            {
                throw new Exception("El prestamo no existe.");
            }

            if (prestamo.IdCarrito.HasValue)
            {
                uow.RepoCarritos.UpdateDisponible(prestamo.IdCarrito.Value, 1);
            }

            IEnumerable<PrestamoDetalle> detalles = uow.RepoPrestamoDetalle.GetByPrestamo(idPrestamo);

            foreach (var detalle in detalles)
            {
                uow.RepoElemento.UpdateEstado(detalle.IdElemento, 1);

            }

            uow.RepoPrestamoDetalle.Delete(idPrestamo);

            uow.RepoPrestamos.Delete(idPrestamo);

            scope.Complete();
        }
    }
    #endregion


    #region Validaciones INsert
    public void ValidarPrestamos(Prestamos prestamos, IEnumerable<int> idsElemento, int? idCarrito)
    {
        Prestamos? oldPrestamo = uow.RepoPrestamos.GetById(prestamos.IdPrestamo);

        #region CURSO
        if (prestamos.IdCurso.HasValue)
        {
            if (uow.RepoCursos.GetById(prestamos.IdCurso.Value) == null)
            {
                throw new Exception("El curso no existe");
            }
        }
        #endregion

        #region USUARIO
        if (uow.RepoUsuarios.GetById(prestamos.IdUsuario) == null)
        {
            throw new Exception("El usuario no existe");
        }
        #endregion

        #region DOCENTE
        if (uow.RepoDocentes.GetById(prestamos.IdDocente) == null)
        {
            throw new Exception("El docente no existe");
        }
        #endregion

        #region ELEMENTO
        if (idsElemento == null || !idsElemento.Any())
        {
            throw new Exception("Debe prestar al menos un elemento.");
        }

        foreach (int idElementos in idsElemento)
        {
            if(uow.RepoElemento.GetById(idElementos) == null)
            {
                throw new Exception($"El elemento {idElementos} no existe.");
            }

            if (!uow.RepoElemento.GetDisponible(idElementos))
            {
                throw new Exception($"El elemento {idElementos} no esta disponible.");
            }

            if(uow.RepoPrestamoDetalle.GetByElemento(idElementos) != null)
            {
                throw new Exception($"El elemento {idElementos} ya esta en un prestamo activo.");
            }

            if (idsElemento.Count(x => x == idElementos) > 1)
            {
                throw new Exception($"El elemento {idElementos} está repetido en la lista de prestamos.");
            }
        }
        #endregion

        #region CARRITO
        if (idCarrito.HasValue)
        {
            if (uow.RepoCarritos.GetById(idCarrito.Value) == null)
            {
                throw new Exception("El carrito no existe.");
            }

            if (uow.RepoCarritos.GetCountByCarrito(idCarrito.Value) < 25)
            {
                throw new Exception("El carrito debe tener al menos 25 elementos para ser prestado.");
            }

            if (!uow.RepoCarritos.GetDisponible(idCarrito.Value))
            {
                throw new Exception("El carrito no esta disponible.");
            }

        }

        #endregion
    }
    #endregion

}

