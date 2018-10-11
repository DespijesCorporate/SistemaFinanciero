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
    public class ConfigRepository : CAD
    {
        public Config Save(Config pConfig)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                cmd = CrearComando("SGRC_SP_Config_Save");
                cmd.Parameters.AddWithValue("@ConfigId", pConfig.ConfigID);
                cmd.Parameters["@ConfigId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.
                cmd.Parameters.AddWithValue("@ConfigDescripcion", pConfig.ConfigDesc);
                cmd.Parameters.AddWithValue("@ConfigObservacion", pConfig.ConfigObs);
                cmd.Parameters.AddWithValue("@ConfigStatus", pConfig.Estado);
                cmd.Parameters.AddWithValue("@saved", 1);

                Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@saved"].Value);
            }
            catch(Exception Ex)
            {
                pConfig.Mensaje = Ex.Message.ToString();
                throw new Exception("Ocurrio un error al guardar un item: " + Ex.Message, Ex);
            }
            pConfig.Accion = vResultado;
            if(vResultado == 0)
            {                
                pConfig.Mensaje = "Se genero un error al insertar el item!";
            }
            else
            {                
                pConfig.Mensaje = "Se ingreso un item correctamente!";
            }
            return pConfig;
        }

        
        public List<Config> GetConfig()
        {
            List<Config> ConfigList = new List<Config>();
            Config oCongif = new Config();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Config_Get"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                ConfigList = (from DataRow dr in dt.Rows

                                  select new Config()
                                  {
                                      ConfigID = Convert.ToString(dr["ConfigId"]),
                                      ConfigDesc = Convert.ToString(dr["ConfigDescripcion"]),
                                      ConfigObs = Convert.ToString(dr["ConfigObservacion"]),
                                      Estado = Convert.ToString(dr["ConfigStatus"]),
                                      Accion = 1,
                                      Mensaje = "Se cargaron correctamente los datos de las Configuración!"
                                  }).ToList();

                if (ConfigList.Count == 0)
                {
                    oCongif.Accion = 0;
                    oCongif.Mensaje = "No se encontraron registros!";
                    ConfigList.Add(oCongif);
                }
            }

            catch (Exception ex)
            {
                oCongif.Accion = 0;
                oCongif.Mensaje = ex.Message.ToString();
                ConfigList.Add(oCongif);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return ConfigList;
        }

        public Config CheckConfig(string id, string descripcion)
        {
            Config vResultado = new Config(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Config_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@ConfigId", id); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@ConfigDescripcion", descripcion); //Agregamos los parametros.

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

        public ConfigItem CheckConfigItem(string idConfig, string id, string descripcion, string abreviatura)
        {
            ConfigItem vResultado = new ConfigItem(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_ConfigItem_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@ConfigId", idConfig); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@ConfigItemId", id); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@ConfigItemDescripcion", descripcion); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@ConfigItemAbreviatura", abreviatura); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente el elemento!";

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

        public Config GetItem(string Id)
        {
            Config vResultado = new Config(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Config_Get"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@ConfigId", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.ConfigID = (string)consulta["ConfigId"];
                        vResultado.ConfigDesc = (string)consulta["ConfigDescripcion"];
                        vResultado.ConfigObs = (string)consulta["ConfigObservacion"];
                        vResultado.Estado = (string)consulta["ConfigStatus"];                        
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el item correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos del almacen: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Config Del(string Id)
        {
            SqlCommand cmd = new SqlCommand();
            Config vResultado = new Config();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Config_Del");
                cmd.Parameters.AddWithValue("@ConfigId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el item!";
                    vResultado.ConfigID = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.ConfigID = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        /************/

        public ConfigItem SaveItem(ConfigItem pConfig)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                cmd = CrearComando("SGRC_SP_ConfigItem_Save");
                cmd.Parameters.AddWithValue("@ConfigItemId", pConfig.ConfigItemID);
                cmd.Parameters.AddWithValue("@ConfigId", pConfig.ConfigID);
                cmd.Parameters["@ConfigItemId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.
                cmd.Parameters.AddWithValue("@Descripcion", pConfig.ConfigItemDescripcion);
                cmd.Parameters.AddWithValue("@Observacion", pConfig.ConfigItemObservacion);
                cmd.Parameters.AddWithValue("@Abreviatura", pConfig.ConfigItemAbreviatura);
                cmd.Parameters.AddWithValue("@Estado", pConfig.Estado);
                cmd.Parameters.AddWithValue("@saved", 1);

                Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@saved"].Value);
            }
            catch(Exception Ex)
            {
                pConfig.Mensaje = Ex.Message.ToString();
                throw new Exception("Ocurrio un error al guardar un item: " + Ex.Message, Ex);
            }
            pConfig.Accion = vResultado;
            if(vResultado == 0)
            {                
                pConfig.Mensaje = "Se genero un error al insertar el item!";
            }
            else
            {                
                pConfig.Mensaje = "Se ingreso un item correctamente!";
            }
            return pConfig;
        }

        public List<ConfigItem> GetConfigItem(string ConfigId, string ConfigItemId)
        {
            List<ConfigItem> ConfigList = new List<ConfigItem>();
            ConfigItem oCongif = new ConfigItem();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetConfigItem"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@ConfigId", ConfigId); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@ConfigItemId", ConfigItemId); //Agregamos los parametros.
                              
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                ConfigList = (from DataRow dr in dt.Rows

                              select new ConfigItem()
                              {
                                  ConfigItemID = Convert.ToString(dr["ConfigItemID"]),
                                  ConfigID = Convert.ToString(dr["ConfigID"]),
                                  ConfigDescripcion = Convert.ToString(dr["ConfigDescripcion"]),
                                  ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                  ConfigItemAbreviatura = Convert.ToString(dr["ConfigItemAbreviatura"]),
                                  ConfigItemObservacion = Convert.ToString(dr["ConfigItemObservacion"]),
                                  Estado = Convert.ToString(dr["ConfigItemStatus"]),
                                  Accion = 1,
                                  Mensaje = "Se cargaron correctamente los datos de las Configuración!"
                              }).ToList();

                if (ConfigList.Count == 0)
                {
                    oCongif.Accion = 0;
                    oCongif.Mensaje = "No se encontraron registros!";
                    ConfigList.Add(oCongif);
                }
            }

            catch (Exception ex)
            {
                oCongif.Accion = 0;
                oCongif.Mensaje = ex.Message.ToString();
                ConfigList.Add(oCongif);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return ConfigList;
        }

        public ConfigItem GetDetalleItem(int Id)
        {
            ConfigItem vResultado = new ConfigItem(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetConfigDetalleItem"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.ConfigItemID = Convert.ToString(consulta["ConfigItemID"]);
                        vResultado.ConfigID = (string)consulta["ConfigID"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDesc"];
                        vResultado.ConfigItemAbreviatura = (string)consulta["ConfigItemValor"];
                        vResultado.ConfigItemObservacion = (string)consulta["ConfigItemObs"];
                        vResultado.Estado = (string)consulta["Estado"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el item correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos del almacen: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public ConfigItem DelItem(string ConfigId, string ConfigItemId)
        {
            SqlCommand cmd = new SqlCommand();
            ConfigItem vResultado = new ConfigItem();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_ConfigItem_Del");
                cmd.Parameters.AddWithValue("@ConfigId", ConfigId);
                cmd.Parameters.AddWithValue("@ConfigItemId", ConfigItemId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito el item!";
                    vResultado.ConfigItemID = ConfigItemId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.ConfigItemID = ConfigItemId;
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
