using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using appcitas.Context;
using appcitas.Models;
using appcitas.Repository;
using appcitas.Services;
using System.Threading.Tasks;
using System.IO;

namespace appcitas.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard_Atenciones_realizadas()
        {
            return View();
        }

        public ActionResult Dashboard_Atencion_por_tiempo_espera()
        {
            return View();
        }

        public ActionResult Dashboard_flujo_por_intervalo()
        {
            return View();
        }

        public ActionResult Dashboard_de_efectividad()
        {
            return View();
        }

        public ActionResult Dashboard_citas_programadas()
        {
            return View();
        }

        public ActionResult Dashboard_atencion_asesor_citas_resultado()
        {
            return View();
        }
        public ActionResult Dashboard_atenciones_realizadas()
        {
            return View();
        }
        public ActionResult Dashboard_citas_diarias_razones_cancelacion()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReporteCitasPorEstado(int sucursalid, int estadocita)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteCitasPorEstado(sucursalid, estadocita), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReporteAtencionPorTiempoEspera(int SucursalId, int tipoCita, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteAtencionPorTiempoEspera(SucursalId, tipoCita, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReporteFlujoPorIntervalo(int SucursalId, int tipoCita, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteFlujoPorIntervalo(SucursalId, tipoCita, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReporteEfectividad(int SucursalId, int tipoCita, string ejecutivo, string tipoRazon, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteEfectividad(SucursalId, tipoCita, ejecutivo, tipoRazon, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DashboardAtencionCubiculo(int sucursalId, string cubiculoId, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.GetDashboardAtencionCubiculo(sucursalId, cubiculoId, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DashboardAtencionPorCita(int sucursalId, string cubiculoId, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.GetDashboardAtencionPorCita(sucursalId, cubiculoId, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DashboardResolucionPorCita(int sucursalId, string cubiculoId, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.GetDashboardResolucionPorCita(sucursalId, cubiculoId, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReporteAtencionesRealizadas(string fecha1, string fecha2, int sucursalid)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteAtencionesRealizadas(sucursalid, fecha1, fecha2), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReporteCitasDiarias(string fecha1, int sucursalid, int codtiporazon, int codrazon)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteCitasDiarias(sucursalid, fecha1, codtiporazon, codrazon), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Reportes> list = new List<Reportes>();
                Reportes obj = new Reportes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

    }
}