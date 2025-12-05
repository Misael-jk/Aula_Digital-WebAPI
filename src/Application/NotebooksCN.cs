using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using Core.Entities.Aggregates.Carritos;
using Core.Entities.Aggregates.Notebooks;
using Core.Entities.Aggregates.Usuario;
using Core.Entities.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CapaNegocio;

public class NotebooksCN
{
    private readonly IUowNotebooks uow;
    private readonly IMapperNotebooks mapperNotebook;
    private readonly IMapperHistorialNotebookG mapperHistorialNotebookG;

    public NotebooksCN(IMapperNotebooks mapperNotebooks, IUowNotebooks uow, IMapperHistorialNotebookG mapperHistorialNotebookG)
    {
        this.uow = uow;
        this.mapperNotebook = mapperNotebooks;
        this.mapperHistorialNotebookG = mapperHistorialNotebookG;
    }

    #region READ
    public IEnumerable<NotebooksDTO> GetAll()
    {
        return mapperNotebook.GetAllDTO();
    }

    public IEnumerable<NotebooksDTO> ObtenerPorEstados(string estado)
    {
        return mapperNotebook.GetAllByEstado(estado);
    }

    public IEnumerable<NotebooksDTO> ObtenerPorCarrito(string carrito)
    {
        return mapperNotebook.GetNotebooksByCarritoDTO(carrito);
    }

    public IEnumerable<NotebooksDTO> ObtenerPorFiltros(string? text, int? idCarrito, string? equipo)
    {
        return mapperNotebook.GetByFiltros(text, idCarrito, equipo);
    }
    #endregion

