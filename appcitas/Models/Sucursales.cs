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
    public class Sucursales
    {
        [Key]
        public int SucursalId { get; set; }
        public string SucursalNombre { get; set; }
        public string SucursalAbreviatura { get; set; }
        public string SucursalUbicacion { get; set; }
        public bool SucursalEsCanal { get; set; }
        public bool SucursalEsCentroAtencion { get; set; }
        public string SucursalTipoAtencion { get; set; }
        public string SucSegmentoId { get; set; }
        public string ConfigItemDescripcion { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }        
       
    }
}