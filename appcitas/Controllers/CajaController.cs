using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appcitas.Repository;
using appcitas.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace appcitas.Controllers
{
    public class CajaController : Controller
    {
        

        public ActionResult InicioCaja()
        {
            return View();
        }

        public ActionResult ClientesCaja()
        {
            return View();
        }

        public ActionResult PrestamoCaja()
        {
            return View();
        }

        public ActionResult Cajero()
        {
            return View();
        }

        //LISTA DE ESTADOS
        [HttpPost]
        public JsonResult ListaTiposTrans()
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.ListaTiposTrans(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrosTiposTransacciones> list = new List<OtrosTiposTransacciones>();
                OtrosTiposTransacciones obj = new OtrosTiposTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula Datos sobre pago a prestamo
        [HttpPost]
        public JsonResult GetDatosPagoPrestamo(int id)
        {
            CajaRepository SucRep = new CajaRepository();
            try
            {
                return Json(SucRep.PagoPrestamo(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoPrestamo> list = new List<GetDatosPagoPrestamo>();
                GetDatosPagoPrestamo obj = new GetDatosPagoPrestamo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Realiza Pago a prestamo
        [HttpPost]
        public JsonResult PagoPrestamo(int codigo, string Capital, string Intereses, string Mora, int TipoPago, string Observacion)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.PagoPrestamo(codigo, Capital, Intereses, Mora, TipoPago, Cajero, Observacion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<GetDatosPagoPrestamo> list = new List<GetDatosPagoPrestamo>();
                GetDatosPagoPrestamo obj = new GetDatosPagoPrestamo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult OtrosPagos(decimal Monto, string Observacion, int ListaTiposTrans, string Clave)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.OtrosPagos(Monto, Observacion, Cajero, ListaTiposTrans, Clave), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Calcula registros de cajero
        [HttpPost]
        public JsonResult GetDatosCajero(string fecha)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GetDatosCajero(Cajero, fecha), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Cajero> list = new List<Cajero>();
                Cajero obj = new Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Carga transacciones de cajero
        [HttpPost]
        public JsonResult GetTransaccionesCajero(string fecha)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GetTransaccionesCajero(Cajero, fecha), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Transaccion de cajero
        [HttpPost]
        public JsonResult GeneraTransCajero(decimal Monto, int Tipo)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GeneraTransCajero(Monto, Tipo, Cajero), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Registro de Cajero
        [HttpPost]
        public JsonResult AgregaRegCajero()
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.AgregaRegCajero(Cajero), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Carga transacciones de cajero
        [HttpPost]
        public JsonResult GetRecibos()
        {
            CajaRepository CitaRep = new CajaRepository();
            try
            {
                return Json(CitaRep.GetRecibos(), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Recibos> list = new List<Recibos>();
                Recibos obj = new Recibos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //Genera Registro de Cajero
        [HttpPost]
        public JsonResult AnularRecibo(int Documento)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.AnularRecibo(Documento), JsonRequestBehavior.AllowGet);
                //(string)(Session["usuario"])
            }
            catch (Exception ex)
            {
                //throw;
                List<Transac_Cajero> list = new List<Transac_Cajero>();
                Transac_Cajero obj = new Transac_Cajero();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult GuardarDatosArqueo(int CA_Secuencia, int CA_B_1, int CA_B_2, int CA_B_5, int CA_B_10, int CA_B_20, int CA_B_50, int CA_B_100, int CA_B_500, int CA_M_1, int CA_M_2, int CA_M_5, int CA_M_10, int CA_M_20, int CA_M_50)
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.GuardarDatosArqueo(CA_Secuencia, CA_B_1, CA_B_2, CA_B_5, CA_B_10, CA_B_20, CA_B_50, CA_B_100, CA_B_500, CA_M_1, CA_M_2, CA_M_5, CA_M_10, CA_M_20, CA_M_50), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult CierreCaja(int Secuencia, float Diferencia )
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.CierreCaja(Secuencia, Diferencia), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //Realiza Pago de otros Ingresos
        [HttpPost]
        public JsonResult ValidaCajero()
        {
            CajaRepository CitaRep = new CajaRepository();
            var Cajero = (string)(Session["usuario"]);
            try
            {
                return Json(CitaRep.ValidaCajero(Cajero), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<OtrasTransacciones> list = new List<OtrasTransacciones>();
                OtrasTransacciones obj = new OtrasTransacciones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
    }
}