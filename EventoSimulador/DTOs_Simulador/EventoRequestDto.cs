using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoSimulador.DTOs_Simulador
{
    public class EventoRequestDto
    {
        public string Tipo_Evento { get; set; } = string.Empty;
        public string local_Evento { get; set; } = string.Empty;
    }
}