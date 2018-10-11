using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using appcitas.Context;
using appcitas.Models;
using appcitas.Repository;

namespace appcitas.Controllers
{
    public class PrioridadesController : Controller
    {
        //private appcitasContext db = new appcitasContext();
        //static List<Prioridades> Prioridades = new List<Prioridades>();
        Prioridades pPrioridad = new Prioridades();
        //
        // GET: /Prioridades/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(string obj)
        {
            //JavaScriptSerializer j = new JavaScriptSerializer();
            //object a = j.Deserialize(obj, typeof(object));

            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj);

            foreach (var item in json)
            {

            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(Prioridades prioridad)
        {
            try
            {
                PrioridadRepository PrioRep = new PrioridadRepository();
                if (ModelState.IsValid)
                {
                    PrioRep.Save(prioridad);
                    //db.Prioridad.Add(prioridad);
                    //db.SaveChanges();
                }
                return Json(prioridad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(prioridad, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAll()
        {
            PrioridadRepository PrioRep = new PrioridadRepository();
            try
            {
                return Json(PrioRep.GetPrioridades(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Prioridades> list = new List<Prioridades>();
                Prioridades obj = new Prioridades();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetOne(int id)
        {
            Prioridades obj = new Prioridades();
            PrioridadRepository PrioRep = new PrioridadRepository();
            try
            {
                if (id > 0)
                {
                    obj = PrioRep.GetPrioridad(id);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;

                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult CheckOne(string nombre, string codigo)
        {
            Prioridades obj = new Prioridades();
            PrioridadRepository PrioridadRep = new PrioridadRepository();
            try
            {
                if (nombre != "" || codigo != "")
                {
                    obj = PrioridadRep.CheckPrioridad(nombre, nombre);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parámetro tiene un valor incorrecto!";
                }
            }
            catch (Exception ex)
            {
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult delete(int id)
        {
            Prioridades obj = new Prioridades();
            PrioridadRepository PrioRep = new PrioridadRepository();
            try
            {
                if (id > 0)
                {
                    obj = PrioRep.Del(id);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;

                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }
    }
}