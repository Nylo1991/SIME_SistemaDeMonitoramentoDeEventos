using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoSimulador.DTOs_Simulador
{
    public class EventoRequestDto
    {
        public string Tipo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}