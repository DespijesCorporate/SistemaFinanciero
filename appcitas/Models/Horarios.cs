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
    public class Horarios
    {
        [Key]
        public int      SucursalId { get; set; }
        [Key]
        public string   SucHorarioId { get; set; }
        public bool     SucHorarioIndLaboral { get; set; }
        public bool     SucHorarioCorrido { get; set; }
        public string   SucHorarioHoraInicio { get; set; }
        public string   SucHorarioHoraFinal { get; set; }
        public int      Orden { get; set; }
        public int      Accion { get; set; }
        public string   Mensaje { get; set; }
    }
}