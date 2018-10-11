using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace appcitas.Services
{
    public static class EvaluarFunciones
    {
        public static object Evaluar(string nombreFuncion, string[] paramArrays=null)
        {
            object resultado = null;
            switch (nombreFuncion)
            {
                case "Frecuencia":
                    if (paramArrays.Count() > 1)
                        resultado = Funciones.Frecuencia(DateTime.Parse(paramArrays[0], CultureInfo.CurrentCulture),
                            DateTime.Parse(paramArrays[1], CultureInfo.CurrentCulture));
                    else if (paramArrays.Count() == 1)
                        resultado = Funciones.Frecuencia(DateTime.Parse(paramArrays[0], CultureInfo.CurrentCulture));
                    break;
                default:
                    break;
            }

            return resultado;
        }
    }
}