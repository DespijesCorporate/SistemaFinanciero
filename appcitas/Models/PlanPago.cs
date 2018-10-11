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
    public class PlanPago
    {
        public decimal Capital { get; set; }
        public decimal Interes { get; set; }
        public int Num { get; set; }
        public decimal Total { get; set; }
        public String Fecha { get; set; }
        //public string TipoPres { get; set; }
        public string Codigo { get; set; }
        public decimal Saldo { get; set; }

        public int Accion { get; set; }
    }
}