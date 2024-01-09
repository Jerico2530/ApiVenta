using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface IClienteRepositorio : IRepositorio<Cliente>
    {
        Task<Cliente> ActualizarCliente(Cliente entidad);
    }
}
