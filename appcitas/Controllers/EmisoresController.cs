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
    public class EmisoresController : Controller
    {
        Emisores pEmisor = new Emisores();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(string obj)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(obj);

            foreach (var item in json)
            {

            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByMarca(string MarcaId)
        {
            EmisorRepository EmisorRep = new EmisorRepository();
            try
            {
                return Json(EmisorRep.GetEmisoresByMarca(MarcaId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Emisores> list = new List<Emisores>();
                Emisores obj = new Emisores();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckOne(string EmisorCuenta)
        {
            Emisores obj = new Emisores();
            EmisorRepository EmisorRep = new EmisorRepository();
            try
            {
                if (EmisorCuenta != "")
                {
                    obj = EmisorRep.CheckEmisor(EmisorCuenta);
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
        public JsonResult GetEmisorById(int id)
        {
            Emisores obj = new Emisores();
            EmisorRepository EmisorRep = new EmisorRepository();
            try
            {
                if (id > 0)
                {
                    obj = EmisorRep.GetEmisorById(id);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
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
        public JsonResult SaveData(Emisores emisor)
        {
            try
            {
                EmisorRepository EmisorRep = new EmisorRepository();
                if (ModelState.IsValid)
                {
                    EmisorRep.Save(emisor);
                }
                return Json(emisor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(emisor, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult delete(int EmisorId)
        {
            Emisores obj = new Emisores();
            EmisorRepository EmisorRep = new EmisorRepository();
            try
            {
                if (EmisorId > 0)
                {
                    obj = EmisorRep.Del(EmisorId);
                }
                else
                {
                    obj.Accion = 0;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }
            }
            catch (Exception ex)
            {
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}