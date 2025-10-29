using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDTOs;
using CapaEntidad;
using System.Text.RegularExpressions;

namespace CapaNegocio
{
    public class ModeloCN
    {
        private readonly IRepoModelo repoModelo;
        private readonly IRepoTipoElemento repoTipoElemento;
        private readonly IMapperModelo mapperModelo;

        public ModeloCN(IRepoModelo repoModelo, IMapperModelo mapperModelo, IRepoTipoElemento repoTipoElemento)
        {
            this.repoModelo = repoModelo;
            this.mapperModelo = mapperModelo;
            this.repoTipoElemento = repoTipoElemento;
        }

        #region READ MODELO
        public IEnumerable<ModeloDTO> ObtenerModelos()
        {
            {
                return mapperModelo.GetAll();
            }
        }
        #endregion

        #region INSERT MODELO
        public void CrearModelo(Modelos modeloNEW)
        {
            ValidarDatos(modeloNEW);

            Modelos? modeloOld = repoModelo.GetById(modeloNEW.IdModelo);

            if (modeloOld != null)
            {
                throw new Exception("Ya existe un modelo con ese Id");
            }

            if (repoTipoElemento.GetById(modeloNEW.IdTipoElemento) == null)
            {
                throw new Exception("No existe el TipoElemento con el Id proporcionado");
            }

            if (repoModelo.GetByNombre(modeloNEW.NombreModelo) != null)
            {
                throw new Exception("Ya existe un modelo con esa descripcion");
            }

            repoModelo.Insert(modeloNEW);
        }
        #endregion

        #region UPDATE MODELO
        public void ActualizarModelo(Modelos modeloNEW)
        {
            ValidarDatos(modeloNEW);

            Modelos? modeloOld = repoModelo.GetById(modeloNEW.IdModelo);

            if (modeloOld == null)
            {
                throw new Exception("No existe el modelo que desea actualizar");
            }

            if (repoTipoElemento.GetById(modeloNEW.IdTipoElemento) == null)
            {
                throw new Exception("No existe el TipoElemento con el Id proporcionado");
            }

            if (modeloOld.NombreModelo == modeloNEW.NombreModelo && modeloOld != null)
            {
                throw new Exception("Ya existe otro modelo con la misma descripcion");
            }

            repoModelo.Update(modeloNEW);
        }
        #endregion

        #region DESHABILITAR MODELO
        public void DeshabilitarModelo(int idModelo)
        {
            Modelos? modeloOld = repoModelo.GetById(idModelo);
            if (modeloOld == null)
            {
                throw new Exception("No se encontro el modelo");
            }

            repoModelo.Update(modeloOld);
        }
        #endregion

        #region FILTER
        public Modelos? ObtenerPorId(int idModelo)
        {
            return repoModelo.GetById(idModelo);
        }
        #endregion

        #region VALIDACIONES
        public void ValidarDatos(Modelos modeloNEW)
        {
            if (string.IsNullOrWhiteSpace(modeloNEW.NombreModelo))
            {
                throw new Exception("El nombre del modelo no puede estar vacia");
            }

            if (modeloNEW.NombreModelo.Length > 40)
            {
                throw new Exception("La nombre del modelo no puede superar los 40 caracteres");
            }

            if (!Regex.IsMatch(modeloNEW.NombreModelo, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s\-]+$"))
            {
                throw new Exception("El modelo contiene caracteres invalidas");
            }
        }
        #endregion
    }
}
