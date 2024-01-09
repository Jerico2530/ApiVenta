using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class MarcaRepositorio : Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly BackendContext _db;

        public MarcaRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Marca> ActualizarMarca(Marca entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Marcas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
