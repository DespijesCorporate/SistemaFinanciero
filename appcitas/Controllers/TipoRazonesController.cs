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
    public class TipoRazonesController : Controller
    {
        // GET: TipoRazones
        TipoRazones pTipoRazon = new TipoRazones();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tipo_razones()
        {
            return View();
        }

        public ActionResult Tipo_razones_autoservicio()
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
        public JsonResult SaveData(TipoRazones tiporazon)
        {
            try
            {
                TipoRazonRepository TipoRaRep = new TipoRazonRepository();
                if (ModelState.IsValid)
                {
                    TipoRaRep.Save(tiporazon);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(tiporazon, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(tiporazon, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult SaveData1(TipoRazones tiporazon)
        {
            try
            {
                TipoRazonRepository TipoRaRep = new TipoRazonRepository();
                if (ModelState.IsValid)
                {
                    TipoRaRep.Save1(tiporazon);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(tiporazon, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(tiporazon, JsonRequestBehavior.AllowGet);
            }

        }

        
        [HttpGet]
        public JsonResult GetAllTipoRazones()
        {
            TipoRazonRepository TipoRazonRep = new TipoRazonRepository();
            try
            {
                return Json(TipoRazonRep.GetTipoRazones(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<TipoRazones> list = new List<TipoRazones>();
                TipoRazones obj = new TipoRazones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            TipoRazonRepository TipoRazonRep = new TipoRazonRepository();
            try
            {
                return Json(TipoRazonRep.GetTipoRazones(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<TipoRazones> list = new List<TipoRazones>();
                TipoRazones obj = new TipoRazones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetAll1()
        {
            TipoRazonRepository TipoRazonRep1 = new TipoRazonRepository();
            try
            {
                return Json(TipoRazonRep1.GetTipoRazonesAutoservicio(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<TipoRazones> list = new List<TipoRazones>();
                TipoRazones obj = new TipoRazones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetOne(int id)
        {
            TipoRazones obj = new TipoRazones();
            TipoRazonRepository TipoRazonRep = new TipoRazonRepository();
            try
            {
                if (id > 0)
                {
                    obj = TipoRazonRep.GetTipoRazon(id);
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
        public JsonResult CheckOne(string abreviatura, string descripcion)
        {
            TipoRazones obj = new TipoRazones();
            TipoRazonRepository TipRaRep = new TipoRazonRepository();
            try
            {
                if (descripcion != "" || abreviatura != "")
                {
                    obj = TipRaRep.CheckTipoRazon(descripcion, abreviatura);
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
            TipoRazones obj = new TipoRazones();
            TipoRazonRepository TipoRazonRep = new TipoRazonRepository();
            try
            {
                if (id > 0)
                {
                    obj = TipoRazonRep.Del(id);
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
