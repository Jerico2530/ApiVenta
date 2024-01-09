using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface IColorRepositorio : IRepositorio<Color>
    {
        Task<Color> ActualizarColor(Color entidad);
    }
}
