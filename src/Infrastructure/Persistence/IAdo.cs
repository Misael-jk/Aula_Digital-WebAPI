using System.Data;

namespace Infrastructure.Persistence;

public interface IAdo
{
    public IDbConnection GetDbConnection();
}
