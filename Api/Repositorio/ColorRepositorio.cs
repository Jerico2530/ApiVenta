using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class ColorRepositorio : Repositorio<Color>, IColorRepositorio
    {
        private readonly BackendContext _db;

        public ColorRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Color> ActualizarColor(Color entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Colores.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
