using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface IMarcaRepositorio : IRepositorio<Marca>
    {
        Task<Marca> ActualizarMarca(Marca entidad);
    }
}
