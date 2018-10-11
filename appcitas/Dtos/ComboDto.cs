using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ComboDto
    {
        public Guid ComboId { get; set; }

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

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string SegementoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Marca")]
        [StringLength(50, ErrorMessage = "Este campo no puede ser mas larga de 50 caracteres")]
        public string Marca { get; set; }

        [Required, MaxLength(10), Column(TypeName = "VARCHAR")]
        public string MarcaId { get; set; }

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

        [Display(Name = "Combo")]
        [Required(ErrorMessage = "este campo es requerido")]
        public string ComboName { get; set; }

        [Required(ErrorMessage = "Este campo es requerido"), MaxLength(10), Column(TypeName = "VARCHAR")]
        [Display(Name = "Combo")]
        public string ComboOpId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Cargo")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string PrimerCargoId { get; set; }

        [Required]
        public string PrimerCargoName { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Currency)]
        public decimal MontoPrimerCargo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaPrimerCargo { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Cargo")]
        [MaxLength(10), Column(TypeName = "VARCHAR")]
        public string SegundoCargoId { get; set; }

        [Required]
        public string SegundoCargoName { get; set; }

        [Display(Name = "Monto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Currency)]
        public decimal MontoSegundoCargo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha de Cargo")]
        [DataType(DataType.Date)]
        public DateTime FechaSegundoCargo { get; set; }
    }
}