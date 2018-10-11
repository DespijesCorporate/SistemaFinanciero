using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ItemDeReclamoDto
    {
        [Display(Name = "Reclamo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string ReclamoId { get; set; }

        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string ItemDeReclamoId { get; set; }

        [Display(Name = "Descripcion")]
        [MaxLength(250,ErrorMessage = "Este campo solo puede contener 250 caracteres maximo")]
        public string ItemDeReclamoDescripcion { get; set; }
    
        public virtual ReclamoDto Reclamo { get; set; }

        public virtual List<VariableDeItemDto> VariablesAEvaluar { get; set; }

        public int Accion { get; set; }

        public string Mensaje { get; set; }
    }
}