namespace Shared
{
    public class Eventos
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Mensagem { get; set; } = string.Empty;

        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}