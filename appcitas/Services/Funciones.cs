using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Services
{
    public static class Funciones
    {
        public static object Frecuencia(DateTime FechaCargo)
        {
            return (DateTime.Now.Date - FechaCargo.Date).TotalDays;
        }

        public static object Frecuencia(DateTime FechaCargo1, DateTime FechaCargo2)
        {
            return (DateTime.Now.Date - FechaCargo1.Date).TotalDays;
        }
    }
}