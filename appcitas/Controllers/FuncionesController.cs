using appcitas.Context;
using appcitas.Dtos;
using appcitas.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace appcitas.Controllers
{
    public class FuncionesController : Controller
    {
        private AppcitasContext _context;

        public FuncionesController()
        {
            _context = new AppcitasContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Funciones
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NuevaFuncion()
        {
            ViewBag.Title = "Creando Nueva Funcion";
            ViewBag.TiposDeRetorno = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "DDR")
                .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
            var funcion = new FuncionDto()
            {
                Parametros = new List<ParametroDto>()
            };

            return PartialView("FuncionDataForm", funcion);
        }

        [HttpGet]
        public ActionResult EditarFuncion(string id)
        {
            ViewBag.Title = "Editando Funcion";
            ViewBag.TiposDeRetorno = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "DDR")
                .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);

            var funcionEnDb = _context.Funciones.Include(f => f.Parametros)
                .SingleOrDefault(f => f.FuncionCodigo == id);
            if (funcionEnDb == null)
            {
                return HttpNotFound();
            }

            return PartialView("FuncionDataForm", Mapper.Map<Funcion, FuncionDto>(funcionEnDb));
        }

        [HttpPost]
        public JsonResult EliminarFuncion(string id)
        {
            var funcionEnDb = _context.Funciones.SingleOrDefault(f => f.FuncionCodigo == id);

            if (funcionEnDb == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            _context.Funciones.Remove(funcionEnDb);
            _context.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarFuncion(FuncionDto fn)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Title = "Editando Funcion";
                    ViewBag.TiposDeRetorno = _context.ItemsDeConfiguracion.Where(x => x.ConfigID == "DDR")
                        .ToList().Select(Mapper.Map<ConfigItem, ConfigItemDto>);
                    foreach (var item in fn.Parametros)
                    {
                        item.Tipo = Mapper.Map<ConfigItem, ConfigItemDto>(_context
                            .ItemsDeConfiguracion
                            .SingleOrDefault(x => x.ConfigItemID == item.TipoId && x.ConfigID == item.ConfigID));
                    }

                    fn.Accion = 0;
                    fn.Mensaje = "los datos enviados no son correctos, verifiquelos e intente de nuevo";
                    return Json(fn, JsonRequestBehavior.AllowGet);
                }

                var funcionEnDb = _context.Funciones.SingleOrDefault(f => f.FuncionCodigo == fn.FuncionCodigo);
                fn.ConfigId = "DDR";
                if (funcionEnDb == null)
                {
                    _context.Funciones.Add(Mapper.Map<FuncionDto, Funcion>(fn));
                }
                else
                {
                    var ParametrosEnDb = _context.ParametrosDeFunciones.Where(x => x.FuncionCodigo == fn.FuncionCodigo);
                    _context.ParametrosDeFunciones.RemoveRange(ParametrosEnDb);

                    if(fn.Parametros!= null && fn.Parametros.Count>0)
                        _context.ParametrosDeFunciones.AddRange(fn.Parametros.Select(Mapper.Map<ParametroDto, Parametro>));

                    funcionEnDb.FuncionNombre = fn.FuncionNombre;
                    funcionEnDb.FuncionDescripcion = fn.FuncionDescripcion;
                    funcionEnDb.FuncionNumeroParametros = fn.FuncionNumeroParametros;
                    funcionEnDb.FuncionTipoDeRetorno = fn.FuncionTipoDeRetorno;
                }

                if (_context.ChangeTracker.HasChanges())
                    _context.SaveChanges();

                fn.Accion = 1;
                fn.Mensaje = "datos guardados exitosamente!";
                return Json(fn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                fn.Accion = 0;
                fn.Mensaje = ex.Message.ToString();
                return Json(fn, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerDataDeFuncion(string id)
        {
            FuncionDto objDto = null;
            ViewBag.Title = "Datos de Funcion " + id;
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return HttpNotFound();
                }
                else
                {                    
                    objDto = Mapper.Map<Funcion, FuncionDto>(_context.Funciones.Include(x=>x.Parametros.Select(p=>p.Tipo)).SingleOrDefault(v => v.FuncionCodigo == id));
                    var tipo = _context.ItemsDeConfiguracion.SingleOrDefault(x => x.ConfigItemID == objDto.FuncionTipoDeRetorno);

                    objDto.TipoDeRetorno = Mapper.Map<ConfigItem, ConfigItemDto>(tipo);

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
                        
            return PartialView("ViewFuncionData",objDto);
        }

        [HttpPost]
        public JsonResult ObtenerFunciones()
        {
            try
            {
                var funciones = (from fn in _context.Funciones
                                 .Include(f=>f.Parametros)
                                 .Include(f=>f.TipoDeRetorno)
                                 select new FuncionDto
                                 {
                                     FuncionCodigo = fn.FuncionCodigo,
                                     FuncionNombre = fn.FuncionNombre,
                                     FuncionDescripcion = fn.FuncionDescripcion,
                                     FuncionNumeroParametros = fn.FuncionNumeroParametros,
                                     FuncionTipoDeRetorno = fn.FuncionTipoDeRetorno,                                     
                                     Accion = 1,
                                     Mensaje = "se caragaron correctamente los registros"
                                 }).ToList();
                if(funciones.Count ==0)
                {
                    var tmp = new FuncionDto { Accion = 0, Mensaje = "No se encontraron registros!" };
                    funciones.Add(tmp);
                }

                return Json(funciones, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<FuncionDto> list = new List<FuncionDto>();
                FuncionDto obj = new FuncionDto
                {
                    Accion = 0,
                    Mensaje = ex.Message.ToString()
                };
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
    }
}