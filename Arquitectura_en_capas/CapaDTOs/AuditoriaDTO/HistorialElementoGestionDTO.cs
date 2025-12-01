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
