using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ResultadoEvaluacionVariableDto
    {
        [Key]
        public Guid Id { get; set; }
        public string VariableCodigo { get; set; }
        public string VariableNombre { get; set; }
        public string VariableValorActual { get; set; }
        public string CondicionLogica { get; set; }
        public string VariableValorComparativo { get; set; }
        public bool Resultado { get; set; }
        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}