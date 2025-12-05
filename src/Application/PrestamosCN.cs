using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Transactions;
using CapaDatos.InterfaceUoW;
using System.Collections;
using Core.Entities.Catalogos;
using Core.Entities.Aggregates.Usuario;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Notebooks;
using Core.Entities.Aggregates.Docentes;
using Core.Entities.Aggregates.Prestamos;

namespace CapaNegocio;

public class PrestamosCN
{
    private readonly IUowPrestamos uow;
    private readonly IMapperPrestamos mapperPrestamos;
    private readonly IMapperNotebooksCarro mapperNotebooksCarro;
    private readonly IMapperPrestamoDetalle mapperPrestamoDetalle;

    public PrestamosCN(IMapperPrestamos mapperPrestamos, IUowPrestamos uow, IMapperNotebooksCarro mapperNotebooksCarro, IMapperPrestamoDetalle mapperPrestamoDetalle)
    {
        this.uow = uow;
        this.mapperPrestamos = mapperPrestamos;
        this.mapperNotebooksCarro = mapperNotebooksCarro;
        this.mapperPrestamoDetalle = mapperPrestamoDetalle;
    }

    #region READ PRESTAMO
    public IEnumerable<PrestamosDTO> ObtenerTodo()
    {
        return mapperPrestamos.GetAllDTO();
    }

    public Prestamos? ObtenerPrestamoPorID(int idPrestamo)
    {
        return uow.RepoPrestamos.GetById(idPrestamo);
    }
    #endregion

