using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface IVentaRepositorio : IRepositorio<Venta>
    {
        Task<Venta> ActualizarVenta(Venta entidad);
    }
}
