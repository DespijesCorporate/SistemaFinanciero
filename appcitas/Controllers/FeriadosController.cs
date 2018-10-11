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
    public class FeriadosController : Controller
    {
        //private appcitasContext db = new appcitasContext();
        //static List<Feriados> feriados = new List<Feriados>();
        Feriados pFeriado = new Feriados();
        //
        // GET: /Feriados/
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
        public JsonResult SaveData(Feriados feriado)
        {
            try
            {
                FeriadoRepository FeriadoRep = new FeriadoRepository();
                if (ModelState.IsValid)
                {
                    FeriadoRep.Save(feriado);
                    //db.Feriado.Add(feriado);
                    //db.SaveChanges();
                }
                return Json(feriado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(feriado, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAll()
        {
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                return Json(FeriadoRep.GetFeriados(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Feriados> list = new List<Feriados>();
                Feriados obj = new Feriados();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetOne(int id)
        {
            Feriados obj = new Feriados();
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                if (id > 0)
                {
                    obj = FeriadoRep.GetFeriado(id);
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
        public JsonResult GetAllByYear(int year)
        {
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                return Json(FeriadoRep.GetFeriadosByYear(year), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Feriados> list = new List<Feriados>();
                Feriados obj = new Feriados();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getAniosFeriado()
        {
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                return Json(FeriadoRep.GetAniosFeriado(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Feriados> list = new List<Feriados>();
                Feriados obj = new Feriados();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckOne(string fecha)
        {
            Feriados obj = new Feriados();
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                if (fecha != "")
                {
                    obj = FeriadoRep.CheckFeriado(fecha);
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
            Feriados obj = new Feriados();
            FeriadoRepository FeriadoRep = new FeriadoRepository();
            try
            {
                if (id > 0)
                {
                    obj = FeriadoRep.Del(id);
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