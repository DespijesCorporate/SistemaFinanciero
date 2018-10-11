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
    public class SegmentosRepository : CAD
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public SucursalesSegmento Save(SucursalesSegmento pSegmento)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Segmento_Save");
                cmd.Parameters.AddWithValue("@SucursalId", pSegmento.SucursalId);
                cmd.Parameters.AddWithValue("@SucSegmentoId", pSegmento.SucSegmentoId);
                cmd.Parameters["@SucSegmentoId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                pSegmento.Accion = vResultado;
                //vResultado = Convert.ToInt32(cmd.Parameters["@Id"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pSegmento.Accion = 0;
                pSegmento.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar el Segmento: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pSegmento.Accion = vResultado;
            if (vResultado == 0)
            {
                pSegmento.Mensaje = "Se genero un error al insertar la información del segmento!";
            }
            else
            {
                pSegmento.Mensaje = "Se ingreso el Segmento correctamente!";
            }
            return pSegmento;
        }

        public List<SucursalesSegmento> GetSegmentos()
        {
            List<SucursalesSegmento> SegmentosList = new List<SucursalesSegmento>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetSegmento"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSegmento"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SegmentosList = (from DataRow dr in dt.Rows

                                select new SucursalesSegmento()
                                {
                                    SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                    SucSegmentoId = Convert.ToString(dr["SucSegmentoId"]),
                                    ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargaron correctamente los datos de las Segmentos"
                                }).ToList();
                if (SegmentosList.Count == 0)
                {
                    SucursalesSegmento ss = new SucursalesSegmento();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los segmentos!";
                    SegmentosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                SucursalesSegmento oSegmento = new SucursalesSegmento();
                oSegmento.Accion = 0;
                oSegmento.Mensaje = ex.Message.ToString();
                SegmentosList.Add(oSegmento);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SegmentosList;
        }

        public SucursalesSegmento GetSegmento(int Id)
        {
            SucursalesSegmento vResultado = new SucursalesSegmento(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetSegmento"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.SucursalId = (int)consulta["SucursalId"];
                        vResultado.SucSegmentoId = (string)consulta["SucSegmentoId"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la Segmento correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de segmento: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public SucursalesSegmento Del(int SucursalId, string SucSegmentoId)
        {
            SqlCommand cmd = new SqlCommand();
            SucursalesSegmento vResultado = new SucursalesSegmento();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelSegmento");
                cmd.Parameters.AddWithValue("@SegmentoId", SucursalId);
                cmd.Parameters.AddWithValue("@SucSegmentoId", SucSegmentoId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el Segmento!";
                    vResultado.SucSegmentoId = SucSegmentoId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.SucSegmentoId = SucSegmentoId;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<SucursalesSegmento> GetSegmentosBySucursal(int id)
        {
            List<SucursalesSegmento> SegmentosList = new List<SucursalesSegmento>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetSegmentoBySucursalId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@SucursalId", id); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSegmento"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SegmentosList = (from DataRow dr in dt.Rows

                                select new SucursalesSegmento()
                                {
                                    SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                    SucSegmentoId = Convert.ToString(dr["SucSegmentoId"]),
                                    ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                    Accion = 1,
                                    Mensaje = "Se cargaron correctamente los datos de las Segmentos"
                                }).ToList();
                if (SegmentosList.Count == 0)
                {
                    SucursalesSegmento ss = new SucursalesSegmento();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los segmentos!";
                    SegmentosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                SucursalesSegmento oSegmento = new SucursalesSegmento();
                oSegmento.Accion = 0;
                oSegmento.Mensaje = ex.Message.ToString();
                SegmentosList.Add(oSegmento);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SegmentosList;
        }

        public SucursalesSegmento DelSegmento(int SucursalId, string SucSegmentoId)
        {
            SqlCommand cmd = new SqlCommand();
            SucursalesSegmento vResultado = new SucursalesSegmento();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelSegmento");
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                cmd.Parameters.AddWithValue("@SucSegmentoId", SucSegmentoId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el segmento!";
                    vResultado.SucSegmentoId = SucSegmentoId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.SucSegmentoId = SucSegmentoId;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public int CheckSegmento(int Sucid, string Segmento)
        {
            int vResultado = 0; //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Segmento_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", Sucid); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@Segmento", Segmento); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado = (int)consulta["Registros"];
                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado = -1;
                //vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al verificar la existencia de los cubiculos: " + Ex.Message, Ex);
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