using CapaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.InterfacesDTO
{
    public interface IMapperNotebooksCarro
    {
        public IEnumerable<NotebooksCarroDTO> GetAll(int idCarrito);
    }
}
