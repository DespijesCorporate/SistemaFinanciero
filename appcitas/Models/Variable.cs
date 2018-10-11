using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace appcitas.Models
{
    public class Variable
    {
        [Key, Display(Name = "Codigo")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableCodigo { get; set; }

        [StringLength(200, ErrorMessage = "Esta campo solo puede tener 200 caracteres maximos")]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableNombre { get; set; }

        [Display(Name = "Origen")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string OrigenId { get; set; }

        [Display(Name = "Tipo de Dato de Retorno")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string DatoDeRetornoId { get; set; }

        [Display(Name = "Tipo de Variable")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoId { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableDescripcion { get; set; }

        [Display(Name = "Formula")]
        public string VariableFormula { get; set; }

        [Display(Name = "Valor")]
        public string VariableValor { get; set; }
    }
}