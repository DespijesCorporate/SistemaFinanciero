using appcitas.Context;
using appcitas.Dtos;
using appcitas.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace appcitas.Controllers
{
    public class ConfiguracionesMotorController : Controller
    {
        private AppcitasContext _context;
        public ConfiguracionesMotorController()
        {
            _context = new AppcitasContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: ConfiguracionesMotor
        public ActionResult Index()
        {
            ViewBag.Existe = false;
            return View();
        }

        #region Reclamos        
        [HttpPost]
        public JsonResult ObtenerReclamos()
        {
            try
            {
                var reclamos = (from r in _context.Reclamos
                                 select new ReclamoDto
                                 {
                                     ReclamoId = r.ReclamoId,
                                     ReclamoNombre = r.ReclamoNombre,
                                     ReclamoDescripcion = r.ReclamoDescripcion,
                                     Accion = 1,
                                     Mensaje = "se cargaron correctament los datos!"
                                 }).ToList();
                if (reclamos.Count == 0)
                {
                    var tmp = new ReclamoDto { Accion = 0, Mensaje = "No se encontraron registros!" };
                    reclamos.Add(tmp);
                }

                return Json(reclamos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<ReclamoDto> list = new List<ReclamoDto>();
                ReclamoDto obj = new ReclamoDto() { Accion = 0, Mensaje = ex.Message.ToString() };
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult CreateReclamo()
        {
            var reclamo = new ReclamoDto();
            return PartialView("_ReclamoForm", reclamo);
        }

        [HttpGet]
        public ActionResult EditReclamo(string id)
        {
            var reclamoEnDb = _context.Reclamos.SingleOrDefault(v => v.ReclamoId == id);

            if (reclamoEnDb == null)
                return HttpNotFound();

            return PartialView("_ReclamoForm", Mapper.Map<Reclamo, ReclamoDto>(reclamoEnDb));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarReclamo(ReclamoDto reclamo)
        {
            var reclamoEnDb = _context.Reclamos
                .SingleOrDefault(x => x.ReclamoId == reclamo.ReclamoId);

            try
            {
                if (!ModelState.IsValid)
                {
                    reclamo.Accion = 0;
                    reclamo.Mensaje = "los datos enviados no son correctos, verifiquelos e intente de nuevo";
                    return Json(reclamo, JsonRequestBehavior.AllowGet);
                }

                if (reclamoEnDb == null)
                    _context.Reclamos.Add(Mapper.Map<ReclamoDto, Reclamo>(reclamo));
                else
                {
                    reclamoEnDb.ReclamoNombre = reclamo.ReclamoNombre;
                    reclamoEnDb.ReclamoDescripcion = reclamo.ReclamoDescripcion;
                }

                _context.SaveChanges();

                reclamo.Accion = 1;
                reclamo.Mensaje = "datos guardados exitosamente!";
                return Json(reclamo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                reclamo.Accion = 0;
                reclamo.Mensaje = ex.Message.ToString();
                return Json(reclamo, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public  JsonResult EliminarReclamo(string id)
        {
            var reclamoEnDb = _context.Reclamos
                .SingleOrDefault(x => x.ReclamoId == id);

            if (reclamoEnDb == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            _context.Reclamos.Remove(reclamoEnDb);
            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Items de Reclamos
        [HttpGet]
        public ActionResult CrearItem(string reclamoId)
        {
            ViewBag.Tipos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TANLD");
            ViewBag.Segmentos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "SEGM");
            var model = new ItemDeReclamoDto { ReclamoId = reclamoId };
            return PartialView("_ItemForm", model);
        }

        [HttpGet]
        public ActionResult EditarItem(string reclamoId, string itemId)
        {
            var itemEnDb = _context.ItemsDeReclamo
                .SingleOrDefault(x => x.ReclamoId == reclamoId
                && x.ItemDeReclamoId == itemId);

            if (itemEnDb == null)
                return HttpNotFound();
            ViewBag.Tipos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "TANLD");
            ViewBag.Segmentos = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "SEGM");
            return PartialView("_ItemForm", Mapper.Map<ItemDeReclamo, ItemDeReclamoDto>(itemEnDb));
        }

        [HttpPost]
        public JsonResult ObtenerItemsDeReclamo(string id)
        {
            try
            {
                var items = (from r in _context.ItemsDeReclamo
                             where r.ReclamoId == id
                                select new ItemDeReclamoDto
                                {
                                    ReclamoId = r.ReclamoId,
                                    ItemDeReclamoId = r.ItemDeReclamoId,
                                    ItemDeReclamoDescripcion = r.ItemDeReclamoDescripcion,
                                    Accion = 1,
                                    Mensaje = "se cargaron correctament los datos!"
                                }).ToList();
                if (items.Count == 0)
                {
                    var tmp = new ItemDeReclamoDto { Accion = 0, Mensaje = "No se encontraron registros!" };
                    items.Add(tmp);
                }

                return Json(items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<ItemDeReclamoDto> list = new List<ItemDeReclamoDto>();
                ItemDeReclamoDto obj = new ItemDeReclamoDto() { Accion = 0, Mensaje = ex.Message.ToString() };
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarItemsDeReclamo(ItemDeReclamoDto item)
        {
            var itemEnDb = _context.ItemsDeReclamo
                .SingleOrDefault(x => x.ReclamoId == item.ReclamoId
                && x.ItemDeReclamoId == item.ItemDeReclamoId);

            try
            {
                if (!ModelState.IsValid)
                {
                    item.Accion = 0;
                    item.Mensaje = "los datos enviados no son correctos, verifiquelos e intente de nuevo";
                    return Json(item, JsonRequestBehavior.AllowGet);
                }

                if (itemEnDb == null)
                    _context.ItemsDeReclamo.Add(Mapper.Map<ItemDeReclamoDto, ItemDeReclamo>(item));
                else
                {
                    itemEnDb.ItemDeReclamoDescripcion = item.ItemDeReclamoDescripcion;
                }

                _context.SaveChanges();

                item.Accion = 1;
                item.Mensaje = "datos guardados exitosamente!";
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                item.Accion = 0;
                item.Mensaje = ex.Message.ToString();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EliminarItem(string reclamoId, string itemId)
        {
            var itemEnDb = _context.ItemsDeReclamo
                .SingleOrDefault(x => x.ReclamoId == reclamoId && x.ItemDeReclamoId == itemId);

            if (itemEnDb == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            _context.ItemsDeReclamo.Remove(itemEnDb);
            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Variables de Items
        [HttpGet]
        public ActionResult CrearVariable(string reclamoId, string itemId)
        {
            var model = new VariableDeItemDto { ReclamoId = reclamoId, ItemDeReclamoId = itemId };
            ViewBag.Variables = _context.Variables.ToList();

            ViewBag.Condiciones = new List<Condiciones>
                       {
                           new Condiciones {Text = "==", Value = "=="},
                           new Condiciones {Text = "<", Value = "<"},
                           new Condiciones {Text = ">", Value = ">"},
                           new Condiciones {Text = "<=", Value = "<="},
                           new Condiciones {Text = ">=", Value = ">="},
                           new Condiciones {Text = "!=", Value = "!="},
                       };
            return PartialView("_VariableForm", model);
        }

        [HttpGet]
        public ActionResult EditarVariable(string reclamoId, string itemId, Guid variableId)
        {
            var variableEnDb = _context.VariablesDeItem.Include(x => x.Variable)
                .SingleOrDefault(x => x.ReclamoId == reclamoId && x.ItemDeReclamoId == itemId
                && x.VariableDeItemId == variableId);

            if (variableEnDb == null)
                return HttpNotFound();

            ViewBag.Variables = _context.Variables.ToList();
            ViewBag.Condiciones = new List<Condiciones>
                       {
                           new Condiciones {Text = "==", Value = "=="},
                           new Condiciones {Text = "<", Value = "<"},
                           new Condiciones {Text = ">", Value = ">"},
                           new Condiciones {Text = "<=", Value = "<="},
                           new Condiciones {Text = ">=", Value = ">="},
                           new Condiciones {Text = "!=", Value = "!="},
                       };
            var dto = Mapper.Map<VariableDeItem, VariableDeItemDto>(variableEnDb);
            dto.VariableNombre = variableEnDb.Variable.VariableNombre;
            return PartialView("_VariableForm", dto);
        }

        [HttpPost]
        public JsonResult ObtenerVariablesDeItem(string reclamoId, string itemId)
        {
            try
            {
                var variables = (from r in _context.VariablesDeItem
                                 .Include(x=>x.Variable)
                                 where r.ReclamoId == reclamoId && r.ItemDeReclamoId == itemId
                             select new VariableDeItemDto
                             {
                                 VariableDeItemId=r.VariableDeItemId,
                                 ReclamoId = r.ReclamoId,
                                 ItemDeReclamoId = r.ItemDeReclamoId,
                                 VariableCodigo = r.VariableCodigo,
                                 VariableNombre = r.Variable.VariableNombre,
                                 CondicionLogica = r.CondicionLogica,
                                 ValorAEvaluar = r.ValorAEvaluar,
                                 Accion = 1,
                                 Mensaje = "se cargaron correctament los datos!"
                             }).ToList();
                if (variables.Count == 0)
                {
                    var tmp = new VariableDeItemDto { Accion = 0, Mensaje = "No se encontraron registros!" };
                    variables.Add(tmp);
                }

                return Json(variables, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<VariableDeItemDto> list = new List<VariableDeItemDto>();
                VariableDeItemDto obj = new VariableDeItemDto() { Accion = 0, Mensaje = ex.Message.ToString() };
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarVariablesDeItem(VariableDeItemDto variable)
        {
            var varEnDb = _context.VariablesDeItem
               .SingleOrDefault(x => x.VariableDeItemId == variable.VariableDeItemId);

            try
            {
                if (!ModelState.IsValid)
                {
                    variable.Accion = 0;
                    variable.Mensaje = "los datos enviados no son correctos, verifiquelos e intente de nuevo";
                    return Json(variable, JsonRequestBehavior.AllowGet);
                }

                if (varEnDb == null)
                    _context.VariablesDeItem.Add(Mapper.Map<VariableDeItemDto, VariableDeItem>(variable));
                else
                {
                    varEnDb.VariableCodigo = variable.VariableCodigo;
                    varEnDb.CondicionLogica = variable.CondicionLogica;
                    varEnDb.ValorAEvaluar = variable.ValorAEvaluar;
                    varEnDb.VariableNombre = variable.VariableNombre;
                }

                variable.Accion = 1;
                variable.Mensaje = "los datos se guardaron exitosamente!";
                _context.SaveChanges();

                return Json(variable, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                variable.Accion = 0;
                variable.Mensaje = ex.Message.ToString();
                return Json(variable, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EliminarVariableDeItem(Guid id)
        {
            var varEnDb = _context.VariablesDeItem
                .SingleOrDefault(x => x.VariableDeItemId == id);

            if (varEnDb == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            _context.VariablesDeItem.Remove(varEnDb);
            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}