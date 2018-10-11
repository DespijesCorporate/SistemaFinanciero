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
    public class Razones
    {
        [Key]
        public int TipoId { get; set; }
        [Key]
        public int RazonId { get; set; }

        public string RazonDescripcion { get; set; }

        public string RazonAbreviatura { get; set; }
        public string TipoAbreviatura { get; set; }
        public string RazonGroup { get; set; }
        public string RazonStatus { get; set; }
        public string ConfigItemDescripcion { get; set; }
        public int cantidadRegistros { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}