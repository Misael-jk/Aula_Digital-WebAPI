namespace CapaDTOs.AuditoriaDTO;

public class HistorialNotebookGestionDTO
{
    public int IdHistorialNotebook { get; set; }
    public required string Usuario { get; set; }
    public required string AccionRealizada { get; set; }
    public DateTime FechaCambio { get; set; }
}
