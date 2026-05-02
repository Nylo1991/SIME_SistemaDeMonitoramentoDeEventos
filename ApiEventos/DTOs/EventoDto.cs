namespace ApiEventos.DTOs.Response
{
    public class EventoDto
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Mensagem { get; set; } = string.Empty;

        public DateTime DataHora { get; set; }
    }
}