using CapaDTOs;

namespace CapaDatos.InterfacesDTO;

public interface IMapperRankingDocente
{
    public IEnumerable<RankingDocentesDTO> GetAllDTO();
}
