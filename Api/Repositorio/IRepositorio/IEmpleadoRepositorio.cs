using BiblotecApi.Models;

namespace Api.Repositorio.IRepositorio
{
    public interface IEmpleadoRepositorio : IRepositorio<Empleado>
    {
        Task<Empleado> ActualizarEmpleado(Empleado entidad);
    }
}
