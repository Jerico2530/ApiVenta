using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class PrendaRepositorio:Repositorio<Prenda> , IPrendaRepositorio
    {
        private readonly BackendContext _db;

        public PrendaRepositorio(BackendContext db):base(db)
        {
            _db = db;
        }
        public async Task<Prenda> Actualizar(Prenda entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Prendas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
        
        } 


}
