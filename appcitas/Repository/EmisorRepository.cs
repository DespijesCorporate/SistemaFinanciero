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
    public class EmisorRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Emisores Save(Emisores pEmisor)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Emisor_Save");
                cmd.Parameters.AddWithValue("@EmisorId", pEmisor.EmisorId);
                cmd.Parameters["@EmisorId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@EmisorCuenta", pEmisor.EmisorCuenta);
                cmd.Parameters.AddWithValue("@MarcaId", pEmisor.MarcaId);
                cmd.Parameters.AddWithValue("@Producto", pEmisor.Producto);
                cmd.Parameters.AddWithValue("@Familia", pEmisor.Familia);
                cmd.Parameters.AddWithValue("@SegmentoId", pEmisor.SegmentoId);
                cmd.Parameters.AddWithValue("@EmisorStatus", pEmisor.EmisorStatus);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@EmisorId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pEmisor.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar el emisor.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pEmisor.Accion = vResultado;
            if (vResultado == 0)
            {
                pEmisor.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pEmisor.Mensaje = "Emisor ingresado correctamente!";
            }
            return pEmisor;
        }

        public List<Emisores> GetEmisores()
        {
            List<Emisores> EmisoresList = new List<Emisores>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Feriado_Get"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetFeriado"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                EmisoresList = (from DataRow dr in dt.Rows
                                select new Emisores()
                                {
                                    EmisorId        = Convert.ToInt32(dr["EmisorId"]),
                                    EmisorCuenta    = Convert.ToString(dr["EmisorCuenta"]),
                                    MarcaId         = Convert.ToString(dr["MarcaId"]),
                                    MarcaTarjeta    = Convert.ToString(dr["MarcaTarjeta"]),
                                    Producto        = Convert.ToString(dr["Producto"]),
                                    Familia         = Convert.ToString(dr["Familia"]),
                                    SegmentoId      = Convert.ToString(dr["SegmentoId"]),
                                    Segmento        = Convert.ToString(dr["Segmento"]),
                                    EmisorStatus    = Convert.ToString(dr["EmisorStatus"]),
                                    EmisorStatusDescripcion = Convert.ToString(dr["EmisorStatusDescripcion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del día no hábil."
                                }).ToList();
                if (EmisoresList.Count == 0)
                {
                    Emisores ss = new Emisores();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    EmisoresList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Emisores oFeriado = new Emisores();
                oFeriado.Accion = 0;
                oFeriado.Mensaje = ex.Message.ToString();
                EmisoresList.Add(oFeriado);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EmisoresList;
        }

        public List<Emisores> GetEmisoresByMarca(string MarcaId)
        {
            List<Emisores> EmisoresList = new List<Emisores>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Emisor_GetByMarca"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@MarcaId", MarcaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetFeriado"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                EmisoresList = (from DataRow dr in dt.Rows
                                select new Emisores()
                                {
                                    EmisorId        = Convert.ToInt32(dr["EmisorId"]),
                                    EmisorCuenta    = Convert.ToString(dr["EmisorCuenta"]),
                                    MarcaId         = Convert.ToString(dr["MarcaId"]),
                                    MarcaTarjeta    = Convert.ToString(dr["MarcaTarjeta"]),
                                    Producto        = Convert.ToString(dr["Producto"]),
                                    Familia         = Convert.ToString(dr["Familia"]),
                                    SegmentoId      = Convert.ToString(dr["SegmentoId"]),
                                    Segmento        = Convert.ToString(dr["Segmento"]),
                                    EmisorStatus    = Convert.ToString(dr["EmisorStatus"]),
                                    EmisorStatusDescripcion = Convert.ToString(dr["EmisorStatusDescripcion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del emisor."
                                }).ToList();
                if (EmisoresList.Count == 0)
                {
                    Emisores ss = new Emisores();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    EmisoresList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Emisores oFeriado = new Emisores();
                oFeriado.Accion = 0;
                oFeriado.Mensaje = ex.Message.ToString();
                EmisoresList.Add(oFeriado);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EmisoresList;
        }

        public Emisores GetEmisorById(int Id)
        {
            Emisores vResultado = new Emisores(); //Se crea una variable que contendra los datos del feriado.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Emisor_GetById"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@EmisorId", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.EmisorId         = (int)consulta["EmisorId"];
                        vResultado.EmisorCuenta     = (string)consulta["EmisorCuenta"];
                        vResultado.MarcaId          = (string)consulta["MarcaId"];
                        vResultado.MarcaTarjeta     = (string)consulta["MarcaTarjeta"];
                        vResultado.Producto         = (string)consulta["Producto"];
                        vResultado.Familia          = (string)consulta["Familia"];
                        vResultado.SegmentoId       = (string)consulta["SegmentoId"];
                        vResultado.Segmento         = (string)consulta["Segmento"];
                        vResultado.EmisorStatus     = (string)consulta["EmisorStatus"];
                        vResultado.EmisorStatusDescripcion = (string)consulta["EmisorStatusDescripcion"]; 
                        vResultado.Accion           = 1;
                        vResultado.Mensaje          = "Se cargó correctamente el emisor!";
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Hubo un inconveniente al cargar la información: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Emisores CheckEmisor(string EmisorCuenta)
        {
            Emisores vResultado = new Emisores(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Emisor_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@EmisorCuenta", EmisorCuenta); //Agregamos los parametros.

                AbrirConexion(); /*Se abre la conexion a la BD.*/
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el emisor!";
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Hubo un inconveniente al cargar la información: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Emisores Del(int EmisorId)
        {
            SqlCommand cmd = new SqlCommand();
            Emisores vResultado = new Emisores();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Emisor_Delete");
                cmd.Parameters.AddWithValue("@EmisorId", EmisorId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con éxito el emisor!";
                    vResultado.EmisorId = EmisorId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.EmisorId = EmisorId;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
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