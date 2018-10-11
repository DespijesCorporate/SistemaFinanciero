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
    public class MensajesResultado
    {
        [Key]
        public int Estado { get; set; }
        public string Mensaje { get; set; }
        public int Num { get; set; }
    }
}