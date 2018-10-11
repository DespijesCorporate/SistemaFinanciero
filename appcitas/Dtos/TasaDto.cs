using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class TasaDto
    {
        public Guid TasaId { get; set; }

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
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Familia")]
        [StringLength(50, ErrorMessage = "Este campo no puede contener mas de 50 caracteres")]
        public string Familia { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Segmento")]
        [StringLength(100)]
        public string Segmento { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string SegementoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Marca")]
        [StringLength(50, ErrorMessage = "Este campo no puede ser mas larga de 50 caracteres")]
        public string Marca { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string MarcaId { get; set; }

        [Display(Name = "Tasa Anualizada Actual")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public decimal TasaAnualizadaActual { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Cambio")]
        public DateTime FechaDelCambio { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Saldo Actual")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal SaldoActual { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Limite")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Limite { get; set; }
    }
}