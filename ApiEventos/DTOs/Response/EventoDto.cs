namespace ApiEventos.DTOs.Response
{
    /// <summary>
    /// DTO de retorno de eventos
    /// </summary>
    public class EventoDto
    {
        /// <summary>
        /// ID do evento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tipo do evento
        /// </summary>
        public string Tipo_Evento { get; set; } = string.Empty;

        /// <summary>
        /// Local do evento
        /// </summary>
        public string local_Evento { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora do evento
        /// </summary>
        public DateTime DataHora { get; set; }
    }
}