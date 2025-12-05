using System.Net.Mail;

namespace Core.ValueObjects;

public class EmailAddress : IEquatable<EmailAddress>
{
    public string Email { get; }

    public EmailAddress(string Email)
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            throw new ArgumentException("La direccion de correo electronico no puede estar vacia.", nameof(Email));
        }

        if (!IsValidFormat(Email))
        {
            throw new FormatException($"'{Email}' no es un formato de correo electrónico valido.");
        }

        this.Email = Email;
    }

    private bool IsValidFormat(string email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            throw new Exception("Email invalido, intente de nuevo");
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EmailAddress);
    }

    public bool Equals(EmailAddress? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Email == other.Email;
    }

    public override int GetHashCode()
    {
        return Email.GetHashCode();
    }
}
