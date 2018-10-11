using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ReclamoDto
    {
        [Key, Display(Name = "Codigo"), Required(ErrorMessage = "este campo es requerido")]
        public string ReclamoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Nombre de Reclamo/Producto")]
        public string ReclamoNombre { get; set; }

        [Display(Name = "Descripción")]
        public string ReclamoDescripcion { get; set; }

        public int Accion { get; set; }

        public string Mensaje { get; set; }
    }
}