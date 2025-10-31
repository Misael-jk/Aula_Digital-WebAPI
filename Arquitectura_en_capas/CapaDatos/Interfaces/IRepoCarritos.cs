using CapaEntidad;
using System.Data;

namespace CapaDatos.Interfaces;

public interface IRepoCarritos
{
    public void Insert(Carritos carrito);
    public void Update(Carritos carrito);
    public void Delete(int idCarrito);
    public IEnumerable<Carritos> GetAll();
    public Carritos? GetById(int idCarrito);
    public Carritos? GetByNumeroSerie(string numeroSerieCarrito);
    public Carritos? GetByEquipo(string equipo);
    public int GetCountByCarrito(int idCarrito);
    public void UpdateDisponible(int idCarrito, int idEstadoMantenimiento);
    public bool GetDisponible(int idCarrito);
    public bool GetEstadoEnPrestamo(int idCarrito);
    public void SetTransaction(IDbTransaction? transaction);
}
