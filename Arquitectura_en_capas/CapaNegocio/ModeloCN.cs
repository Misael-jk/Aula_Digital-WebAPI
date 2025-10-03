using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;

namespace CapaNegocio
{
    public class ModeloCN
    {
        private readonly IRepoModelo repoModelo;
        private readonly IMapperModelo mapperModelo;

        public ModeloCN(IRepoModelo repoModelo, IMapperModelo mapperModelo)
        {
            this.repoModelo = repoModelo;
            this.mapperModelo = mapperModelo;
        }

        #region READ MODELO
        public IEnumerable<ModeloDTO> ObtenerModelos()
        {
            {
                return mapperModelo.GetAll();
            }
        }
        #endregion
    }
}
