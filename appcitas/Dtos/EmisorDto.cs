using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class EmisorDto
    {
        public int EmisorId { get; set; }
        public string EmisorCuenta { get; set; }
        public string MarcaId { get; set; }
        public string SegmentoId { get; set; }
        public string Marca { get; set; }
        public string Segmento { get; set; }
        public string Producto { get; set; }
        public string Familia { get; set; }
    }
}