using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class CategoriaRepositorio:Repositorio<Categoria>,ICategoriaRepositorio
    {
            private readonly BackendContext _db;

            public CategoriaRepositorio(BackendContext db) : base(db)
            {
                _db = db;
            }
            public async Task<Categoria> ActualizarCategoria(Categoria entidad)
            {
                entidad.FechaCreacion = DateTime.Now;
                _db.Categorias.Update(entidad);
                await _db.SaveChangesAsync();
                return entidad;
            }
        }
}
