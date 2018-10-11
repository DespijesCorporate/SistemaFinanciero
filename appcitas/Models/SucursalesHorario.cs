using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class SucursalesHorario
    {
        public int SucursalId { get; set; }

        public string SucHorarioId { get; set; }
        public string SucHorarioIndLaboral { get; set; }
        public string SucHorarioHoraInicio { get; set; }
        public string SucHorarioFinal { get; set; }

        public string Orden { get; set; }

        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}