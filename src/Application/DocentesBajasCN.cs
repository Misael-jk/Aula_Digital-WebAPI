using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;

namespace CapaNegocio;

public class DocentesBajasCN
{
    private readonly IRepoDocentes repoDocentes;
    private readonly IMapperDocentesBajas _mapperDocentesBajas;

    public DocentesBajasCN(IMapperDocentesBajas mapperDocentesBajas, IRepoDocentes repoDocentes)
    {
        _mapperDocentesBajas = mapperDocentesBajas;
        this.repoDocentes = repoDocentes;
    }

    public IEnumerable<DocentesBajasDTO> GetAllDTO()
    {
        return _mapperDocentesBajas.GetAllDTO();
    }

    public void HabilitarDocente(int idDocente)
    {
        Docentes? docentes = repoDocentes.GetById(idDocente);

        if (docentes == null)
        {
            throw new Exception("El elemento no existe.");
        }

        if (docentes.Habilitado)
        {
            throw new Exception("El elemento ya esta habilitado.");
        }

        docentes.Habilitado = true;
        docentes.FechaBaja = null;

        repoDocentes.Update(docentes);
    }
}
