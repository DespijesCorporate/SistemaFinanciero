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
    public class Prioridades
    {
        [Key]
        public int PrioridadId { get; set; }
        public string PrioridadNombre { get; set; }
        public string PrioridadCodigo { get; set; }
        public int PrioridadNivel { get; set; }
        public int cantidadRegistros { get; set; }
        public string Identificador { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}