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
    public class TallaController : ControllerBase
    {
        private readonly ILogger<TallaController> _logger;
        private readonly ITallaRepositorio _tallaRepo;
        private readonly IMapper _mapper;
        private readonly BackendContext _context;
        protected APIResponse _response;

        public TallaController(ILogger<TallaController> logger, ITallaRepositorio tallaRepo, IMapper mapper, BackendContext context)
        {
            _logger = logger;
            _tallaRepo = tallaRepo;
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategoria()
        {
            try
            {
                _logger.LogInformation("Obtener las Talla");

                IEnumerable<Talla> tallaList = await _tallaRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<TallaDto>>(tallaList);
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


        [HttpGet("id:int", Name = "GetTalla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetTalla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Talla con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Talla = await _tallaRepo.Obtener(v => v.IdTalla == id);
                if (Talla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<TallaDto>(Talla);
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
        public async Task<ActionResult<APIResponse>> CrearTalla([FromBody] TallaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _tallaRepo.Obtener(v => v.NumeroTalla.ToLower() == createDto.NumeroTalla.ToLower()) != null)
                {
                    ModelState.AddModelError("TallaExiste", "La Talla con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Talla modelo = _mapper.Map<Talla>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _tallaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetTalla", new { id = modelo.IdTalla }, _response);
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
        public async Task<IActionResult> DeleteTalla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Talla = await _tallaRepo.Obtener(v => v.IdTalla == id);
                if (Talla == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                if (_context.Prendas.Any(p => p.IdTalla == id)) 
                {
                    ModelState.AddModelError("RelacionTalla", "No se puede eliminar la talla porque está relacionada con prendas existentes.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _tallaRepo.Remover(Talla);
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
        public async Task<IActionResult> UpdateTalla(int id, [FromBody] TallaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdTalla)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Talla modelo = _mapper.Map<Talla>(updateDto);

            await _tallaRepo.ActualizarTalla(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialTalla(int id, JsonPatchDocument<TallaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var talla = await _tallaRepo.Obtener(v => v.IdTalla == id, tracked: false);

            TallaUpdateDto tallaDto = _mapper.Map<TallaUpdateDto>(talla);


            if (talla == null) return BadRequest();

            patchDto.ApplyTo(tallaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Talla model = _mapper.Map<Talla>(tallaDto);

            await _tallaRepo.ActualizarTalla(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
