using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;
using BAC.Crypto;

namespace appcitas.Repository
{
    public class CitaRepository : CAD
    {
		private CryptoAes aes = new CryptoAes("8@c9@n@m@");
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        //SE AGREGA NUEVO REPOSITORIO
        public List<PlanPago> GetPlanPagos(int CodPrest)
        {
            List<PlanPago> CitasList = new List<PlanPago>();
            try
            {
                SqlCommand cmd = CrearComando("SP_Calculo_Pago_Cod"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CodPrest", CodPrest);
                //cmd.Parameters.AddWithValue("@Monto", Monto);//Agregamos los parametros. 
                //cmd.Parameters.AddWithValue("@Tasa", Tasa);
                //cmd.Parameters.AddWithValue("@Plazo", Plazo);
                //cmd.Parameters.AddWithValue("@Frecuency", Frecuency);
                //cmd.Parameters.AddWithValue("@FecAprob", Fecha);
                //cmd.Parameters.AddWithValue("@Tipo", Tipopres);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new PlanPago()
                             {
                                 Codigo = Convert.ToString(dr["Codigo"]),
                                 Num = Convert.ToInt32(dr["Num"]),
                                 Fecha = Convert.ToString(dr["Fecha"]),
                                 Capital = Convert.ToDecimal(dr["Cuota"]),
                                 Interes = Convert.ToDecimal(dr["Interes"]),
                                 //Frecuency = Convert.ToString(dr["Frecuency"]),
                                 Total = Convert.ToDecimal(dr["Capital"]),
                                 Saldo = Convert.ToDecimal(dr["Saldo"]),
                                 Accion = 1
                                 //Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    PlanPago ss = new PlanPago();
                    ss.Accion = 0;
                    //ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                PlanPago oCita = new PlanPago();
                //oCita.Accion = 0;
                //oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }
        //FIN POCA

        public Citas Save(Citas pCita)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Cita_Save");
                cmd.Parameters.AddWithValue("@CitaId", pCita.CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@CitaIdentificacion", pCita.CitaIdentificacion);
                cmd.Parameters.AddWithValue("@CitaNombre", pCita.CitaNombre);
                cmd.Parameters.AddWithValue("@CitaCorreoElectronico", pCita.CitaCorreoElectronico);
                cmd.Parameters.AddWithValue("@CitaCuenta", Encrypt(pCita.CitaCuenta));
                cmd.Parameters.AddWithValue("@CitaTarjeta", Encrypt(pCita.CitaTarjeta));
                cmd.Parameters.AddWithValue("@CitaTelefonoCelular", pCita.CitaTelefonoCelular);
                cmd.Parameters.AddWithValue("@CitaTelefonoCasa", pCita.CitaTelefonoCasa);
                cmd.Parameters.AddWithValue("@CitaTelefonoOficina", pCita.CitaTelefonoOficina);
                cmd.Parameters.AddWithValue("@TramiteId", pCita.TramiteId);
                cmd.Parameters.AddWithValue("@SucursalId", pCita.SucursalId);
                cmd.Parameters.AddWithValue("@CitaSegmentoId", pCita.CitaSegmentoId);
                cmd.Parameters.AddWithValue("@PosicionId", pCita.PosicionId);
                cmd.Parameters.AddWithValue("@CitaFecha", pCita.CitaFecha);
                cmd.Parameters.AddWithValue("@CitaHora", pCita.CitaHora);
                cmd.Parameters.AddWithValue("@CitaEstado", pCita.CitaEstado);
                cmd.Parameters.AddWithValue("@EmisorId", pCita.EmisorId);
                cmd.Parameters.AddWithValue("@CitaTicket", pCita.CitaTicket);
                cmd.Parameters.AddWithValue("@usuarioCreacion", HttpContext.Current.Session["usuario"]);
                cmd.Parameters.AddWithValue("@agenciaOrigen", HttpContext.Current.Session["SucursalId"]);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCita.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar cita.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCita.Accion = vResultado;
            if (vResultado == 0)
            {
                pCita.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pCita.Mensaje = "Cita ingresada correctamente!";
            }
            return pCita;
        }

        public Citas ProgramarCita(Citas pCita)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Cita_SaveProgramar");
                cmd.Parameters.AddWithValue("@CitaId", pCita.CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@PosicionId", pCita.PosicionId);
                cmd.Parameters.AddWithValue("@CitaFecha", pCita.CitaFecha);
                cmd.Parameters.AddWithValue("@CitaHora", pCita.CitaHora);
                cmd.Parameters.AddWithValue("@CitaTicket", pCita.CitaTicket);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCita.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar programar cita.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCita.Accion = vResultado;
            if (vResultado == 0)
            {
                pCita.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pCita.Mensaje = "Cita programada correctamente!";
            }
            return pCita;
        }

        public Citas AsignarRazon(Citas pCita)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_CitaRazon_Save");
                cmd.Parameters.AddWithValue("@CitaId", pCita.CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput;

                cmd.Parameters.AddWithValue("@TipoRazonId", pCita.TipoRazonId);
                cmd.Parameters.AddWithValue("@RazonId", pCita.RazonId);
                cmd.Parameters.AddWithValue("@TipoOrigenExtraId", pCita.TipoOrigenExtraId);
                cmd.Parameters.AddWithValue("@CodigoListadoOrigenExtraId", pCita.CodigoListadoOrigenExtraId);
                cmd.Parameters.AddWithValue("@DatoExtraId", pCita.DatoExtraId);
                cmd.Parameters.AddWithValue("@RazonOrigen", pCita.RazonOrigen);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCita.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar razón.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCita.Accion = vResultado;
            if (vResultado == 0)
            {
                pCita.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pCita.Mensaje = "Razón guardada correctamente!";
            }
            return pCita;
        }

        public List<Citas> GetCitas()
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_Get"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                                select new Citas()
                                {
                                    CitaId = Convert.ToInt32(dr["CitaId"]),
                                    CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                    CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                    CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                    CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                    CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                    CitaHora = Convert.ToString(dr["CitaHora"]),
                                    tramite = Convert.ToString(dr["tramite"]),
                                    CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                    segmento = Convert.ToString(dr["segmento"]),
                                    sucursal = Convert.ToString(dr["sucursal"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del día no hábil."
                                }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetCitasByIdentificacion(string CitaId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_Get"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaBusqueda", CitaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                                select new Citas()
                                {
                                    CitaId                = Convert.ToInt32(dr["CitaId"]),
                                    flag_historico        = Convert.ToInt32(dr["flag_historico"]),
                                    CitaIdentificacion    = Convert.ToString(dr["CitaIdentificacion"]),
                                    CitaNombre            = Convert.ToString(dr["CitaNombre"]),
                                    CitaFecha             = Convert.ToString(dr["CitaFechaHora"]),
                                    CitaFechaHora         = Convert.ToString(dr["CitaFecha"]),
                                    CitaHora              = Convert.ToString(dr["CitaHora"]),
                                    CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                    CitaTelefonoCelular   = Convert.ToString(dr["CitaTelefonoCelular"]),
                                    CitaTelefonoCasa      = Convert.ToString(dr["CitaTelefonoCasa"]),
                                    CitaTelefonoOficina   = Convert.ToString(dr["CitaTelefonoOficina"]),
                                    TramiteId             = Convert.ToInt32(dr["TramiteId"]),
                                    tramite               = Convert.ToString(dr["tramite"]),
                                    duracion              = Convert.ToInt32(dr["duracion"]),
                                    CitaCuenta            = Decryt( Convert.ToString(dr["CitaCuenta"])),
                                    CitaTarjeta           = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                    CitaSegmentoId        = Convert.ToString(dr["CitaSegmentoId"]),
                                    segmento              = Convert.ToString(dr["segmento"]),
                                    SucursalId            = Convert.ToString(dr["SucursalId"]),
                                    sucursal              = Convert.ToString(dr["sucursal"]),
                                    PosicionId            = Convert.ToString(dr["PosicionId"]),
                                    CitaTicket            = Convert.ToString(dr["CitaTicket"]),
                                    CitaEstado            = Convert.ToString(dr["CitaEstado"]),
                                    citaVencida           = Convert.ToString(dr["citaVencida"]),
                                    EmisorId              = Convert.ToInt32(dr["EmisorId"]),
                                    EmisorCuenta          = Convert.ToString(dr["EmisorCuenta"]),
                                    Familia               = Convert.ToString(dr["Familia"]),
                                    Producto              = Convert.ToString(dr["Producto"]),
                                    MarcaId               = Convert.ToString(dr["MarcaId"]),
                                    MarcaTarjeta          = Convert.ToString(dr["MarcaTarjeta"]),
                                    Accion                = 1,
                                    Mensaje               = "Se cargó correctamente la información de la cita."
                                }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetCitasProgramadasBySurcursal(int SucursalId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_Programada_GetBySucursal"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 CitaId                 = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion     = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha              = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre             = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico  = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular    = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora               = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta    = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite                = Convert.ToString(dr["tramite"]),
                                 PosicionId             = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion    = Convert.ToString(dr["posicionDescripcion"]),
                                 PosicionInicioDescanso = Convert.ToString(dr["PosicionInicioDescanso"]),
                                 PosicionFinalDescanso  = Convert.ToString(dr["PosicionFinalDescanso"]),
                                 CitaCuenta             = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 segmento               = Convert.ToString(dr["segmento"]),
                                 sucursal               = Convert.ToString(dr["sucursal"]),
                                 CitaTicket             = Convert.ToString(dr["CitaTicket"]),
                                 PrioridadId            = Convert.ToString(dr["PrioridadId"]),
                                 itemOrden              = Convert.ToString(dr["itemOrden"]),
                                 Accion                 = 1,
                                 Mensaje                = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetRazonesByCita(int CitaId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Razon_GetByCita"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 TipoId                     = Convert.ToInt32(dr["TipoId"]),
                                 tipoRazonAbreviatura       = Convert.ToString(dr["tipoRazonAbreviatura"]),
                                 RazonId                    = Convert.ToInt32(dr["RazonId"]),
                                 RazonDescripcion           = Convert.ToString(dr["RazonDescripcion"]),
                                 RazonAbreviatura           = Convert.ToString(dr["RazonAbreviatura"]),
                                 TipoEtiquetaListadoExtra   = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                 RazonGroup                 = Convert.ToString(dr["RazonGroup"]),
                                 RazonGrupo                 = Convert.ToString(dr["RazonGrupo"]),
                                 RazonStatus                = Convert.ToString(dr["RazonStatus"]),
                                 listadoExtraDescripcion    = Convert.ToString(dr["listadoDescripcionExtraCONFIG"]),
                                 DatoExtraId                = Convert.ToString(dr["DatoExtraId"]),
                                 TipoOrigenExtraId          = Convert.ToString(dr["TipoOrigenExtraId"]),
                                 CodigoListadoOrigenExtraId = Convert.ToString(dr["CodigoListadoOrigenExtraId"]),
                                 listadoExtra               = Convert.ToString(dr["listadoExtra"]),
                                 RazonOrigen                = Convert.ToString(dr["RazonOrigen"]),
                                 Accion                     = 1,
                                 Mensaje                    = "Se cargó correctamente la información de la razón."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetRazonByTipo(int razonId, int tipoRazonId, string datoExtraId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Razon_GetByTipo"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@razonId", razonId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@tipoRazonId", tipoRazonId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@datoExtraId", datoExtraId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 TipoId                     = Convert.ToInt32(dr["TipoId"]),
                                 tipoRazonAbreviatura       = Convert.ToString(dr["tipoRazonAbreviatura"]),
                                 RazonId                    = Convert.ToInt32(dr["RazonId"]),
                                 RazonDescripcion           = Convert.ToString(dr["RazonDescripcion"]),
                                 RazonAbreviatura           = Convert.ToString(dr["RazonAbreviatura"]),
                                 TipoEtiquetaListadoExtra   = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                 RazonGroup                 = Convert.ToString(dr["RazonGroup"]),
                                 RazonGrupo                 = Convert.ToString(dr["RazonGrupo"]),
                                 RazonStatus                = Convert.ToString(dr["RazonStatus"]),
                                 listadoExtraDescripcion    = Convert.ToString(dr["listadoDescripcionExtraCONFIG"]),
                                 TipoOrigenExtraId          = Convert.ToString(dr["TipoOrigenExtraId"]),
                                 CodigoListadoOrigenExtraId = Convert.ToString(dr["CodigoListadoOrigenExtraId"]),
                                 listadoExtra               = Convert.ToString(dr["listadoExtra"]),
                                 DatoExtraId                = Convert.ToString(datoExtraId),
                                 Accion                     = 1,
                                 Mensaje                    = "Se cargó correctamente la información de la razón."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetCitasBySurcursalyEstado(int SucursalId, int estadocita)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Citas_GetBySucursalyEstado"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@estadocita", estadocita); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
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
                                 CitaHoraClienteIniciaAtencion = Convert.ToString(dr["CitaHoraClienteIniciaAtencion"]),
                                 CitaHoraClienteSaleAtencion = Convert.ToString(dr["CitaHoraClienteSaleAtencion"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 SucursalReferido = Convert.ToString(dr["SucursalReferido"]),
                                 duracion = Convert.ToInt32(dr["tiempo_en_cita"]),
                                 PromedioTiempoEspera = Convert.ToInt32(dr["tiempo_espera"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> CheckEmisorCuenta(string EmisorCuenta)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Emisor_Get"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@EmisorCuenta", EmisorCuenta);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                            select new Citas()
                            {
                                EmisorId        = Convert.ToInt32(dr["EmisorId"]),
                                EmisorCuenta    = Convert.ToString(dr["EmisorCuenta"]),
                                MarcaId         = Convert.ToString(dr["MarcaId"]),
                                MarcaTarjeta    = Convert.ToString(dr["MarcaTarjeta"]),
                                Producto        = Convert.ToString(dr["Producto"]),
                                Familia         = Convert.ToString(dr["Familia"]),
                                CitaSegmentoId  = Convert.ToString(dr["CitaSegmentoId"]),
                                Accion          = 1,
                                Mensaje         = "Se cargó correctamente la información de la cita."
                            }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "Emisor de cuenta deconocido, verifique su número de cuenta!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> CheckCitaFecha(string cubiculoIdGlobal, string fechaGlobal, int CitaIdGlobal)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_ValFecha"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@Cubiculo", cubiculoIdGlobal);
                cmd.Parameters.AddWithValue("@FechaGlobal", fechaGlobal);
                cmd.Parameters.AddWithValue("@CitaId", CitaIdGlobal);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                
                                 //Accion = 1,
                                 //Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 1;
                    //ss.Mensaje = "Emisor de cuenta deconocido, verifique su número de cuenta!";
                    CitasList.Add(ss);
                }
                if (CitasList.Count == 1 && CitaIdGlobal != -1)
                {
                    Citas ss = new Citas();
                    ss.Accion = 1;
                    CitasList.Add(ss);
                }
                else 
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public Citas deleteRazon(int citaId, int tipoRazonId, int razonId, string datoExtraId)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_CitaRazon_Delete");
                cmd.Parameters.AddWithValue("@citaId", citaId);
                cmd.Parameters.AddWithValue("@tipoRazonId", tipoRazonId);
                cmd.Parameters.AddWithValue("@razonId", razonId);
                cmd.Parameters.AddWithValue("@datoExtraId", datoExtraId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito la razón!";
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas Clientecancela(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_Clientecancela");
                cmd.Parameters.AddWithValue("@CitaId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se registró que cliente cancela cita!";
                    vResultado.CitaId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<Citas> GetTiempoEspera_CentrosAtencion()
        {
            List<Citas> List = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Reporte_GetTiempoEspera_CentrosAtencion"); //Pasamos el procedimiento almacenado. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                List = (from DataRow dr in dt.Rows
                        select new Citas()
                        {
                            sucursal            = Convert.ToString(dr["sucursal"]),
                            NumeroClientes      = Convert.ToInt32(dr["NumeroClientes"]),
                            SumaTiempoEspera    = Convert.ToInt32(dr["SumaTiempoEspera"]),
                            MaximoTiempoEspera  = Convert.ToInt32(dr["MaximoTiempoEspera"]),
                            MinimoTiempoEspera  = Convert.ToInt32(dr["MinimoTiempoEspera"]),
                            PromedioTiempoEspera = Convert.ToInt32(dr["PromedioTiempoEspera"]),
                            Accion = 1,
                            Mensaje = "Se cargó correctamente la información."
                        }).ToList();
                if (List.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    List.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                List.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return List;
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
