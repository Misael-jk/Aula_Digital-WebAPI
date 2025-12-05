namespace Core.Entities.Aggregates.Usuario;

public class Usuarios
{
    public int IdUsuario {get; set;}
    public required string Usuario {get; set;}
    public required string Password {get; set;}
    public required string Nombre {get; set;}
    public required string Apellido {get; set;}
    public int IdRol { get; set; }
    public required string Email {get; set;}
    public string? FotoPerfil { get; set; }
    public bool Habilitado { get; set; }
    public DateTime? FechaBaja { get; set; }


    public List<RefreshToken> Token = new();
    private List<UsuarioHorario> Horarios = new();


    #region Refrescar Token
    public void AddRefreshToken(string token, DateTime emitido, DateTime expiracion)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Token vacío.");
        }

        if (Token.Any(t => t.Token == token))
        {
            throw new InvalidOperationException("Token duplicado");
        }

        Token.Add(new RefreshToken
        {
            IdUsuario = IdUsuario,
            Token = token,
            Emitido = emitido,
            Expiracion = expiracion,
            Revocado = false,
            FechaRevocado = null,
            ReemplazoPorToken = null
        });
    }
    #endregion

    #region Revocar Token
    public void RevokeToken(string token, DateTime? fechaRevocado = null, string? reemplazoPorToken = null)
    {
        RefreshToken t = Token.SingleOrDefault(x => x.Token == token) 
            ?? throw new InvalidOperationException("Token no encontrado");

        t.Revocar(fechaRevocado ?? DateTime.UtcNow, reemplazoPorToken);
    }
    #endregion

    #region Añadir Horario
    public void AddHorario(int idHorario)
    {
        if (Horarios.Any(h => h.IdHorario == idHorario))
        {
            return;
        }

        Horarios.Add(new UsuarioHorario
        {
            IdUsuario = IdUsuario,
            IdHorario = idHorario
        });
    }
    #endregion

    #region Remover Horario
    public void RemoveHorario(int idHorario)
    {
        UsuarioHorario? item = Horarios.SingleOrDefault(h => h.IdHorario == idHorario);

        if (item is not null)
        {
            Horarios.Remove(item);
        }
    }
    #endregion

    #region Deshabilitar
    public void Disable(DateTime fechaBaja)
    {
        Habilitado = false;
        FechaBaja = fechaBaja;
    }
    #endregion
}
