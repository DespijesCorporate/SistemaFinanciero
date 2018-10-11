using appcitas.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ParametroDto
    {
        [Key]
        public int ParametroId { get; set; }

        [Display(Name = "Nombre de Parametro")]
        public string ParametroName { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string TipoId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string FuncionCodigo { get; set; }

        public string ConfigID { get; set; }

        public virtual ConfigItemDto Tipo { get; set; }
    }
}