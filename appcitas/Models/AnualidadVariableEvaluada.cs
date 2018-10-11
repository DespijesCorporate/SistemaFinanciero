using appcitas.ClasesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace appcitas.Models
{
    public class AnualidadVariableEvaluada
    {
        [ForeignKey("Anualidad")]
        public Guid AnualidadId { get; set; }

        [ForeignKey("Variable")]
        public string VariableCodigo { get; set; }

        public string CondicionLogica { get; set; }

        public string ValorAEvaluar { get; set; }

        public string ValorActual { get; set; }

        public bool EvaluacionCondicion { get; set; }

        public string ReclamoId { get; set; }

        public string ItemDeReclamoId { get; set; }

        public string ItemDeReclamoNombre { get; set; }

        public Guid VariableDeItemId { get; set; }

        public string VariableNombre { get; set; }

        public virtual Anualidad Anualidad { get; set; }

        public virtual Variable Variable { get; set; }
    }
}