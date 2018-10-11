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
    public class CajaRepository : CAD
    {
        //LISTA DE Tipos de Transacciones
        public List<OtrosTiposTransacciones> ListaTiposTrans()
        {
            List<OtrosTiposTransacciones> EstadosList = new List<OtrosTiposTransacciones>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SF_SP_ListaTiposTransacciones"); //Pasamos el procedimiento almacenado.  
                //cmd.Parameters.AddWithValue("@atencion", sucursalAtencion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                EstadosList = (from DataRow dr in dt.Rows

                               select new OtrosTiposTransacciones()
                               {
                                   OTT_Descripcion = Convert.ToString(dr["OTT_Descripcion"]),
                                   OTT_Codigo = Convert.ToInt32(dr["OTT_Codigo"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos de las Tipos de Transacciones"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    OtrosTiposTransacciones ss = new OtrosTiposTransacciones();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tipos de Transacciones!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                OtrosTiposTransacciones oSucursal = new OtrosTiposTransacciones();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA PROCESO DE CARGAR LISTA DE ESTADOS

        //Get datos de pago a prestamo
        public List<GetDatosPagoPrestamo> PagoPrestamo(int id)
        {
            List<GetDatosPagoPrestamo> EstadosList = new List<GetDatosPagoPrestamo>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GetDatosPagoPrestamo"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@CodPrest", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                EstadosList = (from DataRow dr in dt.Rows

                               select new GetDatosPagoPrestamo()
                               {
                                   Prestamo = Convert.ToInt32(dr["Codigo_Prestamo"]),
                                   Capital = Convert.ToString(dr["Capital"]),
                                   Intereses = Convert.ToString(dr["Intereses"]),
                                   Mora = Convert.ToString(dr["Mora"]),
                                   //Total = Convert.ToDecimal(dr["Total"]),
                                   Saldo = Convert.ToDecimal(dr["Saldo"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos del pago de prestamo"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    GetDatosPagoPrestamo ss = new GetDatosPagoPrestamo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se pudo calcular información sobre pago del prestamo!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                GetDatosPagoPrestamo oSucursal = new GetDatosPagoPrestamo();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA PROCESO de pago de prestamo

        //Genera pago a Prestamo
        public MensajesResultado PagoPrestamo(int codigo, string Capital, string Intereses, string Mora, int TipoPago, string Cajero, string Observacion)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = codigo;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_PagoPrestamo");
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@Capital", Convert.ToDecimal(Capital));
                cmd.Parameters.AddWithValue("@Intereses", Convert.ToDecimal(Intereses));
                cmd.Parameters.AddWithValue("@Mora", Convert.ToDecimal(Mora));
                cmd.Parameters.AddWithValue("@TipoPago", TipoPago);
                cmd.Parameters.AddWithValue("@Agrego", Cajero);
                cmd.Parameters.AddWithValue("@Observacion", Observacion);
                cmd.Parameters.AddWithValue("@Recibo", ParameterDirection.InputOutput);
                //cmd.Parameters["@Recibo"].Direction = ParameterDirection.InputOutput;

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                vControl = Convert.ToInt32(cmd.Parameters["@Recibo"].Value);

                if (vControl > 0)
                {

                    //if (a == 1)
                    //{
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Pago Realizado Exitosamente!";
                        vResultado.Num = vControl;
                        
                    //}
                    //else
                    //{
                        //vResultado.Estado = 2;
                        //vResultado.Mensaje = " ";
                    //}
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El pago no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Genera otros Pagos
        public MensajesResultado OtrosPagos(decimal Monto, string Observacion, string Cajero, int ListaTiposTrans, string Clave)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_OtrosPagos");
                cmd.Parameters.AddWithValue("@Monto", Monto);
                cmd.Parameters.AddWithValue("@Observacion", Observacion);
                cmd.Parameters.AddWithValue("@Agrego", Cajero);
                cmd.Parameters.AddWithValue("@Tipo_trans", ListaTiposTrans);
                cmd.Parameters.AddWithValue("@Clave", Clave);
                cmd.Parameters.AddWithValue("@secuencia",1);
                cmd.Parameters["@secuencia"].Direction = ParameterDirection.InputOutput;


                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                vControl = Convert.ToInt32(cmd.Parameters["@secuencia"].Value);

                if (vControl > 0)
                {

                    if (a == 1)
                    {
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Pago Realizado Exitosamente!";
                        vResultado.Num = vControl;
                    }
                    else
                    {
                        //vResultado.Estado = 2;
                        //vResultado.Mensaje = " ";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El pago no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Calcular datos de cajero
        public List<Cajero> GetDatosCajero(string Cajero, string fecha)
        {
            List<Cajero> EstadosList = new List<Cajero>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_Get_Reg_Cajero"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@cajero", Cajero);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                EstadosList = (from DataRow dr in dt.Rows

                               select new Cajero()
                               {
                                   CA_Codigo_Cajero = Convert.ToString(dr["CA_Codigo_Cajero"]),
                                   CA_Fecha = Convert.ToString(dr["CA_Fecha"]),
                                   CA_Valor_Inicial_Dia = Convert.ToDecimal(dr["CA_Valor_Inicial_Dia"]),
                                   CA_Valor_Entrega = Convert.ToDecimal(dr["CA_Valor_Entrega"]),
                                   CA_Valor_Recib = Convert.ToDecimal(dr["CA_Valor_Recib"]),
                                   CA_Valor_Entrega_Supervisor = Convert.ToDecimal(dr["CA_Valor_Entrega_Supervisor"]),
                                   CA_Valor_Recib_Supervisor = Convert.ToDecimal(dr["CA_Valor_Recib_Supervisor"]),
                                   SaldoCaja = Convert.ToDecimal(dr["SaldoCaja"]),
                                   CA_B_1 = Convert.ToInt32(dr["CA_B_1"]),
                                   CA_B_2 = Convert.ToInt32(dr["CA_B_2"]),
                                   CA_B_5 = Convert.ToInt32(dr["CA_B_5"]),
                                   CA_B_10 = Convert.ToInt32(dr["CA_B_10"]),
                                   CA_B_20 = Convert.ToInt32(dr["CA_B_20"]),
                                   CA_B_50 = Convert.ToInt32(dr["CA_B_50"]),
                                   CA_B_100 = Convert.ToInt32(dr["CA_B_100"]),
                                   CA_B_500 = Convert.ToInt32(dr["CA_B_500"]),
                                   CA_M_1 = Convert.ToInt32(dr["CA_M_1"]),
                                   CA_M_2 = Convert.ToInt32(dr["CA_M_2"]),
                                   CA_M_5 = Convert.ToInt32(dr["CA_M_5"]),
                                   CA_M_10 = Convert.ToInt32(dr["CA_M_10"]),
                                   CA_M_20 = Convert.ToInt32(dr["CA_M_20"]),
                                   CA_M_50 = Convert.ToInt32(dr["CA_M_50"]),
                                   CA_Cajero_Estado = Convert.ToInt32(dr["CA_Cajero_Estado"]),
                                   CA_Secuencia = Convert.ToInt32(dr["CA_Secuencia"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos del pago de prestamo"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    Cajero ss = new Cajero();
                    ss.Accion = 0;
                    ss.Mensaje = "No se pudo calcular información sobre pago del prestamo!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cajero oSucursal = new Cajero();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA PROCESO de pago de prestamo

        //Carga transacciones de cajero
        public List<Transac_Cajero> GetTransaccionesCajero(string Cajero, string fecha)
        {
            List<Transac_Cajero> EstadosList = new List<Transac_Cajero>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_Get_Transac_Cajero"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@cajero", Cajero);
                cmd.Parameters.AddWithValue("@fecha", fecha);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                EstadosList = (from DataRow dr in dt.Rows

                               select new Transac_Cajero()
                               {
                                   TC_Numero = Convert.ToInt32(dr["TC_Numero"]),
                                   TC_Cajero = Convert.ToString(dr["TC_Cajero"]),
                                   TC_Fecha = Convert.ToString(dr["TC_Fecha"]),
                                   TC_Mto_Entrega = Convert.ToDecimal(dr["TC_Mto_Entrega"]),
                                   TC_Mto_Recib = Convert.ToDecimal(dr["TC_Mto_Recib"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos de transacciones de cajero"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    Transac_Cajero ss = new Transac_Cajero();
                    ss.Accion = 0;
                    ss.Mensaje = "No se pudo calcular información sobre transacciones de cajero!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Transac_Cajero oSucursal = new Transac_Cajero();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA

        //Genera Transaccion de Cajero
        public MensajesResultado GeneraTransCajero(decimal Monto, int Tipo, string Cajero)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_Genera_Trans_Cajero");
                cmd.Parameters.AddWithValue("@Monto", Monto);
                cmd.Parameters.AddWithValue("@Tipo", Tipo);
                cmd.Parameters.AddWithValue("@Cajero", Cajero);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {

                    if (a == 1)
                    {
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Transacción Realizado Exitosamente!";
                    }
                    else
                    {
                        //vResultado.Estado = 2;
                        //vResultado.Mensaje = " ";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("La Transacción no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Genera Transaccion de Cajero
        public MensajesResultado AgregaRegCajero(string Cajero)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_Agrega_Reg_Cajero");
                cmd.Parameters.AddWithValue("@Cajero", Cajero);
                cmd.Parameters.AddWithValue("@Respuesta", 0);
                cmd.Parameters["@Respuesta"].Direction = ParameterDirection.InputOutput;

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                vControl = Convert.ToInt32(cmd.Parameters["@Respuesta"].Value);

                if (vControl > 0)
                {

                    if (vControl == 1)
                    {
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Registro Agregado para la fecha Actual!";
                    }
                    else if (vControl == 0)
                    {
                        vResultado.Estado = 0;
                        vResultado.Mensaje = "El Registro no pudo ser agregado ";
                    }
                    else if (vControl == 2)
                    {
                        vResultado.Estado = 2;
                        vResultado.Mensaje = "Ya Existe un Registro creado para este dia, Proceso cancelado.";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El registro no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Recibos
        public List<Recibos> GetRecibos()
        {
            List<Recibos> EstadosList = new List<Recibos>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_GetRecibos"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                EstadosList = (from DataRow dr in dt.Rows

                               select new Recibos()
                               {
                                   RE_Numero = Convert.ToInt32(dr["RE_Numero"]),
                                   RE_Fecha = Convert.ToString(dr["RE_Fecha"]),
                                   RE_Observacion = Convert.ToString(dr["RE_Observacion"]),
                                   RE_Total_Rec = Convert.ToDecimal(dr["RE_Total_Rec"]),
                                   RE_agrego = Convert.ToString(dr["RE_agrego"]),
                                   RE_Documento = Convert.ToInt32(dr["RE_Documento"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los Recibos"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    Recibos ss = new Recibos();
                    ss.Accion = 0;
                    ss.Mensaje = "No se pudieron cargar recibos!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Recibos oSucursal = new Recibos();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA

        public MensajesResultado AnularRecibo(int Documento)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_AnulaRecibo");
                cmd.Parameters.AddWithValue("@Documento", Documento);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {

                    if (a == 1)
                    {
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Recibo Anulado!";
                    }
                    else if (a == 0)
                    {
                        vResultado.Estado = 0;
                        vResultado.Mensaje = "El Recibo no pudo ser anulado.";
                    }
                    else if (a == 2)
                    {
                        vResultado.Estado = 2;
                        vResultado.Mensaje = "Ya Existe un Registro creado para este dia";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El registro no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Guarda datos para Arqueo
        public MensajesResultado GuardarDatosArqueo(int CA_Secuencia, int CA_B_1, int CA_B_2, int CA_B_5, int CA_B_10, int CA_B_20, int CA_B_50, int CA_B_100, int CA_B_500, int CA_M_1, int CA_M_2, int CA_M_5, int CA_M_10, int CA_M_20, int CA_M_50)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_GuardaDatosArqueo");
                cmd.Parameters.AddWithValue("@CA_Secuencia", CA_Secuencia);
                cmd.Parameters.AddWithValue("@CA_B_1", CA_B_1);
                cmd.Parameters.AddWithValue("@CA_B_2", CA_B_2);
                cmd.Parameters.AddWithValue("@CA_B_5", CA_B_5);
                cmd.Parameters.AddWithValue("@CA_B_10", CA_B_10);
                cmd.Parameters.AddWithValue("@CA_B_20", CA_B_20);
                cmd.Parameters.AddWithValue("@CA_B_50", CA_B_50);
                cmd.Parameters.AddWithValue("@CA_B_100", CA_B_100);
                cmd.Parameters.AddWithValue("@CA_B_500", CA_B_500);
                cmd.Parameters.AddWithValue("@CA_M_1", CA_M_1);
                cmd.Parameters.AddWithValue("@CA_M_2", CA_M_2);
                cmd.Parameters.AddWithValue("@CA_M_5", CA_M_5);
                cmd.Parameters.AddWithValue("@CA_M_10", CA_M_10);
                cmd.Parameters.AddWithValue("@CA_M_20", CA_M_20);
                cmd.Parameters.AddWithValue("@CA_M_50", CA_M_50);
                
                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {

                    if (a == 1)
                    {
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Datos guardados exitosamente!";
                    }
                    else
                    {
                        //vResultado.Estado = 2;
                        //vResultado.Mensaje = " ";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El pago no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Cierra Caja
        public MensajesResultado CierreCaja(int Secuencia, float Diferencia)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_CierreCaja");
                cmd.Parameters.AddWithValue("@Secuencia", Secuencia);
                cmd.Parameters.AddWithValue("@Diferencia", Diferencia);
                

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {

                    //if (a == 1)
                    //{
                        vResultado.Estado = 1;
                        vResultado.Mensaje = "Cierre de Caja Realizado Exitosamente!";
                    //}
                    //else
                    //{
                        //vResultado.Estado = 2;
                        //vResultado.Mensaje = " ";
                    //}
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El Cierre no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        //Cierra Caja
        public MensajesResultado ValidaCajero(string Cajero)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = 1;
            int vControl = -1;
            try
            {
                cmd = CrearComando("SF_SP_ValidaRegCajero");
                cmd.Parameters.AddWithValue("@Cajero", Cajero);
                cmd.Parameters.AddWithValue("@Alerta", 0);
                cmd.Parameters["@Alerta"].Direction = ParameterDirection.InputOutput;

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                vControl = Convert.ToInt32(cmd.Parameters["@Alerta"].Value);

                if (vControl > 0)
                {

                    if (vControl == 1)
                    {
                    vResultado.Estado = 1;
                    vResultado.Mensaje = "No Puede Agregar Transacciones cuando el Cajero ya esta cerrado.";
                    }
                    else
                    {
                        vResultado.Estado = 2;
                        vResultado.Mensaje = "No a agregado su registro de caja para este dia, proceso cancelado.";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1)
                {
                    throw new Exception("El Cierre no pudo ser realizado por el siguiente error: " + ex.Message, ex);
                }
                else
                {
                    //throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }
    }
}