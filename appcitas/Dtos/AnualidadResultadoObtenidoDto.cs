using appcitas.ClaseBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class AnualidadResultadoObtenidoDto : ResultadoObtenido
    {
        public Guid AnualidadId { get; set; }        
    }
}