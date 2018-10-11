using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appcitas.Repository;
using appcitas.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace appcitas.Controllers
{
    public class PrestamosController : Controller
    {
        // GET: Prestamos

        Prestamos pPrestamo = new Prestamos();

        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Prestamo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult exportReport(int PrestamoId)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "rpt_Contrato.rpt"));
            rd.SetDatabaseLogon("sa", "Dinant031989");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                rd.SetParameterValue("@CodPrestamo", PrestamoId);
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);


                rd.PrintToPrinter(1, false,0,0);



                //return File(stream, "application/pdf", "Employee_list.pdf");
                return View();
            }
            catch
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult SolicitudCredito(int PrestamoId)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Solicitud_Crédito.rpt"));
            rd.SetDatabaseLogon("sa", "Dinant031989");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                rd.SetParameterValue("@CodPrestamo", PrestamoId);
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.PrintToPrinter(1, true, 0, 0);

                //return File(stream, "application/pdf", "Employee_list.pdf");
                return View();
            }
            catch
            {
                throw;
            }
        }


        //[HttpPost]
        //[ValidateInput(false)]
        public ActionResult ReciboOtros(int recibo)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Pago_Servicio.rpt"));
            rd.SetDatabaseLogon("sa", "Dinant031989");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                rd.SetParameterValue("@NunRecibo", recibo);
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.PrintToPrinter(1, true, 0, 0);

                //return File(stream, "application/pdf", "Employee_list.pdf");
                return View();
            }
            catch
            {
                throw;
            }
        }


        public ActionResult ReciboPagoPrestamo(int recibo)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Formatos"), "Rpt_Pago_Cuota.rpt"));
            rd.SetDatabaseLogon("sa", "Dinant031989");
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                rd.SetParameterValue("@NumRecibo", recibo);
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.PrintToPrinter(1, true, 0, 0);

                //return File(stream, "application/pdf", "Employee_list.pdf");
                return View();
            }
            catch
            {
                throw;
            }
        }


        [HttpPost]
        public JsonResult GetAll(int ClienteId)
        {
            
            PrestamoRepository PrestamoRep = new PrestamoRepository();
            try
            {
                return Json(PrestamoRep.GetAllByTipo(ClienteId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Prestamos> list = new List<Prestamos>();
                Prestamos obj = new Prestamos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }




        [HttpPost]
        public JsonResult SaveData(Prestamos Prestam)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.Save(Prestam);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(Prestam, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(Prestam, JsonRequestBehavior.AllowGet);
            }

        }

        //CONTROLADOR VER PRESTAMOS EN BOTON DE ACCION
        [HttpPost]
        public JsonResult GetDatosPrestamos(int id)
        {
            try
            {
                PrestamoRepository PrestamoRep = new PrestamoRepository();
                if (ModelState.IsValid)
                {
                    PrestamoRep.GetDatosPrestamos(id);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                return Json(PrestamoRep.GetDatosPrestamos(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(id, JsonRequestBehavior.AllowGet);
            }

        }

    }

}
