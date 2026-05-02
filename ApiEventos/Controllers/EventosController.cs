using ApiEventos.Config;
using ApiEventos.Data;
using ApiEventos.DTOs.Request;
using ApiEventos.DTOs.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared;

namespace ApiEventos.Controllers
{
    /// <summary>
    /// API de eventos - Controller responsável pelo gerenciamento
    /// completo dos eventos do sistema
    /// </summary>
    /// <remarks>
    /// Esta API permite:
    /// - Listar eventos
    /// - Buscar evento por ID
    /// - Criar novos eventos
    /// - Atualizar eventos existentes
    /// - Remover eventos
    /// 
    /// Utiliza:
    /// - Entity Framework Core (SQLite)
    /// - DTOs para entrada e saída
    /// - AutoMapper para conversão
    /// - Padrão de resposta unificado
    /// </remarks>
    [ApiController]
    [Route("api/v1/evento")]
    public class EventoController : ControllerBase
    {
        /// <summary>
        /// Contexto do banco de dados
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Mapper para conversão entre entidade e DTO
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Configurações da API
        /// </summary>
        private readonly ApiConfig _config;

        /// <summary>
        /// Construtor com injeção de dependência
        /// </summary>
        public EventoController(
            AppDbContext context,
            IMapper mapper,
            IOptions<ApiConfig> config)
        {
            _context = context;
            _mapper = mapper;
            _config = config.Value;
        }

        // =========================
        // PADRÃO DE RESPOSTA
        // =========================

        /// <summary>
        /// Retorno padrão de sucesso
        /// </summary>
        private ApiResponseDto<T> Sucesso<T>(T dados, string mensagem = "OK")
        {
            return new ApiResponseDto<T>
            {
                Status = 200,
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados,
                Data = DateTime.Now
            };
        }

        /// <summary>
        /// Retorno padrão de erro
        /// </summary>
        private ApiResponseDto<string> Erro(int status, string mensagem, string detalhe = "")
        {
            return new ApiResponseDto<string>
            {
                Status = status,
                Sucesso = false,
                Mensagem = mensagem,
                Dados = detalhe,
                Data = DateTime.Now
            };
        }

        // =========================
        // GET - LISTAR TODOS
        // =========================

        /// <summary>
        /// GET: api/v1/evento
        /// </summary>
        /// <remarks>
        /// Retorna todos os eventos ordenados do mais recente para o mais antigo.
        /// </remarks>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var eventos = await _context.Eventos
                    .OrderByDescending(e => e.DataHora)
                    .ToListAsync();

                var dto = _mapper.Map<List<EventoDto>>(eventos);

                return Ok(Sucesso(dto, "Eventos listados com sucesso"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "Erro ao listar eventos", ex.Message));
            }
        }

        // =========================
        // GET - POR ID
        // =========================

        /// <summary>
        /// GET: api/v1/evento/{id}
        /// </summary>
        /// <remarks>
        /// Retorna um evento específico com base no ID informado.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound(Erro(404, "Evento não encontrado"));

                var dto = _mapper.Map<EventoDto>(evento);

                return Ok(Sucesso(dto, "Evento encontrado"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "Erro ao buscar evento", ex.Message));
            }
        }

        // =========================
        // POST - CRIAR
        // =========================

        /// <summary>
        /// POST: api/v1/evento
        /// </summary>
        /// <remarks>
        /// Cria um novo evento validando:
        /// - Tipo obrigatório
        /// - Mensagem obrigatória
        /// - Tamanho máximo da mensagem
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] EventoRequestDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(Erro(400, "Objeto inválido"));

                if (string.IsNullOrWhiteSpace(dto.Tipo))
                    return BadRequest(Erro(400, "Tipo obrigatório"));

                if (string.IsNullOrWhiteSpace(dto.Mensagem))
                    return BadRequest(Erro(400, "Mensagem obrigatória"));

                if (dto.Mensagem.Length > _config.MaxDescricaoLength)
                    return BadRequest(Erro(400,
                        $"Mensagem deve ter no máximo {_config.MaxDescricaoLength} caracteres"));

                var evento = _mapper.Map<Eventos>(dto);
                evento.DataHora = DateTime.Now;

                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<EventoDto>(evento);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = evento.Id },
                    Sucesso(responseDto, "Evento criado com sucesso"));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, Erro(500, "Erro ao salvar no banco", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "Erro interno", ex.Message));
            }
        }

        // =========================
        // PUT - ATUALIZAR
        // =========================

        /// <summary>
        /// PUT: api/v1/evento/{id}
        /// </summary>
        /// <remarks>
        /// Atualiza um evento existente validando:
        /// - ID
        /// - Existência no banco
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] EventoRequestDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(Erro(400, "Objeto inválido"));

                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound(Erro(404, "Evento não encontrado"));

                if (string.IsNullOrWhiteSpace(dto.Tipo))
                    return BadRequest(Erro(400, "Tipo obrigatório"));

                if (string.IsNullOrWhiteSpace(dto.Mensagem))
                    return BadRequest(Erro(400, "Mensagem obrigatória"));

                evento.Tipo = dto.Tipo;
                evento.Mensagem = dto.Mensagem;
                evento.DataHora = DateTime.Now;

                await _context.SaveChangesAsync();

                var responseDto = _mapper.Map<EventoDto>(evento);

                return Ok(Sucesso(responseDto, "Evento atualizado com sucesso"));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, Erro(500, "Erro ao atualizar banco", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "Erro interno", ex.Message));
            }
        }

        // =========================
        // DELETE - REMOVER
        // =========================

        /// <summary>
        /// DELETE: api/v1/evento/{id}
        /// </summary>
        /// <remarks>
        /// Remove um evento do sistema com base no ID informado.
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound(Erro(404, "Evento não encontrado"));

                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();

                return Ok(Sucesso<string>(null, "Evento removido com sucesso"));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, Erro(500, "Erro ao deletar no banco", ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "Erro interno", ex.Message));
            }
        }
    }
}