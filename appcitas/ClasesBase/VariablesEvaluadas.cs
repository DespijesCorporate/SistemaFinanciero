using appcitas.Dtos;
using System;

namespace appcitas.ClasesBase
{
    public class VariablesEvaluadas
    {
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
        public VariableDto Variable { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}