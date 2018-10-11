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

        public ActionResult Dashboard_Atencion_por_tiempo_espera()
        {
            return View();
        }
        public ActionResult Dashboard_razon_cancelacion()
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

        public ActionResult Dashboard_consolidado()
        {
            return View();
        }
        public ActionResult Dashboard_citas_diarias_razones_cancelacion()
        {
            return View();
        }

        public ActionResult Dashboard_estatus_cita()
        {
            return View();
        }

        public ActionResult Dashboard_atencion_asesor()
        {
            return View();
        }

        public ActionResult Dashboard_comportamiento_incidencia()
        {
            return View();
        }

        public ActionResult Dashboard_MantaDatos()
        {
            return View();
        }

        //REPORTE CLIENTES

        [HttpPost]
        public JsonResult ReportesClientes(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReportesClientes(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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

        //REPORTE CREDITOS

        [HttpPost]
        public JsonResult ReporteCreditos(string fecha1, string fecha2, int SucursalId)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteCreditos(fecha1, fecha2, SucursalId), JsonRequestBehavior.AllowGet);
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

        //REPORTE PRESTAMOS POR CLIENTE
        [HttpPost]
        public JsonResult Reporte_PresPorCliente(string IdCliente)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_PresPorCliente(IdCliente), JsonRequestBehavior.AllowGet);
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

        //REPORTE PRESTAMOS VENCIDOS GLOBAL
        [HttpPost]
        public JsonResult Reporte_PrestamosVencidos(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_PrestamosVencidos(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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

        // REPORTE COBRANZAS POR FECHA 
        [HttpPost]
        public JsonResult Reporte_CobranzasPorFecha(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_CobranzasPorFecha(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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

        //REPORTE PRESTAMOS Y SUS CUOTAS POR CLIENTE
        [HttpPost]
        public JsonResult Reporte_PresYsusCuotas(string IdCliente)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_PresYsusCuotas(IdCliente), JsonRequestBehavior.AllowGet);
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

        // REPORTE PRESTAMOS CON MORA 
        [HttpPost]
        public JsonResult Reporte_PrestamosConMora(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_PrestamosConMora(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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

        // REPORTE TOTALES DINERO COBRADO

        [HttpPost]
        public JsonResult Reporte_TotalesDineroCobrado(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_TotalesDineroCobrado(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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

        // REPORTE PRESTAMOS PAGADOS

        [HttpPost]
        public JsonResult Reporte_PrestamosPagados(string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.Reporte_PrestamosPagados(fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult ReporteCitasPorEstado(int sucursalid, int estadocita, string cmb_cubiculo, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteCitasPorEstado(sucursalid, estadocita, cmb_cubiculo, fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult ReporteDashboardWalkin(int sucursalid, int estadocita, string cmb_cubiculo, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteDashboardWalkin(sucursalid, estadocita, cmb_cubiculo, fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult DashboardConsolidado(int sucursalid, int tipoatencion, string cmb_cubiculo, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.DashboardConsolidado(sucursalid, tipoatencion, cmb_cubiculo, fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult DashboardRazonCancelacion(int SucursalId, int tipoCita, string ejecutivo, string tipoRazon, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.DashboardRazonCancelacion(SucursalId, tipoCita, ejecutivo, tipoRazon, fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult DashboardCitasProgramadas(int sucursalId, string cubiculoId, string fecha1, string fecha2)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.DashboardCitasProgramadas(sucursalId, cubiculoId, fecha1, fecha2), JsonRequestBehavior.AllowGet);
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
        public JsonResult ReporteAtencionesRealizadas(string fecha1, string fecha2, int sucursalid, int tipoatencion)
        {
            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.ReporteAtencionesRealizadas(sucursalid, fecha1, fecha2, tipoatencion), JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult Dashboard_comportamiento_incidencia(int SucursalId, int CitaTipo, string fechainicio, string fechafinal)
        {

            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.GetDashboardComportamientoIncidencias(SucursalId, CitaTipo, fechainicio, fechafinal), JsonRequestBehavior.AllowGet);
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
        public JsonResult Dashboard_MantaDatos(int SucursalId, int CitaTipo, string fechainicio, string fechafinal)
        {

            ReporteRepository DashboardList = new ReporteRepository();
            try
            {
                return Json(DashboardList.GetDashboardMantaDatos(SucursalId, CitaTipo, fechainicio, fechafinal), JsonRequestBehavior.AllowGet);
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