    #region Alta
    public void CrearNotebook(Notebooks notebookNEW, int idUsuario)
    {
        ValidarDatos(notebookNEW);

        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            ValidarInsert(notebookNEW);

            uow.RepoNotebooks.Insert(notebookNEW);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 1,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se creó la notebook con número de serie {notebookNEW.NumeroSerie}.",
                Motivo = null,
                IdUsuario = idUsuario
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdNotebook = notebookNEW.IdElemento
            });

            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }
    #endregion

    #region Actualización
    public void ActualizarNotebook(Notebooks notebookNEW, int idUsuario, string motivo, string descripcion)
    {
        ValidarDatos(notebookNEW);

        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            ValidarUpdate(notebookNEW);

            uow.RepoNotebooks.Update(notebookNEW);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 2,
                FechaCambio = DateTime.Now,
                Descripcion = descripcion,
                Motivo = motivo,
                IdUsuario = idUsuario
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdNotebook = notebookNEW.IdElemento
            });
            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }
    #endregion

    #region Deshabilitar Notebook
    public void DeshabilitarNotebook(int idNotebook, string motivo, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            Notebooks? notebook = uow.RepoNotebooks.GetById(idNotebook);
            if (notebook == null)
            {
                throw new Exception("La notebook no existe.");
            }

            if (uow.RepoEstadosMantenimiento.GetById(notebook.IdEstadoMantenimiento) == null)
            {
                throw new Exception("El estado de mantenimiento seleccionado no es valido");
            }

            if (uow.RepoNotebooks.EstaEnPrestamo(notebook.IdElemento))
            {
                throw new Exception("No se puede deshabilitar un carrito que está en préstamo");
            }

            notebook.IdEstadoMantenimiento = 6; // Dado de baja
            notebook.Habilitado = false;
            notebook.FechaBaja = DateTime.Now;

            uow.RepoNotebooks.Update(notebook);
            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 3,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se deshabilitó la notebook con número de serie {notebook.NumeroSerie}.",
                Motivo = motivo,
                IdUsuario = idUsuario
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdNotebook = notebook.IdElemento
            });

            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }
    #endregion

    // Filtros
    #region Filtros para la UI
    public IEnumerable<Modelos> ListarModelosPorTipo(int idModelo)
    {
        return uow.RepoModelo.GetByTipo(1);
    }

    public IEnumerable<Ubicacion> ListarUbicaciones()
    {
        return uow.RepoUbicacion.GetAll();
    }

    public IEnumerable<EstadosMantenimiento> ListarEstadoMantenimiento()
    {
        return uow.RepoEstadosMantenimiento.GetAll();
    }

    public IEnumerable<EstadosMantenimiento> ListarEstadoParaActualizar()
    {
        return uow.RepoEstadosMantenimiento.GetAllForUpdates();
    }

    public int? ObtenerCantidadPorEstados(int idEstado)
    {
        return uow.RepoNotebooks.CantidadEstados(idEstado);
    }

    public int CantidadTotalNotebooks()
    {
        return uow.RepoNotebooks.CantidadTotal();
    }

    public Notebooks? ObtenerNotebookPorID(int idNotebook)
    {
        return uow.RepoNotebooks.GetById(idNotebook);
    }

    public IEnumerable<string> ObtenerEquiposDeNotebooks()
    {
        return uow.RepoNotebooks.GetEquiposNotebooks();
    }

    public EstadosMantenimiento? ObtenerEstadoMantenimientoPorID(int idEatadoMantenimiento)
    {
        return uow.RepoEstadosMantenimiento.GetById(idEatadoMantenimiento);
    }

    public Carritos? ObtenerCarritoPorID(int idCarrito)
    {
        return uow.RepoCarritos.GetById(idCarrito);
    }

    public Carritos? ObtenerCarritoPorEquipo(string equipo)
    {
        return uow.RepoCarritos.GetByEquipo(equipo);
    }

    public IEnumerable<string> ObtenerEquiposDeCarritos()
    {
        return uow.RepoCarritos.GetEquipos();
    }

    public Carritos? ObtenerCarritoPorNotebook(int idNotebook)
    {
        return uow.RepoNotebooks.GetCarritoByNotebook(idNotebook);
    }

    public HistorialCambios? ObtenerUltimaFechaDeModiciacionPorID(int idNotebook)
    {
        return uow.RepoHistorialCambio.GetUltimateDateByIdNotebook(idNotebook);
    }

    public List<(string Modelo, int Cantidad)> GetCantidadPorModelo()
    {
        return uow.RepoNotebooks.GetCantidadPorModelo();
    }

    public List<(string Estado, int Cantidad)> GetCantidadEstado()
    {
        return uow.RepoNotebooks.GetCantidadEstado();
    }

    public List<(string Equipo, int Cantidad)> GetCantidadNotebooksEnCarritos()
    {
        return uow.RepoNotebooks.GetCantidadNotebooksEnCarritos();
    }

    public IEnumerable<string> ObtenerSerieBarrasPatrimonio(string texto, int limite)
    {
        return uow.RepoNotebooks.GetSerieBarraPatrimonio(texto, limite);
    }
    #endregion

    #region HISTORIAL COMPLETO DE DICHO ELEMENTO
    public IEnumerable<HistorialNotebookGestionDTO> ObtenerHistorialPorID(int idNotebook)
    {
        return mapperHistorialNotebookG.GetAllDTO(idNotebook);
    }

    public HistorialCambios? ObtenerHistorialPorIDHistorial(int idHistorial)
    {
        return uow.RepoHistorialCambio.GetById(idHistorial);
    }
    #endregion



    #region VALIDACIONES
    public void ValidarDatos(Notebooks notebooks)
    {
        if (string.IsNullOrEmpty(notebooks.NumeroSerie))
        {
            throw new Exception("El numero de serie es obligatorio");
        }

        if (notebooks.NumeroSerie.Length > 40)
        {
            throw new ValidationException("El número de serie no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(notebooks.NumeroSerie, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El número de serie contiene caracteres inválidos.");
        }

        if (string.IsNullOrEmpty(notebooks.CodigoBarra))
        {
            throw new Exception("El código de barras es obligatorio");
        }

        if (notebooks.CodigoBarra.Length > 40)
        {
            throw new ValidationException("El codigo de barra no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(notebooks.CodigoBarra, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El codigo de barra contiene caracteres inválidos.");
        }

        if ((string.IsNullOrEmpty(notebooks.Patrimonio)))
        {
            throw new Exception("El patrimonio es obligatorio");
        }

        if (notebooks.Patrimonio.Length > 40)
        {
            throw new ValidationException("El patrimonio no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(notebooks.Patrimonio, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El patrimonio contiene caracteres inválidos.");
        }

        if (string.IsNullOrEmpty(notebooks.Equipo))
        {
            throw new Exception("El equipo es obligatorio");
        }

        if (notebooks.Equipo.Length > 40)
        {
            throw new ValidationException("El equipo no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(notebooks.Equipo, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El equipo contiene caracteres inválidos.");
        }
    }
    #endregion

    #region VALIDAR INSERT
    public void ValidarInsert(Notebooks notebooks)
    {
        #region VALIDAR ID NOTEBOOK
        if(notebooks.IdElemento != 0 && uow.RepoNotebooks.GetById(notebooks.IdElemento) != null)
        {
            throw new Exception("La notebook ya existe");
        }
        #endregion

        #region NUMERO SERIE
        Notebooks? nroSerieHabilitado = uow.RepoNotebooks.GetByNumeroSerie(notebooks.NumeroSerie);

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
        #endregion

        #region CODIGO BARRA
        Notebooks? codigoBarraHabilitado = uow.RepoNotebooks.GetByCodigoBarra(notebooks.CodigoBarra);
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
        #endregion

        #region PATRIMONIO
        Notebooks? patrimonioHabilitado = uow.RepoNotebooks.GetByPatrimonio(notebooks.Patrimonio);
        if (patrimonioHabilitado != null)
        {
            if (patrimonioHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese patrimonio ya existe y esta habilitado");
            }
            else
            {
                throw new Exception("La notebook con ese patrimonio ya existe pero esta deshabilitado, por favor habilitelo antes de crear uno nuevo");
            }
        }
        #endregion

        #region EQUIPO
        Notebooks? equipoHabilitado = uow.RepoNotebooks.GetByEquipo(notebooks.Equipo);

        if (equipoHabilitado != null)
        {
            if (equipoHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese equipo ya existe y esta habilitado");
            }
            else
            {
                throw new Exception("La notebook con ese equipo ya existe pero esta deshabilitado, por favor habilitelo antes de crear uno nuevo");
            }
        }
        #endregion

        #region ESTADO
        if (notebooks.IdEstadoMantenimiento != 1)
        {
            throw new Exception("El estado del elemento debe ser 'Disponible' al momento de crearlo");
        }

        if (uow.RepoEstadosMantenimiento.GetById(notebooks.IdEstadoMantenimiento) == null)
        {
            throw new Exception("Estado de mantenimiento de la notebook es invalida");
        }
        #endregion

        #region TIPO ElEMENTO
        if (notebooks.IdTipoElemento != 1)
        {
            throw new Exception("El tipo de elemento debe ser 'Notebook'");
        }

        if (uow.RepoTipoElemento.GetById(notebooks.IdTipoElemento) == null)
        {
            throw new Exception("El tipo elemento es invalido");
        }
        #endregion

        #region UBICACION
        if (notebooks.IdUbicacion != 0)
        {
            Ubicacion? ubicacion = uow.RepoUbicacion.GetById(notebooks.IdUbicacion);

            Carritos? carritos = uow.RepoCarritos.GetById(notebooks.IdCarrito ?? 0);

            if (ubicacion == null)
            {
                throw new Exception("La ubicacion del elemento es inválido.");
            }
        }
        #endregion

        #region MODELO
        if (notebooks.IdModelo != 0)
        {
            Modelos? modelo = uow.RepoModelo.GetById(notebooks.IdModelo);

            if (modelo == null)
            {
                throw new Exception("El modelo de la notebook es inválido.");
            }

            if (modelo.IdTipoElemento != notebooks.IdTipoElemento)
            {
                throw new Exception("El modelo seleccionado no corresponde al tipo de la notebook.");
            }
        }
        #endregion

        //#region CARRITO
        //if (notebooks.IdCarrito.HasValue)
        //{
        //    throw new Exception("La Notebook no puede estar ligada a un carrito al crearse, Valla al area de CARRITOS para llevarlo a cabo");
        //}
        //#endregion

        //#region POSICION CARRITOS
        //if(notebooks.PosicionCarrito.HasValue)
        //{
        //    throw new Exception("La Notebook no puede tener una posicion en el carrito al crearse, Valla al area de CARRITOS para llevarlo a cabo");
        //}
        //#endregion

        #region VARIANTE ELEMENTO
        if (notebooks.IdVarianteElemento.HasValue && notebooks.IdVarianteElemento != 0)
        {
            throw new Exception("La notebook no puede tener variante");
        }
        #endregion
    }
    #endregion

    #region VALIDAR UPDATE
    public void ValidarUpdate(Notebooks notebooks)
    {
        #region VALIDAR ID NOTEBOOK
        Notebooks? notebookOLD = uow.RepoNotebooks.GetById(notebooks.IdElemento);

        if (notebooks.IdElemento == 0 || notebookOLD == null)
        {
            throw new Exception("La notebook no existe");
        }
        #endregion

        #region NUMERO SERIE
        Notebooks? nroSerieHabilitado = uow.RepoNotebooks.GetByNumeroSerie(notebooks.NumeroSerie);

        if (notebookOLD?.NumeroSerie != notebooks.NumeroSerie && nroSerieHabilitado != null)
        {
            if (nroSerieHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otra notebook con el mismo numero de serie.");
            }
            else
            {
                throw new Exception("La notebook con ese numero de serie ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion

        #region CODIGO BARRA
        Notebooks? codigoBarraHabilitado = uow.RepoNotebooks.GetByCodigoBarra(notebooks.CodigoBarra);

        if (notebookOLD?.CodigoBarra != notebooks.CodigoBarra && codigoBarraHabilitado != null)
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
        #endregion

        #region PATRIMONIO
        Notebooks? patrimonioHabilitado = uow.RepoNotebooks.GetByPatrimonio(notebooks.Patrimonio);

        if (notebookOLD?.Patrimonio != notebooks.Patrimonio && patrimonioHabilitado != null)
        {
            if (patrimonioHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otra notebook con el mismo patrimonio.");
            }
            else
            {
                throw new Exception("La notebook con ese patrimonio ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion

        #region EQUIPO
        Notebooks? equipoHabilitado = uow.RepoNotebooks.GetByEquipo(notebooks.Equipo);

        if (equipoHabilitado != null && notebookOLD?.Equipo != notebooks.Equipo)
        {
            if (equipoHabilitado.Habilitado == true)
            {
                throw new Exception("La notebook con ese equipo ya existe y esta habilitado");
            }
            else
            {
                throw new Exception("La notebook con ese equipo ya existe pero esta deshabilitado, por favor habilitelo antes de crear uno nuevo");
            }
        }
        #endregion

        #region UBICACION  

        if (notebooks.IdCarrito.HasValue)
        {
            Carritos? carrito = uow.RepoCarritos.GetById(notebooks.IdCarrito.Value);

            if (carrito == null)
            {
                throw new Exception("El carrito asociado no existe.");
            }

            Ubicacion? ubicacionCarrito = uow.RepoUbicacion.GetById(carrito.IdUbicacion);

            if (ubicacionCarrito == null)
            {
                throw new Exception("La ubicación del carrito es invalida.");
            }

            if (notebooks.IdUbicacion != ubicacionCarrito.IdUbicacion)
            {
                throw new Exception("La ubicación de la notebook no coincide con la ubicación del carrito.");
            }
        }
        else
        {
            Ubicacion? ubicacion = uow.RepoUbicacion.GetById(notebooks.IdUbicacion);
            if (ubicacion == null)
            {
                throw new Exception("La ubicación seleccionada no existe.");
            }
        }
        #endregion

        #region MODELOS
        if (notebooks.IdModelo != 0)
        {
            Modelos? modelo = uow.RepoModelo.GetById(notebooks.IdModelo);

            if (modelo == null)
            {
                throw new Exception("El modelo de la notebook es inválido.");
            }

            if (modelo.IdTipoElemento != notebooks.IdTipoElemento)
            {
                throw new Exception("El modelo seleccionado no corresponde al tipo de la notebook.");
            }
        }
        #endregion

        #region TIPO ELEMENTO
        if (notebookOLD?.IdTipoElemento != notebooks.IdTipoElemento)
        {
            throw new Exception("No se puede cambiar el tipo de elemento");
        }
        #endregion

        #region ESTADO
        if (notebooks.IdEstadoMantenimiento == 2 && notebookOLD.IdEstadoMantenimiento != 2)
        {
            throw new Exception("No se puede cambiar el estado a 'En Prestamo' por que no se hiso un prestamo");
        }

        if (notebooks.IdEstadoMantenimiento != 2 && notebookOLD.IdEstadoMantenimiento == 2)
        {
            throw new Exception("No se puede cambiar el estado de un elemento en prestamo sin terminar su devolucion");
        }

        if (uow.RepoEstadosMantenimiento.GetById(notebooks.IdEstadoMantenimiento) == null)
        {
            throw new Exception("El estado de mantenimiento indicado es inválido.");
        }
        #endregion

        #region CARRITOS Y POSICION

        if (notebooks.IdCarrito.HasValue)
        {
            Carritos? carritoOLD = uow.RepoCarritos.GetById(notebooks.IdCarrito.Value);
            if (carritoOLD == null)
            {
                throw new Exception("El carrito asociado no existe.");
            }

            if (carritoOLD.IdEstadoMantenimiento == 2)
            {
                throw new Exception("No se puede asignar la notebook a un carrito que está en préstamo.");
            }

            Ubicacion? ubicCarrito = uow.RepoUbicacion.GetById(carritoOLD.IdUbicacion);
            if (ubicCarrito == null)
            {
                throw new Exception("La ubicación asociada al carrito es inválida.");
            }

            if (notebooks.PosicionCarrito.HasValue)
            {
                if (notebooks.PosicionCarrito < 1 || notebooks.PosicionCarrito > 25)
                {
                    throw new Exception("La posicion en el carrito debe estar entre 1 y 25");
                }
                Notebooks? notebookEnPosicion = uow.RepoNotebooks.GetNotebookByPosicion(notebooks.IdCarrito.Value, notebooks.PosicionCarrito.Value);

                if (notebookEnPosicion != null && notebookEnPosicion.IdElemento != notebooks.IdElemento)
                {
                    throw new Exception("La posicion en el carrito ya está ocupada por otra notebook");
                }
            }
        }
        else
        {
            if (notebooks.PosicionCarrito.HasValue)
            {
                throw new Exception("La posicion en el carrito no puede estar asignada si no tiene un carrito asociado");
            }
        }

        #endregion

        #region VARIANTE ELEMENTO
        if (notebooks.IdVarianteElemento.HasValue && notebooks.IdVarianteElemento != 0)
        {
           throw new Exception("Las notebooks no pueden tener variante de elemento.");
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
