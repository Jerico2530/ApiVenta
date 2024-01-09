using Api.Models;
using Api.Repositorio.IRepositorio;
using AutoMapper;
using BiblotecApi.Models.Dto;
using BiblotecApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;
using BiblotecApi.Datos;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteRepositorio _ClienteRepo;
        private readonly IMapper _mapper;
        private readonly BackendContext _context;
        protected APIResponse _response;

        public ClienteController(ILogger<ClienteController> logger, IClienteRepositorio ClienteRepo, IMapper mapper, BackendContext context)
        {
            _logger = logger;
            _ClienteRepo = ClienteRepo;
            _mapper = mapper;
            _context = context;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCliente()
        {
            try
            {
                _logger.LogInformation("Obtener las Cliente");

                IEnumerable<Cliente> ClienteList = await _ClienteRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<ClienteDto>>(ClienteList);
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


        [HttpGet("id:int", Name = "GetCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCliente(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Cliente con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Cliente = await _ClienteRepo.Obtener(v => v.IdCliente == id);
                if (Cliente == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<ClienteDto>(Cliente);
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
        public async Task<ActionResult<APIResponse>> CrearCliente([FromBody] ClienteCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _ClienteRepo.Obtener(v => v.NombreCliente.ToLower() == createDto.NombreCliente.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La Cliente con ese Nombre ya existe!");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }
          ;

                Cliente modelo = _mapper.Map<Cliente>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _ClienteRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCliente", new { id = modelo.IdCliente }, _response);
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
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Cliente = await _ClienteRepo.Obtener(v => v.IdCliente == id);
                if (Cliente == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (_context.Ventas.Any(p => p.IdCliente == id))
                {
                    ModelState.AddModelError("RelacionCliente", "No se puede eliminar la cliente porque está relacionada con prendas existentes.");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }

                await _ClienteRepo.Remover(Cliente);
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
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdCliente)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }


            Cliente modelo = _mapper.Map<Cliente>(updateDto);

            await _ClienteRepo.ActualizarCliente(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialCliente(int id, JsonPatchDocument<ClienteUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var Cliente = await _ClienteRepo.Obtener(v => v.IdCliente == id, tracked: false);

            ClienteUpdateDto ClienteDto = _mapper.Map<ClienteUpdateDto>(Cliente);


            if (Cliente == null) return BadRequest();

            patchDto.ApplyTo(ClienteDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Cliente model = _mapper.Map<Cliente>(ClienteDto);

            await _ClienteRepo.ActualizarCliente(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
