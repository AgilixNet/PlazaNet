using Microsoft.AspNetCore.Mvc;
using PlazaNetNegocio.DTOs;
using PlazaNetNegocio.Services;

namespace PlazaNetNegocio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly ISolicitudesService _service;
        private readonly ILogger<SolicitudesController> _logger;

        public SolicitudesController(
            ISolicitudesService service,
            ILogger<SolicitudesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET api/solicitudes
        [HttpGet]
        public async Task<ActionResult<List<SolicitudReadDTO>>> GetAll()
        {
            var lista = await _service.GetAllAsync();
            return Ok(lista);
        }

        // GET api/solicitudes/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SolicitudReadDTO>> GetById(Guid id)
        {
            var s = await _service.GetByIdAsync(id);
            if (s == null) return NotFound();
            return Ok(s);
        }

        // POST api/solicitudes
        // Crea una nueva solicitud (estado = "pendiente")
        [HttpPost]
        public async Task<ActionResult<SolicitudReadDTO>> Create([FromBody] SolicitudCreateDTO dto)
        {
            try
            {
                var creada = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = creada.Id },
                    creada
                );
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al crear solicitud");
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT api/solicitudes/{id}
        // Actualiza campos parciales, incluyendo estado
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SolicitudReadDTO>> Update(
            Guid id,
            [FromBody] SolicitudUpdateDTO dto)
        {
            try
            {
                var actualizada = await _service.UpdateAsync(id, dto);
                if (actualizada == null) return NotFound();
                return Ok(actualizada);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Error de validación al actualizar solicitud");
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE api/solicitudes/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
