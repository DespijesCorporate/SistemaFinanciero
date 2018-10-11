using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class Cajero
    {
        [Key]
        public string CA_Codigo_Cajero { get; set; }
        public string CA_Fecha { get; set; }
        public decimal CA_Valor_Inicial_Dia { get; set; }
        public decimal CA_Valor_Entrega { get; set; }
        public decimal CA_Valor_Entrega_Supervisor { get; set; }
        public decimal CA_Valor_Recib_Supervisor { get; set; }
        public decimal CA_Valor_Recib { get; set; }
        public decimal SaldoCaja { get; set; }
        public int CA_B_1 { get; set; }
        public int CA_B_2 { get; set; }
        public int CA_B_5 { get; set; }
        public int CA_B_10 { get; set; }
        public int CA_B_20 { get; set; }
        public int CA_B_50 { get; set; }
        public int CA_B_100 { get; set; }
        public int CA_B_500 { get; set; }
        public int CA_M_1 { get; set; }
        public int CA_M_2 { get; set; }
        public int CA_M_5 { get; set; }
        public int CA_M_10 { get; set; }
        public int CA_M_20 { get; set; }
        public int CA_M_50 { get; set; }
        public int CA_Cajero_Estado { get; set; }
        public int CA_Secuencia { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}