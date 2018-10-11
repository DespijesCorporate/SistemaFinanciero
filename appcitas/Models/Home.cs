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
    public class Home
    {
        [Key]
        public int      usuarioEncontrado { get; set; }
        public string   Codigo { get; set; }
        public string   Nombre { get; set; }

        public string   CodigoRol { get; set; }
        
        public string   CodigoPrivilegio { get; set; }
        
        public string   usuario { get; set; }
        public string   modulo { get; set; }
        public string   privilegio { get; set; }
        public int SucursalId { get; set; }

        public string PosicionId { get; set; }

        public int      Accion { get; set; }
        public string   Mensaje { get; set; }
    }
}