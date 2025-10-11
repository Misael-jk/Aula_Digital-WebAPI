using CapaEntidad;
using System.Transactions;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using CapaDatos.InterfaceUoW;

namespace CapaNegocio;

public class CarritosCN
{
    private readonly IMapperCarritos mapperCarritos;
    private readonly IUowCarritos uow;

    public CarritosCN(IMapperCarritos mapperCarritos, IUowCarritos uowCarritos)
    {
        this.mapperCarritos = mapperCarritos;
        uow = uowCarritos;
    }

    #region Mostrar Carritos
    public IEnumerable<CarritosDTO> MostrarCarritos()
    {
        return mapperCarritos.GetAllDTO();
    }
    #endregion

    #region INSERT CARRITO
    public void CrearCarrito(Carritos CarritoNEW, int idUsuario)
    {
        ValidarDatos(CarritoNEW);

        try
        {
            uow.BeginTransaction();

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
    public void ActualizarCarrito(Carritos carritoNEW, int idUsuario)
    {
        ValidarDatos(carritoNEW);

        try
        {
            uow.BeginTransaction();

            ValidarReposUpdate(carritoNEW);

            uow.RepoCarritos.Update(carritoNEW);

            HistorialCambios historial = new HistorialCambios
            {
                IdTipoAccion = 2,
                FechaCambio = DateTime.Now,
                Descripcion = $"Se actualizó el carrito con número de serie {carritoNEW.NumeroSerieCarrito}.",
                IdUsuario = idUsuario
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
    public void DeshabilitarCarrito(Carritos carritos, int idEstadoMantenimiento, int idUsuario)
    {
        ValidarDatos(carritos);

        try
        {
            uow.BeginTransaction();

            Carritos? carritoOLD = uow.RepoCarritos.GetById(carritos.IdCarrito);

            if (carritoOLD == null)
            {
                throw new Exception("El carrito no existe");
            }

            if (uow.RepoEstadosMantenimiento.GetById(carritos.IdEstadoMantenimiento) == null)
            {
                throw new Exception("El estado de mantenimiento seleccionado no es valido");
            }

            if (uow.RepoUbicacion.GetById(carritos.IdUbicacion) == null)
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

            if (!uow.RepoCarritos.GetDisponible(carritos.IdCarrito))
            {
                throw new Exception("No se puede deshabilitar un carrito que está en préstamo");
            }

            if (uow.RepoNotebooks.GetByCarrito(carritoOLD.IdCarrito).Any())
            {
                throw new Exception("No se puede deshabilitar un carrito que aún contiene notebooks.");
            }

            carritos.IdEstadoMantenimiento = idEstadoMantenimiento;
            carritos.FechaBaja = DateTime.Now;
            carritos.Habilitado = false;

            uow.RepoCarritos.Update(carritoOLD);

            HistorialCambios historial = new HistorialCambios()
            {
                IdTipoAccion = 3,
                IdUsuario = idUsuario,
                Descripcion = $"Se deshabilito el carrito con numero de serie {carritos.NumeroSerieCarrito}",
                FechaCambio = DateTime.Now
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = carritos.IdCarrito
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

    #region READ CARRITO
    public IEnumerable<Carritos> ListarCarritos()
    {
        return uow.RepoCarritos.GetAll();
    }
    #endregion

    #region AGREGAR NOTEBOOK AL CARRITO
    public void AddNotebook(int idCarrito, int posicion, int idNotebook, int idUsuario)
    {
        Carritos? carrito = uow.RepoCarritos.GetById(idCarrito);

        if (carrito == null)
        {
            throw new Exception("El carrito no existe");
        }

        Notebooks? notebooks = uow.RepoNotebooks.GetById(idNotebook);

        if (notebooks == null)
        {
            throw new Exception("Notebook no encontrado");
        }

        if (notebooks.IdEstadoMantenimiento == 2 && notebooks.IdCarrito != null)
        {
            throw new Exception("Un elemento en prestamo no puede estar asignado a un carrito");
        }

        if (!uow.RepoNotebooks.GetDisponible(idNotebook))
        {
            throw new Exception("La notebook no esta disponible");
        }

        if (notebooks.IdCarrito.HasValue && notebooks.IdCarrito != idCarrito)
        {
            throw new Exception("La notebook ya esta en otro carrito.");
        }

        if (uow.RepoCarritos.GetCountByCarrito(idCarrito) >= 25)
        {
            throw new Exception("El carrito que selecciono esta al maximo de notebooks");
        }

        if (notebooks.PosicionCarrito < 1 || notebooks.PosicionCarrito > 25)
        {
            throw new Exception("La posición en el carrito debe estar entre 1 y 25.");
        }

        if (notebooks.IdTipoElemento == 1 && notebooks.IdCarrito != 0)
        {
            if (notebooks.IdCarrito.HasValue && notebooks.PosicionCarrito.HasValue && uow.RepoNotebooks.DuplicatePosition(notebooks.IdCarrito.Value, notebooks.PosicionCarrito.Value))
            {
                throw new Exception("La posición dentro del carrito ya está ocupada, por favor elija otra.");
            }

            if (notebooks.PosicionCarrito <= 0)
            {
                throw new Exception("Debe asignar una posición válida dentro del carrito.");
            }
        }
        else
        {
            if (notebooks.PosicionCarrito != 0)
            {
                throw new Exception("Solo las notebooks pueden tener posición en carrito.");
            }
        }

        notebooks.IdUbicacion = carrito.IdUbicacion;
        notebooks.IdCarrito = idCarrito;
        notebooks.PosicionCarrito = posicion;

        uow.RepoNotebooks.Update(notebooks);


    }
    #endregion

    #region QUITAR NOTEBOOK DEL CARRITO
    public void RemoveNotebook(int idCarrito, int idNotebook, int idUsuario, int idUbicacion)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            Notebooks? notebooks = uow.RepoNotebooks.GetById(idNotebook);

            if (notebooks == null || notebooks.IdCarrito != idCarrito)
            {
                throw new Exception("La notebook no esta asignada a este carrito");
            }

            if (!uow.RepoNotebooks.GetDisponible(idNotebook))
            {
                throw new Exception("La notebook no esta disponible para quitar del carrito");
            }

            notebooks.IdUbicacion = idUbicacion;
            notebooks.IdCarrito = null;
            notebooks.PosicionCarrito = null;

            uow.RepoNotebooks.Update(notebooks);

            scope.Complete();
        }
    }
    #endregion

    #region VALIDACIONES

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
    }
    #endregion

    #endregion

    #region Validar Insert
    private void ValidarReposInsert(Carritos carritoNEW)
    {
        #region VALIDACION DE CARRITOS
        if(uow.RepoCarritos.GetById(carritoNEW.IdCarrito) == null)
        {
            throw new Exception("El carrito no existe");
        }
        #endregion

        #region UBICACION
        if (uow.RepoUbicacion.GetById(carritoNEW.IdUbicacion) == null)
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

            if (notebooksEnCarrito.Any(nb => !uow.RepoNotebooks.GetDisponible(nb.IdElemento)))
            {
                throw new Exception("No se puede cambiar la ubicacion: existen notebooks en prestamo dentro del carrito.");
            }

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
    }
    #endregion


}

