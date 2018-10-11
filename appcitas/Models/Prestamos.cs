using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class Prestamos
    {
        [Key]
        public int ClienteId { get; set; }
        [Key]
        public int PrestamoId { get; set; }
        public string FecSolicitud { get; set; }
        public string MontoSolicitado { get; set; }
        public string MontoAprobado { get; set; }
        public string PlazoMeses { get; set; }
        public string Interes { get; set; }
        public string Saldo { get; set; }
        public int Estado { get; set; }
        public string Garantia { get; set; }
        public string NomPrestamo { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
        public string Destino { get; set; }
        public string FrecPago { get; set; }
        public string Observaciones { get; set; }
        public string Codigo { get; set; }

        // datos generales estudio socioeconomico
        public string Personas { get; set; }

        // ingresos estudio socioecnonomico
        public string NegocioPropio { get; set; }
        public string Salario { get; set; }
        public string Finca { get; set; }
        public string Otros { get; set; }

        // egresos estudio socioeconomico
        public string Renta { get; set; }
        public string ServicioPublico { get; set; }
        public string Prestamo { get; set; }
        public string Transporte { get; set; }
        public string Alimentacion { get; set; }
        public string Vestuario { get; set; }
        public string Otros1 { get; set; }

        // datos finales estudio socioeconomico
        public string Observaciones1 { get; set; }

    }
}