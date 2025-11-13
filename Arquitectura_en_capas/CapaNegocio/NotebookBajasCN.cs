using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDTOs.MantenimientoDTO;
using CapaEntidad;

namespace CapaNegocio;

public class NotebookBajasCN
{
    private readonly IUowNotebooks uow;
    private readonly IMapperNotebookBajas mapperNotebooks;

    public NotebookBajasCN(IMapperNotebookBajas mapperNotebooks, IUowNotebooks uowNotebooks)
    {
        this.mapperNotebooks = mapperNotebooks;
        uow = uowNotebooks;
    }

    public IEnumerable<NotebooksBajasDTO> GetAllNotebooks()
    {
        return mapperNotebooks.GetAll();
    }

    public Notebooks? ObtenerNotebookPorID(int idNotebook)
    {
        return uow.RepoNotebooks.GetById(idNotebook);
    }

    public void HabilitarNotebook(int idNotebook, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            Notebooks? notebook = uow.RepoNotebooks.GetById(idNotebook);

            if (notebook == null)
                throw new Exception("La notebook no existe.");

            if (notebook.Habilitado)
                throw new Exception("La notebook ya esta habilitada.");

            notebook.Habilitado = true;
            notebook.FechaBaja = null;

            uow.RepoNotebooks.Update(notebook);

            HistorialCambios historialCambios = new HistorialCambios
            {
                IdUsuario = idUsuario,
                IdTipoAccion = 4,
                Descripcion = $"Se habilito la notebook con numero de serie {notebook.NumeroSerie}",
                FechaCambio = DateTime.Now,
                Motivo = null
            };

            uow.RepoHistorialCambio.Insert(historialCambios);

            uow.RepoHistorialNotebook.Insert(new HistorialNotebooks
            {
                IdHistorialCambio = historialCambios.IdHistorialCambio,
                IdNotebook = idNotebook
            });

            uow.Commit();
        }
        catch (Exception ex)
        {
            uow.Rollback();
            throw new Exception("Error al habilitar la notebook: " + ex.Message);
        }
    }
}
