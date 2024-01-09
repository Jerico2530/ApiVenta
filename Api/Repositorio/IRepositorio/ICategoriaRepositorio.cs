using BiblotecApi.Models;
using System.Threading.Tasks;

namespace Api.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio:IRepositorio<Categoria>
    {
        Task<Categoria> ActualizarCategoria(Categoria entidad);
    }
}
