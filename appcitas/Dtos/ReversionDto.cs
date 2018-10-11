using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ReversionDto
    {
        public Guid ReversionId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Numero de Cuenta")]
        [StringLength(50, ErrorMessage = "Este campo no puede contener mas de 50 caracteres")]
        public string NumeroCuenta { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "CIF")]
        [StringLength(25, ErrorMessage = "Este campo no puede contener mas de 25  caracteres")]
        public string CIF { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Familia")]
        [StringLength(50, ErrorMessage = "Este campo no puede contener mas de 50 caracteres")]
        public string Familia { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Segmento")]
        [StringLength(100)]
        public string Segmento { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Marca")]
        [StringLength(50, ErrorMessage = "Este campo no puede ser mas larga de 50 caracteres")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Saldo Actual")]
        [DataType(DataType.Currency)]
        public decimal SaldoActual { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Limite")]
        [DataType(DataType.Currency)]
        public decimal Limite { get; set; }
        
        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_1 { get; set; }

        public string TipoeReversion_1 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_1 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_1 { get; set; }

        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_2 { get; set; }

        public string TipoeReversion_2 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_2 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_2 { get; set; }

        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_3 { get; set; }

        public string TipoeReversion_3 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_3 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_3 { get; set; }

        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_4 { get; set; }

        public string TipoeReversion_4 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_4 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_4 { get; set; }

        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_5 { get; set; }

        public string TipoeReversion_5 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_5 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_5 { get; set; }

        [Display(Name = "Tipo de Reversión")]
        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string TipoReversionId_6 { get; set; }

        public string TipoeReversion_6 { get; set; }

        [Display(Name = "Monto")]
        [DataType(DataType.Currency)]
        public decimal Monto_6 { get; set; }

        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaCargo_6 { get; set; }

        [Display(Name = "Observacion")]
        [DataType(DataType.MultilineText)]
        public string Observacion { get; set; }

        public List<VariableReversionDto> VariablesEvaluadas { get; set; }

        public List<ResultadoReversionDto> ResultadosDeReversion { get; set; }
    }
}