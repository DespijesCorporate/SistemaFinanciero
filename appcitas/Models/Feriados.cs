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
    public class Feriados
    {
        [Key]
        public int FeriadoId { get; set; }
        public string FeriadoFecha { get; set; }
        public string FeriadoFechaFormato { get; set; }
        public int FeriadoFechaNum { get; set; }
        public string FeriadoDescripcion { get; set; }
        public string FeriadoTipoId { get; set; }
        public string FeriadoTipoDescripcion { get; set; }
        public string FeriadoHoraInicio { get; set; }
        public string FeriadoHoraFinal { get; set; }
        public int FeriadoAnio { get; set; }
        public int cantidadRegistros { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}