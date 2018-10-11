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
    public class ConfiguracionController : Controller
    {
        /* Obtiene la vista principal */
        public ActionResult Index()
        {
            return View();
        }

        /**
         * Método encargado de Guardar/Editar un nuevo registro de configuración 
         */
        [HttpPost]
        public JsonResult SaveData(Config Config)
        {
            try
            {
                ConfigRepository SucRep = new ConfigRepository();
                if (ModelState.IsValid)
                {
                    SucRep.Save(Config);
                }
                else
                {
                    Config.Accion = 0;
                    Config.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                return Json(Config, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Config, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost] 
        public JsonResult CheckOne(string id, string descripcion) 
        { 
            Config obj = new Config(); 
            ConfigRepository ConfigRep = new ConfigRepository(); 
            try 
            { 
                if (descripcion != "" || id != "") 
                { 
                    obj = ConfigRep.CheckConfig(id, descripcion); 
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
        public JsonResult CheckOneItem(string idConfig, string id, string descripcion, string abreviatura) 
        { 
            ConfigItem obj = new ConfigItem(); 
            ConfigRepository ConfigRep = new ConfigRepository(); 
            try 
            { 
                if (descripcion != "" || id != "" || abreviatura!= "") 
                { 
                    obj = ConfigRep.CheckConfigItem(idConfig, id, descripcion, abreviatura); 
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

        /**
         * Método encargado de traer todos los registros activos de configuración 
         */
        [HttpPost]
        public JsonResult GetAll()
        {
            ConfigRepository SucRep = new ConfigRepository();
            try
            {
                return Json(SucRep.GetConfig(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Config> list = new List<Config>();
                Config obj = new Config();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /**
         * Método encargado de obtener un registro de configuración por código seleccionado. 
         * (string) id 
         */
        [HttpPost]
        public JsonResult GetOne(string id)
        {
            Config obj = new Config();
            ConfigRepository SucRep = new ConfigRepository();
            try
            {
                if (id != "")
                {
                    obj = SucRep.GetItem(id);
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

        /**
         * Método encargado de eliminar un registro de configuración por código seleccionado. 
         * (obj) Config 
         */
        [HttpPost]
        public JsonResult delete(Config Config)
        {
            Config obj = new Config();
            ConfigRepository ConfigRep = new ConfigRepository();
            try
            {
                if (Config.ConfigID != "")
                {
                    obj = ConfigRep.Del(Config.ConfigID);
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

        /**
         * Método encargado de guardar/editar un item de configuración.
         */
        [HttpPost]
        public JsonResult SaveDataItem(ConfigItem Config)
        {
            try
            {
                ConfigRepository SucRep = new ConfigRepository();
                if (ModelState.IsValid)
                {
                    SucRep.SaveItem(Config);
                }
                else
                {
                    Config.Accion = 0;
                    Config.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                return Json(Config, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(Config, JsonRequestBehavior.AllowGet);
            }
        }


        /**
         * Método encargado de obtener un listado de items de configuración por código seleccionado. 
         * (string) ConfigId 
         * (string) ConfigItemId [opcional] 
         */
        [HttpPost]
        public JsonResult getParametosByIdEncabezado(string ConfigId, string ConfigItemId)
        {
            ConfigRepository SucRep = new ConfigRepository();
            try
            {
                return Json(SucRep.GetConfigItem(ConfigId,ConfigItemId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<ConfigItem> list = new List<ConfigItem>();
                ConfigItem obj = new ConfigItem();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getParametosByIdEncabezado(string id)
        {
            ConfigRepository SucRep = new ConfigRepository();
            try
            {
                return Json(SucRep.GetConfigItem(id, null), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<ConfigItem> list = new List<ConfigItem>();
                ConfigItem obj = new ConfigItem();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /**
         * Método encargado de eliminar un item de configuración por código seleccionado. 
         * (string) ConfigId 
         * (string) ConfigItemId 
         */
        [HttpPost]
        public JsonResult deleteItem(ConfigItem Config)
        {
            ConfigItem obj = new ConfigItem();
            ConfigRepository ConfigRep = new ConfigRepository();
            try
            {
                if (Config.ConfigID != "")
                {
                    obj = ConfigRep.DelItem(Config.ConfigID, Config.ConfigItemID);
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