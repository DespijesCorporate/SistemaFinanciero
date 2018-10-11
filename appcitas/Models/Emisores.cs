using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AccesoDatos;
using appcitas.Context;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;

namespace appcitas.Models
{
    public class Emisores
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmisorId { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string   EmisorCuenta { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string   MarcaId { get; set; }

        [NotMapped]
        public string   MarcaTarjeta { get; set; }

        [Required, MaxLength(100), Column(TypeName = "VARCHAR")]
        public string   Producto { get; set; }

        [Required, MaxLength(100), Column(TypeName = "VARCHAR")]
        public string   Familia { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string   SegmentoId { get; set; }

        [NotMapped]
        public string   Segmento { get; set; }

        [MaxLength(1), Column(TypeName = "CHAR")]
        public string   EmisorStatus { get; set; }

        [NotMapped]
        public string   EmisorStatusDescripcion { get; set; }

        [NotMapped]
        public int      cantidadRegistros { get; set; }

        [NotMapped]
        public int      usuarioEncontrado { get; set; }

        [NotMapped]
        public string   usuario { get; set; }

        [NotMapped]
        public string   modulo { get; set; }

        [NotMapped]
        public int      Accion { get; set; }

        [NotMapped]
        public string   Mensaje { get; set; }
    }
}