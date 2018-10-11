using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class ItemDeReclamo
    {
        [Key, ForeignKey("Reclamo"), Column(Order = 0)]
        public string ReclamoId { get; set; }

        [Key, Column(Order = 1)]
        public string ItemDeReclamoId { get; set; }

        public string ItemDeReclamoDescripcion { get; set; }
        
        public virtual Reclamo Reclamo { get; set; }

        public virtual List<VariableDeItem> VariablesAEvaluar { get; set; }
    }
}