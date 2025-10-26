using CapaDatos.Interfaces;
using CapaDatos.InterfacesDTO;
using CapaDatos.InterfaceUoW;
using CapaDatos.Repos;
using CapaDTOs;
using CapaEntidad;
using System.Configuration;

namespace CapaNegocio;

public class CarritosBajasCN
{
    private readonly IUowCarritos uow;
    private readonly IMapperCarritosBajas _mapperCarritosBajas;

    public CarritosBajasCN(IMapperCarritosBajas mapperCarritosBajas, IUowCarritos uowCarritos)
    {
        _mapperCarritosBajas = mapperCarritosBajas;
        uow = uowCarritos;
    }

    public IEnumerable<CarritosBajasDTO> GetAllDTO()
    {
        return _mapperCarritosBajas.GetAllDTO();
    }

    public void HabilitarCarrito(int idCarrito, int idUsuario)
    {
        try
        {
            uow.BeginTransaction();

            Carritos? carritos = uow.RepoCarritos.GetById(idCarrito);

            if (carritos == null)
            {
                throw new Exception("El elemento no existe.");
            }

            if (carritos.Habilitado)
            {
                throw new Exception("El elemento ya esta habilitado.");
            }

            if (uow.RepoCarritos.GetDisponible(carritos.IdCarrito))
            {
                throw new Exception("No se puede habilitar un carrito disponible");
            }

            carritos.Habilitado = true;
            carritos.IdEstadoMantenimiento = 1;
            carritos.FechaBaja = null;

            uow.RepoCarritos.Update(carritos);

            HistorialCambios historial = new HistorialCambios()
            {
                IdTipoAccion = 2,
                IdUsuario = idUsuario,
                Descripcion = $"Se habilito el carrito con numero de serie {carritos.EquipoCarrito}",
                Motivo = null,
                FechaCambio = DateTime.Now
            };

            uow.RepoHistorialCambio.Insert(historial);

            uow.RepoHistorialCarrito.Insert(new HistorialCarritos
            {
                IdHistorialCambio = historial.IdHistorialCambio,
                IdCarrito = carritos.IdCarrito
            });

            uow.Commit();
        }
        catch (Exception)
        {
            uow.Rollback();
            throw;
        }
    }
}
