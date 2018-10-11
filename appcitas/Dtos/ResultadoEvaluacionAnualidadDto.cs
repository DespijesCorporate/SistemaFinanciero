using System;

namespace appcitas.Dtos
{
    public class ResultadoEvaluacionAnualidadDto
    {        
        public int ResultadoEvaluacionAnualidadId { get; set; }

        public string ResultadoDescripcion { get; set; }

        public bool ResultadoAceptado { get; set; }
                
        public Guid AnualidadId { get; set; }
    }
}