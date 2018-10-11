using appcitas.ClaseBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class AnualidadResultadoObtenido : ResultadoObtenido
    {
        [ForeignKey("Anualidad")]
        public Guid AnualidadId { get; set; }

        public virtual Anualidad Anualidad { get; set; }
    }
}