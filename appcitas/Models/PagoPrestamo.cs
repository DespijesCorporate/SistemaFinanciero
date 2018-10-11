using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class GetDatosPagoPrestamo
    {
        [Key]
        public decimal codigo { get; set; }
        public decimal Prestamo { get; set; }
        public string Capital { get; set; }
        public string Intereses { get; set; }
        public string Mora { get; set; }
        public decimal Total { get; set; }
        public decimal Saldo { get; set; }
        public int Accion { get; set; }
        public int TipoPago { get; set; }
        public string Mensaje { get; set; }
    }
}