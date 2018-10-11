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
    public class MantaDatos
    {
        [Key]
        public int CitaId { get; set; }
        public string CitaFecha { get; set; }
        public string CitaFechaFormat { get; set; }
        public string CitaTicket { get; set; }
        public int CitaTipo { get; set; }
        public string CitaTipoDescripcion { get; set; }
        public string CitaEstado { get; set; }
        public string CitaEstadoDescripcion { get; set; }
        public string CitaCuenta { get; set; }
        public string CitaTarjeta { get; set; }
        public int EmisorId { get; set; }
        public string EmisorCuenta { get; set; }
        public string MarcaId { get; set; }
        public string Marca { get; set; }
        public string Segmentos { get; set; }
        public string Familia { get; set; }
        public string CitaNombre { get; set; }
        public string CitaIdentificacion { get; set; }
        public string CitaCorreoElectronico { get; set; }
        public string CitaTelefonoCelular { get; set; }
        public int SucursalId { get; set; }
        public string SucursalNombre { get; set; }
        public string CitaUsuarioAtendio { get; set; }
        public string DiaAtencion { get; set; }
        public string FechaAtencion { get; set; }
        public string HoraCita { get; set; }
        public string HoraAtencion { get; set; }
        public string HoraFinalizaAtencion { get; set; }
        public string Resolucion { get; set; }
        public string HerramientaRescate { get; set; }
        public string RazonCancelacion { get; set; }
        public string DescResolucion { get; set; }
        public int TramiteId { get; set; }
        public string Tramite { get; set; }
        public string Canal { get; set; }
        public string Banco { get; set; }
        public string MotivoInsatisfaccion { get; set; }
        public string CuentasAdicionales { get; set; }
        public string TarjetasAdicionales { get; set; }
        public string RazonCancelacionAdicional { get; set; }
        public string HerramientaRescateAdicional { get; set; }
        public string Producto { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
        public string HoraClienteLlego { get; set; }
        public string SucursalOrigen { get; set; }
    }
}