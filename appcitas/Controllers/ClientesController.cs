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

namespace appcitas.Controllers
{
    public class ClientesController : Controller
    {
        // GET: AtencionCitas

        Clientes pClientes = new Clientes();
        public ActionResult Index()
        {
            var s = Session["SucursalId"];

            return View();
        }

        [HttpPost]
        public JsonResult CheckOne(string identidad, string rtn)
        {
            Clientes obj = new Clientes();
            ClientesRepository TipRaRep = new ClientesRepository();
            try
            {
                if (rtn != "" || identidad != "")
                {
                    obj = TipRaRep.CheckClientes(rtn, identidad);
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

        public ActionResult Clientes()
        {
            return View();
        }
        public ActionResult Aprobacion_Creditos()
        {
            return View();
        }
        public ActionResult Reporte_Clientes()
        {
            return View();
        }
        public ActionResult Reporte_Creditos()
        {
            return View();
        }
        public ActionResult Reporte_PresPorCliente()
        {
            return View();
        }

        public ActionResult Reporte_PresYsusCuotas()
        {
            return View();
        }
        public ActionResult Reporte_CobranzasHoy()
        {
            return View();
        }
        public ActionResult Reporte_TotalesDineroCobrado()
        {
            return View();
        }
        public ActionResult Reporte_PrestamosVencidos()
        {
            return View();
        }
        public ActionResult Reporte_PrestamosConMora()
        {
            return View();
        }
        public ActionResult Reporte_PrestamosPagados()
        {
            return View();
        }
        public ActionResult Transacciones_Creditos()
        {
            return View();
        }
        public ActionResult Plan_Pago()
        {
            return View();
        }

        public ActionResult Bienvenida()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            ClientesRepository ClientesAll = new ClientesRepository();
            try
            {
                return Json(ClientesAll.GetClientes(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Clientes> list = new List<Clientes>();
                Clientes obj = new Clientes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSolicitudesPrestamos(string CitaIdentificacion)
        {
            ClientesRepository CitaRep = new ClientesRepository();
            try
            {
                return Json(CitaRep.GetSolicitudesByIdentificacion(CitaIdentificacion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Solicitudes> list = new List<Solicitudes>();
                Solicitudes obj = new Solicitudes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        
        
        


        [HttpPost]
        public JsonResult AprobarRechazarSolicitud(int Accion,int Id, int Monto )
        {
            ClientesRepository CitaRep = new ClientesRepository();
            try
            {
                return Json(CitaRep.AprobarRechazarSolicitudByCodigo(Accion, Id, Monto), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Solicitudes> list = new List<Solicitudes>();
                Solicitudes obj = new Solicitudes();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTransaccionesPrestamos(int codigo)
        {
            ClientesRepository CitaRep = new ClientesRepository();
            try
            {
                return Json(CitaRep.GetTransaccionesPrestamos(codigo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<TransaccionesPrestamos> list = new List<TransaccionesPrestamos>();
                TransaccionesPrestamos obj = new TransaccionesPrestamos();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetRazonesByCita(int CitaId)
        {
            //CitaRepository AtenCitas = new CitaRepository();
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
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
        public JsonResult GetCitaTarjetas(int CitaId)
        {
            //CitaRepository AtenCitas = new CitaRepository();
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitaTarjetas(CitaId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Atencion> list = new List<Atencion>();
                Atencion obj = new Atencion();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AgregarHerramienta(CitaHerramienta Herramientas)
        {
            try
            {
                AtencionCitasRepository CitaRep = new AtencionCitasRepository();
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(Herramientas.HerramientaObservacion))
                    {
                        Herramientas.HerramientaObservacion = "";
                    }
                    CitaRep.SaveHerramienta(Herramientas);
                }
                else
                {
                    Herramientas.Accion = 0;
                    Herramientas.Mensaje = "Los datos no tienen el formato correcto!";
                }
                return Json(Herramientas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Herramientas.Accion = 0;
                Herramientas.Mensaje = ex.Message.ToString();
                return Json(Herramientas, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCubiculosBySucursal(int id)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosBySucursalAndCita(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetCubiculosBySucursalAtencion(int id)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosBySucursalAtencion(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult ContadorUsuariosFecha()
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.ConteoUsuarioFecha(), JsonRequestBehavior.AllowGet);
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
        public JsonResult CampoCitaRazon(int CitaId)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.Campocitarazon(CitaId), JsonRequestBehavior.AllowGet);
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


        [HttpGet]
        public JsonResult GetCubiculosBySucursal2(int id)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosBySucursal(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetCubiculosByCitas(int id)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosBySucursalAndCita(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetHerramientasByCita(int CitaId)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetHerramientasByCita(CitaId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<CitaHerramienta> list = new List<CitaHerramienta>();
                CitaHerramienta obj = new CitaHerramienta();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCitasBySucursal(int sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitasBySurcursal(sucursalid), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCitasProgramadasLlamadas(int sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitasProgramadasLlamadas(sucursalid), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCitasWalkinLlamadas(int sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitasWalkinLlamadas(sucursalid), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetSucursalName(int sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetSucursalName(sucursalid), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCitasProgramadasCola(string Posicionid, string Sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                int Suc = Convert.ToInt32(Sucursalid);
                return Json(AtenCitas.GetCitasProgramadasCola(Posicionid, Suc), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCitasPendientes(string Posicionid, string Sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                int Suc = Convert.ToInt32(Sucursalid);
                return Json(AtenCitas.GetCitasPendientes(Posicionid, Suc), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Atencion obj = new Atencion();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCitasDia(string Posicionid, string Sucursalid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                int Suc = Convert.ToInt32(Sucursalid);
                return Json(AtenCitas.GetCitasDia(Suc, Posicionid), JsonRequestBehavior.AllowGet);
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

        public JsonResult FinalizarCita(int CitaId, string Observacion, string Resolucion)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.FinalizarCita(CitaId, Observacion, Resolucion), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AsignarTarjetaRazon(Citas cita)
        {
            try
            {
                AtencionCitasRepository CitaRep = new AtencionCitasRepository();
                if (ModelState.IsValid)
                {
                    CitaRep.AsignarTarjetasRazon(cita);
                }
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(cita, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult deleteTarjetaRazon(int citaId, int tipoRazonId, int razonId, string datoExtraId, string Cuenta, string Tarjeta)
        {
            Citas obj = new Citas();
            AtencionCitasRepository CitaRep = new AtencionCitasRepository();
            try
            {
                if (citaId > 0)
                {
                    obj = CitaRep.deleteTarjetaRazon(citaId, tipoRazonId, razonId, datoExtraId, Cuenta, Tarjeta);
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
        public JsonResult deleteTarjeta(int citaId, string Cuenta, string Tarjeta)
        {
            Citas obj = new Citas();
            AtencionCitasRepository CitaRep = new AtencionCitasRepository();
            try
            {
                if (citaId > 0)
                {
                    obj = CitaRep.deleteTarjeta(citaId, Cuenta, Tarjeta);
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
        public JsonResult GetTarjetasRazonesByCita(int CitaId, string Cuenta, string Tarjeta)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetTarjetasRazonesByCita(CitaId, Cuenta, Tarjeta), JsonRequestBehavior.AllowGet);
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

        public JsonResult SaveTarjetas(int CitaId, string Cuenta, string Tarjeta, string Resolucion, string Comentario, int Emisor, string CuentaOriginal, string TarjetaOriginal)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.SaveTarjetas(CitaId, Cuenta, Tarjeta, Resolucion, Comentario, Emisor, CuentaOriginal, TarjetaOriginal), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IniciarCitaFecha(int CitaId, string Cubiculo)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.IniciaCitaFecha(CitaId, Cubiculo), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCitasById(int CitaId)
        {
            AtencionCitasRepository AtencionCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtencionCitas.GetCitasByID(CitaId));
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
        public JsonResult GetCitasByIdAndPosicion(int CitaId, string PosicionId)
        {
            AtencionCitasRepository AtencionCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtencionCitas.GetCitasByIdAndPosicion(CitaId, PosicionId));
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
        public JsonResult Clientellegocita(int id)
        {
            Citas obj = new Citas();
            AtencionCitasRepository ACRep = new AtencionCitasRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.Clientellegocita(id);
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
        public JsonResult Clienteentro(int id)
        {
            Citas obj = new Citas();
            AtencionCitasRepository ACRep = new AtencionCitasRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.Clienteentro(id);
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
        public JsonResult Clientesalio(int id)
        {
            Citas obj = new Citas();
            AtencionCitasRepository ACRep = new AtencionCitasRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.Clientesalio(id);
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
        public JsonResult SaveData(Clientes clientes)
        {
            try
            {
                ClientesRepository ClienteRep = new ClientesRepository();
                if (ModelState.IsValid)
                {
                    ClienteRep.Save(clientes);
                }
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        //public JsonResult SaveData(Atencion atencion)
        //{
        //    try
        //    {
        //        AtencionCitasRepository CitaRep = new AtencionCitasRepository();
        //        if (ModelState.IsValid)
        //        {
        //            CitaRep.Save(atencion);
        //        }
        //        return Json(atencion, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(atencion, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public JsonResult GetCitaTicket(int citaid)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitaTicket(citaid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Atencion> list = new List<Atencion>();
                Atencion obj = new Atencion();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Clienteabandona(int id, string motivo)
        {
            Citas obj = new Citas();
            AtencionCitasRepository ACRep = new AtencionCitasRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.Clienteabandona(id, motivo);
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
        //[ValidateAntiForgeryToken]
        public async Task<System.Web.Mvc.JsonResult> EnviarEmail(int numeroGestion)
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
            string asunto = "";


            string[] lines;
            var list = new List<string>();
            var ruta = AppDomain.CurrentDomain.BaseDirectory + @"\Content\Publicidad\EncuestaCitas.txt";//(@"Content\Publicidad\CorreoCitas.txt");

            var fileStream = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            var file = new System.IO.StreamReader(fileStream, System.Text.Encoding.UTF8, true, 128);
            using (var streamReader = new StreamReader(fileStream, Encoding.GetEncoding("iso-8859-1")))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
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
            asunto = lines[13];

            AtencionCitasRepository RepAtencion = new AtencionCitasRepository();
            Atencion obj = new Atencion();

            ConfigRepository SucRep = new ConfigRepository();
            List<ConfigItem> ConfigList = new List<ConfigItem>();
            ConfigList = SucRep.GetConfigItem("ENC", "LINKENCUES");

            obj = RepAtencion.GetCitasByIDFinCita(numeroGestion);

            //string asunto = "BAC Credomatic – Encuesta. ";
            //string textoAccion = "https://www.sucursalelectronica.com";
            string textoAccion = ConfigList[0].ConfigItemAbreviatura.ToString();
            string recordatorioTexto = mensajeIntro;
            string tituloCorreo = mensajeTitulo;
            string cuerpoCorreo = "";
            cuerpoCorreo = "<p>" + mensajeSaludo + obj.CitaNombre + "<br><br>" +
                                "<span>BAC – Credomatic le " + recordatorioTexto + " <b>" + textoAccion + "</b> " + mensajeFooter + "</span><br><br>" +
                            "</p>" +
                            "<p>" + mensajeFinal6 + "</p>";

            try
            {
                await EmailService.EnviarEmail(obj.CitaCorreoElectronico, obj.CitaNombre, tituloCorreo, cuerpoCorreo, asunto, mensajeFinal,
            mensajeFinal1, mensajeFinal2, mensajeFinal3, mensajeFinal4, mensajeFinal5, mensajeFinal7);
                obj.Accion = 1;
                obj.Mensaje = "Correo electrónico enviado exitósamente";
            }
            catch (Exception ex)
            {
                obj.Accion = numeroGestion;
                obj.Mensaje = "No se pudo enviar correo electrónico! " + ex;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPromedioTiempoEspera(int id)
        {
            Atencion obj = new Atencion();
            AtencionCitasRepository ACRep = new AtencionCitasRepository();
            try
            {
                if (id > 0)
                {
                    obj = ACRep.GetPromedioTiempoEspera(id);
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
        public JsonResult deleteHerramienta(int CitaId, string HerramientaId)
        {
            CitaHerramienta obj = new CitaHerramienta();
            AtencionCitasRepository CitaRep = new AtencionCitasRepository();
            try
            {
                bool go = true;
                if (CitaId <= 0)
                {
                    go = false;
                }
                else if (String.IsNullOrEmpty(HerramientaId))
                {
                    go = false;
                }
                else if (HerramientaId == "-1")
                {
                    go = false;
                }
                else
                {
                    //No pasa nada
                }

                if (go)
                {
                    obj = CitaRep.deleteHerramienta(CitaId, HerramientaId);
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
        public JsonResult GetReporteCentroAtencion()
        {
            AtencionCitasRepository AtencionCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtencionCitas.GetReporteCentroAtencion());
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
        public JsonResult GetCitasLlamadoTicket(int Id)
        {
            AtencionCitasRepository AtencionCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtencionCitas.GetCitasLlamadoTicket(Id));
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
        public JsonResult GetCitaByIdentificacion(string Id)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetCitaByIdentificacion(Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Atencion> list = new List<Atencion>();
                Atencion obj = new Atencion();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AgregarTarjetaHerramienta(CitaHerramienta Herramientas)
        {
            try
            {
                AtencionCitasRepository CitaRep = new AtencionCitasRepository();
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(Herramientas.HerramientaObservacion))
                    {
                        Herramientas.HerramientaObservacion = "";
                    }
                    CitaRep.SaveTarjetaHerramienta(Herramientas);
                }
                else
                {
                    Herramientas.Accion = 0;
                    Herramientas.Mensaje = "Los datos no tienen el formato correcto!";
                }
                return Json(Herramientas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Herramientas.Accion = 0;
                Herramientas.Mensaje = ex.Message.ToString();
                return Json(Herramientas, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTarjetaHerramientasByCita(int CitaId, string Cuenta, string Tarjeta)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.GetTarjetaHerramientasByCita(CitaId, Cuenta, Tarjeta), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<CitaHerramienta> list = new List<CitaHerramienta>();
                CitaHerramienta obj = new CitaHerramienta();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult deleteTarjetaHerramienta(int CitaId, string HerramientaId, string Cuenta, string Tarjeta)
        {
            CitaHerramienta obj = new CitaHerramienta();
            AtencionCitasRepository CitaRep = new AtencionCitasRepository();
            try
            {
                bool go = true;
                if (CitaId <= 0)
                {
                    go = false;
                }
                else if (String.IsNullOrEmpty(HerramientaId))
                {
                    go = false;
                }
                else if (HerramientaId == "-1")
                {
                    go = false;
                }
                else
                {
                    //No pasa nada
                }

                if (go)
                {
                    obj = CitaRep.deleteTarjetaHerramienta(CitaId, HerramientaId, Cuenta, Tarjeta);
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
        public JsonResult CambiarCuenta(int CitaId, string Segmento, int Emisor, string Cuenta)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.CambiarCuenta(CitaId, Segmento, Emisor, Cuenta), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CambiarTarjeta(int CitaId, string Segmento, int EmisorTar, string Tarjeta)
        {
            AtencionCitasRepository AtenCitas = new AtencionCitasRepository();
            try
            {
                return Json(AtenCitas.CambiarTarjeta(CitaId, Segmento, EmisorTar, Tarjeta), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Citas obj = new Citas();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                //list.Add(obj);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckUserInfo()
        {
            SucursalRepository sucursal = new SucursalRepository();
            try
            {
                return Json(sucursal.CheckUserInfo((string)(Session["usuario"])), JsonRequestBehavior.AllowGet);
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
    }
}
