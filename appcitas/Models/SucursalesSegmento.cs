using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class SucursalesSegmento
    {
        public int SucursalId { get; set; }
        public string SucSegmentoId { get; set; }
        public string ConfigItemDescripcion { get; set; }

        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}