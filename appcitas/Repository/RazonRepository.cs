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
    public class RazonRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Razones Save(Razones pRazon)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Razon_Save");
                cmd.Parameters.AddWithValue("@TipoId", pRazon.TipoId);
                cmd.Parameters.AddWithValue("@RazonId", pRazon.RazonId);
                cmd.Parameters["@RazonId"].Direction = ParameterDirection.InputOutput; //Se indica que el TipoId sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@RazonDescripcion", pRazon.RazonDescripcion);
                cmd.Parameters.AddWithValue("@RazonAbreviatura", pRazon.RazonAbreviatura);
                cmd.Parameters.AddWithValue("@RazonGroup", pRazon.RazonGroup);
                cmd.Parameters.AddWithValue("@RazonStatus", pRazon.RazonStatus);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@RazonId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pRazon.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pRazon.Accion = vResultado;
            if (vResultado == 0)
            {
                pRazon.Mensaje = "Se genero un error al insertar la información de la razon!";
            }
            else
            {
                pRazon.Mensaje = "Se ingreso la razon correctamente!";
            }
            return pRazon;
        }

        public List<Razones> GetRazones()
        {
            List<Razones> RazonesList = new List<Razones>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetRazon"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                RazonesList = (from DataRow dr in dt.Rows

                                   select new Razones()
                                   {
                                       TipoId = Convert.ToInt32(dr["TipoId"]),
                                       RazonId = Convert.ToInt32(dr["RazonId"]),
                                       RazonDescripcion = Convert.ToString(dr["RazonDescripcion"]),
                                       RazonAbreviatura = Convert.ToString(dr["RazonAbreviatura"]),
                                       TipoAbreviatura = Convert.ToString(dr["TipoAbreviatura"]),
                                       RazonGroup = Convert.ToString(dr["RazonGroup"]),
                                       RazonStatus = Convert.ToString(dr["RazonStatus"]),
                                       ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                       Accion = 1,
                                       Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                   }).ToList();
                if (RazonesList.Count == 0)
                {
                    Razones ss = new Razones();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Razones!";
                    RazonesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Razones oRazon = new Razones();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                RazonesList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return RazonesList;
        }

        public Razones GetRazon(int Id, int TipoId)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetRazon"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@RazonId", Id);
                cmd.Parameters.AddWithValue("@TipoId", TipoId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TipoId = Convert.ToInt32(consulta["TipoId"]);
                        vResultado.RazonId = Convert.ToInt32(consulta["RazonId"]);
                        vResultado.RazonAbreviatura = (string)consulta["RazonAbreviatura"];
                        vResultado.RazonDescripcion = (string)consulta["RazonDescripcion"];
                        vResultado.TipoAbreviatura = (string)consulta["TipoAbreviatura"];
                        vResultado.RazonGroup = (string)consulta["RazonGroup"];
                        vResultado.RazonStatus = (string)consulta["RazonStatus"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el tipo de razon correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }


        public Razones CheckRazon(string descripcion, string abreviatura)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Razon_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@RazonDescripcion", descripcion); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@RazonAbreviatura", abreviatura); //Agregamos los parametros.

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

        public Razones Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Razones vResultado = new Razones();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelRazon");
                cmd.Parameters.AddWithValue("@RazonId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el tipo de razon!";
                    vResultado.TipoId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.TipoId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Razones GetAllByTipo2(int TipoId)
        {
            Razones vResultado = new Razones(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetRazonByTipoId"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@TipoId", TipoId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.TipoId = Convert.ToInt32(consulta["TipoId"]);
                        vResultado.RazonId = Convert.ToInt32(consulta["RazonId"]);
                        vResultado.RazonAbreviatura = (string)consulta["RazonAbreviatura"];
                        vResultado.RazonDescripcion = (string)consulta["RazonDescripcion"];
                        vResultado.TipoAbreviatura = (string)consulta["TipoAbreviatura"];
                        vResultado.RazonGroup = (string)consulta["RazonGroup"];
                        vResultado.RazonStatus = (string)consulta["RazonStatus"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el tipo de razon correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<Razones> GetAllByTipo(int TipoId)
        {
            List<Razones> RazonesList = new List<Razones>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetRazonByTipoId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@TipoId", TipoId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                RazonesList = (from DataRow dr in dt.Rows

                               select new Razones()
                               {
                                   TipoId = Convert.ToInt32(dr["TipoId"]),
                                   RazonId = Convert.ToInt32(dr["RazonId"]),
                                   RazonDescripcion = Convert.ToString(dr["RazonDescripcion"]),
                                   RazonAbreviatura = Convert.ToString(dr["RazonAbreviatura"]),
                                   TipoAbreviatura = Convert.ToString(dr["TipoAbreviatura"]),
                                   RazonGroup = Convert.ToString(dr["RazonGroup"]),
                                   RazonStatus = Convert.ToString(dr["RazonStatus"]),
                                   ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                               }).ToList();
            }
            catch (Exception ex)
            {
                Razones oRazon = new Razones();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                RazonesList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return RazonesList;
        }
    }
}