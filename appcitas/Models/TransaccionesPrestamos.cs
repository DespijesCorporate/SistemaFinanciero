using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccesoDatos;
using appcitas.Context;
using System.Data;
using System.Data.SqlClient;

namespace appcitas.Models
{
    public class TransaccionesPrestamos
    {
        [Key]
        public int TRP_Codigo_Prestamo { get; set; }
        public string Fecha { get; set; }
        public decimal TRP_Otorgado { get; set; }
        public decimal TRP_Capital { get; set; }
        public decimal TRP_Interes { get; set; }
        public decimal TRP_Mora { get; set; }
        public string TRP_Agrego { get; set; }
        public decimal Saldo { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}