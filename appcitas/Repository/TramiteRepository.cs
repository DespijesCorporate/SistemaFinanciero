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
    public class TramiteRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);
        }

        public Tramites Save(Tramites pTramite)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Tramite_Save");
                cmd.Parameters.AddWithValue("@TramiteId", pTramite.TramiteId);
                cmd.Parameters["@TramiteId"].Direction = ParameterDirection.InputOutput; //Se indica que el TramiteId sera un parametro de Entrada/Salida.
                cmd.Parameters.AddWithValue("@TramiteDescripcion", pTramite.TramiteDescripcion);
                cmd.Parameters.AddWithValue("@TramiteAbreviatura", pTramite.TramiteAbreviatura);
                cmd.Parameters.AddWithValue("@TramiteDuracion", pTramite.TramiteDuracion);
                cmd.Parameters.AddWithValue("@TramiteAlertaPrevia", pTramite.TramiteAlertaPrevia);
                cmd.Parameters.AddWithValue("@TramiteToleranciaAlCliente", pTramite.TramiteToleranciaAlCliente);
                cmd.Parameters.AddWithValue("@TramiteToleranciaDelCliente", pTramite.TramiteToleranciaDelCliente);
                cmd.Parameters.AddWithValue("@TramiteToleranciaFinalizacion", pTramite.TramiteToleranciaFinalizacion);
                cmd.Parameters.AddWithValue("@TramiteTiempoMuerto", pTramite.TramiteTiempoMuerto);
                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@TramiteId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pTramite.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar el trámite.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pTramite.Accion = vResultado;
            if (vResultado == 0)
            {
                pTramite.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pTramite.Mensaje = "Trámite ingresado correctamente!";
            }
            return pTramite;
        }

        public List<Tramites> GetTramites()
        {
            List<Tramites> TramitesList = new List<Tramites>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Tramite_Get"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                TramitesList = (from DataRow dr in dt.Rows
                                select new Tramites()
                                {
                                    TramiteId = Convert.ToInt32(dr["TramiteId"]),
                                    TramiteDescripcion = Convert.ToString(dr["TramiteDescripcion"]),
                                    TramiteAbreviatura = Convert.ToString(dr["TramiteAbreviatura"]),
                                    TramiteDuracion = Convert.ToInt32(dr["TramiteDuracion"]),
                                    TramiteAlertaPrevia = Convert.ToInt32(dr["TramiteAlertaPrevia"]),
                                    TramiteToleranciaAlCliente = Convert.ToInt32(dr["TramiteToleranciaAlCliente"]),
                                    TramiteToleranciaDelCliente = Convert.ToInt32(dr["TramiteToleranciaDelCliente"]),
                                    TramiteToleranciaFinalizacion = Convert.ToInt32(dr["TramiteToleranciaFinalizacion"]),
                                    TramiteTiempoMuerto = Convert.ToInt32(dr["TramiteTiempoMuerto"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del trámite."
                                }).ToList();
                if (TramitesList.Count == 0)
                {
                    Tramites ss = new Tramites();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tramites!";
                    TramitesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Tramites oTramite = new Tramites();
                oTramite.Accion = 0;
                oTramite.Mensaje = ex.Message.ToString();
                TramitesList.Add(oTramite);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return TramitesList;
        }

        public List<Tramites> GetTramitesComboBox()
        {
            List<Tramites> TramitesList = new List<Tramites>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Tramite_Get_ComboBox"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                TramitesList = (from DataRow dr in dt.Rows
                                select new Tramites()
                                {
                                    TramiteId = Convert.ToInt32(dr["TramiteId"]),
                                    TramiteDescripcion = Convert.ToString(dr["TramiteDescripcion"]),
                                    TramiteAbreviatura = Convert.ToString(dr["TramiteAbreviatura"]),
                                    TramiteDuracion = Convert.ToInt32(dr["TramiteDuracion"]),
                                    TramiteAlertaPrevia = Convert.ToInt32(dr["TramiteAlertaPrevia"]),
                                    TramiteToleranciaAlCliente = Convert.ToInt32(dr["TramiteToleranciaAlCliente"]),
                                    TramiteToleranciaDelCliente = Convert.ToInt32(dr["TramiteToleranciaDelCliente"]),
                                    TramiteToleranciaFinalizacion = Convert.ToInt32(dr["TramiteToleranciaFinalizacion"]),
                                    TramiteTiempoMuerto = Convert.ToInt32(dr["TramiteTiempoMuerto"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del trámite."
                                }).ToList();
                if (TramitesList.Count == 0)
                {
                    Tramites ss = new Tramites();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tramites!";
                    TramitesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Tramites oTramite = new Tramites();
                oTramite.Accion = 0;
                oTramite.Mensaje = ex.Message.ToString();
                TramitesList.Add(oTramite);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return TramitesList;
        }

        public Tramites GetTramite(int Id)
        {
            Tramites vResultado = new Tramites(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Tramite_Get"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TramiteId", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TramiteId = (int)consulta["TramiteId"];
                        vResultado.TramiteDescripcion = (string)consulta["TramiteDescripcion"];
                        vResultado.TramiteAbreviatura = (string)consulta["TramiteAbreviatura"];
                        vResultado.TramiteDuracion = (int)consulta["TramiteDuracion"];
                        vResultado.TramiteAlertaPrevia = (int)consulta["TramiteAlertaPrevia"];
                        vResultado.TramiteToleranciaAlCliente = (int)consulta["TramiteToleranciaAlCliente"];
                        vResultado.TramiteToleranciaDelCliente = (int)consulta["TramiteToleranciaDelCliente"];
                        vResultado.TramiteToleranciaFinalizacion = (int)consulta["TramiteToleranciaFinalizacion"];
                        vResultado.TramiteTiempoMuerto = (int)consulta["TramiteTiempoMuerto"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el Trámite!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
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

        public Tramites CheckTramite(string descripcion, string abreviatura)
        {
            Tramites vResultado = new Tramites(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Tramite_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TramiteDescripcion", descripcion); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@TramiteAbreviatura", abreviatura); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el Trámite!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
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

        public Tramites Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Tramites vResultado = new Tramites();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Tramite_Delete");
                cmd.Parameters.AddWithValue("@TramiteId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito el trámite!";
                    vResultado.TramiteId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.TramiteId = Id;
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