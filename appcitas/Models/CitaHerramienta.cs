using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class CitaHerramienta
    {
        public int CitaId { get; set; }
        public string CitaCuenta { get; set; }
        public string CitaTarjeta { get; set; }
        public string HerramientaId { get; set; }
        public string HerramientaObservacion { get; set; }
        public string HerramientaDescripcion { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}