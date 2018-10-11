using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using appcitas.Context;
using appcitas.Models;
using appcitas.Repository;
using System.Configuration;
using System.Web.SessionState;

namespace appcitas.Controllers
{
    public class MantenimientoController : Controller
    {
        public ActionResult dias_no_habiles()
        {
            return View();
        }
    }
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {

            return View();
        }

        [HttpPost]
        public JsonResult CheckUserAccessAgency(string CodigoModulo, string PassUsuario)
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.CheckUserAccessModule((string)(Session["usuario"]), CodigoModulo, PassUsuario), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckUserAccessModule(string CodigoUsuario, string PassUsuario, string CodigoModulo)
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.CheckUserAccessModule(CodigoUsuario, PassUsuario, CodigoModulo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckUserAccessModule2(string CodigoUsuario, string PassUsuario, string CodigoModulo)
        {
//            WSValidarUsuario.AuthenticationService.AuthenticationService ws = new WSValidarUsuario.AuthenticationService.AuthenticationService();
//            ws.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
//#if DEBUG
//            Boolean ValidaUsuario = true;
//#else
//            Boolean ValidaUsuario = ws.ValidarUsuario(CodigoUsuario, PassUsuario, "BAC_NT");
//#endif
            if (true)
            {
                HomeRepository Home = new HomeRepository();
                try
                {
                    return Json(Home.CheckUserAccessModule(CodigoUsuario,PassUsuario, CodigoModulo), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    List<Home> list = new List<Home>();
                    Home obj = new Home();
                    obj.Accion = 0;
                    obj.Mensaje = ex.Message.ToString();
                    list.Add(obj);
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = "Usuario no es válido con el AD";
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /*********************
        [HttpPost]
        public JsonResult GetUsersInfoCitas(string CodigoUsuario)
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.GetUsersInfoCitas(CodigoUsuario), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        *********************/

        [HttpPost]
        public JsonResult GetUserInfo()
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.GetUserInfo((string)(Session["usuario"])), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUserRol()
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.GetUserRol((string)(Session["usuario"])), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUserPrivilegio(string CodigoRol)
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.GetUserPrivilegio(CodigoRol), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getAgencySession()
        {
            return Json(Session["SucursalId"], JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult getAgencySessionName()
        {
            return Json(Session["SucursalName"], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult registerAgencySession(string SucursalId)
        {
            try
            {
                HttpContext.Session["SucursalId"] = SucursalId;
            }
            catch (Exception)
            {
                HttpContext.Session["SucursalId"] = "-1";
            }
            return Json(Session["SucursalId"], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult registerAgencySessionName(string SucursalName)
        {
            try
            {
                HttpContext.Session["SucursalName"] = SucursalName;
            }
            catch (Exception)
            {
                HttpContext.Session["SucursalName"] = "Sin Asignar";
            }
            return Json(Session["SucursalName"], JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckUserAccess(string privilegio)
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.CheckUserAccess((string)(Session["usuario"]), "SGRC", privilegio), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /*****************
        [HttpPost]
        public JsonResult CheckUserInfo()
        {
            HomeRepository Home = new HomeRepository();
            try
            {
                return Json(Home.CheckUserInfo((string)(Session["usuario"])), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Home> list = new List<Home>();
                Home obj = new Home();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
         *******************/
    }
}
