using BiblotecApi.Models;
using Api.Repositorio.IRepositorio;

namespace Api.Repositorio.IRepositorio
{
    public interface IPrendaRepositorio:IRepositorio<Prenda>
    {
        Task<Prenda> Actualizar(Prenda entidad);


    }
}
