using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDTOs;
using CapaEntidad;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CapaNegocio
{
    public class ElementosCN
    {
        private readonly IMapperElementos _mapperElementos;
        private readonly IUowElementos uow;

        public ElementosCN(IMapperElementos mapperElementos, IUowElementos uow)
        {
            _mapperElementos = mapperElementos;
            this.uow = uow;
        }

        // PROCEDIMIENTOS CRUD
        #region Metodos de Lectura para la UI DTOs

        #region Mostrar Elementos completos
        public IEnumerable<ElementosDTO> ObtenerElementos()
        {
            return _mapperElementos.GetAllDTO();
        }
        #endregion

        //#region mostrar por carrito 
        //public IEnumerable<ElementosDTO> ObtenerPorCarrito(int idCarrito)
        //{
        //    return _mapperElementos.GetByCarritoDTO(idCarrito);
        //}
        //#endregion

        //#region mostrar por Codigo de Barra
        //public ElementosDTO? ObtenerPorCodigoBarra(string codigoBarra)
        //{
        //    return _mapperElementos.GetByCodigoBarraDTO(codigoBarra);
        //}
        //#endregion

        //#region mostrar por tipo
        //public IEnumerable<ElementosDTO> ObtenerPorTipo(int idTipoElemento)
        //{
        //    return _mapperElementos.GetByTipoDTO(idTipoElemento);
        //}
        //#endregion

        #endregion

        #region INSERT ELEMENTO
        public void CrearElemento(Elemento elementoNEW, int idUsuario)
        {
            ValidarDatos(elementoNEW);

            try
            {
                uow.BeginTransaction();

                ValidarInsert(elementoNEW);

                uow.RepoElemento.Insert(elementoNEW);

                HistorialCambios historial = new HistorialCambios
                {
                    IdTipoAccion = 1,
                    FechaCambio = DateTime.Now,
                    Descripcion = $"Se creó el elemento con número de serie {elementoNEW.NumeroSerie}.",
                    Motivo = null,
                    IdUsuario = idUsuario
                };

                uow.RepoHistorialCambio.Insert(historial);

                uow.RepoHistorialElementos.Insert(new HistorialElementos
                {
                    IdHistorialCambio = historial.IdHistorialCambio,
                    IdElementos = elementoNEW.IdElemento
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

        #region UPDATE ELEMENTO
        public void ActualizarElemento(Elemento elementoNEW, int idUsuario)
        {
            ValidarDatos(elementoNEW);

            try
            {
                uow.BeginTransaction();
                ValidarUpdate(elementoNEW);

                uow.RepoElemento.Update(elementoNEW);

                HistorialCambios historial = new HistorialCambios
                {
                    IdTipoAccion = 2,
                    FechaCambio = DateTime.Now,
                    Descripcion = $"Se modifico el elemento con número de serie {elementoNEW.NumeroSerie}.",
                    IdUsuario = idUsuario
                };

                uow.RepoHistorialCambio.Insert(historial);

                uow.RepoHistorialElementos.Insert(new HistorialElementos
                {
                    IdHistorialCambio = historial.IdHistorialCambio,
                    IdElementos = elementoNEW.IdElemento
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

        #region DESHABILITAR ELEMENTO
        public void DeshabilitarElemento(int idElemento, int idEstadoMantenimiento, int idUsuario)
        {
            try
            {
                uow.BeginTransaction();

                Elemento? elemento = uow.RepoElemento.GetById(idElemento);

                if (elemento == null)
                {
                    throw new Exception("El elemento no existe.");
                }

                if (elemento.IdEstadoMantenimiento == 2)
                {
                    throw new Exception("No se puede deshabilitar un elemento que está en préstamo.");
                }

                if (!elemento.Habilitado)
                {
                    throw new Exception("El elemento ya está deshabilitado.");
                }

                elemento.Habilitado = false;
                elemento.IdEstadoMantenimiento = idEstadoMantenimiento;
                elemento.FechaBaja = DateTime.Now;

                uow.RepoElemento.Update(elemento);

                HistorialCambios historial = new HistorialCambios
                {
                    IdTipoAccion = 3,
                    FechaCambio = DateTime.Now,
                    Descripcion = $"Se dio de baja el elemento con número de serie {elemento.NumeroSerie}.",
                    IdUsuario = idUsuario
                };

                uow.RepoHistorialCambio.Insert(historial);

                uow.RepoHistorialElementos.Insert(new HistorialElementos
                {
                    IdHistorialCambio = historial.IdHistorialCambio,
                    IdElementos = elemento.IdElemento
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



        // FILTROS PARA LA UI
        #region Filtros
        public IEnumerable<Modelos> ListarModelos()
        {
            return uow.RepoModelo.GetAll();
        }

        public IEnumerable<Ubicacion> ListarUbicaciones()
        {
            return uow.RepoUbicacion.GetAll();
        }

        public IEnumerable<EstadosMantenimiento> ListarEstadoMantenimiento()
        {
            return uow.RepoEstadosMantenimiento.GetAll();
        }
        #endregion



        // REPOS PARA CONSULATAS DE LA UI PARA ELEMENTOS
        #region ELEMENTOS
        public Elemento? ObtenerPorId(int idElemento)
        {
            return uow.RepoElemento.GetById(idElemento);
        }

        public Elemento? ObtenerPorNumeroSerie(string numeroSerie)
        {
            return uow.RepoElemento.GetByNumeroSerie(numeroSerie);
        }
        #endregion

        #region ESTADOS MANTENIMIENTO
        public IEnumerable<EstadosMantenimiento> ObtenerEstadosMantenimiento()
        {
            return uow.RepoEstadosMantenimiento.GetAll();
        }
        #endregion

        #region TIPOS ELEMENTO
        public IEnumerable<TipoElemento> ListarTiposElemento()
        {
            return uow.RepoTipoElemento.GetAll();
        }
        #endregion

        #region MODELOS
        public IEnumerable<Modelos> ObtenerModelosPorTipo(int idTipoElemento)
        {
            return uow.RepoModelo.GetByTipo(idTipoElemento);
        }
        #endregion

        #region VARIANTES ELEMENTO
        public IEnumerable<VariantesElemento> ObtenerVariantesPorTipo(int idTipoElemento)
        {
            return uow.RepoVarianteElemento.GetByTipo(idTipoElemento);
        }

        public VariantesElemento? ObtenerVariantePorID(int idVariante)
        {
            return uow.RepoVarianteElemento.GetById(idVariante);
        }
        #endregion

        #region UBICACIONES
        public IEnumerable<Ubicacion> ObtenerUbicaciones()
        {
            return uow.RepoUbicacion.GetAll();
        }
        #endregion




        // VALIDACIONES 
        #region VALIDACIONES
        private void ValidarDatos(Elemento elemento)
        {
            if (string.IsNullOrEmpty(elemento.NumeroSerie))
            {
                throw new Exception("El numero de serie es obligatorio");
            }

            if (elemento.NumeroSerie.Length > 40)
            {
                throw new ValidationException("El número de serie no puede superar los 40 caracteres.");
            }

            if (!Regex.IsMatch(elemento.NumeroSerie, @"^[A-Z0-9\-]+$"))
            {
                throw new ValidationException("El número de serie contiene caracteres inválidos.");
            }

            if (string.IsNullOrEmpty(elemento.CodigoBarra))
            {
                throw new Exception("El código de barras es obligatorio");
            }

            if (elemento.CodigoBarra.Length > 40)
            {
                throw new ValidationException("El codigo de barra no puede superar los 40 caracteres.");
            }

            if (!Regex.IsMatch(elemento.CodigoBarra, @"^[A-Z0-9\-]+$"))
            {
                throw new ValidationException("El codigo de barra contiene caracteres inválidos.");
            }

            if ((string.IsNullOrEmpty(elemento.Patrimonio)))
            {
                throw new Exception("El patrimonio es obligatorio");
            }

            if (elemento.Patrimonio.Length > 40)
            {
                throw new ValidationException("El patrimonio no puede superar los 40 caracteres.");
            }

            if (!Regex.IsMatch(elemento.Patrimonio, @"^[A-Z0-9\-]+$"))
            {
                throw new ValidationException("El patrimonio contiene caracteres inválidos.");
            }
        }
        #endregion

        #region VALIDACION INSERT
        public void ValidarInsert(Elemento elementoNEW)
        {
            #region VALIDACION ELEMENTO
            if (elementoNEW.IdElemento != 0 && uow.RepoElemento.GetById(elementoNEW.IdElemento) != null)
            {
                throw new Exception("El elemento ya existe");
            }
            #endregion

            #region NUMERO SERIE
            Elemento? nroSerieHabilitado = uow.RepoElemento.GetByNumeroSerie(elementoNEW.NumeroSerie);

            if (nroSerieHabilitado != null)
            {
                if (nroSerieHabilitado.Habilitado == true)
                {
                    throw new Exception("El elemento ya existe con ese numero de serie y está habilitado.");
                }
                else
                {
                    throw new Exception("El elemento ya existe con ese numero de serie pero está deshabilitado, por favor habilitelo antes de crear uno nuevo.");
                }
            }
            #endregion

            #region CODIGO BARRA
            Elemento? codigoBarraHabilitado = uow.RepoElemento.GetByCodigoBarra(elementoNEW.CodigoBarra);

            if (codigoBarraHabilitado != null)
            {
                if (codigoBarraHabilitado.Habilitado == true)
                {
                    throw new Exception("El elemento ya existe con ese codigo de barra y está habilitado.");
                }
                else
                {
                    throw new Exception("El elemento ya existe con ese codigo de barra pero está deshabilitado, por favor habilitelo antes de crear uno nuevo.");
                }
            }
            #endregion

            #region PATRIMONIO
            Elemento? patrimonioHabilitado = uow.RepoElemento.GetByPatrimonio(elementoNEW.Patrimonio);

            if (patrimonioHabilitado != null)
            {
                if (patrimonioHabilitado.Habilitado == true)
                {
                    throw new Exception("El elemento ya existe con ese patrimonio y está habilitado.");
                }
                else
                {
                    throw new Exception("El elemento ya existe con ese patrimonio pero está deshabilitado, por favor habilitelo antes de crear uno nuevo.");
                }
            }
            #endregion

            #region ESTADO
            if (elementoNEW.IdEstadoMantenimiento != 1)
            {
                throw new Exception("El estado del elemento debe ser 'Disponible' al momento de crearlo");
            }

            if (uow.RepoEstadosMantenimiento.GetById(elementoNEW.IdEstadoMantenimiento) == null)
            {
                throw new Exception("Estado de mantenimiento del elemento invalido");
            }
            #endregion

            #region UBICACION
            if (uow.RepoUbicacion.GetById(elementoNEW.IdUbicacion) == null)
            {
                throw new Exception("Ubicacion del elemento invalida");
            }
            #endregion

            #region MODELO
            if (elementoNEW.IdModelo != 0)
            {
                Modelos? modelo = uow.RepoModelo.GetById(elementoNEW.IdModelo);

                if (modelo == null)
                {
                    throw new Exception("El modelo del elemento es inválido.");
                }

                if (modelo.IdTipoElemento != elementoNEW.IdTipoElemento)
                {
                    throw new Exception("El modelo seleccionado no corresponde al tipo de elemento.");
                }
            }
            #endregion

            #region VARIANTE ELEMENTO
            if (elementoNEW.IdVarianteElemento != 0)
            {
                VariantesElemento? variante = uow.RepoVarianteElemento.GetById(elementoNEW.IdVarianteElemento ?? 0);
                if (variante == null)
                {
                    throw new ValidationException("Variante del elemento inválida.");
                }

                if (variante.IdTipoElemento != elementoNEW.IdTipoElemento)
                {
                    throw new ValidationException("La variante seleccionada no corresponde al tipo de elemento.");
                }

                if (variante.IdModelo != 0)
                {
                    if (elementoNEW.IdModelo == 0)
                    {
                        throw new ValidationException("La variante seleccionada requiere que se especifique un modelo.");
                    }

                    if (variante.IdModelo != elementoNEW.IdModelo)
                    {
                        throw new ValidationException("La variante seleccionada no corresponde al modelo elegido.");
                    }
                }
            }
            #endregion

            #region TIPO ELEMENTO
            if (uow.RepoTipoElemento.GetById(elementoNEW.IdTipoElemento) == null)
            {
                throw new Exception("El tipo elemento es invalido");
            }
            #endregion
        }
        #endregion

        #region VALIDACION UPDATE
        public void ValidarUpdate(Elemento elementoNEW)
        {
            #region VALIDAR ELEMENTO
            Elemento? elementoOLD = uow.RepoElemento.GetById(elementoNEW.IdElemento);

            if (elementoOLD == null)
            {
                throw new Exception("El elemento no existe");
            }
            #endregion

            #region UBICACION
            if (uow.RepoUbicacion.GetById(elementoNEW.IdUbicacion) == null)
            {
                throw new Exception("Ubicacion del elemento invalida");
            }
            #endregion

            #region MODELO
            if (elementoNEW.IdModelo != 0)
            {
                Modelos? modelo = uow.RepoModelo.GetById(elementoNEW.IdModelo);

                if (modelo == null)
                {
                    throw new ValidationException("Modelo del elemento inválido.");
                }

                if (modelo.IdTipoElemento != elementoNEW.IdTipoElemento)
                {
                    throw new ValidationException("El modelo seleccionado no corresponde al tipo de elemento.");
                }
            }
            #endregion

            #region PATRIMONIO
            if (uow.RepoElemento.GetByPatrimonio(elementoNEW.Patrimonio) == null)
            {
                throw new Exception("El patrimonio no existe en otro elemento, por favor elija uno existente");
            }

            Elemento? patrimonioHabilitado = uow.RepoElemento.GetByPatrimonio(elementoNEW.Patrimonio);

            if (elementoOLD.Patrimonio != elementoNEW.Patrimonio && patrimonioHabilitado != null)
            {
                if (patrimonioHabilitado.Habilitado == true)
                {
                    throw new Exception("Ya existe otro elemento con el mismo patrimonio.");
                }
                else
                {
                    throw new Exception("El elemento ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
                }
            }
            #endregion

            #region TIPO ELEMENTO
            if (uow.RepoModelo.GetByTipo(elementoNEW.IdTipoElemento) == null)
            {
                throw new Exception("El modelo debe ser correspondiente al tipo de elemento");
            }
            #endregion

            #region NUMERO DE SERIE
            Elemento? nroSerieHabilitado = uow.RepoElemento.GetByNumeroSerie(elementoNEW.NumeroSerie);

            if (elementoOLD.NumeroSerie != elementoNEW.NumeroSerie && nroSerieHabilitado != null)
            {
                if (nroSerieHabilitado.Habilitado == true)
                {
                    throw new Exception("Ya existe otro elemento con el mismo numero de serie.");
                }
                else
                {
                    throw new Exception("El elemento ya existe pero está deshabilitado, por favor habilitelo antes de actualizar uno nuevo.");
                }
            }
            #endregion

            #region CODIGO DE BARRA
            Elemento? codigoBarraHabilitado = uow.RepoElemento.GetByCodigoBarra(elementoNEW.CodigoBarra);

            if (elementoOLD.CodigoBarra != elementoNEW.CodigoBarra && codigoBarraHabilitado != null)
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

            #region ESTADO
            if (elementoNEW.IdEstadoMantenimiento == 2 && elementoOLD.IdEstadoMantenimiento != 2)
            {
                throw new Exception("No se puede cambiar el estado a 'En Prestamo' por que no se hiso un prestamo");
            }

            if (elementoOLD.IdEstadoMantenimiento == 2 && elementoNEW.IdEstadoMantenimiento != 2)
            {
                throw new Exception("No se puede cambiar el estado de un elemento en prestamo sin terminar su devolucion");
            }
            #endregion

            #region VARIANTE ELEMENTO
            if (elementoNEW.IdVarianteElemento != 0)
            {
                VariantesElemento? variante = uow.RepoVarianteElemento.GetById(elementoNEW.IdVarianteElemento.Value);

                if (variante == null)
                {
                    throw new ValidationException("Variante del elemento inválida.");
                }

                if (variante.IdTipoElemento != elementoNEW.IdTipoElemento)
                {
                    throw new ValidationException("La variante seleccionada no corresponde al tipo de elemento.");
                }

                if (variante.IdModelo != 0)
                {
                    if (elementoNEW.IdModelo == 0)
                    {
                        throw new ValidationException("La variante seleccionada requiere que se especifique un modelo.");
                    }

                    if (variante.IdModelo != elementoNEW.IdModelo)
                    {
                        throw new ValidationException("La variante seleccionada no corresponde al modelo elegido.");
                    }
                }
            }
            #endregion

        }
        #endregion
    }
}
