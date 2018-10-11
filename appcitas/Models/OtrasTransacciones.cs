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
    public class OtrasTransacciones
    {
        [Key]
        public int OT_Secuencia { get; set; }
        public string OT_Monto { get; set; }
        public int OT_Tipo_Transaccion { get; set; }
        public string OT_Descripcion { get; set; }
        public int OT_Num_Recibo { get; set; }
        public int OT_Tipo_Movimiento { get; set; }
        public int OT_Clave { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}