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
    public class SucursalesController : Controller
    {
        Sucursales pSucursal = new Sucursales();
        #region Sucursales
        //
        // GET: /Sucursales/
        public ActionResult Index()
        {
            return View();
        }

        //LISTA DE ESTADOS
        [HttpPost]
        public JsonResult ListaEstados()
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.ListaEstados(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveData(Sucursales sucursal)
        {
            try
            {
                SucursalRepository SucRep = new SucursalRepository();
                if (ModelState.IsValid)
                {
                    SucRep.Save(sucursal);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                else
                {
                    sucursal.Accion = 0;
                    sucursal.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                return Json(sucursal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(sucursal, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetAll()
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.GetSucursales(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult getSucursalesCentroAtencion()
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.getSucursalesByAtencion(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getSucursalesByAtencion()
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.getSucursalesByAtencion(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getSucursalesCanal()
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.getSucursalesCanal(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getSucursalesBySegmento(string SegmentoId)
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.getSucursalesBySegmento(SegmentoId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult getSegmentosBySucursal(int SucursalId)
        {
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                return Json(SucRep.getSegmentosBySucursal(SucursalId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetOne(int id)
        {
            Sucursales obj = new Sucursales();
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                if (id > 0)
                {
                    obj = SucRep.GetSucursal(id);
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
        public JsonResult delete(int id)
        {
            Sucursales obj = new Sucursales();
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                if (id > 0)
                {
                    obj = SucRep.Del(id);
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
        public JsonResult CheckSucursal(string Nombre, string Abreviatura)
        {
            Sucursales obj = new Sucursales();
            SucursalRepository SucRep = new SucursalRepository();
            try
            {
                if (Nombre != string.Empty || Abreviatura != string.Empty)
                {
                    obj.Accion = SucRep.CheckSucursal(Nombre, Abreviatura);
                    if(obj.Accion == -1)
                    {
                        obj.Mensaje = "Se genero un error al verificar la existencia de la sucursal!";
                    }
                }
                else
                {
                    obj.Accion = -1;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;
                obj.Accion = -1;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetUsersInfoCitas(string CodigoUsuario)
        {
            SucursalRepository Sucursal = new SucursalRepository();
            try
            {
                return Json(Sucursal.GetUsersInfoCitas(CodigoUsuario), JsonRequestBehavior.AllowGet);
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
        #endregion

        #region SucursalesHorario
        [HttpPost]
        public JsonResult SaveDataHorarios(Horarios horarios)
        {
            try
            {
                HorarioRepository SucHor = new HorarioRepository();
                if (ModelState.IsValid)
                {
                    SucHor.Save(horarios);
                }
                else
                {
                    horarios.Accion = 0;
                    horarios.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                //return Json(horarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                horarios.Accion = 0;
                horarios.Mensaje = ex.Message.ToString();
            }

            return Json(horarios, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetAllHorarios()
        {
            HorarioRepository SucHor = new HorarioRepository();
            try
            {
                return Json(SucHor.GetHorarios(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetHorariosBySucursal(int id)
        {
            HorarioRepository SucHor = new HorarioRepository();
            try
            {
                return Json(SucHor.GetHorariosBySucurcal(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetHorariosByID(string SucId, string HorId)
        {
            HorarioRepository SucHor = new HorarioRepository();
            try
            {
                return Json(SucHor.GetHorario(SucId, HorId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;                
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();                
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult deleteHorario(int SucursalId, string HorarioId)
        {
            Horarios obj = new Horarios();
            HorarioRepository SSRep = new HorarioRepository();
            try
            {
                if (HorarioId != "")
                {
                    obj = SSRep.Del(SucursalId, HorarioId);
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
        public JsonResult CheckHorario(int SucId, string Horario)
        {
            SucursalesHorario obj = new SucursalesHorario();
            HorarioRepository SucRep = new HorarioRepository();
            try
            {
                if (SucId != -1 || Horario != string.Empty)
                {
                    obj.Accion = SucRep.CheckHorario(SucId, Horario);
                    if (obj.Accion == -1)
                    {
                        obj.Mensaje = "Se genero un error al verificar la existencia del horario!";
                    }
                }
                else
                {
                    obj.Accion = -1;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;
                obj.Accion = -1;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region SucursalesCubiculo
        [HttpPost]
        public JsonResult SaveDataCubiculos(Cubiculo cubiculo)
        {
            try
            {
                CubiculoRepository SucCub = new CubiculoRepository();
                if (ModelState.IsValid)
                {
                    SucCub.Save(cubiculo);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                else
                {
                    cubiculo.Accion = 0;
                    cubiculo.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                //return Json(horarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                cubiculo.Accion = 0;
                cubiculo.Mensaje = ex.Message.ToString();
            }

            return Json(cubiculo, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetAllCubiculo(int id)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculos(id), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCubiculoById(int id, string cub)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculo(id, cub), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetCubiculosBySucursal(int sucursalid)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosBySucursal(sucursalid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCubiculosByCita(int sucursalid)
        {
            CubiculoRepository SucCub = new CubiculoRepository();
            try
            {
                return Json(SucCub.GetCubiculosByCita(sucursalid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                List<Cubiculo> list = new List<Cubiculo>();
                Cubiculo obj = new Cubiculo();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult deleteCubiculo(int SucursalId, string CubId)
        {
            Cubiculo obj = new Cubiculo();
            CubiculoRepository SSRep = new CubiculoRepository();
            try
            {
                if (CubId != "")
                {
                    obj = SSRep.Del(SucursalId, CubId);
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
        public JsonResult CheckCubiculo(int SucId, string Cubiculo)
        {
            Cubiculo obj = new Cubiculo();
            CubiculoRepository SucRep = new CubiculoRepository();
            try
            {
                if (SucId != -1 || Cubiculo != string.Empty)
                {
                    obj.Accion = SucRep.CheckCubiculo(SucId, Cubiculo);
                    if (obj.Accion == -1)
                    {
                        obj.Mensaje = "Se genero un error al verificar la existencia del cubiculo!";
                    }
                }
                else
                {
                    obj.Accion = -1;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;
                obj.Accion = -1;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region SucursalesSegmentos

        [HttpPost]
        public JsonResult SaveDataSegmentos(SucursalesSegmento segmentos)
        {
            try
            {
                SegmentosRepository SucSeg = new SegmentosRepository();
                if (ModelState.IsValid)
                {
                    SucSeg.Save(segmentos);
                    //db.Sucursal.Add(sucursal);
                    //db.SaveChanges();
                }
                else
                {
                    segmentos.Accion = 0;
                    segmentos.Mensaje = "Los datos enviados no son correctos, intente de nuevo!";
                }
                //return Json(horarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                segmentos.Accion = 0;
                segmentos.Mensaje = ex.Message.ToString();
            }

            return Json(segmentos, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetAllSegmentos()
        {
            SegmentosRepository SucSeg = new SegmentosRepository();
            try
            {
                return Json(SucSeg.GetSegmentos(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Sucursales> list = new List<Sucursales>();
                Sucursales obj = new Sucursales();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetSegmentosBySucursal(int id)
        {
            SegmentosRepository SucSeg = new SegmentosRepository();
            try
            {
                return Json(SucSeg.GetSegmentosBySucursal(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<SucursalesSegmento> list = new List<SucursalesSegmento>();
                SucursalesSegmento obj = new SucursalesSegmento();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSegmentosBySucursalId(int id)
        {
            SegmentosRepository SucSeg = new SegmentosRepository();
            try
            {
                return Json(SucSeg.GetSegmentosBySucursal(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<SucursalesSegmento> list = new List<SucursalesSegmento>();
                SucursalesSegmento obj = new SucursalesSegmento();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllRazones(int TipoId)
        {
            RazonRepository RazonRep = new RazonRepository();
            try
            {
                return Json(RazonRep.GetAllByTipo(TipoId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                List<Razones> list = new List<Razones>();
                Razones obj = new Razones();
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                list.Add(obj);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult deleteSegmento(int SucursalId, string SucSegmentoId)
        {
            SucursalesSegmento obj = new SucursalesSegmento();
            SegmentosRepository SSRep = new SegmentosRepository();
            try
            {
                if (SucSegmentoId != "")
                {
                    obj = SSRep.DelSegmento(SucursalId, SucSegmentoId);
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
        public JsonResult CheckSegmento(int SucId, string Segmento)
        {
            SucursalesSegmento obj = new SucursalesSegmento();
            SegmentosRepository SucRep = new SegmentosRepository();
            try
            {
                if (SucId != -1 || Segmento != string.Empty)
                {
                    obj.Accion = SucRep.CheckSegmento(SucId, Segmento);
                    if (obj.Accion == -1)
                    {
                        obj.Mensaje = "Se genero un error al verificar la existencia del cubiculo!";
                    }
                }
                else
                {
                    obj.Accion = -1;
                    obj.Mensaje = "El parametro tiene un valor incorrecto!";
                }

            }
            catch (Exception ex)
            {
                //throw;
                obj.Accion = -1;
                obj.Mensaje = ex.Message.ToString();
                //return Json(list, JsonRequestBehavior.AllowGet);
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        #endregion  

        [HttpPost]
        public JsonResult CheckUserInfo()
        {
            SucursalRepository Sucursal = new SucursalRepository();
            try
            {
                return Json(Sucursal.CheckUserInfo((string)(Session["usuario"])), JsonRequestBehavior.AllowGet);
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
