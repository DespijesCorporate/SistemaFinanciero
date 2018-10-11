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
    public class RazonesController : Controller
    {
        //private appcitasContext db = new appcitasContext();
        //static List<Razones> sucursales = new List<Razones>();

        Razones pRazon = new Razones();
        //
        // GET: /Razones/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Razones()
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
        public JsonResult SaveData(Razones razon)
        {
            try
            {
                RazonRepository RazonRep = new RazonRepository();
                if (ModelState.IsValid)
                {
                    RazonRep.Save(razon);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(razon, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(razon, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAll(int TipoId)
        {
            /*RazonRepository RazonRep = new RazonRepository();
            try
            {
                return Json(RazonRep.GetRazones(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Razones> list = new List<Razones>();
                Razones obj = new Razones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }*/

            RazonRepository RazonRep = new RazonRepository();
            try
            {
                return Json(RazonRep.GetAllByTipo(TipoId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Razones> list = new List<Razones>();
                Razones obj = new Razones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetOne(int id, int tipo_id)
        {
            Razones obj = new Razones();
            RazonRepository RazonRep = new RazonRepository();
            try
            {
                if (id > 0)
                {
                    obj = RazonRep.GetRazon(id,tipo_id);
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
        public JsonResult CheckOne(string descripcion, string abreviatura)
        {
            Razones obj = new Razones();
            RazonRepository RaRep = new RazonRepository();
            try
            {
                if (descripcion != "" || abreviatura != "")
                {
                    obj = RaRep.CheckRazon(descripcion, abreviatura);
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
            Razones obj = new Razones();
            RazonRepository RazonRep = new RazonRepository();
            try
            {
                if (id > 0)
                {
                    obj = RazonRep.Del(id);
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