using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class  EmpleadoRepositorio : Repositorio<Empleado>, IEmpleadoRepositorio
    {
        private readonly BackendContext _db;

        public EmpleadoRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Empleado> ActualizarEmpleado(Empleado entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Empleados.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
