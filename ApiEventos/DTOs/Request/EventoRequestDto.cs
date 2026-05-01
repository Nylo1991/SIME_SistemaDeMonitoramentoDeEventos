namespace ApiEventos.DTOs.Request
{
    /// <summary>
    /// DTO utilizado para criação e atualização de eventos
    /// </summary>
    public class EventoRequestDto
    {
        /// <summary>
        /// Tipo do evento (Alerta, Falha, etc)
        /// </summary>
        public string Tipo_Evento { get; set; } = string.Empty;

        /// <summary>
        /// Local onde ocorreu o evento
        /// </summary>
        public string local_Evento { get; set; } = string.Empty;
    }
}