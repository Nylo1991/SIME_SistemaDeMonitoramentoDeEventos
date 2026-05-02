using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiEventos.Data;
using ApiEventos.DTOs.Request;
using ApiEventos.DTOs.Response;
using Shared;

namespace ApiEventos.Controllers
{
    /// <summary>
    /// API de Eventos - Controller responsável pelo gerenciamento
    /// completo dos eventos do sistema
    /// </summary>
    [ApiController]
    [Route("api/v1/evento")]
    public class EventoController : ControllerBase
    {
        /// <summary>
        /// Contexto do banco de dados
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor com injeção de dependência
        /// </summary>
        public EventoController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET - LISTAR TODOS
        // =========================

        /// <summary>
        /// GET: api/v1/evento - Retorna todos os eventos
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _context.Eventos
                    .OrderByDescending(e => e.DataHora)
                    .ToListAsync();

                var dtos = eventos.Select(e => new EventoDto
                {
                    Id = e.Id,
                    Tipo_Evento = e.Tipo_Evento,
                    local_Evento = e.local_Evento,
                    DataHora = e.DataHora
                });

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar eventos: {ex.Message}");
            }
        }

        // =========================
        // GET - POR ID
        // =========================

        /// <summary>
        /// GET: api/v1/evento/{id} - Retorna evento por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound("Evento não encontrado!");

                var dto = new EventoDto
                {
                    Id = evento.Id,
                    Tipo_Evento = evento.Tipo_Evento,
                    local_Evento = evento.local_Evento,
                    DataHora = evento.DataHora
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar evento {id}: {ex.Message}");
            }
        }

        // =========================
        // POST - CRIAR
        // =========================

        /// <summary>
        /// POST: api/v1/evento - Cria um novo evento
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventoRequestDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Objeto inválido!");

                if (string.IsNullOrWhiteSpace(dto.Tipo_Evento))
                    return BadRequest("Tipo do evento é obrigatório!");

                if (string.IsNullOrWhiteSpace(dto.local_Evento))
                    return BadRequest("Local do evento é obrigatório!");

                var evento = new Eventos
                {
                    Tipo_Evento = dto.Tipo_Evento,
                    local_Evento = dto.local_Evento,
                    DataHora = DateTime.Now
                };

                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar evento: {ex.Message}");
            }
        }

        // =========================
        // PUT - ATUALIZAR
        // =========================

        /// <summary>
        /// PUT: api/v1/evento/{id} - Atualiza um evento
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EventoRequestDto dto)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound("Evento não encontrado!");

                evento.Tipo_Evento = dto.Tipo_Evento;
                evento.local_Evento = dto.local_Evento;
                evento.DataHora = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok("Evento atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar evento: {ex.Message}");
            }
        }

        // =========================
        // DELETE - REMOVER
        // =========================

        /// <summary>
        /// DELETE: api/v1/evento/{id} - Remove um evento
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido!");

            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound("Evento não encontrado!");

                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar evento {id}: {ex.Message}");
            }
        }
    }
}