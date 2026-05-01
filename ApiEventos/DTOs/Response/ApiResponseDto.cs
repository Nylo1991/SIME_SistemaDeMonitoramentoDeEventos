namespace ApiEventos.DTOs.Response
{
    /// <summary>
    /// Padrão de resposta da API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Código HTTP
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Indica sucesso ou erro
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagem da operação
        /// </summary>
        public string Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Dados retornados
        /// </summary>
        public T? Dados { get; set; }

        /// <summary>
        /// Data da resposta
        /// </summary>
        public DateTime Data { get; set; } = DateTime.Now;
    }
}