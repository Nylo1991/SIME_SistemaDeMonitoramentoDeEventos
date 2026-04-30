using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiEventos.Data;
using Shared;
using ApiEventos.Config;
using Microsoft.Extensions.Options;

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
    /// Utiliza Entity Framework com banco SQLite
    /// e padrão de resposta customizado.
    /// </remarks>
    [ApiController]
    [Route("api/v1/evento")]
    public class EventoController : ControllerBase
    {
        /// <summary>
        /// Contexto do banco de dados - responsável pela persistência dos eventos
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Configurações da API vindas do appsettings.json
        /// </summary>
        private readonly ApiConfig _config;

        /// <summary>
        /// Construtor da classe - recebe as dependências via injeção
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public EventoController(AppDbContext context, IOptions<ApiConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        // =========================
        // MODELO PADRÃO DE RESPOSTA
        // =========================

        /// <summary>
        /// Modelo de resposta para sucesso
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        private object Sucesso(object? dados, string mensagem = "OK")
        {
            return new
            {
                status = 200,
                sucesso = true,
                mensagem,
                dados,
                data = DateTime.Now
            };
        }

        /// <summary>
        /// Modelo de resposta para erro
        /// </summary>
        /// <param name="status"></param>
        /// <param name="erro"></param>
        /// <param name="mensagem"></param>
        /// <param name="detalhe"></param>
        /// <returns></returns>
        private object Erro(int status, string erro, string mensagem, string? detalhe = null)
        {
            return new
            {
                status,
                sucesso = false,
                erro,
                mensagem,
                detalhe,
                data = DateTime.Now
            };
        }

        // =========================
        // GET - LISTAR TODOS
        // =========================

        /// <summary>
        /// GET: api/v1/evento - Retorna todos os eventos
        /// </summary>
        /// <remarks>
        /// Retorna a lista de eventos ordenados do mais recente para o mais antigo.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var eventos = await _context.Eventos
                                            .OrderByDescending(e => e.DataHora)
                                            .ToListAsync();

                return Ok(Sucesso(eventos, "Eventos listados com sucesso"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "ERRO_INTERNO", "Erro ao listar eventos", ex.Message));
            }
        }

        // =========================
        // GET - POR ID
        // =========================

        /// <summary>
        /// GET: api/v1/evento/{id} - Retorna um evento por ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Busca um evento específico com base no ID informado.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound(Erro(404, "NAO_ENCONTRADO", "Evento não encontrado"));

                return Ok(Sucesso(evento, "Evento encontrado"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, Erro(500, "ERRO_INTERNO", "Erro ao buscar evento", ex.Message));
            }
        }

        // =========================
        // POST - CRIAR
        // =========================

        /// <summary>
        /// POST: api/v1/evento - Cria um novo evento
        /// </summary>
        /// <param name="evento"></param>
        /// <remarks>
        /// Cria um novo evento validando:
        /// - Tipo obrigatório
        /// - Mensagem obrigatória
        /// - Tamanho máximo da mensagem
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Receber([FromBody] Eventos evento)
        {
            try
            {
                if (evento == null)
                    return BadRequest(Erro(400, "VALIDACAO", "Objeto inválido"));

                if (string.IsNullOrWhiteSpace(evento.Tipo))
                    return BadRequest(Erro(400, "VALIDACAO", "Tipo obrigatório"));

                if (string.IsNullOrWhiteSpace(evento.Mensagem))
                    return BadRequest(Erro(400, "VALIDACAO", "Mensagem obrigatória"));

                if (evento.Mensagem.Length > _config.MaxDescricaoLength)
                    return BadRequest(Erro(400, "VALIDACAO",
                        $"Mensagem deve ter no máximo {_config.MaxDescricaoLength} caracteres"));

                evento.DataHora = DateTime.Now;

                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = evento.Id },
                    Sucesso(evento, "Evento criado com sucesso"));
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_BANCO", "Erro ao salvar no banco",
                    dbEx.InnerException?.Message ?? dbEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_INTERNO", "Erro inesperado", ex.Message));
            }
        }

        // =========================
        // PUT - ATUALIZAR
        // =========================

        /// <summary>
        /// PUT: api/v1/evento/{id} - Atualiza um evento existente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evento"></param>
        /// <remarks>
        /// Atualiza os dados de um evento existente:
        /// - Valida ID
        /// - Verifica existência
        /// - Atualiza Tipo, Mensagem e Data
        /// </remarks>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Eventos evento)
        {
            try
            {
                if (evento == null)
                    return BadRequest(Erro(400, "VALIDACAO", "Objeto inválido"));

                if (id != evento.Id)
                    return BadRequest(Erro(400, "VALIDACAO", "ID inválido"));

                var existente = await _context.Eventos.FindAsync(id);

                if (existente == null)
                    return NotFound(Erro(404, "NAO_ENCONTRADO", "Evento não encontrado"));

                if (string.IsNullOrWhiteSpace(evento.Tipo))
                    return BadRequest(Erro(400, "VALIDACAO", "Tipo obrigatório"));

                if (string.IsNullOrWhiteSpace(evento.Mensagem))
                    return BadRequest(Erro(400, "VALIDACAO", "Mensagem obrigatória"));

                existente.Tipo = evento.Tipo;
                existente.Mensagem = evento.Mensagem;
                existente.DataHora = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(Sucesso(existente, "Evento atualizado com sucesso"));
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_BANCO", "Erro ao atualizar no banco",
                    dbEx.InnerException?.Message ?? dbEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_INTERNO", "Erro inesperado", ex.Message));
            }
        }

        // =========================
        // DELETE - REMOVER
        // =========================

        /// <summary>
        /// DELETE: api/v1/evento/{id} - Remove um evento
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Remove um evento do banco de dados com base no ID informado.
        /// </remarks>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {
                var evento = await _context.Eventos.FindAsync(id);

                if (evento == null)
                    return NotFound(Erro(404, "NAO_ENCONTRADO", "Evento não encontrado"));

                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();

                return Ok(Sucesso(null, "Evento removido com sucesso"));
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_BANCO", "Erro ao deletar no banco",
                    dbEx.InnerException?.Message ?? dbEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    Erro(500, "ERRO_INTERNO", "Erro inesperado", ex.Message));
            }
        }
    }
}