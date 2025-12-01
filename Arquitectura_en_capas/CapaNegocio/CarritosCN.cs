using CapaEntidad;
using System.Transactions;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using CapaDatos.InterfaceUoW;
using System.Globalization;
using CapaDTOs.AuditoriaDTO;

namespace CapaNegocio;

public class CarritosCN
{
    private readonly IMapperCarritos mapperCarritos;
    private readonly IMapperHistorialCarritosG mapperHistorialCarritosG;
    private readonly IUowCarritos uow;

    public CarritosCN(IMapperCarritos mapperCarritos, IUowCarritos uowCarritos, IMapperHistorialCarritosG mapperHistorialCarritosG)
    {
        this.mapperCarritos = mapperCarritos;
        uow = uowCarritos;
        this.mapperHistorialCarritosG = mapperHistorialCarritosG;
    }

    // CRUD
    #region READ CARRITO
    public IEnumerable<CarritosDTO> MostrarCarritos()
    {
        return mapperCarritos.GetAllDTO();
    }

    public IEnumerable<CarritosDTO> ObtenerPorEstado(string estado)
    {
        return mapperCarritos.GetAllByEstado(estado);
    }
    #endregion

    #region INSERT CARRITO
    public void CrearCarrito(Carritos CarritoNEW, int idUsuario)
    {
        ValidarDatos(CarritoNEW);

        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            ValidarReposInsert(CarritoNEW);

            uow.RepoCarritos.Insert(CarritoNEW);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 1,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se creó el carrito con número de serie {CarritoNEW.EquipoCarrito}.",
                IdUsuario = idUsuario
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = CarritoNEW.IdCarrito
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

    #region UPDATE CARRITO
    /// <summary>
    /// Estados de mantenimiento: <br/>
    /// 1. Disponible <br/>
    /// 2. En Préstamo <br/>
    /// 3. En Mantenimiento <br/>
    /// 4. Dado de Baja <br/>
    /// </summary>
    /// <param name="carritoNEW"></param>
    /// <param name="idUsuario"></param>
    public void ActualizarCarrito(Carritos carritoNEW, int idUsuario, string Motivo, string Descripcion)
    {
        ValidarDatos(carritoNEW);

        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            ValidarReposUpdate(carritoNEW);

            uow.RepoCarritos.Update(carritoNEW);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 2,
                FechaCambio = DateTime.Now,
                Descripcion = Descripcion,
                IdUsuario = idUsuario,
                Motivo = Motivo
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = carritoNEW.IdCarrito
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

    #region DESHABILITAR UN CARRITO
    public void DeshabilitarCarrito(int idCarritos, string motivo, int idUsuario)
    {
        try
        {
            ValidarPermisos(idUsuario);

            uow.BeginTransaction();

            Carritos? carritoOLD = uow.RepoCarritos.GetById(idCarritos);

            if (carritoOLD == null)
            {
                throw new Exception("El carrito no existe");
            }

            if (uow.RepoEstadosMantenimiento.GetById(carritoOLD.IdEstadoMantenimiento) == null)
            {
                throw new Exception("El estado de mantenimiento seleccionado no es valido");
            }

            if (uow.RepoUbicacion.GetById(carritoOLD.IdUbicacion) == null)
            {
                throw new Exception("La ubicacion seleccionada no existe");
            }

            if (carritoOLD.IdModelo.HasValue)
            {
                Modelos? modeloCarrito = uow.RepoModelo.GetById(carritoOLD.IdModelo.Value);
                if (modeloCarrito == null)
                {
                    throw new Exception("El modelo del carrito no es valido.");
                }
            }

            if (uow.RepoCarritos.GetEstadoEnPrestamo(carritoOLD.IdCarrito))
            {
                throw new Exception("No se puede deshabilitar un carrito que está en préstamo");
            }

            if (uow.RepoNotebooks.GetByCarrito(carritoOLD.IdCarrito).Any())
            {
                throw new Exception("No se puede deshabilitar un carrito que aún contiene notebooks.");
            }

            carritoOLD.IdEstadoMantenimiento = 6;
            carritoOLD.FechaBaja = DateTime.Now;
            carritoOLD.Habilitado = false;

            uow.RepoCarritos.Update(carritoOLD);

            HistorialCambios historial = new HistorialCambios()
            {
                IdTipoAccion = 3,
                IdUsuario = idUsuario,
                Descripcion = $"Se deshabilito el carrito con numero de serie {carritoOLD.NumeroSerieCarrito}",
                Motivo = motivo,
                FechaCambio = DateTime.Now
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = carritoOLD.IdCarrito
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

    #region DELETE CARRITO
    public void EliminarCarrito(int idCarrito)
    {

        Carritos? carrito = uow.RepoCarritos.GetById(idCarrito);

        if (carrito == null)
        {
            throw new Exception("El carrito no existe");
        }

        uow.RepoCarritos.Delete(idCarrito);
    }
    #endregion




    // FILTROS Y CONSULTAS DE LOS REPOS RELACIONADOS CON EL CARRITO
    #region CARRITOS
    public IEnumerable<Carritos> ListarCarritos()
    {
        return uow.RepoCarritos.GetAll();
    }

    public Carritos? ObtenerCarritoPorID(int idCarrito)
    {
        return uow.RepoCarritos.GetById(idCarrito);
    }

    public Carritos? ObtenerCarritoPorEquipo(string equipo)
    {
        return uow.RepoCarritos.GetByEquipo(equipo);
    }

    public HistorialCambios? ObtenerUltimaFechaDeModiciacionPorID(int idCarrito)
    {
        return uow.RepoHistorialCambio.GetUltimateDateByIdCarrito(idCarrito);
    }

    public int ObtenerCantidadPorCarrito(int idCarrito)
    {
        return uow.RepoCarritos.GetCountByCarrito(idCarrito);
    }
    #endregion

    #region ESTADO MANTENIMIENTO
    public IEnumerable<EstadosMantenimiento> ListarEstadosMatenimiento()
    {
        return uow.RepoEstadosMantenimiento.GetAll();
    }

    public EstadosMantenimiento? ObtenerEstadoMantenimientoPorID(int idEatadoMantenimiento)
    {
        return uow.RepoEstadosMantenimiento.GetById(idEatadoMantenimiento);
    }

    public IEnumerable<EstadosMantenimiento> ListarEstadoParaActualizar()
    {
        return uow.RepoEstadosMantenimiento.GetAllForUpdates();
    }
    #endregion

    #region UBICACION
    public IEnumerable<Ubicacion> ListarUbicaciones()
    {
        return uow.RepoUbicacion.GetAll();
    }
    #endregion

    #region MODELOS
    public IEnumerable<Modelos> ListarModelosCarritos()
    {
        return uow.RepoModelo.GetAll();
    }

    public IEnumerable<Modelos> ListarModelosPorTipo(int idTipo)
    {
        return uow.RepoModelo.GetByTipo(idTipo);
    }

    public Modelos? ObtenerModeloPorID(int idModelo)
    {
        return uow.RepoModelo.GetById(idModelo);
    }
    #endregion

    #region NOTEBOOK
    public IEnumerable<Notebooks> ObtenerSeriePorNotebook()
    {
        return uow.RepoNotebooks.GetNroSerieByNotebook();
    }

    public IEnumerable<Notebooks> ObtenerCodBarraPorNotebook()
    {
        return uow.RepoNotebooks.GetCodBarraByNotebook();
    }

    public IEnumerable<Notebooks> ObtenerNotebooksPorCarrito(int idCarrito)
    {
        return uow.RepoNotebooks.GetNotebookByCarrito(idCarrito);
    }

    public Notebooks? ObtenerNotebookPorPosicion(int? idCarrito, int posicion)
    {
        return uow.RepoNotebooks.GetNotebookByPosicion(idCarrito, posicion);
    }

    public Notebooks? ObtenerPorSerie(string numSerie)
    {
        return uow.RepoNotebooks.GetByNumeroSerie(numSerie);
    }

    public Notebooks? ObtenerPorSerieOCodBarraOPatrimonio(string? numSerie, string? CodBarra, string? patrimonio)
    {
        return uow.RepoNotebooks.GetNotebookBySerieOrCodigoOrPatrimonio(numSerie, CodBarra, patrimonio);
    }

    public Notebooks? ObtenerNotebookPorID(int idNotebook)
    {
        return uow.RepoNotebooks.GetById(idNotebook);
    }
    #endregion

    #region Historiales 
    public IEnumerable<HistorialCarritoGestionDTO> ObtenerHistorialPorID(int idCarrito)
    {
        return mapperHistorialCarritosG.GetAllDTO(idCarrito);
    }

    public HistorialCambios? ObtenerHistorialPorIDHistorial(int idHistorial)
    {
        return uow.RepoHistorialCambio.GetById(idHistorial);
    }
    #endregion




    // OPERACIONES CON NOTEBOOKS EN EL CARRITO
    #region AGREGAR NOTEBOOK AL CARRITO
    public void AddNotebook(int idCarrito, int posicion, int idNotebook, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            #region VALIDACION DE CARRITOS
            Carritos? carrito = uow.RepoCarritos.GetById(idCarrito);

            if (carrito == null)
            {
                throw new Exception("El carrito no existe");
            }

            if (carrito.IdEstadoMantenimiento == 2)
            {
                throw new Exception("El carrito esta en prestamo");
            }

            //if (!uow.RepoCarritos.GetDisponible(idCarrito))
            //{
            //    throw new Exception("El carrito no esta disponible para agregar notebooks");
            //}

            if (carrito.IdUbicacion <= 0)
            {
                throw new Exception("El carrito no tiene una ubicación valida asignada");
            }

            #endregion

            #region VALIDACION NOTEBOOK
            Notebooks? notebooks = uow.RepoNotebooks.GetById(idNotebook);

            if (notebooks == null)
            {
                throw new Exception("Notebook no encontrado");
            }

            if (notebooks.IdEstadoMantenimiento == 2)
            {
                throw new Exception("Un elemento en prestamo no puede estar asignado a un carrito");
            }

            if (!uow.RepoNotebooks.GetDisponible(idNotebook))
            {
                throw new Exception("La notebook no esta disponible para estar asignado a un carrito");
            }

            if (notebooks.IdCarrito.HasValue && notebooks.IdCarrito != idCarrito)
            {
                throw new Exception("La notebook ya esta en otro carrito.");
            }

            if (notebooks.IdTipoElemento != 1)
            {
                throw new Exception("El elemento que intenta agregar no es una notebook.");
            }
            #endregion

            #region VALIDACIONES DE LA POSICION Y CAPACIDAD DEL CARRITO
            int cantidadActual = uow.RepoCarritos.GetCountByCarrito(idCarrito);

            if (cantidadActual >= carrito.Capacidad)
            {
                throw new Exception("El carrito que selecciono esta al maximo de notebooks (capacidad: " + carrito.Capacidad + ").");
            }

            if (posicion < 1 || posicion > carrito.Capacidad)
            {
                throw new Exception($"La posición en el carrito debe estar entre 1 y {carrito.Capacidad}.");
            }

            if (uow.RepoNotebooks.DuplicatePosition(idCarrito, posicion))
            {
                throw new Exception("La posición dentro del carrito ya está ocupada, elija otra.");
            }
            #endregion

            if (notebooks.IdUbicacion != carrito.IdUbicacion)
            {
                notebooks.IdUbicacion = carrito.IdUbicacion;
            }

            notebooks.IdCarrito = idCarrito;
            notebooks.PosicionCarrito = posicion;

            uow.RepoNotebooks.Update(notebooks);

            HistorialCambios? historial = new HistorialCambios
            {
                IdTipoAccion = 2,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se agregó la notebook con número de serie {notebooks.NumeroSerie} al carrito con número de serie {carrito.NumeroSerieCarrito}.",
                IdUsuario = idUsuario,
                Motivo = null
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = carrito.IdCarrito
            });

            uow.RepoHistorialNotebooks.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdNotebook = notebooks.IdElemento
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

    #region QUITAR NOTEBOOK DEL CARRITO
    public void RemoveNotebook(int idCarrito, int idNotebook, int idUsuario, int idUbicacion)
    {
        try
        {
            uow.BeginTransaction();

            ValidarPermisos(idUsuario);

            #region VALIDACION DE LA NOTEBOOK
            Notebooks? notebooks = uow.RepoNotebooks.GetById(idNotebook);

            if (notebooks == null)
            {
                throw new Exception("La notebook no existe");
            }

            if (notebooks.IdCarrito != idCarrito)
            {
                throw new Exception("La notebook no pertenece al carrito seleccionado");
            }

            if (notebooks.IdTipoElemento != 1)
            {
                throw new Exception("El elemento que intenta quitar no es una notebook.");
            }

            if (notebooks.IdEstadoMantenimiento == 2)
            {
                throw new Exception("Un elemento en prestamo no puede estar asignado a un carrito");
            }

            if (!notebooks.IdCarrito.HasValue)
            {
                throw new Exception("La notebook no esta asignada a ningun carrito");
            }

            if (!uow.RepoNotebooks.GetDisponible(idNotebook))
            {
                throw new Exception("La notebook no esta disponible para quitar del carrito");
            }


            #endregion

            #region VALIDACIONES CARRITO
            Carritos? carrito = uow.RepoCarritos.GetById(idCarrito);

            if (carrito == null)
            {
                throw new Exception("El carrito no existe.");
            }

            if (!uow.RepoCarritos.GetDisponible(idCarrito))
            {
                throw new Exception("El carrito no esta disponible para quitar notebooks");
            }

            if (carrito.IdUbicacion <= 0)
            {
                throw new Exception("El carrito no tiene una ubicación valida asignada");
            }

            if (carrito.IdEstadoMantenimiento == 2)
            {
                throw new Exception("El carrito esta en prestamo");
            }

            if (uow.RepoCarritos.GetCountByCarrito(idCarrito) <= 0)
            {
                throw new Exception("El carrito no tiene notebooks para quitar");
            }
            #endregion

            #region VALIDAR UBICACION
            if (idUbicacion <= 0)
            {
                throw new Exception("La ubicación seleccionada no es valida.");
            }
            if (uow.RepoUbicacion.GetById(idUbicacion) == null)
            {
                throw new Exception("La ubicación seleccionada no existe.");
            }
            #endregion

            notebooks.IdUbicacion = idUbicacion;
            notebooks.IdCarrito = null;
            notebooks.PosicionCarrito = null;

            uow.RepoNotebooks.Update(notebooks);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 2,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se quitó la notebook con número de serie {notebooks.NumeroSerie} del carrito con número de serie {carrito?.NumeroSerieCarrito}.",
                IdUsuario = idUsuario,
                Motivo = null
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = idCarrito
            });

            uow.RepoHistorialNotebooks.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdNotebook = notebooks.IdElemento
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




    // VALIDACIONES
    #region VALIDACIONES DE CARACTERES
    /// <summary>
    /// Validar Datos del carrito (String, Cadena): <br/>
    /// 1. No puede estar vacio o ser nulo. <br/>
    /// 2. No puede superar los 40 caracteres. <br/>
    /// 3. Solo puede contener letras, números, espacios y guiones. <br/>
    /// </summary>
    /// <param name="carrito"></param>
    /// <exception cref="ValidationException"></exception>
    private void ValidarDatos(Carritos carrito)
    {
        if (string.IsNullOrWhiteSpace(carrito.NumeroSerieCarrito))
        {
            throw new ValidationException("El numero de serie es obligatorio.");
        }

        if (carrito.NumeroSerieCarrito.Length > 40)
        {
            throw new ValidationException("El número de serie no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(carrito.NumeroSerieCarrito, @"^[A-Za-z0-9\-]+$"))
        {
            throw new ValidationException("El número de serie contiene caracteres inválidos.");
        }

        if (string.IsNullOrWhiteSpace(carrito.EquipoCarrito))
        {
            throw new ValidationException("El nombre del equipo es obligatorio.");
        }

        if (carrito.EquipoCarrito.Length > 40)
        {
            throw new ValidationException("El nombre del equipo no puede superar los 40 caracteres.");
        }

        if (!Regex.IsMatch(carrito.EquipoCarrito, @"^[A-Za-z0-9\s\-]+$"))
        {
            throw new ValidationException("El nombre del equipo contiene caracteres inválidos.");
        }

        if (carrito.Capacidad < 25)
        {
            throw new ValidationException("La capacidad del carrito debe ser mayor que 25.");
        }

        if (carrito.Capacidad > 40)
            throw new ValidationException("La capacidad del carrito no puede superar 40.");
    }
    #endregion


    #region Validar Insert
    private void ValidarReposInsert(Carritos carritoNEW)
    {
        #region VALIDACION DE CARRITOS
        if (uow.RepoCarritos.GetById(carritoNEW.IdCarrito) != null)
        {
            throw new Exception("Ya existe ese carrito");
        }
        #endregion

        #region UBICACION
        if (uow.RepoUbicacion.GetById(carritoNEW.IdUbicacion) == null && carritoNEW.IdCarrito != 0)
        {
            throw new Exception("La ubicación seleccionada no existe.");
        }
        #endregion

        #region ESTADO
        if (carritoNEW.IdEstadoMantenimiento != 1)
        {
            throw new Exception("El carrito debe estar disponible al crearse");
        }

        if (uow.RepoEstadosMantenimiento.GetById(carritoNEW.IdEstadoMantenimiento) == null)
        {
            throw new Exception("El estado de mantenimiento seleccionado no es valido.");
        }
        #endregion

        #region MODELOS
        if (carritoNEW.IdModelo != null && uow.RepoModelo.GetById(carritoNEW.IdModelo.Value) == null)
        {
            throw new Exception("El modelo del carrito no es valido.");
        }

        Modelos? modeloCarrito = carritoNEW.IdModelo.HasValue ? uow.RepoModelo.GetById(carritoNEW.IdModelo.Value) : null;

        if (modeloCarrito != null)
        {
            IEnumerable<Modelos> modelosDelTipo = uow.RepoModelo.GetByTipo(modeloCarrito.IdTipoElemento);

            if (!modelosDelTipo.Any(m => m.IdModelo == carritoNEW.IdModelo))
            {
                throw new Exception("El modelo seleccionado no corresponde a un carrito.");
            }
        }
        #endregion

        #region NUMERO SERIE
        Carritos? serieHabilitado = uow.RepoCarritos.GetByNumeroSerie(carritoNEW.NumeroSerieCarrito);

        if (serieHabilitado != null)
        {
            if (serieHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro carrito con el mismo numero de serie.");
            }
            else
            {
                throw new Exception("El carrito ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion

        #region NOMBRE EQUIPO
        Carritos? equipoHabilitado = uow.RepoCarritos.GetByEquipo(carritoNEW.EquipoCarrito);

        if (equipoHabilitado != null)
        {
            if (equipoHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro carrito con el mismo nombre de equipo.");
            }
            else
            {
                throw new Exception("El carrito ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion
    }
    #endregion

    #region Validar Update
    private void ValidarReposUpdate(Carritos carritoNEW)
    {
        #region VALIDAR CARRITO
        Carritos? carritoOLD = uow.RepoCarritos.GetById(carritoNEW.IdCarrito);

        if (carritoOLD == null)
        {
            throw new Exception("El carrito que intenta actualizar no existe.");
        }
        #endregion

        #region ESTADO
        if (uow.RepoEstadosMantenimiento.GetById(carritoNEW.IdEstadoMantenimiento) == null)
        {
            throw new Exception("El estado de mantenimiento seleccionado no es valido.");
        }

        if (!uow.RepoCarritos.GetDisponible(carritoNEW.IdCarrito) && carritoNEW.IdEstadoMantenimiento == 2)
        {
            throw new Exception("No se puede actualizar un carrito que esta en prestamo");
        }

        if (carritoNEW.IdEstadoMantenimiento == 2)
        {
            throw new Exception("No se puede poner el estado del carrito en 'En Préstamo'.");
        }
        #endregion

        #region UBICACION
        if (uow.RepoUbicacion.GetById(carritoNEW.IdUbicacion) == null)
        {
            throw new Exception("La ubicación seleccionada no existe.");
        }

        if (carritoOLD?.IdUbicacion != carritoNEW.IdUbicacion)
        {
            IEnumerable<Notebooks> notebooksEnCarrito = uow.RepoNotebooks.GetByCarrito(carritoNEW.IdCarrito);

            //if (notebooksEnCarrito.Any(nb => uow.RepoNotebooks.EstaEnPrestamo(nb.IdElemento)))
            //{
            //    throw new Exception("No se puede cambiar la ubicacion: existen notebooks en prestamo dentro del carrito.");
            //}

            foreach (Notebooks? notebook in notebooksEnCarrito)
            {
                notebook.IdUbicacion = carritoNEW.IdUbicacion;
                uow.RepoNotebooks.Update(notebook);
            }
        }
        #endregion

        #region MODELOS
        if (carritoNEW.IdModelo != null)
        {
            Modelos? modelo = uow.RepoModelo.GetById(carritoNEW.IdModelo.Value);
            if (modelo == null)
            {
                throw new Exception("El modelo del carrito no es valido.");
            }

            if (!uow.RepoModelo.GetByTipo(modelo.IdTipoElemento).Any(m => m.IdModelo == carritoNEW.IdModelo))
            {
                throw new Exception("El modelo seleccionado no corresponde a un carrito.");
            }
        }
        #endregion

        #region NUMERO SERIE
        Carritos? serie = uow.RepoCarritos.GetByNumeroSerie(carritoNEW.NumeroSerieCarrito);

        if (serie != null && serie.IdCarrito != carritoNEW.IdCarrito)
        {
            throw new Exception("Ya existe otro carrito con el mismo numero de serie.");
        }

        Carritos? serieHabilitado = uow.RepoCarritos.GetByNumeroSerie(carritoNEW.NumeroSerieCarrito);

        if (carritoOLD?.NumeroSerieCarrito != carritoNEW.NumeroSerieCarrito && serieHabilitado != null)
        {
            if (serieHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro carrito con el mismo numero de serie.");
            }
            else
            {
                throw new Exception("El carrito ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion

        #region EQUIPO
        Carritos? equipo = uow.RepoCarritos.GetByEquipo(carritoNEW.EquipoCarrito);

        if (equipo != null && equipo.IdCarrito != carritoNEW.IdCarrito)
        {
            throw new Exception("Ya existe otro carrito con el mismo nombre de equipo.");
        }

        Carritos? equipoHabilitado = uow.RepoCarritos.GetByEquipo(carritoNEW.EquipoCarrito);

        if (carritoOLD?.EquipoCarrito != carritoNEW.EquipoCarrito && equipoHabilitado != null)
        {
            if (equipoHabilitado.Habilitado == true)
            {
                throw new Exception("Ya existe otro carrito con el mismo nombre de equipo.");
            }
            else
            {
                throw new Exception("El carrito ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
            }
        }
        #endregion

        #region CAPACIDAD
        int actuales = uow.RepoCarritos.GetCountByCarrito(carritoNEW.IdCarrito);

        if (actuales > carritoNEW.Capacidad)
        {
            throw new Exception($"No se puede reducir la capacidad a {carritoNEW.Capacidad} porque el carrito contiene {actuales} notebooks. Quite notebooks primero o elija una capacidad mayor o igual a {actuales}");
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