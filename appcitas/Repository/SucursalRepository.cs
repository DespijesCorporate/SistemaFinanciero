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
    public class SucursalRepository : CAD
    {

        //private SqlConnection con;
        ////To Handle connection related activities
        //private void connection()
        //{
        //    string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
        //    con = new SqlConnection(constr);

        //}

        //LISTA DE ESTADOS
        public List<ConfigItem> ListaEstados()
        {
            List<ConfigItem> EstadosList = new List<ConfigItem>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SF_SP_Estados"); //Pasamos el procedimiento almacenado.  
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

                               select new ConfigItem()
                               {
                                   Estado_Descripcion = Convert.ToString(dr["EstadoDescripcion"]),
                                   EstadoCodigo = Convert.ToString(dr["PRES_Estado"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos de las sucursales"
                               }).ToList();
                if (EstadosList.Count == 0)
                {
                    ConfigItem ss = new ConfigItem();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Sucursales!";
                    EstadosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                ConfigItem oSucursal = new ConfigItem();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                EstadosList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return EstadosList;
        }
        //TERMINA PROCESO DE CARGAR LISTA DE ESTADOS


        public Sucursales Save(Sucursales pSucursal)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Sucursal_Save");
                cmd.Parameters.AddWithValue("@Id", pSucursal.SucursalId);
                cmd.Parameters["@Id"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@Nombre", pSucursal.SucursalNombre);
                cmd.Parameters.AddWithValue("@Ubicacion", pSucursal.SucursalUbicacion);
                cmd.Parameters.AddWithValue("@TipoAtencion", pSucursal.SucursalTipoAtencion);
                cmd.Parameters.AddWithValue("@CentroAtencion", pSucursal.SucursalEsCentroAtencion);
                cmd.Parameters.AddWithValue("@Canal", pSucursal.SucursalEsCanal);
                cmd.Parameters.AddWithValue("@Abreviatura", pSucursal.SucursalAbreviatura);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@Id"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pSucursal.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar la sucursal: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pSucursal.Accion = vResultado;
            if(vResultado == 0)
            {
                pSucursal.Mensaje = "Se genero un error al insertar la información de la sucursal!";
            }
            else
            {
                pSucursal.Mensaje = "Se ingreso la sucursal correctamente!";
            }
            return pSucursal;
        }

        public List<Sucursales> GetSucursales()
        {
            List<Sucursales> SurcusalesList = new List<Sucursales>();
            try
            {
               
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetSucursal"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SurcusalesList = (from DataRow dr in dt.Rows

                           select new Sucursales()
                           {
                               SucursalId = Convert.ToInt32(dr["SucursalId"]),
                               SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                               SucursalAbreviatura = Convert.ToString(dr["SucursalAbreviatura"]),
                               SucursalUbicacion = Convert.ToString(dr["SucursalUbicacion"]),
                               SucursalTipoAtencion = Convert.ToString(dr["SucursalTipoAtencion"]),
                               SucursalEsCanal = Convert.ToBoolean(dr["SucursalEsCanal"]),
                               SucursalEsCentroAtencion = Convert.ToBoolean(dr["SucursalEsCentroAtencion"]),
                               Accion = 1,
                               Mensaje = "Se cargaron correctamente los datos de las sucursales"
                           }).ToList();
                if (SurcusalesList.Count == 0)
                {
                    Sucursales ss = new Sucursales();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Sucursales!";
                    SurcusalesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Sucursales oSucursal = new Sucursales();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                SurcusalesList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SurcusalesList;
        }

        public List<Home> GetUsersInfoCitas(string CodigoUsuario)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cubiculos_GetByCitas");
                cmd.Parameters.AddWithValue("@sucursalid", CodigoUsuario);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                Codigo = Convert.ToString(dr["Posicionid"]),
                                Nombre = Convert.ToString(dr["posicionDescripcion"]),
                                usuarioEncontrado = dt.Rows.Count,
                                Accion = 1,
                                Mensaje = "Usuario Encontrado."
                            }).ToList();
                if (HomeList.Count == 0)
                {
                    Home ss = new Home();
                    ss.Accion = 0;
                    ss.Mensaje = "Usuario sin acceso!";
                    HomeList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Home oHome = new Home();
                oHome.Accion = 0;
                oHome.Mensaje = ex.Message.ToString();
                HomeList.Add(oHome);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return HomeList;
        }
        public List<Sucursales> getSucursalesByAtencion()
        {
            List<Sucursales> SurcusalesList = new List<Sucursales>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Sucursal_Get_atencion"); //Pasamos el procedimiento almacenado.  
                //cmd.Parameters.AddWithValue("@atencion", sucursalAtencion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SurcusalesList = (from DataRow dr in dt.Rows

                           select new Sucursales()
                           {
                               SucursalId = Convert.ToInt32(dr["SucursalId"]),
                               SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                               SucursalAbreviatura = Convert.ToString(dr["SucursalAbreviatura"]),
                               SucursalUbicacion = Convert.ToString(dr["SucursalUbicacion"]),
                               SucursalTipoAtencion = Convert.ToString(dr["SucursalTipoAtencion"]),
                               SucursalEsCanal = Convert.ToBoolean(dr["SucursalEsCanal"]),
                               SucursalEsCentroAtencion = Convert.ToBoolean(dr["SucursalEsCentroAtencion"]),
                               Accion = 1,
                               Mensaje = "Se cargaron correctamente los datos de las sucursales"
                           }).ToList();
                if (SurcusalesList.Count == 0)
                {
                    Sucursales ss = new Sucursales();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Sucursales!";
                    SurcusalesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Sucursales oSucursal = new Sucursales();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                SurcusalesList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SurcusalesList;
        }

        public List<Sucursales> getSucursalesCanal()
        {
            List<Sucursales> SurcusalesList = new List<Sucursales>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Sucursal_Get_Canal"); //Pasamos el procedimiento almacenado.  
                //cmd.Parameters.AddWithValue("@atencion", sucursalAtencion);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SurcusalesList = (from DataRow dr in dt.Rows

                           select new Sucursales()
                           {
                               SucursalId = Convert.ToInt32(dr["SucursalId"]),
                               SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                               Accion = 1,
                               Mensaje = "Se cargaron correctamente los datos de las sucursales"
                           }).ToList();
                if (SurcusalesList.Count == 0)
                {
                    Sucursales ss = new Sucursales();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Sucursales!";
                    SurcusalesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Sucursales oSucursal = new Sucursales();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                SurcusalesList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SurcusalesList;
        }

        public List<Sucursales> getSucursalesBySegmento(string SegmentoId)
        {
            List<Sucursales> SurcusalesList = new List<Sucursales>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Sucursal_Get_BySegmento");
                cmd.Parameters.AddWithValue("@SegmentoId", SegmentoId);
                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                SurcusalesList = (from DataRow dr in dt.Rows
                           select new Sucursales()
                           {
                               SucursalId = Convert.ToInt32(dr["SucursalId"]),
                               SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                               Accion = 1,
                               Mensaje = "Se cargaron correctamente los datos de las sucursales"
                           }).ToList();
                if (SurcusalesList.Count == 0)
                {
                    Sucursales ss = new Sucursales();
                    ss.Accion = 0;
                    ss.Mensaje = "Ninguna sucursal atiende el segmento seleccionado!";
                    SurcusalesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Sucursales oSucursal = new Sucursales();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                SurcusalesList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SurcusalesList;
        }

        public List<Sucursales> getSegmentosBySucursal(int SucursalId)
        {
            List<Sucursales> SurcusalesList = new List<Sucursales>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetSegmentoBySucursalId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                SurcusalesList = (from DataRow dr in dt.Rows

                           select new Sucursales()
                           {
                               SucursalId = Convert.ToInt32(dr["SucursalId"]),
                               SucSegmentoId = Convert.ToString(dr["SucSegmentoId"]),
                               ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                               Accion = 1,
                               Mensaje = "Se cargaron correctamente los datos de las sucursales"
                           }).ToList();
                if (SurcusalesList.Count == 0)
                {
                    Sucursales ss = new Sucursales();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las Sucursales!";
                    SurcusalesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Sucursales oSucursal = new Sucursales();
                oSucursal.Accion = 0;
                oSucursal.Mensaje = ex.Message.ToString();
                SurcusalesList.Add(oSucursal);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return SurcusalesList;
        }

        public Sucursales GetSucursal(int Id)
        {
            Sucursales vResultado = new Sucursales(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetSucursal"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.SucursalId = (int)consulta["SucursalId"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalAbreviatura = (string)consulta["SucursalAbreviatura"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.SucursalTipoAtencion = (string)consulta["SucursalTipoAtencion"];
                        vResultado.SucursalEsCentroAtencion = (bool)consulta["SucursalEsCentroAtencion"];
                        vResultado.SucursalEsCanal = (bool)consulta["SucursalEsCanal"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la sucursal correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la sucursal: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Sucursales Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Sucursales vResultado = new Sucursales();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_SUCURSAL_DEL");
                cmd.Parameters.AddWithValue("@Id", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito la sucursal!";
                    vResultado.SucursalId = Id;
                }
                else 
                { 
                    vResultado.Accion = 0; 
                    vResultado.Mensaje = "No se pudo eliminar la sucursal, tiene datos relacionados en otros formularios!"; 
                    vResultado.SucursalId = Id; 
                } 
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.SucursalId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public int CheckSucursal(string Nombre, string Abreviatura)
        {
            int vResultado = 0; //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Sucursal_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalNombre", Nombre); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@SucursalAbreviatura", Abreviatura); //Agregamos los parametros.

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
                throw new Exception("Error al verificar la existencia de las sucursales: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;

        }

        public List<Home> CheckUserInfo(string usuario)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Usuario_Info"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@usuario", usuario);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                usuario = Convert.ToString(dr["usuario"]),
                                SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                PosicionId = Convert.ToString(dr["PosicionId"]),
                                Accion = 1,
                                Mensaje = "Usuario Encontrado."
                            }).ToList();
                if (HomeList.Count == 0)
                {
                    Home ss = new Home();
                    ss.Accion = 0;
                    ss.Mensaje = "Usuario sin acceso!";
                    HomeList.Add(ss);
                }
                else
                {
                    if (HomeList[0].Accion > 0)
                    {
                        HttpContext.Current.Session["usuario"] = usuario;
                        HttpContext.Current.Session["SucursalId"] = HomeList[0].SucursalId;
                        HttpContext.Current.Session["PosicionId"] = HomeList[0].PosicionId;
                    }
                }
            }
            catch (Exception ex)
            {
                Home oHome = new Home();
                oHome.Accion = 0;
                oHome.Mensaje = ex.Message.ToString();
                HomeList.Add(oHome);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return HomeList;
        }
    }
}
