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
    public class Atencion
    {
        [Key]
        public int CitaId { get; set; }
        public string CitaIdentificacion { get; set; }
        public string CitaFecha { get; set; }
        public string CitaHora { get; set; }
        public string CitaNombre { get; set; }
        public string CitaCorreoElectronico { get; set; }
        public string CitaTelefonoCelular { get; set; }
        public string CitaTelefonoCasa { get; set; }
        public string CitaTelefonoOficina { get; set; }
        public string CitaHoraInicioCompleta { get; set; }
        public string CitaHoraFinCompleta { get; set; }
        public string PosicionId { get; set; }
        public string posicionDescripcion { get; set; }
        public int TramiteId { get; set; }
        public string tramite { get; set; }
        public int PrioridadId { get; set; }
        public string CitaTicket { get; set; }
        public string SucursalNombre { get; set; }
        public string SucursalUbicacion { get; set; }
        public string PrioridadNombre { get; set; }
        public int duracion { get; set; }
        public int TiempoMuerto { get; set; }
        public int AlertaPrevia { get; set; }
        public int ToleranciaFinalizacion { get; set; }
        public string CitaCuenta { get; set; }
        public string CitaTarjeta { get; set; }
        public string CitaSegmentoId { get; set; }
        public string segmento { get; set; }
        public string SucursalId { get; set; }
        public string SucursalIdReferido { get; set; }
        public string sucursal { get; set; }
        public string sucursalreferido { get; set; }
        public string CitaEstado { get; set; }
        public int CitaTipo { get; set; }
        public int TipoId { get; set; }
        public int TipoRazonId { get; set; }
        public int RazonId { get; set; }
        public string tipoRazonAbreviatura { get; set; }
        public string RazonDescripcion { get; set; }
        public string RazonAbreviatura { get; set; }
        public string RazonGroup { get; set; }
        public string RazonGrupo { get; set; }
        public string RazonStatus { get; set; }
        public string TipoOrigenExtraId { get; set; }
        public string CodigoListadoOrigenExtraId { get; set; }
        public string DatoExtraId { get; set; }
        public string TipoEtiquetaListadoExtra { get; set; }
        public int DiferenciaMinutos { get; set; }
        public int NumeroClientes { get; set; }
        public int PromedioTiempoEspera { get; set; }

        public int EmisorId { get; set; }
        public string EmisorCuenta { get; set; }
        public string MarcaId { get; set; }
        public string MarcaTarjeta { get; set; }
        public string Producto { get; set; }
        public string Familia { get; set; }
        public string Resolucion { get; set; }
        public string CitaObservacion { get; set; }
        public string CuentaOriginal { get; set; }
        public string TarjetaOriginal { get; set; }
        public bool SucHorarioCorrido { get; set; }
        public int MinutosProximaCitaProgramada { get; set; }
        public bool FlagProximaCitaProgramada { get; set; }
        public int MinutosAtencionCitaPendiente { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }

    }
}