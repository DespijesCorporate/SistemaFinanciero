using appcitas.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class Parametro
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParametroId { get; set; }

        [Display(Name = "Nombre de Parametro")]
        public string ParametroName { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoId { get; set; }

        [MaxLength(5), Column(TypeName = "VARCHAR")]
        public string ConfigID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [ForeignKey("Funcion")]
        public string FuncionCodigo { get; set; }

        public virtual Funcion Funcion { get; set; }
        public virtual ConfigItem Tipo { get; set; }
    }
}