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
    public class OtrosTiposTransacciones
    {
        [Key]
        public int OTT_Codigo { get; set; }
        public string OTT_Descripcion { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}