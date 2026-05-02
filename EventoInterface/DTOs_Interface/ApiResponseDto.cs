using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoInterface.DTOs_Interface
{
    public class ApiResponseDto<T>
    {
        public int Status { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public DateTime Data { get; set; }
    }
}