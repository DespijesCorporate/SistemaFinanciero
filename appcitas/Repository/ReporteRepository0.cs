using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;

namespace appcitas.Repository
{
    public class ReporteRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);
        }

        public List<Reportes> ReporteAtencionPorTiempoEspera(int SucursalId, int tipoCita, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Atencion_por_tiempo_espera"); 
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                cmd.Parameters.AddWithValue("@tipoCita", tipoCita);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                CitaTipoId      = Convert.ToInt32(dr["CitaTipoId"]),
                                CitaTipo        = Convert.ToString(dr["CitaTipo"]),
                                SucursalId      = Convert.ToInt32(dr["SucursalId"]),
                                sucursal        = Convert.ToString(dr["sucursal"]),
                                rango_0_15      = Convert.ToInt32(dr["rango_0_15"]),
                                rango_15_30     = Convert.ToInt32(dr["rango_15_30"]),
                                rango_30_45     = Convert.ToInt32(dr["rango_30_45"]),
                                rango_45_60     = Convert.ToInt32(dr["rango_45_60"]),
                                rango_60_mas    = Convert.ToInt32(dr["rango_60_mas"]),
                                rango_Total     = Convert.ToInt32(dr["rango_Total"]),
                                suma_0_15       = Convert.ToInt32(dr["suma_0_15"]),
                                suma_15_30      = Convert.ToInt32(dr["suma_15_30"]),
                                suma_30_45      = Convert.ToInt32(dr["suma_30_45"]),
                                suma_45_60      = Convert.ToInt32(dr["suma_45_60"]),
                                suma_60_mas     = Convert.ToInt32(dr["suma_60_mas"]),
                                acumulado_15    = Convert.ToInt32(dr["acumulado_15"]),
                                acumulado_30    = Convert.ToInt32(dr["acumulado_30"]),
                                acumulado_45    = Convert.ToInt32(dr["acumulado_45"]),
                                acumulado_60    = Convert.ToInt32(dr["acumulado_60"]),
                                acumulado_total = Convert.ToInt32(dr["acumulado_total"]),
                                total_citas     = Convert.ToInt32(dr["total_citas"]),
                                Accion          = 1,
                                Mensaje         = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Reportes> ReporteFlujoPorIntervalo(int SucursalId, int tipoCita, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Flujo_por_intervalo"); 
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                cmd.Parameters.AddWithValue("@tipoCita", tipoCita);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                CitaTipoId  = Convert.ToInt32(dr["CitaTipoId"]),
                                CitaTipo    = Convert.ToString(dr["CitaTipo"]),
                                SucursalId  = Convert.ToInt32(dr["SucursalId"]),
                                sucursal    = Convert.ToString(dr["sucursal"]),
                                rango0      = Convert.ToInt32(dr["rango0"]),
                                rango1      = Convert.ToInt32(dr["rango1"]),
                                rango2      = Convert.ToInt32(dr["rango2"]),
                                rango3      = Convert.ToInt32(dr["rango3"]),
                                rango4      = Convert.ToInt32(dr["rango4"]),
                                rango5      = Convert.ToInt32(dr["rango5"]),
                                rango6      = Convert.ToInt32(dr["rango6"]),
                                rango7      = Convert.ToInt32(dr["rango7"]),
                                rango8      = Convert.ToInt32(dr["rango8"]),
                                rango9      = Convert.ToInt32(dr["rango9"]),
                                rango10     = Convert.ToInt32(dr["rango10"]),
                                rango11     = Convert.ToInt32(dr["rango11"]),
                                rango12     = Convert.ToInt32(dr["rango12"]),
                                rango13     = Convert.ToInt32(dr["rango13"]),
                                rango14     = Convert.ToInt32(dr["rango14"]),
                                rango15     = Convert.ToInt32(dr["rango15"]),
                                rango16     = Convert.ToInt32(dr["rango16"]),
                                rango17     = Convert.ToInt32(dr["rango17"]),
                                rango18     = Convert.ToInt32(dr["rango18"]),
                                rango19     = Convert.ToInt32(dr["rango19"]),
                                rango20     = Convert.ToInt32(dr["rango20"]),
                                rango21     = Convert.ToInt32(dr["rango21"]),
                                rango22     = Convert.ToInt32(dr["rango22"]),
                                rango23     = Convert.ToInt32(dr["rango23"]),
                                rango_Total = Convert.ToInt32(dr["rango_Total"]),
                                total_citas = Convert.ToInt32(dr["total_citas"]),
                                Accion          = 1,
                                Mensaje         = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Reportes> ReporteEfectividad(int SucursalId, int tipoCita, string ejecutivo, string tipoRazon, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Efectividad"); 
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                cmd.Parameters.AddWithValue("@tipoCita", tipoCita);
                cmd.Parameters.AddWithValue("@ejecutivoId", ejecutivo);
                cmd.Parameters.AddWithValue("@tipoRazon", tipoRazon);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                CitaTipoId          = Convert.ToInt32(dr["CitaTipoId"]),
                                CitaTipo            = Convert.ToString(dr["CitaTipo"]),
                                SucursalId          = Convert.ToInt32(dr["SucursalId"]),
                                sucursal            = Convert.ToString(dr["sucursal"]),
                                ejecutivoId         = Convert.ToString(dr["ejecutivoId"]),
                                citas_retenidas     = Convert.ToInt32(dr["citas_retenidas"]),
                                citas_noRetenidas   = Convert.ToInt32(dr["citas_noRetenidas"]),
                                rango_Total         = Convert.ToInt32(dr["rango_Total"]),
                                total_retenidas     = Convert.ToInt32(dr["total_retenidas"]),
                                total_noRetenidas   = Convert.ToInt32(dr["total_noRetenidas"]),
                                total_citas         = Convert.ToInt32(dr["total_citas"]),
                                Accion              = 1,
                                Mensaje             = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Reportes> ReporteCitasPorEstado(int SucursalId, int estadocita)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Citas_GetByEstado"); 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@estadocita", estadocita);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Reportes()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Convert.ToString(dr["CitaCuenta"]),
                                 CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 estado = Convert.ToString(dr["estado"]),
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["CitaHoraClienteIniciaAtencion"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["CitaHoraClienteSaleAtencion"]),
                                 TiempoEnCita = Convert.ToInt32(dr["tiempo_en_cita"]),
                                 TiempoEspera = Convert.ToInt32(dr["tiempo_espera"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de las citas."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Reportes> GetDashboardAtencionCubiculo(int sucursalId, string posicionId, string fecha1, string fecha2)
        {
            List<Reportes> DashboardList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_GetTiempos_GroupBy_Sucursal_Posicion_Fecha"); 
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@posicionId", posicionId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                DashboardList = (from DataRow dr in dt.Rows
                                select new Reportes()
                                {
                                    CitaTipoId              = Convert.ToInt32(dr["CitaTipoId"]),
                                    CitaTipo                = Convert.ToString(dr["CitaTipo"]),
                                    SucursalId              = Convert.ToInt32(dr["SucursalId"]),
                                    sucursal                = Convert.ToString(dr["sucursal"]),
                                    PosicionId              = Convert.ToString(dr["PosicionId"]),
                                    posicion                = Convert.ToString(dr["posicion"]),
                                    citasAtendidas          = Convert.ToInt32(dr["citasAtendidas"]),
                                    citasAbandonadas        = Convert.ToInt32(dr["citasAbandonadas"]),
                                    PromedioTiempoEspera    = Convert.ToInt32(dr["PromedioTiempoEspera"]),
                                    PromedioTiempoEsperaAbandono    = Convert.ToInt32(dr["PromedioTiempoEsperaAbandono"]),
                                    PromedioTiempoAtencion  = Convert.ToInt32(dr["PromedioTiempoAtencion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información."
                                }).ToList();
                if (DashboardList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    DashboardList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCitas = new Reportes();
                oCitas.Accion = 0;
                oCitas.Mensaje = ex.Message.ToString();
                DashboardList.Add(oCitas);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DashboardList;
        }

        public List<Reportes> GetDashboardAtencionPorCita(int sucursalId, string posicionId, string fecha1, string fecha2)
        {
            List<Reportes> DashboardList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_GetTiempos_By_Sucursal_Posicion_Fecha"); 
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@posicionId", posicionId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                DashboardList = (from DataRow dr in dt.Rows
                                select new Reportes()
                                {
                                    CitaTipoId              = Convert.ToInt32(dr["CitaTipoId"]),
                                    CitaTipo                = Convert.ToString(dr["CitaTipo"]),
                                    SucursalId              = Convert.ToInt32(dr["SucursalId"]),
                                    sucursal                = Convert.ToString(dr["sucursal"]),
                                    PosicionId              = Convert.ToString(dr["PosicionId"]),
                                    posicion                = Convert.ToString(dr["posicion"]),
                                    CitaTicket              = Convert.ToString(dr["CitaTicket"]),
                                    CitaFecha               = Convert.ToString(dr["CitaFecha"]),
                                    TiempoEspera            = Convert.ToInt32(dr["TiempoEspera"]),
                                    TiempoAtencion          = Convert.ToInt32(dr["TiempoAtencion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información."
                                }).ToList();
                if (DashboardList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    DashboardList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCitas = new Reportes();
                oCitas.Accion = 0;
                oCitas.Mensaje = ex.Message.ToString();
                DashboardList.Add(oCitas);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DashboardList;
        }

        public List<Reportes> GetDashboardResolucionPorCita(int sucursalId, string posicionId, string fecha1, string fecha2)
        {
            List<Reportes> DashboardList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_GetResultado_By_Sucursal_Posicion_Fecha"); 
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@posicionId", posicionId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                DashboardList = (from DataRow dr in dt.Rows
                                select new Reportes()
                                {
                                    CitaTipoId              = Convert.ToInt32(dr["CitaTipoId"]),
                                    CitaTipo                = Convert.ToString(dr["CitaTipo"]),
                                    SucursalId              = Convert.ToInt32(dr["SucursalId"]),
                                    sucursal                = Convert.ToString(dr["sucursal"]),
                                    PosicionId              = Convert.ToString(dr["PosicionId"]),
                                    posicion                = Convert.ToString(dr["posicion"]),
                                    CitaTicket              = Convert.ToString(dr["CitaTicket"]),
                                    CitaFecha               = Convert.ToString(dr["CitaFecha"]),
                                    Resolucion              = Convert.ToString(dr["Resolucion"]),
                                    TiempoAtencion          = Convert.ToInt32(dr["TiempoAtencion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información."
                                }).ToList();
                if (DashboardList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    DashboardList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCitas = new Reportes();
                oCitas.Accion = 0;
                oCitas.Mensaje = ex.Message.ToString();
                DashboardList.Add(oCitas);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DashboardList;
        }

        public List<Reportes> ReporteAtencionesRealizadas(int SucursalId, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Atenciones_Realizadas");
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Reportes()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 tramite = Convert.ToString(dr["TramiteAbreviatura"]),
                                 posicionDescripcion = Convert.ToString(dr["PosicionNombre"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["HoraInicioReal"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["HoraFinalReal"]),
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["HoraInicioProgramada"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["HoraFinalProgramada"]),
                                 TiempoEnCita = Convert.ToInt32(dr["TiempoAtencionReal"]),
                                 TiempoEspera = Convert.ToInt32(dr["TiempoAtencionProgramado"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de las citas."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Reportes> ReporteCitasDiarias(int SucursalId, string fecha1, int codtiporazon, int codrazon)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_DiarioCitasRazonesCancelacion");
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@codtiporazon", codtiporazon);
                cmd.Parameters.AddWithValue("@codrazon", codrazon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Reportes()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 tramite = Convert.ToString(dr["TramiteAbreviatura"]),
                                 posicionDescripcion = Convert.ToString(dr["PosicionNombre"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["HoraInicioReal"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["HoraFinalReal"]),
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["HoraInicioProgramada"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["HoraFinalProgramada"]),
                                 TiempoEnCita = Convert.ToInt32(dr["TiempoAtencionReal"]),
                                 TiempoEspera = Convert.ToInt32(dr["TiempoAtencionProgramado"]),
                                 Razones = Convert.ToString(dr["razones_cancelacion"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de las citas."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }
    }
}