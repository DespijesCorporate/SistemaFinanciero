using System;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Dtos
{
    public class VariableDeItemDto
    {        
        public Guid VariableDeItemId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Variable")]
        public string VariableCodigo { get; set; }

        [StringLength(200, ErrorMessage = "Esta campo solo puede tener 200 caracteres maximos")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableNombre { get; set; }

        [MaxLength(5)]
        [Required(ErrorMessage = "este campo es requerido")]
        [Display(Name = "Condición Logíca")]
        public string CondicionLogica { get; set; }
                
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Valor Comparativo")]
        public string ValorAEvaluar { get; set; }

        [Required]
        [Display(Name = "Id de Reclamo")]
        public string ReclamoId { get; set; }

        [Required]
        [Display(Name = "Id de Item")]
        public string ItemDeReclamoId { get; set; }

        public virtual ItemDeReclamoDto ItemDeReclamo { get; set; }

        public virtual VariableDto Variable { get; set; }

        public int Accion { get; set; }

        public string Mensaje { get; set; }
    }
}