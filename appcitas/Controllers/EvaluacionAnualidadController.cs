using appcitas.Context;
using appcitas.Dtos;
using appcitas.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Configuration;
using System.Web.Mvc;
using appcitas.Models;
using System.Collections;
using System.Reflection;
using Z.Expressions;
using AutoMapper;
using System.Text;
using Expressive;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Controllers
{
    public class EvaluacionAnualidadController : MyBaseController
    {
        private AppcitasContext _context;

        public EvaluacionAnualidadController()
        {
            _context = new AppcitasContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: EvaluacionAnualidad
        public ActionResult Index()
        {
            ViewBag.TipoDeAnualidades = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TANLD");
            var model = new AnualidadDto() { Fecha = DateTime.Now.Date, FechaDeCargo = DateTime.Now.Date };
            return View(model);
        }

        //GET: Metodo de WebService de BusquedaPorTarjeta
        [HttpGet]
        public async Task<ActionResult> BuscarPorTarjeta(string tarjeta)
        {
            var obj = await BACWS.GetGeneralByTC(tarjeta);
            if (obj != null)
                return Json(obj, JsonRequestBehavior.AllowGet);
            return new HttpNotFoundResult("No se Encontro ningun dato");
        }

        //GET: Metodo de WebService de BusquedaPorCuenta
        [HttpGet]
        public async Task<ActionResult> BuscarPorCuenta(string cuenta)
        {
            var obj = await BACWS.GetValuesByCif(cuenta);
            if (obj != null)
                return Json(obj, JsonRequestBehavior.AllowGet);

            return new HttpNotFoundResult("No se Encontro ningun dato");
        }

        //GET: Metodo que busca los datos que el motor debe traer de las tabalas
        [HttpGet]
        public ActionResult BuscarDatosDeCatalogos(string cuenta)
        {
            var obj = (from emisor in _context.DbSetEmisores
                       where emisor.EmisorCuenta == cuenta
                       select new EmisorDto
                       {
                           Segmento =
                           ((from itemConf in _context.ItemsDeConfiguracion
                             where itemConf.ConfigID == "SEGM" && itemConf.ConfigItemID == emisor.SegmentoId
                             select new { itemConf.ConfigItemAbreviatura }).FirstOrDefault().ConfigItemAbreviatura),
                           Marca =
                           ((from itemConf in _context.ItemsDeConfiguracion
                             where itemConf.ConfigID == "MRCA" && itemConf.ConfigItemID == emisor.MarcaId
                             select new { itemConf.ConfigItemAbreviatura }).FirstOrDefault().ConfigItemAbreviatura),
                           Producto = emisor.Producto,
                           Familia = emisor.Familia,
                           SegmentoId = emisor.SegmentoId,
                           MarcaId = emisor.MarcaId,
                       }).FirstOrDefault();

            if (obj != null)
                return Json(obj, JsonRequestBehavior.AllowGet);
            else
            {
                return new HttpNotFoundResult("No se encontraron datos");
            }
        }

        //GET: Metodo para Evaluar las variables
        [HttpGet]
        public async Task<ActionResult> EvaluarVariables(string tipoAnualidad, string cuenta, string fechaDeCargo, string segmento, string montoAnualidad)
        {
            try
            {
                List<VariablesAEvaluar> variablesValoresComparativos = ObtenerValoresComparativos(tipoAnualidad, segmento);

                BACObject _BACObject = await BACWS.GetGeneralByTC(cuenta);

                var variablesEvaluadas = new List<AnualidadVariableEvaluadaDto>();
                var resultadoVariables = new List<AnualidadVariableEvaluadaDto>();
                var todasVariablesEvaluadas = new List<AnualidadVariableEvaluadaDto>();
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("Fecha1", fechaDeCargo);
                keyValuePairs.Add("Anualidad", montoAnualidad);

                var variablesPropias = _context.Variables.Where(x => x.OrigenId == "SRC2").ToList();

                if (_BACObject != null && variablesValoresComparativos != null)
                {
                    PropertyInfo[] properties = typeof(BACObject).GetProperties();

                    foreach (var var1 in variablesValoresComparativos)
                    {
                        var tipo = _context.ItemsDeConfiguracion.SingleOrDefault(x => x.ConfigItemID == var1.Variable.DatoDeRetornoId).ConfigItemAbreviatura;

                        switch (var1.Variable.OrigenId)
                        {
                            case "SRC1":
                                foreach (var property in properties)
                                {
                                    if (var1.VariableNombre == property.Name)
                                    {
                                        if (var1.ValorAEvaluar.Contains("{"))
                                        {
                                            EvaluarValoresDeLista(_BACObject, ref variablesEvaluadas, var1, property);
                                        }
                                        else if (!var1.ValorAEvaluar.Contains("["))
                                        {
                                            EvaluarOtrosValores(_BACObject, ref variablesEvaluadas, var1, property);
                                        }
                                        else if (var1.ValorAEvaluar.Contains("["))
                                        {
                                            EvaluarValoresDeFormula(_BACObject, ref variablesEvaluadas, var1, property, montoAnualidad);
                                        }
                                    }
                                }
                                break;
                            case "SRC2":
                                foreach (var v in variablesPropias)
                                {
                                    if (var1.ValorAEvaluar.Contains("{"))
                                    {
                                        EvaluarValoresDeLista(ref variablesEvaluadas, var1, Mapper.Map<Variable, VariableDto>(v), _BACObject, keyValuePairs);
                                    }
                                    else if (!var1.ValorAEvaluar.Contains("["))
                                    {
                                        EvaluarOtrosValores(_BACObject, ref variablesEvaluadas, keyValuePairs, var1, v);
                                    }
                                    else if (var1.ValorAEvaluar.Contains("["))
                                    {

                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                foreach (var variableEval in variablesEvaluadas.GroupBy(x => x.ItemDeReclamoId))
                {
                    foreach (var variablelocal in variableEval)
                    {
                        if (variablelocal.EvaluacionCondicion)
                        {
                            resultadoVariables.Add(variablelocal);
                        }
                        else
                        {
                            resultadoVariables = new List<AnualidadVariableEvaluadaDto>();
                            break;
                        }
                    }
                }

                if (resultadoVariables.Count() == 0)
                {
                    foreach (var variableEval in variablesEvaluadas.GroupBy(x => x.ItemDeReclamoId))
                    {
                        foreach (var variablelocal in variableEval)
                        {
                            todasVariablesEvaluadas.Add(variablelocal);
                        }
                    }
                    todasVariablesEvaluadas.FirstOrDefault().Mensaje = "No se obtuve ningun resultado favorable";
                    todasVariablesEvaluadas.GroupBy(x => x.ItemDeReclamoId);
                    return Json(todasVariablesEvaluadas, JsonRequestBehavior.AllowGet);
                }

                return Json(resultadoVariables, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var tmp = new ResultadoEvaluacionVariableDto() { Accion = 0, Mensaje = ex.Message.ToString() };
                var lista = new List<object>() { tmp };
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        }

        private static void EvaluarOtrosValores(BACObject _BACObject, ref List<AnualidadVariableEvaluadaDto> variablesEvaluadas, Dictionary<string, object> keyValuePairs, VariablesAEvaluar var1, Variable v)
        {
            switch (var1.CondicionLogica)
            {
                case "==":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR1":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = v.VariableValor.ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion = v.VariableValor == var1.ValorAEvaluar,
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        case "DDR6":
                            string valorActual = "";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) == Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "!=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR1":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = v.VariableValor,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion = v.VariableValor != var1.ValorAEvaluar,
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        case "DDR6":
                            string valorActual = "";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) != Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "<=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            string valorActual = "";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) <= Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case ">=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            string valorActual = "0";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) >= Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "<":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            string valorActual = "";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) < Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case ">":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            string valorActual = "";
                            if (v.TipoId == "TIPO1")
                                valorActual = v.VariableValor;
                            else if (v.TipoId == "TIPO2")
                            {
                                var formulaResuelta = FormulaEvaluator.EvaluarFormulaDeVariable(v.VariableFormula, keyValuePairs, _BACObject);
                                valorActual = new Expression(formulaResuelta.ToString()).Evaluate().ToString();
                            }
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = valorActual,
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(valorActual) > Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        //Metodo para evaluar valores comparativos normales contra los valores del WS
        private static void EvaluarOtrosValores(BACObject _BACObject, ref List<AnualidadVariableEvaluadaDto> variablesEvaluadas,
            VariablesAEvaluar var1, PropertyInfo property)
        {
            switch (var1.CondicionLogica)
            {
                case "==":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR1":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion = property.GetValue(_BACObject).ToString() == var1.ValorAEvaluar,
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) == Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "!=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR1":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion = property.GetValue(_BACObject).ToString() != var1.ValorAEvaluar,
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) != Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "<=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) <= Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case ">=":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) >= Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case "<":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) < Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                case ">":
                    switch (var1.Variable.DatoDeRetornoId)
                    {
                        case "DDR6":
                            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                            {
                                VariableCodigo = var1.VariableCodigo,
                                VariableNombre = var1.VariableNombre,
                                VariableDeItemId = var1.VariableDeItemId,
                                ReclamoId = var1.ReclamoId,
                                ItemDeReclamoId = var1.ItemDeReclamoId,
                                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                                Variable = var1.Variable,
                                CondicionLogica = var1.CondicionLogica,
                                ValorActual = property.GetValue(_BACObject).ToString(),
                                ValorAEvaluar = var1.ValorAEvaluar,
                                EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(_BACObject).ToString()) > Convert.ToDecimal(var1.ValorAEvaluar),
                                Accion = 1,
                                Mensaje = "Se cargaron los datos correctamente"
                            });
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        //Metodo para evaluar valores comparativos con formula contra los valores del WS
        private static void EvaluarValoresDeFormula(BACObject bacObject, ref List<AnualidadVariableEvaluadaDto> variablesEvaluadas,
            VariablesAEvaluar var1, PropertyInfo property, string montoAnualidad)
        {

            var parametros = new Dictionary<string, object>
            {
                { "Anualidad", montoAnualidad },
                { "Limite", bacObject.Limite }
            };

            var resultado = FormulaEvaluator.EvaluarFormulaConValoresDeWS(bacObject, var1.ValorAEvaluar, parametros);

            if (var1.Variable.DatoDeRetornoId == "DDR6")
            {
                switch (var1.CondicionLogica)
                {
                    case "==":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) == Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    case "!=":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) != Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    case "<=":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) <= Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    case ">=":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) >= Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    case "<":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) < Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    case ">":
                        variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
                        {
                            VariableCodigo = var1.VariableCodigo,
                            VariableNombre = var1.VariableNombre,
                            VariableDeItemId = var1.VariableDeItemId,
                            ReclamoId = var1.ReclamoId,
                            ItemDeReclamoId = var1.ItemDeReclamoId,
                            ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                            Variable = var1.Variable,
                            CondicionLogica = var1.CondicionLogica,
                            ValorActual = property.GetValue(bacObject).ToString(),
                            ValorAEvaluar = var1.ValorAEvaluar,
                            EvaluacionCondicion =
                                Convert.ToDecimal(property.GetValue(bacObject).ToString()) > Convert.ToDecimal(resultado),
                            Accion = 1,
                            Mensaje = "Se cargaron los datos correctamente"
                        });
                        break;
                    default:
                        break;
                }
            }
        }

        //Metodo para evaluar valores comparativos de tipo lista o rango contra los valores del WS
        private static void EvaluarValoresDeLista(BACObject _BACObject, ref List<AnualidadVariableEvaluadaDto> variablesEvaluadas,
            VariablesAEvaluar var1, PropertyInfo property)
        {
            var valoresParaEvaluarConDelimitador = var1.ValorAEvaluar.Split("{}".ToCharArray())[1];
            var arregloDeValoresAEvaluar = valoresParaEvaluarConDelimitador.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            bool igual = false;
            foreach (var item in arregloDeValoresAEvaluar)
            {
                if (item == property.GetValue(_BACObject).ToString())
                {
                    igual = true;
                    break;
                }
            }

            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
            {
                VariableCodigo = var1.VariableCodigo,
                VariableNombre = var1.VariableNombre,
                VariableDeItemId = var1.VariableDeItemId,
                ReclamoId = var1.ReclamoId,
                ItemDeReclamoId = var1.ItemDeReclamoId,
                ItemDeReclamoNombre = var1.ItemDeReclamoNombre,
                Variable = var1.Variable,
                CondicionLogica = var1.CondicionLogica,
                ValorActual = property.GetValue(_BACObject).ToString(),
                ValorAEvaluar = var1.ValorAEvaluar,
                Accion = 1,
                Mensaje = "Se cargaron los datos correctamente",
                EvaluacionCondicion = igual,
            });
        }

        //Metodo para evaluar valores comparativos de tipo lista o rango contra valores del motor
        public static void EvaluarValoresDeLista(ref List<AnualidadVariableEvaluadaDto> variablesEvaluadas, VariablesAEvaluar variableAEvaluar, VariableDto variable, 
            BACObject bacObject, Dictionary<string, object> keyValuePairs)
        {
            var valoresParaEvaluarConDelimitador = variableAEvaluar.ValorAEvaluar.Split("{}".ToCharArray())[1];
            var arregloDeValoresAEvaluar = valoresParaEvaluarConDelimitador.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            bool igual = false;
            string valorActual = "";

            foreach (var item in arregloDeValoresAEvaluar)
            {
                switch (variable.TipoId)
                {
                    case "TIPO1":
                        valorActual = variable.VariableValor;
                        if (item == variable.VariableValor)
                        {
                            igual = true;
                            break;
                        }
                        else igual = false;                        
                        break;
                    case "TIPO2":
                        var formulaAEvaluar = FormulaEvaluator.EvaluarFormulaDeVariable(variable.VariableFormula, keyValuePairs, bacObject).ToString();                        
                        var ExpresionAEvaluar = new Expression(formulaAEvaluar);
                        valorActual = ExpresionAEvaluar.Evaluate().ToString();
                        if (item == valorActual)
                        {
                            igual = true;
                            break;
                        }
                        else igual = false;
                        break;
                    default:
                        break;
                }
            }

            variablesEvaluadas.Add(new AnualidadVariableEvaluadaDto
            {
                VariableCodigo = variableAEvaluar.VariableCodigo,
                VariableNombre = variableAEvaluar.VariableNombre,
                VariableDeItemId = variableAEvaluar.VariableDeItemId,
                ReclamoId = variableAEvaluar.ReclamoId,
                ItemDeReclamoId = variableAEvaluar.ItemDeReclamoId,
                ItemDeReclamoNombre = variableAEvaluar.ItemDeReclamoNombre,
                Variable = variableAEvaluar.Variable,
                CondicionLogica = variableAEvaluar.CondicionLogica,
                ValorActual = valorActual,
                ValorAEvaluar = variableAEvaluar.ValorAEvaluar,
                Accion = 1,
                Mensaje = "Se cargaron los datos correctamente",
                EvaluacionCondicion = igual,
            });
        }

        //Metodo que obtiene los valores comparativos de acuerdo al tipo de anualidad y segmento
        private List<VariablesAEvaluar> ObtenerValoresComparativos(string tipoAnulidad, string segmento)
        {
            return (from vi in _context.VariablesDeItem.Include(x => x.Variable)
                    join ir in _context.ItemsDeReclamo on vi.ItemDeReclamoId equals ir.ItemDeReclamoId
                    where vi.ReclamoId ==
                    ((from r in _context.Reclamos
                      where r.ReclamoNombre == "Anualidad"
                      select new
                      {
                          r.ReclamoId
                      }).FirstOrDefault().ReclamoId) &&
                      ir.ItemDeReclamoDescripcion.Contains(tipoAnulidad)
                      && ir.ItemDeReclamoDescripcion.ToLower().Contains(segmento.ToLower())
                    select new VariablesAEvaluar
                    {
                        VariableCodigo = vi.VariableCodigo,
                        CondicionLogica = vi.CondicionLogica,
                        ValorAEvaluar = vi.ValorAEvaluar,
                        ReclamoId = vi.ReclamoId,
                        ItemDeReclamoId = vi.ItemDeReclamoId,
                        ItemDeReclamoNombre = ir.ItemDeReclamoDescripcion,
                        VariableDeItemId = vi.VariableDeItemId,
                        VariableNombre = vi.Variable.VariableNombre,
                        Variable = new VariableDto
                        {
                            VariableCodigo = vi.Variable.VariableCodigo,
                            VariableNombre = vi.Variable.VariableNombre,
                            OrigenId = vi.Variable.OrigenId,
                            DatoDeRetornoId = vi.Variable.DatoDeRetornoId,
                            TipoId = vi.Variable.TipoId,
                            VariableDescripcion = vi.Variable.VariableDescripcion,
                            VariableFormula = vi.Variable.VariableFormula,
                            VariableValor = vi.Variable.VariableValor
                        }
                    }).ToList();
        }

        [HttpPost]
        public ActionResult ObtenerResultados(List<AnualidadVariableEvaluadaDto> dataList)
        {
            var listaDeResultados = new List<AnualidadResultadoObtenidoDto>();

            foreach (var item in dataList.GroupBy(x => x.ItemDeReclamoId))
            {
                foreach (var variable in item)
                {
                    if (variable.EvaluacionCondicion)
                    {
                        if (!listaDeResultados.Any(x => x.ItemDeReclamoId == variable.ItemDeReclamoId))
                        {
                            listaDeResultados.Add(new AnualidadResultadoObtenidoDto
                            {
                                ReclamoId = variable.ReclamoId,
                                ItemDeReclamoId = variable.ItemDeReclamoId,
                                ItemDeReclamoDescripcion = variable.ItemDeReclamoNombre
                            });
                        }
                        else
                        {
                            listaDeResultados = null;
                            listaDeResultados = new List<AnualidadResultadoObtenidoDto>();
                            break;
                        }
                    }
                    else break;
                }
            }

            ResultadoVM model = new ResultadoVM() { Resultados = listaDeResultados };
            return Json(new
            {
                statusCode = 1,
                statusMessage = "Se obtuvo resultados",
                resultadosHtml = RenderPartialViewToString("_ResultadosDeEvaluacion", model)
            });
        }

        [HttpPost]
        public ActionResult GuardarAnualidad(AnualidadDto anualidad)
        {
            if (anualidad.Resultados != null)
            {
                foreach (var item in anualidad.Resultados)
                {
                    item.ReclamoId = "001";
                }
            }

            _context.Anualidades.Add(Mapper.Map<AnualidadDto, Anualidad>(anualidad));

            _context.SaveChanges();

            anualidad.Accion = 1;
            anualidad.Mensaje = "Datos guardados exitosamente!";
            return Json(anualidad, JsonRequestBehavior.AllowGet);
        }
    }

    public class VariablesAEvaluar
    {
        public string VariableCodigo { get; set; }
        public string CondicionLogica { get; set; }
        public string ValorAEvaluar { get; set; }
        public string ReclamoId { get; set; }
        public string ItemDeReclamoId { get; set; }
        public string ItemDeReclamoNombre { get; set; }
        public Guid VariableDeItemId { get; set; }
        public string VariableNombre { get; set; }
        public VariableDto Variable { get; set; }
    }

    public class ResultadoVM
    {
        [Display(Name = "Resultados Obtenidos")]
        public List<AnualidadResultadoObtenidoDto> Resultados { get; set; }
    }
}