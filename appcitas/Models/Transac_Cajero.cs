using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class Transac_Cajero
    {
        [Key]
        public int TC_Numero { get; set; }
        public string TC_Cajero { get; set; }
        public string TC_Fecha { get; set; }
        public decimal TC_Mto_Entrega { get; set; }
        public decimal TC_Mto_Recib { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}