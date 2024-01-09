using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {
        private readonly BackendContext _db;

        public VentaRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Venta> ActualizarVenta(Venta entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Ventas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
