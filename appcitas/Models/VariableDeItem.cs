using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace appcitas.Models
{
    public class VariableDeItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public Guid VariableDeItemId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [ForeignKey("Variable")]
        public string VariableCodigo { get; set; }

        [StringLength(200, ErrorMessage = "Esta campo solo puede tener 200 caracteres maximos")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableNombre { get; set; }

        [MaxLength(5)]
        [Required]
        public string CondicionLogica { get; set; }

        [Required]
        public string ValorAEvaluar { get; set; }
        
        [Required]
        public string ReclamoId { get; set; }

        [Required]
        public string ItemDeReclamoId { get; set; }

        public virtual ItemDeReclamo ItemDeReclamo { get; set; }

        public virtual Variable Variable { get; set; }
    }
}