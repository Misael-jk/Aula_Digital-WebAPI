using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDTOs.AuditoriaDTO
{
    public class HistorialElementoGestionDTO
    {
        public int IdHistorialElemento { get; set; }
        public required string Usuario { get; set; }
        public required string AccionRealizada { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
