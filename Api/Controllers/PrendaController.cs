using Api.Models;
using Api.Repositorio.IRepositorio;
using AutoMapper;
using BiblotecApi.Models;
using BiblotecApi.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Api.Models.APIResponse;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrendaController : ControllerBase
    {
        private readonly ILogger<PrendaController> _logger;
        private readonly IPrendaRepositorio _prendaRepo;
        private readonly ICategoriaRepositorio _categoriaRepo;
        private readonly IColorRepositorio _colorRepo;
        private readonly IMarcaRepositorio _marcaRepo;
        private readonly ITallaRepositorio _tallaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public PrendaController(ILogger<PrendaController> logger, ITallaRepositorio tallaRepo, IMarcaRepositorio marcaRepo, IColorRepositorio colorRepo, ICategoriaRepositorio categoriaRepo, IPrendaRepositorio prendaRepo, IMapper mapper)
        {
            _logger = logger;
            _prendaRepo = prendaRepo;
            _categoriaRepo=categoriaRepo;
            _colorRepo = colorRepo;
            _marcaRepo = marcaRepo;
            _tallaRepo = tallaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetPrenda()
        { 
         try
            {
                _logger.LogInformation("Obtener las Prendas");

                IEnumerable<Prenda> prendaList = await _prendaRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<PrendaDto>>(prendaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages=new List<string>() { ex.ToString() };
                return _response;
            }
            
            
        }


        [HttpGet("id:int", Name = "GetPrenda")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< ActionResult<APIResponse>> GetPrenda(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Prenda con Id" + id);
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.IsExitoso=false;
                    return BadRequest(_response);
                }
                var prenda = await _prendaRepo.Obtener(v => v.IdPrenda == id);
                if (prenda == null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    _response.IsExitoso=false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<PrendaDto>(prenda);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            } 
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages=new List<string>() { ex.ToString() };
                return _response;
            }
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearPrenda([FromBody] PrendaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _prendaRepo.Obtener(v => v.IdPrenda == createDto.IdPrenda) != null)
                {
                    ModelState.AddModelError("IdPrendaExiste", "La Prenda con ese IdPrenda ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _categoriaRepo.Obtener(v => v.IdCategoria == createDto.IdCategoria) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Categoria no existe!");
                    return BadRequest(ModelState);
                }

                if (await _colorRepo.Obtener(v => v.IdColor == createDto.IdColor) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id del Color no existe!");
                    return BadRequest(ModelState);
                }

                if (await _marcaRepo.Obtener(v => v.IdMarca == createDto.IdMarca) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Marca no existe!");
                    return BadRequest(ModelState);
                }

                if (await _tallaRepo.Obtener(v => v.IdTalla == createDto.IdTalla) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Talla no existe!");
                    return BadRequest(ModelState);
                }
                if(createDto==null)
                {
                    return BadRequest(createDto);
                }

                Prenda modelo = _mapper.Map<Prenda>(createDto);
                modelo.FechaCreacion = DateTime.Now;

                await _prendaRepo.Crear(modelo);

                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetPrenda", new { id = modelo.IdPrenda }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }

            
        }




        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  async Task<IActionResult> DeletePrenda(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var prenda = await _prendaRepo.Obtener(v => v.IdPrenda == id);
                if (prenda == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }


                 await _prendaRepo.Remover(prenda);
                _response.StatusCode=HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            
        }



        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async  Task<IActionResult> UpdatePrenda(int id, [FromBody] PrendaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdPrenda)
            {
                _response.IsExitoso=false;
                _response.StatusCode=HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _categoriaRepo.Obtener(v => v.IdCategoria == updateDto.IdCategoria) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Categoria no existe!");
                return BadRequest(ModelState);
            }
            if (await _colorRepo.Obtener(v => v.IdColor == updateDto.IdColor) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Color no existe!");
                return BadRequest(ModelState);
            }
            if (await _marcaRepo.Obtener(v => v.IdMarca == updateDto.IdMarca) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Marca no existe!");
                return BadRequest(ModelState);
            }
            if (await _tallaRepo.Obtener(v => v.IdTalla == updateDto.IdTalla) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Talla no existe!");
                return BadRequest(ModelState);
            }


            Prenda modelo = _mapper.Map<Prenda>(updateDto);

            await _prendaRepo.Actualizar(modelo);
            _response.StatusCode=HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialPrenda(int id, JsonPatchDocument<PrendaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var prenda = await _prendaRepo.Obtener(v => v.IdPrenda == id,tracked:false);

            PrendaUpdateDto prendaDto = _mapper.Map<PrendaUpdateDto>(prenda);


            if(prenda==null) return BadRequest();

            patchDto.ApplyTo(prendaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
              Prenda model = _mapper.Map<Prenda>(prendaDto);

             await _prendaRepo.Actualizar(model);
            _response.StatusCode =HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}        
