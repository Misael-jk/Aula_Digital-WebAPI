namespace Core.Entities.Aggregates.Usuario;

public class RefreshToken
{
    public int IdRefreshToken { get; set; }
    public int IdUsuario { get; set; }
    public required string Token { get; set; }
    public DateTime Emitido { get; set; }
    public DateTime Expiracion { get; set; }
    public bool Revocado { get; set; }
    public DateTime? FechaRevocado { get; set; }
    public string? ReemplazoPorToken { get; set; }


    #region Revocar Token
    public void Revocar(DateTime fechaRevocado, string? reemplazoPorToken = null)
    {
        if (Revocado)
        {
            return;
        }

        Revocado = true;

        FechaRevocado = fechaRevocado;
        ReemplazoPorToken = reemplazoPorToken;
    }
    #endregion
}