    #region CREATE PRESTAMO
    public void CrearPrestamo(Prestamos prestamo, IEnumerable<int> idsElementos, int? idCarrito)
    {
        try
        {
            uow.BeginTransaction();

            ValidarPermisos(prestamo.IdUsuario);

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
                    Descripcion = elemento?.IdTipoElemento == 1
                        ? $"La notebook {idElemento} fue prestada."
                        : $"El elemento con numero de serie: {elemento?.NumeroSerie} fue prestado.",
                    Motivo = null
                };

                uow.RepoHistorialCambio.Insert(historial);

                if (elemento?.IdTipoElemento == 1)
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
            ValidarPermisos(prestamo.IdUsuario);

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

    #region READ PRESTAMO DETALLE
    public IEnumerable<PrestamosDetalleDTO> ObtenerPrestamoDetallePorId(int idPrestamo, int? idCarrito)
    {
        return mapperPrestamoDetalle.GetByIdDTO(idPrestamo, idCarrito);
    }
    #endregion

    #region FILTROS
    public bool PrestamoActivo()
    {
        return uow.RepoPrestamos.PrestamoActivo();
    }

    #region Prestamo detalle
    public List<int> ObtenerIDsElementosPorPrestamo(int idPrestamo)
    {
        return uow.RepoPrestamoDetalle.GetIdsElementosByIdPrestamo(idPrestamo);
    }
    #endregion

    #region Estado del prestamo
    public EstadosPrestamo? ObtenerEstadoPrestamoPorID(int idEstadoPrestamo)
    {
        return uow.RepoEstadosPrestamo.GetById(idEstadoPrestamo);
    }
    #endregion

    #region Carritos
    public int ObtenerCantidadPorCarrito(int idCarrito)
    {
        return uow.RepoCarritos.GetCountByCarrito(idCarrito);
    }

    public int ObtenerCantidadDisponiblesPorCarrito(int idCarrito)
    {
        return uow.RepoCarritos.GetCountDisponiblesByCarrito(idCarrito);
    }
    public int ObtenerCantidadPrestadosPorCarrito(int idCarrito)
    {
        return uow.RepoCarritos.GetCountPrestadosByCarrito(idCarrito);
    }

    public Carritos? ObtenerCarritoPorID(int idCarrito)
    {
        return uow.RepoCarritos.GetById(idCarrito);
    }
    #endregion

    #region Docentes
    public Docentes? ObtenerDocentePorDNI(string DNI)
    {
        return uow.RepoDocentes.FiltroGetDocenteByID(DNI);
    }

    public Docentes? ObtenerDocentePorID(int idDocente)
    {
        return uow.RepoDocentes.GetById(idDocente);
    }
    #endregion

    #region Usuarios
    public Usuarios? ObtenerUsuarioPorID(int idUsuario)
    {
        return uow.RepoUsuarios.GetById(idUsuario);
    }
    #endregion

    #region NotebooksCarroDTO
    public IEnumerable<NotebooksCarroDTO> ObtenerNotebooksPorCarrito(int idCarrito)
    {
        return mapperNotebooksCarro.GetAll(idCarrito);
    }

    public IEnumerable<Carritos> ObtenerTodosLosCarritosDiponibles()
    {
        return uow.RepoCarritos.GetAllDisponibles();
    }

    public IEnumerable<int> ObtenerIDsPorCarrito(int idCarrito)
    {
        return uow.RepoNotebooks.GetIdNotebooksByCarrito(idCarrito);
    }

    public IEnumerable<int> ObtenerIDsPrestadosPorCarrito(int idCarrito)
    {
        return uow.RepoNotebooks.GetIdNotebooksPrestadasByCarrito(idCarrito);
    }

    public Elemento? ObtenerElementoPorID(int idElemento)
    {
        return uow.RepoElemento.GetById(idElemento);
    }

    public PrestamosDetalleDTO? ObtenerElementoMapeadoPorID(int idElemento, int? idCarrito)
    {
        return mapperPrestamoDetalle.GetElementoById(idElemento, idCarrito);
    }

    public List<int> ObtenerIDsElementosPorIdPrestamo(int idPrestamo)
    {
        return uow.RepoPrestamoDetalle.GetIdsElementosByIdPrestamo(idPrestamo);
    }

    public IEnumerable<int> ObtenerIDsElementosPorIdDisponibles(int idCarrito)
    {
        return uow.RepoNotebooks.GetIdNotebooksDisponiblesByCarrito(idCarrito);
    }
    #endregion

    #region Cursos
    public IEnumerable<Curso> ObtenerTodosLosCursos()
    {
        return uow.RepoCursos.GetAll();
    }
    public Curso? ObtenerCursoPorID(int idCurso)
    {
        return uow.RepoCursos.GetById(idCurso);
    }
    #endregion

    #region Elementos
    public Elemento? ObtenerElementoPorFiltro(string? NroSerie, string? CodBarra, string? Patrimonio)
    {
        return uow.RepoElemento.GetElementoByNSCBP(NroSerie, CodBarra, Patrimonio);
    }
    #endregion

    #region Modelos
    public Modelos? ObtenerModeloPorID(int idModelo)
    {
        return uow.RepoModelo.GetById(idModelo);
    }
    #endregion

    #region Variantes
    public VariantesElemento? ObtenerVariantePorID(int idVariante)
    {
        return uow.RepoVarianteElemento.GetById(idVariante);
    }
    #endregion

    #region TipoElemento
    public TipoElemento? ObtenerTipoElementoPorID(int idTipoElemento)
    {
        return uow.RepoTipoElemento.GetById(idTipoElemento);
    }

    #endregion

    #region Equipo
    public Notebooks? ObtenerNotebookPorID(int idNotebook)
    {
        return uow.RepoNotebooks.GetById(idNotebook);
    }
    #endregion

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

        ////if(uow.RepoDocentes.ExistsPrestamo(prestamos.IdDocente))
        ////{
        ////    throw new Exception("El docente ya tiene un prestamo previo, solo se permite un prestamo por docente");
        ////}
        #endregion

        #region ELEMENTO
        if (idsElemento == null || !idsElemento.Any())
        {
            throw new Exception("Debe prestar al menos un elemento.");
        }

        foreach (int idElementos in idsElemento)
        {
            if (uow.RepoElemento.GetById(idElementos) == null)
            {
                throw new Exception($"El elemento {idElementos} no existe.");
            }

            if (!uow.RepoElemento.GetDisponible(idElementos))
            {
                throw new Exception($"El elemento {idElementos} no esta disponible.");
            }

            //if (uow.RepoPrestamoDetalle.GetByElemento(idElementos) != null)
            //{
            //    throw new Exception($"El elemento {idElementos} ya esta en un prestamo activo.");
            //}

            if (idsElemento.Count(x => x == idElementos) > 1)
            {
                throw new Exception($"El elemento {idElementos} está repetido en la lista de prestamos.");
            }

            //if (!uow.RepoNotebooks.EstaEnPrestamo(idElementos))
            //{
            //    throw new Exception($"El elemento {idElementos} ya está en un prestamo activo.");
            //}
        }
        #endregion

        #region CARRITO
        if (idCarrito.HasValue)
        {
            if (uow.RepoCarritos.GetById(idCarrito.Value) == null)
            {
                throw new Exception("El carrito no existe.");
            }

            //if (uow.RepoCarritos.GetCountByCarrito(idCarrito.Value) < 25)
            //{
            //    throw new Exception("El carrito debe tener al menos 25 elementos para ser prestado.");
            //}

            if (!uow.RepoCarritos.GetDisponible(idCarrito.Value))
            {
                throw new Exception("El carrito no esta disponible.");
            }
        }

        #endregion
    }
    #endregion

    #region Validar permisos del usuario
    private void ValidarPermisos(int idUsuario)
    {
        Usuarios? usuarios = uow.RepoUsuarios.GetById(idUsuario);

        if (usuarios == null)
        {
            throw new Exception("El usuario no existe.");
        }

        if (usuarios?.IdRol == 3)
        {
            throw new Exception("Este usuario es invitado, no tiene permitido realizar atribuciones en el sistema");
        }
    }
    #endregion

}