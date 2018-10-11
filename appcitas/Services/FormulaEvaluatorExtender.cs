using Expressive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appcitas.Services
{
    public static class FormulaEvaluatorExtender
    {
        public static void DoRegisterFunctions(ref Expression expression)
        {
            //expression.RegisterFunction(new AbsFunction());
        }
    }

    //Este es un ejemplo por si se quiere extender la funcionalidad
    //internal class AbsFunction : IFunction
    //{
    //    #region IFunction Members

    //    public IDictionary<string, object> Variables { get; set; }

    //    public string Name { get { return "Asin"; } }

    //    public object Evaluate(IExpression[] parameters)
    //    {
    //        return Math.Asin(Convert.ToDouble(parameters[0].Evaluate(Variables)));
    //    }

    //    public object Evaluate(IExpression[] parameters, ExpressiveOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}
}
