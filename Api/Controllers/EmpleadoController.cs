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
    public class EmpleadoController : ControllerBase
    {
        private readonly ILogger<EmpleadoController> _logger;
        private readonly IEmpleadoRepositorio _empleadoRepo;
        private readonly IMapper _mapper;
        private readonly BackendContext _context;
        protected APIResponse _response;

        public EmpleadoController(ILogger<EmpleadoController> logger, IEmpleadoRepositorio EmpleadoRepo, IMapper mapper, BackendContext context)
        {
            _logger = logger;
            _empleadoRepo = EmpleadoRepo;
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetEmpleado()
        {
            try
            {
                _logger.LogInformation("Obtener las Empleado");

                IEnumerable<Empleado> EmpleadoList = await _empleadoRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<EmpleadoDto>>(EmpleadoList);
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


        [HttpGet("id:int", Name = "GetEmpleado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Empleado con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Empleado = await _empleadoRepo.Obtener(v => v.IdEmpleado == id);
                if (Empleado == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<EmpleadoDto>(Empleado);
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
        public async Task<ActionResult<APIResponse>> CrearEmpleado([FromBody] EmpleadoCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _empleadoRepo.Obtener(v => v.NombreEmpleado.ToLower() == createDto.NombreEmpleado.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Empleado con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Empleado modelo = _mapper.Map<Empleado>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _empleadoRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetEmpleado", new { id = modelo.IdEmpleado }, _response);
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
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Empleado = await _empleadoRepo.Obtener(v => v.IdEmpleado == id);
                if (Empleado == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (_context.Ventas.Any(p => p.IdEmpleado == id))
                {
                    ModelState.AddModelError("RelacionEmpleado", "No se puede eliminar la empleadow porque está relacionada con prendas existentes.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _empleadoRepo.Remover(Empleado);
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
        public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] EmpleadoUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdEmpleado)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Empleado modelo = _mapper.Map<Empleado>(updateDto);

            await _empleadoRepo.ActualizarEmpleado(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialEmpleado(int id, JsonPatchDocument<EmpleadoUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var Empleado = await _empleadoRepo.Obtener(v => v.IdEmpleado == id, tracked: false);

            EmpleadoUpdateDto EmpleadoDto = _mapper.Map<EmpleadoUpdateDto>(Empleado);


            if (Empleado == null) return BadRequest();

            patchDto.ApplyTo(EmpleadoDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Empleado model = _mapper.Map<Empleado>(EmpleadoDto);

            await _empleadoRepo.ActualizarEmpleado(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
