using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;
using System.Web.SessionState;

namespace appcitas.Repository
{
    public class HomeRepository : SecurityBAC
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appHome.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public List<Home> CheckUserAccessModule(string CodigoUsuario,  string PassUsuario, string CodigoModulo )
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_AutorizadoAlModulo"); 
                cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
                cmd.Parameters.AddWithValue("@CodigoModulo", CodigoModulo);
                cmd.Parameters.AddWithValue("@PassUsuario", PassUsuario);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                usuarioEncontrado   = Convert.ToInt32(dr["usuarioEncontrado"]),
                                Accion              = 1,
                                Mensaje             = "Usuario Encontrado."
                            }).ToList();
                if (HomeList.Count == 0)
                {
                    Home ss = new Home();
                    ss.Accion = 0;
                    ss.Mensaje = "Usuario sin acceso!";
                    HomeList.Add(ss);
                }
                else{
                    if (HomeList[0].usuarioEncontrado > 0)
                    {
                        HttpContext.Current.Session["usuario"] = CodigoUsuario;
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
        
        public List<Home> CheckUserAccess(string usuario, string modulo, string privilegio)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_SP_Usuario_Check"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@modulo", modulo);
                cmd.Parameters.AddWithValue("@privilegio", privilegio);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                usuarioEncontrado   = Convert.ToInt32(dr["usuarioEncontrado"]),
                                usuario             = Convert.ToString(dr["usuario"]),
                                modulo              = Convert.ToString(dr["modulo"]),
                                /*SucursalId          = Convert.ToInt32(dr["SucursalId"]),*/
                                Accion              = 1,
                                Mensaje             = "Usuario Encontrado."
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
                    if (HomeList[0].usuarioEncontrado > 0)
                    {
                        HttpContext.Current.Session["usuario"] = usuario;
                        /*HttpContext.Current.Session["SucursalId"] = HomeList[0].SucursalId;*/
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
        public List<Home> GetUsersInfo(string CodigoUsuario)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_InformacionUsuarioSGRC"); 
                cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                Codigo   = Convert.ToString(dr["Codigo"]),
                                Nombre   = Convert.ToString(dr["Nombre"]),
                                usuarioEncontrado = dt.Rows.Count,
                                Accion              = 1,
                                Mensaje             = "Usuario Encontrado."
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

        /***************************
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
         * ***************************/

        public List<Home> GetUserInfo(string CodigoUsuario)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_InformacionUsuario"); 
                cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                Codigo   = Convert.ToString(dr["Codigo"]),
                                Nombre   = Convert.ToString(dr["Nombre"]),/*
                                SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                PosicionId = Convert.ToString(dr["PosicionId"]),*/
                                usuarioEncontrado = dt.Rows.Count,
                                Accion              = 1,
                                Mensaje             = "Usuario Encontrado."
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

        public List<Home> GetUserRol(string CodigoUsuario)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_RolModulo"); 
                cmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
                cmd.Parameters.AddWithValue("@CodigoModulo", "SGRC");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                CodigoRol   = Convert.ToString(dr["CodigoRol"]),
                                Nombre      = Convert.ToString(dr["Nombre"]),
                                Accion      = 1,
                                Mensaje     = "Rol asignado a usuario, encontrado."
                            }).ToList();
                if (HomeList.Count == 0)
                {
                    Home ss = new Home();
                    ss.Accion = 0;
                    ss.Mensaje = "Usuario sin rol asignado!";
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

        public List<Home> GetUserPrivilegio(string CodigoRol)
        {
            List<Home> HomeList = new List<Home>();
            try
            {
                SqlCommand cmd = CrearComando("SECP_PrivilegiosRolModulo"); 
                cmd.Parameters.AddWithValue("@rol", CodigoRol);
                cmd.Parameters.AddWithValue("@modulo", "SGRC");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                HomeList = (from DataRow dr in dt.Rows
                            select new Home()
                            {
                                CodigoPrivilegio   = Convert.ToString(dr["CodigoPrivilegio"]),
                                Nombre      = Convert.ToString(dr["Nombre"]),
                                Accion      = 1,
                                Mensaje     = "Privilegio asignado a usuario, encontrado."
                            }).ToList();
                if (HomeList.Count == 0)
                {
                    Home ss = new Home();
                    ss.Accion = 0;
                    ss.Mensaje = "Usuario sin privileegio asignado!";
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

        /************************
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
        ************************/
    }
}
