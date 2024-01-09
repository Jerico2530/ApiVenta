using Api.Models;
using Api.Repositorio.IRepositorio;
using AutoMapper;
using BiblotecApi.Models.Dto;
using BiblotecApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BiblotecApi.Datos;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly ILogger<MarcaController> _logger;
        private readonly IMarcaRepositorio _marcaRepo;
        private readonly IMapper _mapper;
        private readonly BackendContext _context;
        protected APIResponse _response;

        public MarcaController(ILogger<MarcaController> logger, IMarcaRepositorio marcaRepo, IMapper mapper, BackendContext context)
        {
            _logger = logger;
            _marcaRepo = marcaRepo;
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetMarca()
        {
            try
            {
                _logger.LogInformation("Obtener las Marca");

                IEnumerable<Marca> marcaList = await _marcaRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<MarcaDto>>(marcaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }
            

        }


        [HttpGet("id:int", Name = "GetMarca")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetMarca(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Marca con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Marca = await _marcaRepo.Obtener(v => v.IdMarca == id);
                if (Marca == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<MarcaDto>(Marca);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;
            }
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearMarca([FromBody] MarcaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _marcaRepo.Obtener(v => v.NombreMarca.ToLower() == createDto.NombreMarca.ToLower()) != null)
                {
                    ModelState.AddModelError("MarcaExiste", "La Categoria con ese Marca ya existe!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Marca modelo = _mapper.Map<Marca>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _marcaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetMarca", new { id = modelo.IdMarca }, _response);
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
        public async Task<IActionResult> DeleteMarca(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Marca = await _marcaRepo.Obtener(v => v.IdMarca == id);
                if (Marca == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (_context.Prendas.Any(p => p.IdMarca == id))
                {
                    ModelState.AddModelError("RelacionMarca", "No se puede eliminar la marca porque está relacionada con prendas existentes.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _marcaRepo.Remover(Marca);
                _response.StatusCode = HttpStatusCode.NoContent;

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
        public async Task<IActionResult> UpdateMarca(int id, [FromBody] MarcaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdMarca)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Marca modelo = _mapper.Map<Marca>(updateDto);

            await _marcaRepo.ActualizarMarca(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialMarca(int id, JsonPatchDocument<MarcaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var marca = await _marcaRepo.Obtener(v => v.IdMarca == id, tracked: false);

            MarcaUpdateDto marcaDto = _mapper.Map<MarcaUpdateDto>(marca);


            if (marca == null) return BadRequest();

            patchDto.ApplyTo(marcaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Marca model = _mapper.Map<Marca>(marcaDto);

            await _marcaRepo.ActualizarMarca(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
