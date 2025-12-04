using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTOs
{
    public class NotebooksCarroDTO
    {
        public required string Equipo { get; set; }
        public required string Modelo { get; set; }
        public int? PosicionCarrito { get; set; }
        public required string NumeroSerie { get; set; }
    }
}
