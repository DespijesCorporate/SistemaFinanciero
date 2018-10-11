using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class Cubiculo
    {
        public int      SucursalId { get; set; }
        public string   sucursal { get; set; }
        public string   SucursalTipoAtencion { get; set; }
        public string   PosicionId { get; set; }
        public string   PosicionNombre { get; set; }
        public string   TipoAtencionId { get; set; }
        public string   TipoAtencionId_COD { get; set; }
        public string   ConfigId { get; set; }
        public string   ConfigItemDescripcion { get; set; }
        public string   posicionDescripcion { get; set; }
        public string   posicionAbreviatura { get; set; }
        public string   UsuarioId { get; set; }
        public string   PosicionHoraInicioDesc { get; set; }
        public string   PosicionHoraFinalDesc { get; set; }
        public int      PosicionHoraInicioDescMin { get; set; }
        public int      PosicionHoraFinalDescMin { get; set; }
        public string itemOrden { get; set; }
        public string PosicionUsuario { get; set; }
        public int      Accion { get; set; }
        public string   Mensaje { get; set; }
    }
}