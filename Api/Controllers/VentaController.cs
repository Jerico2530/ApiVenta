using Api.Models;
using Api.Repositorio.IRepositorio;
using AutoMapper;
using BiblotecApi.Models.Dto;
using BiblotecApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<VentaController> _logger;
        private readonly IVentaRepositorio _ventaRepo;
        private readonly IEmpleadoRepositorio _empleadoRepo;
        private readonly IPrendaRepositorio _prendaRepo;
        private readonly IClienteRepositorio _clienteRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VentaController(ILogger<VentaController> logger, IClienteRepositorio clienteRepo, IEmpleadoRepositorio empleadoRepo, IVentaRepositorio ventaRepo, IPrendaRepositorio prendaRepo, IMapper mapper)
        {
            _logger = logger;
            _ventaRepo = ventaRepo;
            _empleadoRepo = empleadoRepo;
            _prendaRepo= prendaRepo;
            _clienteRepo = clienteRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVenta()
        {
            try
            {
                _logger.LogInformation("Obtener las Venta");

                IEnumerable<Venta> ventaList = await _ventaRepo.ObtenerTodo();
                _response.Resultado = _mapper.Map<IEnumerable<VentaDto>>(ventaList);
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


        [HttpGet("id:int", Name = "GetVenta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVenta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer Venta con Id" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }
                var Venta = await _ventaRepo.Obtener(v => v.IdVenta == id);
                if (Venta == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VentaDto>(Venta);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener Prenda con Id {id}: {ex}");
                _response.IsExitoso = false;
                _response.ErroresMessages = new List<string>() { ex.ToString() };
                return _response;

            }
            

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearVenta([FromBody] VentaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _prendaRepo.Obtener(v => v.IdPrenda == createDto.IdPrenda) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Prenda no existe!");
                    return BadRequest(ModelState);
                }
                if (await _empleadoRepo.Obtener(v => v.IdEmpleado == createDto.IdEmpleado) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Empleado no existe!");
                    return BadRequest(ModelState);
                }
                if (await _clienteRepo.Obtener(v => v.IdCliente == createDto.IdCliente) == null)
                {
                    ModelState.AddModelError("ClaveForeanea", "El Id de la Cliente no existe!");
                    return BadRequest(ModelState);
                }

          ;

                Venta modelo = _mapper.Map<Venta>(createDto);
                modelo.FechaCreacion = DateTime.Now;


                await _ventaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVenta", new { id = modelo.IdVenta }, _response);
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
        public async Task<IActionResult> DeleteVenta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var Venta = await _ventaRepo.Obtener(v => v.IdVenta == id);
                if (Venta == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }


                await _ventaRepo.Remover(Venta);
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
        public async Task<IActionResult> UpdateVenta(int id, [FromBody] VentaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.IdVenta)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            if (await _prendaRepo.Obtener(v => v.IdPrenda == updateDto.IdPrenda) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Prenda no existe!");
                return BadRequest(ModelState);
            }
            if (await _clienteRepo.Obtener(v => v.IdCliente == updateDto.IdCliente) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Cliente no existe!");
                return BadRequest(ModelState);
            }
            if (await _empleadoRepo.Obtener(v => v.IdEmpleado == updateDto.IdEmpleado) == null)
            {
                ModelState.AddModelError("ClaveForeanea", "El Id  de la  Empleado no existe!");
                return BadRequest(ModelState);
            }


            Venta modelo = _mapper.Map<Venta>(updateDto);

            await _ventaRepo.ActualizarVenta(modelo);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }




        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialVenta(int id, JsonPatchDocument<VentaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var Venta = await _ventaRepo.Obtener(v => v.IdVenta == id, tracked: false);

            VentaUpdateDto VentaDto = _mapper.Map<VentaUpdateDto>(Venta);


            if (Venta == null) return BadRequest();

            patchDto.ApplyTo(VentaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Venta model = _mapper.Map<Venta>(VentaDto);

            await _ventaRepo.ActualizarVenta(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
