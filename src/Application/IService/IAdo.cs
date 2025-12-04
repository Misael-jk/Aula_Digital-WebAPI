using System.Data;

namespace Application.IService;

public interface IAdo
{
    public IDbConnection GetDbConnection();
}
