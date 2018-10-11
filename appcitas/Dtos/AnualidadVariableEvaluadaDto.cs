using appcitas.ClasesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class AnualidadVariableEvaluadaDto : VariablesEvaluadas
    {
        public Guid AnualidadId { get; set; }
    }
}