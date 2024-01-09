using BiblotecApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace BiblotecApi.Datos
{
    public class BackendContext : DbContext
    {
        public BackendContext(DbContextOptions<BackendContext> options) : base(options) 
        { 
        }
        public DbSet<Prenda> Prendas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Color> Colores { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Talla> Tallas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria()
                {
                    IdCategoria = 1,
                    NombreCategoria = "Ropa Urbano",
                    FechaCreacion = DateTime.Now,

                }
                );
            modelBuilder.Entity<Color>().HasData(
                new Color()
                {
                    IdColor = 1,
                    NombreColor = "Negro",
                    FechaCreacion = DateTime.Now,

                }
                );
            modelBuilder.Entity<Marca>().HasData(
                new Marca()
                {
                    IdMarca = 1,
                    NombreMarca = "DC",
                    FechaCreacion = DateTime.Now,

                }
                );
            modelBuilder.Entity<Talla>().HasData(
                new Talla()
                {
                    IdTalla = 1,
                    NumeroTalla = "XL",
                    FechaCreacion = DateTime.Now,

                }
                );
        }

    }
}
 