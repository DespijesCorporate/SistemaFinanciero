using appcitas.Context;
using appcitas.Dtos;
using appcitas.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace appcitas.Controllers
{
    public class VariablesController : Controller
    {
        private AppcitasContext _context;

        public VariablesController()
        {
            _context = new AppcitasContext();
        }

        // GET: Variables
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult ViewVariableData(string id)
        {
            VariableDto objDto = new VariableDto();

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return HttpNotFound();
                }
                else
                {
                    objDto = Mapper.Map<Variable, VariableDto>(_context.Variables.SingleOrDefault(v => v.VariableCodigo == id));
                    var origen = _context.ItemsDeConfiguracion.SingleOrDefault(x => x.ConfigItemID == objDto.OrigenId);
                    var datoRetorno = _context.ItemsDeConfiguracion.SingleOrDefault(x => x.ConfigItemID == objDto.DatoDeRetornoId);
                    var tipo = _context.ItemsDeConfiguracion.SingleOrDefault(x => x.ConfigItemID == objDto.TipoId);

                    objDto.OrigenDto = Mapper.Map<ConfigItem, ConfigItemDto>(origen);
                    objDto.DatoDeRetornoDto = Mapper.Map<ConfigItem, ConfigItemDto>(datoRetorno);
                    objDto.TipoDto = Mapper.Map<ConfigItem, ConfigItemDto>(tipo);

                    if (objDto == null)
                    {
                        return HttpNotFound("No se encontro ninguna variable con este id");
                    }
                }
            }
            catch (Exception exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, exception.Message.ToString());
            }

            return PartialView(objDto);
        }

        public ActionResult NewVariable()
        {
            ViewBag.Origenes = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "SRC")
                                      .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.Tipos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "DDR")
                                    .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.TiposVariables = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TIPO")
                                             .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.VariablesList = _context.Variables.ToList().Select(Mapper.Map<Variable, VariableDto>);
            ViewBag.FuncionesList = _context.Funciones.ToList().Select(Mapper.Map<Funcion, FuncionDto>);

            ViewBag.Titulo = "Creando Variable";


            var viewModel = new VariableDto();
            return PartialView("VariableForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarVariable(VariableDto vm)
        {
            var variableEnDb = _context.Variables
                                      .SingleOrDefault(v => v.VariableCodigo == vm.VariableCodigo);
            ViewBag.Title = variableEnDb == null ? "Creando Variable" : "Editando Variable";

            try
            {
                if(!ModelState.IsValid)
                {
                    vm.Accion = 0;
                    vm.Mensaje = "los datos enviados no son correctos, verifiquelos e intente de nuevo";
                    return Json(vm, JsonRequestBehavior.AllowGet);
                }

                if (variableEnDb == null)
                    _context.Variables.Add(Mapper.Map<VariableDto, Variable>(vm));
                else
                {
                    variableEnDb.VariableNombre = vm.VariableNombre;
                    variableEnDb.OrigenId = vm.OrigenId;
                    variableEnDb.DatoDeRetornoId = vm.DatoDeRetornoId;
                    variableEnDb.TipoId = vm.TipoId;
                    variableEnDb.VariableDescripcion = vm.VariableDescripcion;
                    variableEnDb.VariableFormula = vm.VariableFormula;
                    variableEnDb.VariableValor = vm.VariableValor;
                }

                if (_context.ChangeTracker.HasChanges())
                    _context.SaveChanges();

                vm.Accion = 1;
                vm.Mensaje = "datos guardados exitosamente!";
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                vm.Accion = 0;
                vm.Mensaje = ex.Message.ToString();
                return Json(vm, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditarVariable(string id)
        {
            var variableEnDb = _context.Variables.SingleOrDefault(v => v.VariableCodigo == id);

            if (variableEnDb == null)
                return HttpNotFound();
            ViewBag.Origenes = _context.ItemsDeConfiguracion.Where(x=>x.ConfigID == "SRC")
                                       .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.Tipos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "DDR")
                                    .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.TiposVariables = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TIPO")
                                             .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            ViewBag.VariablesList = _context.Variables.Where(x=>x.VariableCodigo!=id)
                .ToList().Select(Mapper.Map<Variable, VariableDto>);
            ViewBag.FuncionesList = _context.Funciones.ToList().Select(Mapper.Map<Funcion, FuncionDto>);
            ViewBag.Titulo = "Editando Variable";

            return PartialView("VariableForm", Mapper.Map<Variable,VariableDto>(variableEnDb));
        }

        public JsonResult EliminarVariable(string id)
        {
            var variableEnDb = _context.Variables.SingleOrDefault(v => v.VariableCodigo == id);

            if (variableEnDb == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            _context.Variables.Remove(variableEnDb);

            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerVariables()
        {
            try
            {
                var variables = (from v in _context.Variables
                                 select new VariableDto
                                 {
                                     VariableCodigo = v.VariableCodigo,
                                     VariableNombre = v.VariableNombre,
                                     OrigenId = v.OrigenId,
                                     DatoDeRetornoId = v.DatoDeRetornoId,
                                     TipoId = v.TipoId,
                                     VariableDescripcion = v.VariableDescripcion,
                                     VariableFormula = v.VariableFormula,
                                     VariableValor = v.VariableValor,                                     
                                     Accion = 1,
                                     Mensaje = "se cargaron correctament los datos!"
                                 }).ToList();
                if(variables.Count == 0)
                {
                    var tmp = new VariableDto { Accion = 0, Mensaje = "No se encontraron registros!" };
                    variables.Add(tmp);
                }

                return Json(variables, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<VariableDto> list = new List<VariableDto>();
                VariableDto obj = new VariableDto() { Accion = 0, Mensaje = ex.Message.ToString() };
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
    }
}