using CapaDTOs;
using CapaDTOs.AuditoriaDTO;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.InterfacesDTO
{
    public interface IMapperHistorialElementoG
    {
        public IEnumerable<HistorialElementoGestionDTO> GetAllDTO(int idElemento);
    }
}
