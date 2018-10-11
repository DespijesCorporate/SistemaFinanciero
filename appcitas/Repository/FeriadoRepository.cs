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
    public class FeriadoRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Feriados Save(Feriados pFeriado)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Feriado_Save");
                cmd.Parameters.AddWithValue("@FeriadoId", pFeriado.FeriadoId);
                cmd.Parameters["@FeriadoId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@FeriadoFecha", pFeriado.FeriadoFecha);
                cmd.Parameters.AddWithValue("@FeriadoDescripcion", pFeriado.FeriadoDescripcion);
                cmd.Parameters.AddWithValue("@FeriadoTipoId", pFeriado.FeriadoTipoId);
                cmd.Parameters.AddWithValue("@FeriadoHoraInicio", pFeriado.FeriadoHoraInicio);
                cmd.Parameters.AddWithValue("@FeriadoHoraFinal", pFeriado.FeriadoHoraFinal);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@FeriadoId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pFeriado.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar el día no hábil.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pFeriado.Accion = vResultado;
            if (vResultado == 0)
            {
                pFeriado.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                pFeriado.Mensaje = "Día no hábil ingresado correctamente!";
            }
            return pFeriado;
        }

        public List<Feriados> GetFeriados()
        {
            List<Feriados> FeriadosList = new List<Feriados>();
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
                FeriadosList = (from DataRow dr in dt.Rows
                                select new Feriados()
                                {
                                    FeriadoId = Convert.ToInt32(dr["FeriadoId"]),
                                    FeriadoFecha = Convert.ToString(dr["FeriadoFecha"]),
                                    FeriadoFechaFormato = Convert.ToString(dr["FeriadoFechaFormato"]),
                                    FeriadoFechaNum = Convert.ToInt32(dr["FeriadoFechaNum"]),
                                    FeriadoDescripcion = Convert.ToString(dr["FeriadoDescripcion"]),
                                    FeriadoTipoId = Convert.ToString(dr["FeriadoTipoId"]),
                                    FeriadoTipoDescripcion = Convert.ToString(dr["FeriadoTipoDescripcion"]),
                                    FeriadoHoraInicio = Convert.ToString(dr["FeriadoHoraInicio"]),
                                    FeriadoHoraFinal = Convert.ToString(dr["FeriadoHoraFinal"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del día no hábil."
                                }).ToList();
                if (FeriadosList.Count == 0)
                {
                    Feriados ss = new Feriados();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    FeriadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Feriados oFeriado = new Feriados();
                oFeriado.Accion = 0;
                oFeriado.Mensaje = ex.Message.ToString();
                FeriadosList.Add(oFeriado);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return FeriadosList;
        }

        public List<Feriados> GetFeriadosByYear(int Year)
        {
            List<Feriados> FeriadosList = new List<Feriados>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Feriado_Get"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@FeriadoAnio", Year); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetFeriado"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                FeriadosList = (from DataRow dr in dt.Rows
                                select new Feriados()
                                {
                                    FeriadoId = Convert.ToInt32(dr["FeriadoId"]),
                                    FeriadoFecha = Convert.ToString(dr["FeriadoFecha"]),
                                    FeriadoFechaFormato = Convert.ToString(dr["FeriadoFechaFormato"]),
                                    FeriadoFechaNum = Convert.ToInt32(dr["FeriadoFechaNum"]),
                                    FeriadoDescripcion = Convert.ToString(dr["FeriadoDescripcion"]),
                                    FeriadoTipoId = Convert.ToString(dr["FeriadoTipoId"]),
                                    FeriadoTipoDescripcion = Convert.ToString(dr["FeriadoTipoDescripcion"]),
                                    FeriadoHoraInicio = Convert.ToString(dr["FeriadoHoraInicio"]),
                                    FeriadoHoraFinal = Convert.ToString(dr["FeriadoHoraFinal"]),
                                    Accion = 1,
                                    Mensaje = "Se cargó correctamente la información del día no hábil."
                                }).ToList();
                if (FeriadosList.Count == 0)
                {
                    Feriados ss = new Feriados();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    FeriadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Feriados oFeriado = new Feriados();
                oFeriado.Accion = 0;
                oFeriado.Mensaje = ex.Message.ToString();
                FeriadosList.Add(oFeriado);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return FeriadosList;
        }

        public List<Feriados> GetAniosFeriado()
        {
            List<Feriados> FeriadoList = new List<Feriados>();
            Feriados oCongif = new Feriados();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Feriado_Get_years"); //Pasamos el procedimiento almacenado.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                FeriadoList = (from DataRow dr in dt.Rows

                              select new Feriados()
                              {
                                  FeriadoAnio = Convert.ToInt32(dr["FeriadoAnio"]),
                                  Accion = 1,
                                  Mensaje = "Se cargaron correctamente los años!"
                              }).ToList();

                if (FeriadoList.Count == 0)
                {
                    oCongif.Accion = 0;
                    oCongif.Mensaje = "No se encontraron registros!";
                    FeriadoList.Add(oCongif);
                }
            }

            catch (Exception ex)
            {
                oCongif.Accion = 0;
                oCongif.Mensaje = ex.Message.ToString();
                FeriadoList.Add(oCongif);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return FeriadoList;
        }

        public Feriados GetFeriado(int Id)
        {
            Feriados vResultado = new Feriados(); //Se crea una variable que contendra los datos del feriado.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Feriado_Get"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@FeriadoId", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.FeriadoId = (int)consulta["FeriadoId"];
                        vResultado.FeriadoFecha = (string)consulta["FeriadoFecha"];
                        vResultado.FeriadoFechaFormato = (string)consulta["FeriadoFechaFormato"];
                        vResultado.FeriadoFechaNum = (int)consulta["FeriadoFechaNum"];
                        vResultado.FeriadoDescripcion = (string)consulta["FeriadoDescripcion"];
                        vResultado.FeriadoTipoId = (string)consulta["FeriadoTipoId"];
                        vResultado.FeriadoTipoDescripcion = (string)consulta["FeriadoTipoDescripcion"];
                        vResultado.FeriadoHoraInicio = consulta["FeriadoHoraInicio"] == null ? "Todo el día" : (string)consulta["FeriadoHoraInicio"];
                        vResultado.FeriadoHoraFinal = consulta["FeriadoHoraFinal"] == null ? "Todo el día" : (string)consulta["FeriadoHoraFinal"]; 
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el día no hábil!";
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

        public Feriados CheckFeriado(string fecha)
        {
            Feriados vResultado = new Feriados(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Feriado_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@FeriadoFecha", fecha); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el feriado!";

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

        public Feriados Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Feriados vResultado = new Feriados();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Feriado_Delete");
                cmd.Parameters.AddWithValue("@FeriadoId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito el día no hábil!";
                    vResultado.FeriadoId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.FeriadoId = Id;
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