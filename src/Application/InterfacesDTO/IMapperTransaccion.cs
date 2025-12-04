using CapaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.InterfacesDTO;

public interface IMapperTransaccion
{
    public IEnumerable<TransaccionDTO> GetAllDTO();
}
