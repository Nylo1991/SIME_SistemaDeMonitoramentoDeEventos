namespace ApiEventos.DTOs.Response
{
    public class ApiResponseDto<T>
    {
        public int Status { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}