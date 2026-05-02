using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventoInterface.DTOs_Interface
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Tipo_Evento { get; set; } = string.Empty;
        public string local_Evento { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
    }
}