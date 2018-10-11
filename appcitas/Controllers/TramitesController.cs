using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using appcitas.Context;
using appcitas.Models;
using appcitas.Repository;

//using System.Threading.Tasks.Task;

namespace appcitas.Controllers
{
    public class TramitesController : Controller
    {
        //private appcitasContext db = new appcitasContext();
        //static List<Tramites> tramites = new List<Tramites>();
        Tramites pTramite = new Tramites();
        //
        // GET: /Tramites/
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
        public JsonResult SaveData(Tramites tramite)
        {
            try
            {
                TramiteRepository TramiteRep = new TramiteRepository();
                if (ModelState.IsValid)
                {
                    TramiteRep.Save(tramite);
                    //db.Tramite.Add(tramite);
                    //db.SaveChanges();
                }
                return Json(tramite, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(tramite, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAll()
        {
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                return Json(TramiteRep.GetTramites(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Tramites> list = new List<Tramites>();
                Tramites obj = new Tramites();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllTramites()
        {
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                return Json(TramiteRep.GetTramites(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Tramites> list = new List<Tramites>();
                Tramites obj = new Tramites();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllTramitesComboBox()
        {
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                return Json(TramiteRep.GetTramitesComboBox(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Tramites> list = new List<Tramites>();
                Tramites obj = new Tramites();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetOne(int id)
        {
            Tramites obj = new Tramites();
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                if (id > 0)
                {
                    obj = TramiteRep.GetTramite(id);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parámetro tiene un valor incorrecto!";
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
            Tramites obj = new Tramites();
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                if (descripcion != "" || abreviatura != "")
                {
                    obj = TramiteRep.CheckTramite(descripcion,abreviatura);
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
            Tramites obj = new Tramites();
            TramiteRepository TramiteRep = new TramiteRepository();
            try
            {
                if (id > 0)
                {
                    obj = TramiteRep.Del(id);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parámetro tiene un valor incorrecto!";
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