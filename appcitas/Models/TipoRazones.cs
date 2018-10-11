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
    public class TipoRazones
    {
        [Key]
        public int TipoId { get; set; }
        
        public string TipoDescripcion { get; set; }
        
        public string TipoAbreviatura { get; set; }
        
        public int TipoTieneListadoExtra { get; set; }
        
        public string TipoEtiquetaListadoExtra { get; set; }
        
        public string TipoOrigenListadoExtra { get; set; }

        public string TipoCodigoListadoExtra { get; set; }
        public string TipoStatus { get; set; }
        public int cantidadRegistros { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}