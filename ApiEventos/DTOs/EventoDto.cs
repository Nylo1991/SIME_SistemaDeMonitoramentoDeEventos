namespace ApiEventos.DTOs.Response
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Tipo_Evento { get; set; } = string.Empty;
        public string local_Evento { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
    }
}