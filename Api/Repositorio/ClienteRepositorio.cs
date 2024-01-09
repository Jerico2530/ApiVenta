using Api.Repositorio.IRepositorio;
using BiblotecApi.Datos;
using BiblotecApi.Models;

namespace Api.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
    {
        private readonly BackendContext _db;

        public ClienteRepositorio(BackendContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Cliente> ActualizarCliente(Cliente entidad)
        {
            entidad.FechaCreacion = DateTime.Now;
            _db.Clientes.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
