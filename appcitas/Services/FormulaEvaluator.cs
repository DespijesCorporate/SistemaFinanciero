using appcitas.Context;
using appcitas.Dtos;
using appcitas.Models;
using AutoMapper;
using Expressive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace appcitas.Services
{
    public static class FormulaEvaluator
    {
        public static object EvaluarFormulaConValoresDeWS(BACObject bacObject, string expresion, Dictionary<string, object> parameters = null)
        {
            var expression = new Expression(expresion);
            //RegisterFunctions.DoRegisterFunctions(ref expression);
            var variables = expression.ReferencedVariables;

            PropertyInfo[] properties = typeof(BACObject).GetProperties();
            var variablesDictionary = new Dictionary<string, object>();
            foreach (var item in variables)
            {
                foreach (var property in properties)
                {
                    if (property.Name == item)
                    {
                        variablesDictionary.Add(item, Convert.ToDecimal(property.GetValue(bacObject)));
                    }
                }

                if (item.StartsWith("@"))
                {
                    var nombre = item.Trim('@');
                    foreach (var param in parameters)
                    {
                        if (param.Key == nombre)
                            variablesDictionary.Add(item, Convert.ToDecimal(param.Value));
                    }                    
                }
                if (item.StartsWith("$"))
                {
                    string[] paramArray = null;
                    string fnNombre = "";
                    if (item.Contains("("))
                    {
                        var parametroString = item.Split("()".ToCharArray())[1];
                        paramArray = parametroString.Split(',');
                        fnNombre = item.Split("$(".ToCharArray())[1];
                    }
                    else
                    {
                        fnNombre = item.Split("$".ToCharArray())[1];
                    }
                    var resultado = EvaluarFunciones.Evaluar(fnNombre, paramArray);
                    variablesDictionary.Add(item, Convert.ToDecimal(resultado));
                }
            }


            return expression.Evaluate(variablesDictionary);
        }

        public static object EvaluarFormulaConValoresDeMotor(string expresion, BACObject bacObject, Dictionary<string, object> parameters = null)
        {
            var expression = new Expression(expresion);
            //RegisterFunctions.DoRegisterFunctions(ref expression);
            var variables = expression.ReferencedVariables;

            List<VariableDto> variablesDeMotor = new List<VariableDto>();

            using (var ctxt = new AppcitasContext())
            {
                variablesDeMotor = ctxt.Variables.Where(x => x.OrigenId == "SRC2").Select(x => new VariableDto
                {
                    VariableCodigo = x.VariableCodigo,
                    VariableNombre = x.VariableNombre,
                    VariableDescripcion = x.VariableDescripcion,
                    VariableFormula = x.VariableFormula,
                    VariableValor = x.VariableValor,
                    OrigenId = x.OrigenId,
                    DatoDeRetornoId = x.DatoDeRetornoId,
                    TipoId = x.TipoId
                }).ToList();
            }
            
            var variablesDictionary = new Dictionary<string, object>();
            foreach (var item in variables)
            {
                foreach (var v in variablesDeMotor)
                {
                    if (item == v.VariableNombre)
                    {
                        switch (v.TipoId)
                        {
                            case "TIPO1":
                                variablesDictionary.Add(item, Convert.ToDecimal(v.VariableValor));
                                break;
                            case "TIPO2":
                                variablesDictionary.Add(item, Convert.ToDecimal(EvaluarFormulaDeVariable(v.VariableFormula, parameters, bacObject)));
                                break;
                            default:
                                break;
                        }
                    }
                }


                if(item.Contains("@"))
                {
                    var nombre = item.Trim('@');
                    foreach (var param in parameters)
                    {
                        if (param.Key == nombre)
                            variablesDictionary.Add(item, Convert.ToDecimal(param.Value));
                    }
                }
                if(item.Contains("$"))
                {
                    string[] paramArray = null;
                    string fnNombre = "";
                    if (item.Contains("("))
                    {
                        var parametroString = item.Split("()".ToCharArray())[1];
                        paramArray = parametroString.Split(',');
                        fnNombre = item.Split("$(".ToCharArray())[1];
                    }
                    else
                    {
                        fnNombre = item.Split("$".ToCharArray())[1];
                    }
                    var resultado = EvaluarFunciones.Evaluar(fnNombre, paramArray);
                    variablesDictionary.Add(item, Convert.ToDecimal(resultado));
                }
            }

            return expression.Evaluate(variablesDictionary);
        }

        public static object EvaluarFormulaDeVariable(string expresion, Dictionary<string, object> parametros = null, BACObject bacObject = null, string opcion="")
        {
            do
            {
                var miExpression = new Expression(expresion);
                var variables = miExpression.ReferencedVariables;

                if (variables.FirstOrDefault().StartsWith("@"))
                {
                    var nombreParam = variables.FirstOrDefault().Trim('@');
                    var valorARemplazarPor = parametros.Any(x => x.Key == nombreParam) ? parametros.Where(x => x.Key == nombreParam).FirstOrDefault().Value : 0;
                    var newExpresion = expresion.Replace("[" + variables.FirstOrDefault() + "]", valorARemplazarPor.ToString());
                    expresion = newExpresion;
                }
                else if (variables.FirstOrDefault().StartsWith("$"))
                {
                    string[] paramArray = null;
                    string fnNombre = "";
                    if (variables.FirstOrDefault().Contains("("))
                    {
                        var parametroString = variables.FirstOrDefault().Split("()".ToCharArray())[1];
                        paramArray = parametroString.Split(',');
                        fnNombre = variables.FirstOrDefault().Split("$(".ToCharArray())[1];
                        for (int i = 0; i < paramArray.Length; i++)
                        {
                            var p = paramArray[i].Trim('@');
                            if (parametros.Any(x=>x.Key == p))
                            {
                                paramArray[i] = parametros.Where(x => x.Key == p).FirstOrDefault().Value.ToString();
                            }
                        }
                    }
                    else
                    {
                        fnNombre = variables.FirstOrDefault().Split("$".ToCharArray())[1];
                    }

                    var resultadoFormula = EvaluarFunciones.Evaluar(fnNombre, paramArray);
                    var newExpresion = expresion.Replace("[" + variables.FirstOrDefault() + "]", resultadoFormula.ToString());
                    expresion = newExpresion;
                }
                else
                {
                    PropertyInfo[] properties = typeof(BACObject).GetProperties();
                    bool noEsWS = false;
                    foreach (var property in properties)
                    {
                        if (property.Name == variables.FirstOrDefault())
                        {
                            var newExpresion = expresion.Replace("[" + variables.FirstOrDefault() + "]", property.GetValue(bacObject).ToString());
                            expresion = newExpresion;
                            noEsWS = true;
                        }
                    }

                    if (!noEsWS)
                    {
                        using (var ctxt = new AppcitasContext())
                        {
                            var variablesEnDb = ctxt.Variables.ToList();

                            foreach (var item in variablesEnDb)
                            {
                                string newExpresion = "";
                                if (variables.FirstOrDefault() == item.VariableNombre)
                                {
                                    if (!string.IsNullOrEmpty(item.VariableFormula))
                                    {
                                        newExpresion = expresion.Replace("[" + variables.FirstOrDefault() + "]", item.VariableFormula);
                                        expresion = newExpresion;
                                    }
                                    else
                                    {
                                        newExpresion = expresion.Replace("[" + variables.FirstOrDefault() + "]", item.VariableValor != "" ? item.VariableValor : "0");
                                        expresion = newExpresion;
                                    }
                                }
                            }                            
                        }                        
                    }
                }

            } while (expresion.Contains("["));
            return expresion;
        }
    }
}