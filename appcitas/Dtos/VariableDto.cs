using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class VariableDto
    {
        [Key]
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableCodigo { get; set; }

        [StringLength(200, ErrorMessage = "Esta campo solo puede tener 200 caracteres maximos")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string VariableNombre { get; set; }

        [Display(Name = "Origen")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string OrigenId { get; set; }

        [Display(Name = "Tipo de Dato de Retorno")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string DatoDeRetornoId { get; set; }

        [Display(Name = "Tipo de Variable")]
        public string TipoId { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string VariableDescripcion { get; set; }

        [Display(Name = "Formula")]
        [DataType(DataType.MultilineText)]
        public string VariableFormula { get; set; }

        [Display(Name = "Valor")]
        public string VariableValor { get; set; }

        public int Accion { get; set; }

        public string Mensaje { get; set; }

        public virtual ConfigItemDto OrigenDto { get; set; }
        public virtual ConfigItemDto DatoDeRetornoDto { get; set; }
        public virtual ConfigItemDto TipoDto { get; set; }
    }
}