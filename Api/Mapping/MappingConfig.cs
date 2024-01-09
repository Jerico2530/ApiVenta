using AutoMapper;
using BiblotecApi.Models;
using BiblotecApi.Models.Dto;

namespace Api.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        { 

            CreateMap<Prenda, PrendaDto>().ReverseMap();
            CreateMap<Prenda, PrendaCreateDto>().ReverseMap();
            CreateMap<Prenda, PrendaUpdateDto>().ReverseMap();

            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaCreateDto>().ReverseMap();
            CreateMap<Categoria, CategoriaUpdateDto>().ReverseMap();


            CreateMap<Color, ColorDto>().ReverseMap();
            CreateMap<Color, ColorCreateDto>().ReverseMap();
            CreateMap<Color, ColorUpdateDto>().ReverseMap();


            CreateMap<Marca, MarcaDto>().ReverseMap();
            CreateMap<Marca, MarcaCreateDto>().ReverseMap();
            CreateMap<Marca, MarcaUpdateDto>().ReverseMap();


            CreateMap<Talla, TallaDto>().ReverseMap();
            CreateMap<Talla, TallaCreateDto>().ReverseMap();
            CreateMap<Talla, TallaUpdateDto>().ReverseMap();

            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<Cliente, ClienteCreateDto>().ReverseMap();
            CreateMap<Cliente, ClienteUpdateDto>().ReverseMap();

            CreateMap<Empleado, EmpleadoDto > ().ReverseMap();
            CreateMap<Empleado, EmpleadoCreateDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoUpdateDto>().ReverseMap();

            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<Venta, VentaCreateDto>().ReverseMap();
            CreateMap<Venta, VentaUpdateDto>().ReverseMap();
        }

    }
}
