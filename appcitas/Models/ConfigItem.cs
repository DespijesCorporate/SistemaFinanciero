using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace appcitas.Models
{
    public class ConfigItem
    {
        [Key, Column("ConfigItemId", Order = 1, TypeName = "VARCHAR")]
        [MaxLength(10)]
        public string ConfigItemID { get; set; }

        [MaxLength(5)]
        [Key, Column("ConfigId", Order = 0, TypeName = "VARCHAR")]
        public string ConfigID { get; set; }

        [NotMapped]
        public string ConfigDescripcion { get; set; }

        [StringLength(500)]
        [Column("ConfigItemDescripcion")]
        public string ConfigItemDescripcion { get; set; }

        [StringLength(50)]
        public string ConfigItemAbreviatura { get; set; }

        [StringLength(200)]
        public string ConfigItemObservacion { get; set; }

        [NotMapped]
        public int cantidadRegistros { get; set; }

        [StringLength(1)]
        [Column("ConfigItemStatus")]
        public string Estado { get; set; }

        [NotMapped]
        public int Accion { get; set; }

        [NotMapped]
        public string Mensaje { get; set; }

        public string Estado_Descripcion { get; set; }
        public string EstadoCodigo { get; set; }
    }
}