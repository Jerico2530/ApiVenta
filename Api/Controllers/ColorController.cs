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
    public class ColorController : ControllerBase
    {
        private readonly ILogger<ColorController> _logger;
        private readonly IColorRepositorio _colorRepo;
        private readonly IMapper _mapper;
        private readonly BackendContext _context;
        protected APIResponse _response;

        public ColorController(ILogger<ColorController> logger, IColorRepositorio colorRepo, IMapper mapper, BackendContext context)
        {
            _logger = logger;
            _colorRepo = colorRepo;
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetColor()
        {
            try
            {
                _logger.LogInformation("Obtener las Color");

                IEnumerable<Color> colorList = await _colorRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<ColorDto>>(colorList);
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


        [HttpGet("id:int", Name = "GetColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetColor(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Color con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Color = await _colorRepo.Obtener(v => v.IdColor == id);
                if (Color == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<ColorDto>(Color);
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
        public async Task<ActionResult<APIResponse>> CrearColor([FromBody] ColorCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _colorRepo.Obtener(v => v.NombreColor.ToLower() == createDto.NombreColor.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Color con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Color modelo = _mapper.Map<Color>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _colorRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetColor", new { id = modelo.IdColor }, _response);
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
        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Color = await _colorRepo.Obtener(v => v.IdColor == id);
                if (Color == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (_context.Prendas.Any(p => p.IdTalla == id))
                {
                    ModelState.AddModelError("RelacionColor", "No se puede eliminar la color porque está relacionada con prendas existentes.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _colorRepo.Remover(Color);
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
        public async Task<IActionResult> UpdateColor(int id, [FromBody] CategoriaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdCategoria)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Color modelo = _mapper.Map<Color>(updateDto);

            await _colorRepo.ActualizarColor(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialColor(int id, JsonPatchDocument<ColorUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var color = await _colorRepo.Obtener(v => v.IdColor == id, tracked: false);

            ColorUpdateDto colorDto = _mapper.Map<ColorUpdateDto>(color);


            if (color == null) return BadRequest();

            patchDto.ApplyTo(colorDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Color model = _mapper.Map<Color>(colorDto);

            await _colorRepo.ActualizarColor(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
