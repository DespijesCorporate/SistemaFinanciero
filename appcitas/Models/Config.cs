using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appcitas.Models
{
    public class Config
    {
        [Key]
        [StringLength(5)]
        [Column("ConfigId")]
        public string ConfigID { set; get; }

        [StringLength(500)]
        [Column("ConfigDescripcion")]
        public string ConfigDesc { set; get; }

        [StringLength(500)]
        [Column("ConfigObservacion")]
        public string ConfigObs { set; get; }

        [StringLength(1)]
        [Column("ConfigStatus")]
        public string Estado { set; get; }

        [NotMapped]
        public int cantidadRegistros { get; set; }
        [NotMapped]
        public int Accion { get; set; }
        [NotMapped]
        public string Mensaje { get; set; }  
    }
}