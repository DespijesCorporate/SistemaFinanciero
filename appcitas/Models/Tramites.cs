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
    public class Tramites
    {
        [Key]
        public int TramiteId { get; set; }
        public string TramiteDescripcion { get; set; }
        public string TramiteAbreviatura { get; set; }
        public int TramiteDuracion { get; set; }
        public int TramiteAlertaPrevia { get; set; }
        public int TramiteToleranciaAlCliente { get; set; }
        public int TramiteToleranciaDelCliente { get; set; }
        public int TramiteToleranciaFinalizacion { get; set; }
        public int TramiteTiempoMuerto { get; set; }
        public int cantidadRegistros { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}