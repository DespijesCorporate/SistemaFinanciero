using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class FuncionDto
    {
        [Key]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Codigo")]
        public string FuncionCodigo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(300, ErrorMessage = "este campo tiene una longitud maxima de 300 caracteres")]
        [Display(Name = "Nombre")]
        public string FuncionNombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Número de Parametros")]
        public int FuncionNumeroParametros { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo de Retorno")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string FuncionTipoDeRetorno { get; set; }

        [MaxLength(5), Column(TypeName = "VARCHAR")]
        public string ConfigId { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string FuncionDescripcion { get; set; }
        
        public virtual List<ParametroDto> Parametros { get; set; }
        public virtual ConfigItemDto TipoDeRetorno { get; set; }

        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}