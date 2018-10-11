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
using System.Text;
using System.Data.Entity;



namespace appcitas.Controllers
{
    

    public class CitasController : Controller
    {
        //private AppcitasContext _context;

        //public CitasController()
        //{
        //    _context = new AppcitasContext();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    _context.Dispose();
        //}

        Citas pCita = new Citas();
        //
        // GET: /Citas/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vista_cita_cliente()
        {
            return View();
        }

        public ActionResult programacion_cc()
        {
            return View();
        }

        //POCA PLAN DE PAGO
        [HttpPost]
        public JsonResult GetPlanPagos(int CodPrest)
        {
            CitaRepository CitaRep = new CitaRepository();
            try
            {
                return Json(CitaRep.GetPlanPagos(CodPrest), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        //FIN NUEVO JSON
        //FIN POCA PLAN DE PAGO

        [HttpPost]
        public JsonResult GetCitasProgramadasBySucursal(int sucursalid)
        {
            CitaRepository AtenCitas = new CitaRepository();
            try
            {
                return Json(AtenCitas.GetCitasProgramadasBySurcursal(sucursalid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult SaveData(Citas cita)
        {
            try
            {
                CitaRepository CitaRep = new CitaRepository();
                if (ModelState.IsValid)
                {
                    CitaRep.Save(cita);
                }
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ProgramarCita(Citas cita)
        {
            try
            {
                CitaRepository CitaRep = new CitaRepository();
                if (ModelState.IsValid)
                {
                    CitaRep.ProgramarCita(cita);
                }
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AsignarRazon(Citas cita)
        {
            try
            {
                CitaRepository CitaRep = new CitaRepository();
                if (ModelState.IsValid)
                {
                    CitaRep.AsignarRazon(cita);
                }
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetAllByCustomerId(string CitaIdentificacion)
        {
            CitaRepository CitaRep = new CitaRepository();
            try
            {
                return Json(CitaRep.GetCitasByIdentificacion(CitaIdentificacion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public JsonResult GetCitasProgramadasBySucursal(int sucursalid)
        //{
        //    CitaRepository AtenCitas = new CitaRepository();
        //    try
        //    {
        //        return Json(AtenCitas.GetCitasProgramadasBySurcursal(sucursalid), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        List<Citas> list = new List<Citas>();
        //        Citas obj = new Citas();
        //        obj.Accion = 0;
        //        obj.Mensaje = ex.Message.ToString();
        //        list.Add(obj);
        //        return Json(list, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult CitaFechaHora(string cubiculoIdGlobal, string fechaGlobal, int CitaIdGlobal)
        {
            //var funcionEnDb = _context.CitasProgramadas;//.Where(f => f.PosicionId == cubiculoIdGlobal && f.CitaFecha.Substring(1,19) == fechaGlobal);

            //if (funcionEnDb == null)
            //{

            //    return Json(false, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    //_context.Funciones.Remove(funcionEnDb);
            //    //_context.SaveChanges();

            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}

            CitaRepository Citas = new CitaRepository();
            try
            {
                return Json(Citas.CheckCitaFecha(cubiculoIdGlobal,fechaGlobal, CitaIdGlobal), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckEmisorCuenta(string EmisorCuenta)
        {
            CitaRepository Citas = new CitaRepository();
            try
            {
                return Json(Citas.CheckEmisorCuenta(EmisorCuenta), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetRazonesByCita(int CitaId)
        {
            CitaRepository AtenCitas = new CitaRepository();
            try
            {
                return Json(AtenCitas.GetRazonesByCita(CitaId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetRazonByTipo(int razonId, int tipoRazonId, string datoExtraId)
        {
            CitaRepository AtenCitas = new CitaRepository();
            try
            {
                return Json(AtenCitas.GetRazonByTipo(razonId, tipoRazonId, datoExtraId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCitasBySucursalyEstado(int sucursalid, int estadocita)
        {
            CitaRepository AtenCitas = new CitaRepository();
            try
            {
                return Json(AtenCitas.GetCitasBySurcursalyEstado(sucursalid, estadocita), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Citas> list = new List<Citas>();
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult deleteRazon(int citaId, int tipoRazonId, int razonId, string datoExtraId)
        {
            Citas obj = new Citas();
            CitaRepository CitaRep = new CitaRepository();
            try
            {
                if (citaId > 0)
                {
                    obj = CitaRep.deleteRazon(citaId, tipoRazonId, razonId, datoExtraId);
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
        public JsonResult Clientecancela(int id)
        {
            Citas obj = new Citas();
            CitaRepository ACRep = new CitaRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.Clientecancela(id);
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
        public JsonResult GetTiempoEspera_CentrosAtencion()
        {
            CitaRepository Citas = new CitaRepository();
            try
            {
                return Json(Citas.GetTiempoEspera_CentrosAtencion());
            }
            catch (Exception ex)
            {
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<System.Web.Mvc.JsonResult> EnviarEmail(string emailCliente, string nombreCliente, string numeroGestion, string sucursal, string tramite, string fecha, string hora, string horaFinal, string accionCita)
        {

            string mensajeIntro = "";
            string mensajeFooter = "";
            string mensajeTitulo = "";
            string mensajeSaludo = "";
            string mensajeFinal = "";
            string mensajeFinal1 = "";
            string mensajeFinal2 = "";
            string mensajeFinal3 = "";
            string mensajeFinal4 = "";
            string mensajeFinal5 = "";
            string mensajeFinal6 = "";
            string mensajeFinal7 = "";
            string mensajeAsunto = "";


            string[] lines;
            var list = new List<string>();
            var ruta = AppDomain.CurrentDomain.BaseDirectory + @"\Content\Publicidad\CorreoCitas.txt";//(@"Content\Publicidad\CorreoCitas.txt");

            var fileStream = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            var file = new System.IO.StreamReader(fileStream, System.Text.Encoding.UTF8, true, 128);
            using (var streamReader = new StreamReader(fileStream,Encoding.GetEncoding("iso-8859-1")))
            {
                string line;
                while((line = streamReader.ReadLine()) != null) 
                {
                    list.Add(line);
                }
            }
            lines = list.ToArray();

            mensajeTitulo = lines[0];
            mensajeSaludo = lines[1];
            mensajeIntro = lines[2];
            mensajeFooter = lines[3];
            mensajeFinal = lines[4];
            mensajeFinal1 = lines[5];
            mensajeFinal2 = lines[6];
            mensajeFinal3 = lines[7];
            mensajeFinal4 = lines[8];
            mensajeFinal5 = lines[9];
            mensajeFinal6 = lines[10];
            mensajeFinal7 = lines[11];
            mensajeAsunto = lines[12];

            string asunto = mensajeAsunto + numeroGestion;
            string textoAccion = "creado";
            string recordatorioTexto = "informa";
            //string mensajeIntro = "";
            //string mensajeFooter = "";
            
            /* Creación de evento (adjunto)*/
            string schLocation = sucursal;
            string schSubject = asunto;
            string schDescription = asunto;
           // System.DateTime schBeginDate = Convert.ToDateTime(fecha+" "+hora);
            //System.DateTime schEndDate = Convert.ToDateTime(fecha + " " + horaFinal);

            //String[] cuerpoCita = { "BEGIN:VCALENDAR",
            //                        "PRODID:-//BAC Credomatic//Panama//Pa",
            //                        "BEGIN:VEVENT",
            //                        "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
            //                        "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
            //                        "LOCATION:" + schLocation,
            //                        "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + schDescription,
            //                        "SUMMARY:" + schSubject, 
            //                        "PRIORITY:3",
            //                        "END:VEVENT", 
            //                        "END:VCALENDAR" };
            //string hoy = System.DateTime.Now.ToString("yyyyMMddhhmmss");
            //string directorioCita = HttpContext.Server.MapPath(@"\iCal\");
            //directorioCita = directorioCita + "Cita"+hoy+".ics";
            
            /*using (FileStream fs = new FileStream(directorioCita, FileMode.OpenOrCreate))
            using (StreamWriter str = new StreamWriter(fs))
            {
                str.Flush();
                str.Close();
                fs.Close();
            }*/
            /*  .WriteAllLines */
            /*var tempFile = System.IO.Path.GetTempFileName();
            
            using (var file = System.IO.File.Create(directorioCita))
            {
                System.IO.File.WriteAllLines(directorioCita, cuerpoCita);
                file.Close();
            }*/
            //directorioCita = "";
            //mensajeIntro = "Nos complace comunicarle que su CITA para el inicio del trámite de cancelación tarjeta de crédito, ha sido confirmada para ser atendida:";
            //mensajeFooter = "Agradecemos su puntualidad, por favor anunciarse a su llegada en la recepción de la sucursal asignada.<br> De no poder asistir a la misma, presentarse en sucursal Centro de Fidelizacion Plaza New York en Panamá para programar una nueva cita y gestionar la cancelación de la tarjeta de crédito.<br><br>Agradecemos su preferencia.";
            switch (accionCita){
                case "1":
                    textoAccion = "creado";
                    asunto = asunto+" creada";
                    break;
                case "2":
                    textoAccion = "modificado";
                    asunto = asunto+" modificada";
                    break;
                case "3":
                    textoAccion = "cancelado";
                    asunto = asunto+" cancelada";
                    mensajeIntro = "BAC – Credomatic le " + recordatorioTexto + " que se ha <b>" + textoAccion + "</b> una cita de atención de acuerdo a su solicitud.";
                    mensajeFooter = "";
                    //directorioCita = "";
                    break;
                case "4":
                    recordatorioTexto = "recuerda";
                    break;
            }
            string tituloCorreo = mensajeTitulo;
            string cuerpoCorreo = "";
            cuerpoCorreo = "<p>Estimado(a): " + nombreCliente + "<br><br>" +
                                "<span>"+mensajeIntro+"</span><br><br>" +
                            "</p>" +
                            "<p><b>Cita No.:</b> " + numeroGestion + "</p>" +
                            "<p><b>Centro de Atención:</b> " + sucursal + "</p>" +
                            "<p><b>Trámite:</b> " + tramite + "</p>" +
                            "<p><b>Fecha:</b> " + fecha + "</p>" +
                            "<p><b>Hora:</b> " + hora + "</p><br>" +
                            "<p><span>"+mensajeFooter+"</span></p>"+
                            "<p>" + mensajeFinal6 + "</p>";
            Citas obj = new Citas();
            try
            {
                await EmailService.EnviarEmail(emailCliente, nombreCliente, tituloCorreo, cuerpoCorreo, asunto, mensajeFinal,
                mensajeFinal1,mensajeFinal2,mensajeFinal3,mensajeFinal4,mensajeFinal5,mensajeFinal7);
                obj.Accion = 1;
                obj.Mensaje = "Correo electrónico enviado exitósamente";
                /*GC.Collect();
                GC.WaitForPendingFinalizers();*/
            }
            catch (Exception ex)
            {
                obj.Accion = 0;
                obj.Mensaje = "No se pudo enviar correo electrónico! "+ex;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
