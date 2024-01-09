using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface ITallaRepositorio : IRepositorio<Talla>
    {
        Task<Talla> ActualizarTalla(Talla entidad);

    }
}
