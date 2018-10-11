using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;
using System.Globalization;
using BAC.Crypto;

namespace appcitas.Repository
{
    public class ReporteRepository : CAD
    {
		private CryptoAes aes = new CryptoAes("8@c9@n@m@");
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);
        }

        //REPORTE CLIENTES
        public List<Reportes> ReportesClientes(string fecha1, string fecha2)
        {
            List<Reportes> Clientes = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_ReporteClientes");
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                Clientes = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                CLI_Codigo = Convert.ToString(dr["CLI_Codigo"]),
                                CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                CLI_RTN = Convert.ToString(dr["CLI_RTN"]),
                                CLI_Nombre = Convert.ToString(dr["CLI_Nombre"]),
                                CLI_Fecha_Nacimiento = Convert.ToString(dr["CLI_Fecha_Nacimiento"]),
                                CLI_Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                CLI_Agrego = Convert.ToString(dr["CLI_Agrego"]),
                                CLI_Fecha_Agrego = Convert.ToString(dr["CLI_Fecha_Agrego"]),
                                Accion = 1,
                                Mensaje = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (Clientes.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    Clientes.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                Clientes.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return Clientes;
        }

        //REPORTE CREDITOS

        public List<Reportes> ReporteCreditos(string fecha1, string fecha2, int SucursalId)
        {
            List<Reportes> Creditos = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_ReportePrestamo");
                cmd.Parameters.AddWithValue("@fechaSolicitud1", fecha1);
                cmd.Parameters.AddWithValue("@fechaSolicitud2", fecha2);
                cmd.Parameters.AddWithValue("@estado", SucursalId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                Creditos = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                PRES_Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                PRES_Codigo_CLI = Convert.ToString(dr["PRES_Codigo_CLI"]),
                                CLI_Nombre = Convert.ToString(dr["CLI_Nombre"]),
                                PRES_Fecha_Solicitud = Convert.ToString(dr["PRES_Fecha_Solicitud"]),
                                PRES_mto_Solicitado = Convert.ToString(dr["PRES_mto_Solicitado"]),
                                PRES_mto_Aprobado = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                PRES_Plazo_Meses = Convert.ToString(dr["PRES_Plazo_Meses"]),
                                PRES_Porc_Interes = Convert.ToString(dr["PRES_Porc_Interes"]),
                                PRES_Saldo = Convert.ToString(dr["PRES_Saldo"]),
                                Estado = Convert.ToString(dr["Estado"]),
                                FrecuenciaPago = Convert.ToString(dr["FrecuenciaPago"]),
                                PRES_Num_Pagos = Convert.ToString(dr["PRES_Num_Pagos"]),
                                Destino = Convert.ToString(dr["DES_Descripcion"]),
                                Garantia = Convert.ToString(dr["GAR_Descripcion"]),
                                PRES_Agrego = Convert.ToString(dr["PRES_Agrego"]),
                                PRES_Fecha_Agrego = Convert.ToString(dr["PRES_Fecha_Agrego"]),
                                Accion = 1,
                                Mensaje = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (Creditos.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    Creditos.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                Creditos.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return Creditos;
        }

        //REPORTE PRESTAMOS POR CLIENTE
        public List<Reportes> Reporte_PresPorCliente(string IdCliente)
        {
            List<Reportes> PresPorCliente = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_ConsulPrestaCliente");
                cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                PresPorCliente = (from DataRow dr in dt.Rows
                                  select new Reportes()
                                  {

                                      CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                      CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                      Garantia = Convert.ToString(dr["GAR_Descripcion"]),
                                      PRES_mto_Aprobado = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                      PRES_Fecha_Aprob = Convert.ToString(dr["PRES_Fecha_Aprob"]),
                                      Accion = 1,
                                      Mensaje = "Se cargó correctamente la información de las citas."
                                  }).ToList();
                if (PresPorCliente.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    PresPorCliente.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                PresPorCliente.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PresPorCliente;
        }


        // REPORTE PRESTAMOS VENCIDOS GLOBAL 
        public List<Reportes> Reporte_PrestamosVencidos(string fecha1, string fecha2)
        {
            List<Reportes> PresVencidosGlobal = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Prestamos_Vencidos");
                cmd.Parameters.AddWithValue("@FechaInicio", fecha1);
                cmd.Parameters.AddWithValue("@FechaFin", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                PresVencidosGlobal = (from DataRow dr in dt.Rows
                                      select new Reportes()
                                      {
                                          PRES_Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                          PRES_Codigo_CLI = Convert.ToString(dr["PRES_Codigo_CLI"]),
                                          CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                          CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                          Tipo_Prestamo = Convert.ToString(dr["Tipo_Prestamo"]),
                                          PRES_Fecha_Vencimiento = Convert.ToString(dr["PRES_Fecha_Vencimiento"]),
                                          Monto = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                          Capital = Convert.ToString(dr["Capital"]),
                                          Mora = Convert.ToString(dr["Mora"]),
                                          Accion = 1,
                                          Mensaje = "Se cargó correctamente la información de las citas."
                                      }).ToList();
                if (PresVencidosGlobal.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    PresVencidosGlobal.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                PresVencidosGlobal.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PresVencidosGlobal;
        }

        //REPORTE COBRANZAS POR RANGO DE FECHA
        public List<Reportes> Reporte_CobranzasPorFecha(string fecha1, string fecha2)
        {
            List<Reportes> CobranzaPorFechas = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Fechas_Cobranza");
                cmd.Parameters.AddWithValue("@FechaInicio", fecha1);
                cmd.Parameters.AddWithValue("@FechaFin", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CobranzaPorFechas = (from DataRow dr in dt.Rows
                                     select new Reportes()
                                     {
                                         PRES_Codigo = Convert.ToString(dr["codigoPrest"]),
                                         CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                         CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                         No_Cuota = Convert.ToString(dr["No_Cuota"]),
                                         Total = Convert.ToString(dr["Total"]),
                                         Fecha_Pago = Convert.ToString(dr["Fecha_Pago"]),
                                         Accion = 1,
                                         Mensaje = "Se cargó correctamente la información de las citas."
                                     }).ToList();
                if (CobranzaPorFechas.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CobranzaPorFechas.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CobranzaPorFechas.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CobranzaPorFechas;
        }

        //REPORTE PRESTAMO Y SUS CUOTAS POR CLIENTE
        public List<Reportes> Reporte_PresYsusCuotas(string IdCliente)
        {
            List<Reportes> PresYsusCuotas = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Prest_Cuotas_Cliente");
                cmd.Parameters.AddWithValue("@Id_Cliente", IdCliente);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                PresYsusCuotas = (from DataRow dr in dt.Rows
                                  select new Reportes()
                                  {
                                      PRES_Codigo = Convert.ToString(dr["codigoPrest"]),
                                      CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                      CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                      No_Cuota = Convert.ToString(dr["No_Cuota"]),
                                      Monto = Convert.ToString(dr["Monto"]),
                                      Fecha_Pago = Convert.ToString(dr["Fecha_Pago"]),
                                      Accion = 1,
                                      Mensaje = "Se cargó correctamente la información de las citas."
                                  }).ToList();
                if (PresYsusCuotas.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    PresYsusCuotas.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                PresYsusCuotas.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PresYsusCuotas;
        }

        // REPORTE PRESTAMOS CON MORA 

        public List<Reportes> Reporte_PrestamosConMora(string fecha1, string fecha2)
        {
            List<Reportes> PresMora = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Prestamos_Mora");
                cmd.Parameters.AddWithValue("@FechaInicio", fecha1);
                cmd.Parameters.AddWithValue("@FechaFin", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                PresMora = (from DataRow dr in dt.Rows
                            select new Reportes()
                            {
                                PRES_Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                PRES_Codigo_CLI = Convert.ToString(dr["PRES_Codigo_CLI"]),
                                CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                Tipo_Prestamo = Convert.ToString(dr["Tipo_Prestamo"]),
                                PRES_Fecha_Vencimiento = Convert.ToString(dr["PRES_Fecha_Vencimiento"]),
                                Monto = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                Capital = Convert.ToString(dr["Capital"]),
                                Mora = Convert.ToString(dr["Mora"]),
                                Accion = 1,
                                Mensaje = "Se cargó correctamente la información de las citas."
                            }).ToList();
                if (PresMora.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    PresMora.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                PresMora.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PresMora;
        }

        //REPORTE TOTALES DINERO COBRADO

        public List<Reportes> Reporte_TotalesDineroCobrado(string fecha1, string fecha2)
        {
            List<Reportes> TotalDineroCobrado = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Total_Dinero_Cobrado");
                cmd.Parameters.AddWithValue("@FechaInicio", fecha1);
                cmd.Parameters.AddWithValue("@FechaFin", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                TotalDineroCobrado = (from DataRow dr in dt.Rows
                                      select new Reportes()
                                      {
                                          FechaTransaccion = Convert.ToString(dr["TRP_Fecha_Trans"]),
                                          Capital = Convert.ToString(dr["Capital"]),
                                          Interes = Convert.ToString(dr["Interes"]),
                                          Mora = Convert.ToString(dr["Mora"]),
                                          Total = Convert.ToString(dr["Total"]),
                                          Accion = 1,
                                          Mensaje = "Se cargó correctamente la información de las citas."
                                      }).ToList();
                if (TotalDineroCobrado.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    TotalDineroCobrado.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                TotalDineroCobrado.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return TotalDineroCobrado;
        }

        // REPORTE PRESTAMOS PAGADOS

        public List<Reportes> Reporte_PrestamosPagados(string fecha1, string fecha2)
        {
            List<Reportes> PresPagados = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Prestamos_Pagados");
                cmd.Parameters.AddWithValue("@FechaInicio", fecha1);
                cmd.Parameters.AddWithValue("@FechaFin", fecha2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                PresPagados = (from DataRow dr in dt.Rows
                               select new Reportes()
                               {
                                   PRES_Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                   CLI_Nombre = Convert.ToString(dr["Nombre"]),
                                   PRES_Fecha_Aprob = Convert.ToString(dr["PRES_Fecha_Aprob"]),
                                   PRES_Fecha_Cancel = Convert.ToString(dr["PRES_Fecha_Cancel"]),
                                   Capital = Convert.ToString(dr["Capital"]),
                                   Interes = Convert.ToString(dr["Interes"]),
                                   Mora = Convert.ToString(dr["Mora"]),
                                   Accion = 1,
                                   Mensaje = "Se cargó correctamente la información de las citas."
                               }).ToList();
                if (PresPagados.Count == 0)
                {
                    Reportes ss = new Reportes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    PresPagados.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Reportes oCita = new Reportes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                PresPagados.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PresPagados;
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

        public List<Reportes> ReporteDashboardWalkin(int SucursalId, int estadocita, string cmb_cubiculo, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Dashboard_Walkin");
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@estadocita", estadocita);
                cmd.Parameters.AddWithValue("@cmb_cubiculo", cmb_cubiculo);
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
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 DiaSemana = Convert.ToString(dr["DiaSemana"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaHora = Convert.ToString(dr["CitaHoraProgramada"]),
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["CitaHoraClienteIniciaAtencion"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["CitaHoraClienteSaleAtencion"]),
                                 TiempoEnCita = Convert.ToInt32(dr["tiempo_en_cita"]),
                                 TiempoEspera = Convert.ToInt32(dr["tiempo_espera"]),
                                 Resolucion = Convert.ToString(dr["Resolucion"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 Marca = Convert.ToString(dr["MarcaTarjeta"]),
                                 Tarjetas = Convert.ToString(dr["tarjetas"]),
                                 Familia = Convert.ToString(dr["Familia"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 Razones = Convert.ToString(dr["razones_cancelacion"]),
                                 Herramientas = Convert.ToString(dr["herramientas"]),
                                 estado = Convert.ToString(dr["EstadoCita"]),
                                 OrigenCita = Convert.ToString(dr["OrigenCita"]),
                                 SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                                 CitaUsuarioAtendio = Convert.ToString(dr["CitaUsuarioAtendio"]),
                                 CitaTicket = Convert.ToString(dr["Ticket"]),
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
        public List<Reportes> DashboardConsolidado(int SucursalId, int tipoatencion, string cmb_cubiculo, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Dashboard_Consolidado");
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@tipoatencion", tipoatencion);
                cmd.Parameters.AddWithValue("@cmb_cubiculo", cmb_cubiculo);
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
                                 SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                                 DiaSemana = Convert.ToString(dr["DiaSemana"]),
                                 TiempoEspera = Convert.ToInt32(dr["tiempo_espera"]),
                                 TiempoEnCita = Convert.ToInt32(dr["tiempo_en_cita"]),
                                 citasAtendidas = Convert.ToInt32(dr["ClientesAtendidos"]),
                                 TotalNoAtendidas = Convert.ToInt32(dr["ClientesEmitidos"]),
                                 CitasAbandonadas = Convert.ToInt32(dr["ClientesAbandonaron"]),
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
                                 CitaTipoId = Convert.ToInt32(dr["CitaTipoId"]),
                                 CitaTipo = Convert.ToString(dr["CitaTipo"]),
                                 SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 ejecutivoId = Convert.ToString(dr["ejecutivoId"]),
                                 citas_retenidas = Convert.ToInt32(dr["citas_retenidas"]),
                                 citas_noRetenidas = Convert.ToInt32(dr["citas_noRetenidas"]),
                                 rango_Total = Convert.ToInt32(dr["rango_Total"]),
                                 total_retenidas = Convert.ToInt32(dr["total_retenidas"]),
                                 total_noRetenidas = Convert.ToInt32(dr["total_noRetenidas"]),
                                 total_citas = Convert.ToInt32(dr["total_citas"]),
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


        public List<Reportes> DashboardRazonCancelacion(int SucursalId, int tipoCita, string ejecutivo, string tipoRazon, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Dashboard_Razon_Cancelacion");
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
                                 CodigoRazon = Convert.ToString(dr["CodigoRazon"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 CitaTipo = Convert.ToString(dr["CitaTipo"]),
                                 Razones = Convert.ToString(dr["razones_cancelacion"]),
                                 ListadoExtra = Convert.ToString(dr["listado_extra"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 estado = Convert.ToString(dr["EstadoCita"]),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 Marca = Convert.ToString(dr["MarcaTarjeta"]),
                                 Familia = Convert.ToString(dr["Familia"]),
                                 Tarjetas = Convert.ToString(dr["tarjetas"]),
                                 CitaUsuarioAtendio = Convert.ToString(dr["CitaUsuarioAtendio"]),
                                 TipoRazon = Convert.ToString(dr["TipoRazon"]),
                                 Resolucion = Convert.ToString(dr["Resolucion"]),
                                 ComentResolucion = Convert.ToString(dr["ComentResolucion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
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

        public List<Reportes> ReporteCitasPorEstado(int SucursalId, int estadocita,string cmb_cubiculo, string fecha1, string fecha2)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Citas_GetByEstado"); 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@estadocita", estadocita);
                cmd.Parameters.AddWithValue("@cmb_cubiculo", cmb_cubiculo);
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
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 DiaSemana = Convert.ToString(dr["DiaSemana"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaHora = Convert.ToString(dr["CitaHoraProgramada"]),
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["CitaHoraClienteIniciaAtencion"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["CitaHoraClienteSaleAtencion"]),
                                 TiempoEnCita = Convert.ToInt32(dr["tiempo_en_cita"]),
                                 TramiteDuracion = Convert.ToInt32(dr["tramiteDuracion"]),                
                                 TiempoEspera = Convert.ToInt32(dr["tiempo_espera"]),
                                 Resolucion = Convert.ToString(dr["Resolucion"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 Marca = Convert.ToString(dr["MarcaTarjeta"]),
                                 Familia = Convert.ToString(dr["Familia"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 Razones = Convert.ToString(dr["razones_cancelacion"]),
                                 Herramientas = Convert.ToString(dr["herramientas"]),
                                 estado = Convert.ToString(dr["EstadoCita"]),
                                 OrigenCita = Convert.ToString(dr["OrigenCita"]),
                                 SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                                 Tarjetas = Convert.ToString(dr["tarjetas"]),
                                 CitaUsuarioAtendio = Convert.ToString(dr["CitaUsuarioAtendio"]),
                                 CitaTicket = Convert.ToString(dr["Ticket"]),
                                 HoraLLego = Convert.ToString(dr["horaLlego"]),
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

        public List<Reportes> DashboardCitasProgramadas(int sucursalId, string posicionId, string fecha1, string fecha2)
        {
            List<Reportes> DashboardList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Citas_programadas"); 
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@PosicionId", posicionId);
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
                                    sucursal    = Convert.ToString(dr["sucursal"]),
                                    PosicionId  = Convert.ToString(dr["PosicionId"]),
                                    CitaNombre  = Convert.ToString(dr["CitaNombre"]),
                                    CitaCuenta  = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                    CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                    segmento    = Convert.ToString(dr["segmento"]),
                                    Marca       = Convert.ToString(dr["marca"]),
                                    Familia     = Convert.ToString(dr["Familia"]),
                                    CitaFecha   = Convert.ToString(dr["CitaFecha"]),
                                    CitaHora    = Convert.ToString(dr["CitaHora"]),
                                    Razones     = Convert.ToString(dr["razones"]),
                                    SucursalOrigen = Convert.ToString(dr["SucursalOrigen"]),
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

        public List<Reportes> ReporteAtencionesRealizadas(int SucursalId, string fecha1, string fecha2, int tipoatencion)
        {
            List<Reportes> CitasList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Atenciones_Realizadas");
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId);
                cmd.Parameters.AddWithValue("@fecha1", fecha1);
                cmd.Parameters.AddWithValue("@fecha2", fecha2);
                cmd.Parameters.AddWithValue("@tipoatencion", tipoatencion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Reportes()
                             {
                                 DiaSemana = Convert.ToString(dr["DiaSemana"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 citasAtendidas = Convert.ToInt32(dr["NumeroTickets"]),
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

        public List<Reportes> GetDashboardComportamientoIncidencias(int sucursalId, int CitaTipo, string fecha1, string fecha2)
        {
            List<Reportes> DashboardList = new List<Reportes>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_Comportamiento_Incidencia");
                //DateTime dti = DateTime.ParseExact(fecha1, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                //DateTime dtf = DateTime.ParseExact(fecha2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                DateTime dti = Convert.ToDateTime(fecha1);
                DateTime dtf = Convert.ToDateTime(fecha2);
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@CitaTipo", CitaTipo);
                cmd.Parameters.AddWithValue("@FechaInicial", dti);
                cmd.Parameters.AddWithValue("@FechaFinal", dtf);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                DashboardList = (from DataRow dr in dt.Rows
                                 select new Reportes()
                                 {
                                     SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                     SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                                     citasAtendidas = Convert.ToInt32(dr["CantidadAtendidos"]),
                                     CantidadWalkinAtendidos = Convert.ToInt32(dr["CantidadWalkinAtendidos"]),
                                     CitasVencidas = Convert.ToInt32(dr["Vencidas"]),
                                     CitasAbandonadas = Convert.ToInt32(dr["Abandonadas"]),
                                     AbandonadasWalkin = Convert.ToInt32(dr["AbandonadasWalkin"]),
                                     CitasCanceladas = Convert.ToInt32(dr["Canceladas"]),
                                     TotalNoAtendidas = Convert.ToInt32(dr["NoAtendidas"]),
                                     Efectividad = Convert.ToDecimal(dr["Efectividad"]),
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

        public List<MantaDatos> GetDashboardMantaDatos(int sucursalId, int CitaTipo, string fecha1, string fecha2)
        {
            List<MantaDatos> DashboardList = new List<MantaDatos>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_MantaDatos");
                //DateTime dti = DateTime.ParseExact(fecha1, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                //DateTime dtf = DateTime.ParseExact(fecha2, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                DateTime dti = Convert.ToDateTime(fecha1);
                DateTime dtf = Convert.ToDateTime(fecha2);
                cmd.Parameters.AddWithValue("@SucursalId", sucursalId);
                cmd.Parameters.AddWithValue("@CitaTipo", CitaTipo);
                cmd.Parameters.AddWithValue("@FechaInicio", dti);
                cmd.Parameters.AddWithValue("@FechaFin", dtf);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                DashboardList = (from DataRow dr in dt.Rows
                                 select new MantaDatos()
                                 {
                                     CitaId = Convert.ToInt32(dr["CitaId"]),
                                     CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                     CitaFechaFormat = Convert.ToString(dr["CitaFechaFormat"]),
                                     CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                     CitaTipo = Convert.ToInt32(dr["CitaTipo"]),
                                     CitaTipoDescripcion = Convert.ToString(dr["CitaTipoDescripcion"]),
                                     CitaEstado = Convert.ToString(dr["CitaEstado"]),
                                     CitaEstadoDescripcion = Convert.ToString(dr["CitaEstadoDescripcion"]),
                                     CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                     CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                     EmisorId = Convert.ToInt32(dr["EmisorId"]),
                                     EmisorCuenta = Convert.ToString(dr["EmisorCuenta"]),
                                     MarcaId = Convert.ToString(dr["MarcaId"]),
                                     Marca = Convert.ToString(dr["Marca"]),
                                     Producto = Convert.ToString(dr["Producto"]),
                                     Segmentos = Convert.ToString(dr["Segmentos"]),
                                     Familia = Convert.ToString(dr["Familia"]),
                                     CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                     CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                     CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                     CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                     SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                     SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                                     CitaUsuarioAtendio = Convert.ToString(dr["CitaUsuarioAtendio"]),
                                     DiaAtencion = Convert.ToString(dr["DiaAtencion"]),
                                     FechaAtencion = Convert.ToString(dr["FechaAtencion"]),
                                     HoraCita = Convert.ToString(dr["HoraCita"]),
                                     HoraAtencion = Convert.ToString(dr["HoraAtencion"]),
                                     HoraFinalizaAtencion = Convert.ToString(dr["HoraFinalizaAtencion"]),
                                     HoraClienteLlego = Convert.ToString(dr["HoraClienteLlego"]),
                                     Resolucion = Convert.ToString(dr["Resolucion"]),
                                     HerramientaRescate = Convert.ToString(dr["HerramientaRescate"]),
                                     RazonCancelacion = Convert.ToString(dr["RazonCancelacion"]),
                                     DescResolucion = Convert.ToString(dr["DescResolucion"]),
                                     TramiteId = Convert.ToInt32(dr["TramiteId"]),
                                     Tramite = Convert.ToString(dr["Tramite"]),
                                     Canal = Convert.ToString(dr["Canal"]),
                                     Banco = Convert.ToString(dr["Banco"]),
                                     MotivoInsatisfaccion = Convert.ToString(dr["MotivoInsatisfaccion"]),
                                     CuentasAdicionales = Convert.ToString(dr["CuentasAdicionales"]),
                                     TarjetasAdicionales = Convert.ToString(dr["TarjetasAdicionales"]),
                                     RazonCancelacionAdicional = Convert.ToString(dr["RazonCancelacionAdicional"]),
                                     HerramientaRescateAdicional = Convert.ToString(dr["HerramientaRescateAdicional"]),
                                     SucursalOrigen = Convert.ToString(dr["SucursalOrigen"]),
                                     Accion = 1,
                                     Mensaje = "Se cargó correctamente la información."
                                 }).ToList();
                if (DashboardList.Count == 0)
                {
                    MantaDatos ss = new MantaDatos();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    DashboardList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                MantaDatos oCitas = new MantaDatos();
                oCitas.Accion = 0;
                oCitas.Mensaje = ex.Message.ToString();
                DashboardList.Add(oCitas);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return DashboardList;
        }
		private string Decryt(string data)
        {
            string dataDecrypted = string.Empty;
            try
            {
                if (data != null)
                {
                    if (data.Trim().Length != 0)
                        dataDecrypted = aes.Decrypt(data);
                }
            }
            catch
            {
                dataDecrypted = "Error en Decrypt";
            }
            return dataDecrypted;
        }
        private string Encrypt(string data)
        {
            string dataEncrypted = string.Empty;
            try
            {
                if (data != null)
                {
                    if (data.Trim().Length != 0)
                        dataEncrypted = aes.Encrypt(data);
                }
            }
            catch
            {
                dataEncrypted = "Error en Encrypt";
            }
            return dataEncrypted;
        }
    }
}
