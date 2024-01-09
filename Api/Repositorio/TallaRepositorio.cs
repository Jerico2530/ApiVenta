using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class TallaRepositorio : Repositorio<Talla>, ITallaRepositorio
    {
        private readonly BackendContext _db;

        public TallaRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Talla> ActualizarTalla(Talla entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Tallas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
