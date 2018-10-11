using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class Reclamo
    {
        [Key]
        public string ReclamoId { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        public string ReclamoNombre { get; set; }
        
        public string ReclamoDescripcion { get; set; }

        public virtual List<ItemDeReclamo> ItemsDeReclamo { get; set; }
    }
}