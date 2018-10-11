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
    public class CubiculoRepository : CAD
    {

        public Cubiculo Save(Cubiculo pCubiculo)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Cubiculo_Save");
                cmd.Parameters.AddWithValue("@SucursalId", pCubiculo.SucursalId);                

                cmd.Parameters.AddWithValue("@PosicionId", pCubiculo.PosicionId);
                cmd.Parameters.AddWithValue("@PosicionNombre", pCubiculo.PosicionNombre);
                cmd.Parameters.AddWithValue("@TipoAtencionId", pCubiculo.TipoAtencionId);
                cmd.Parameters.AddWithValue("@UsuarioId", pCubiculo.UsuarioId);
                cmd.Parameters.AddWithValue("@DescansoInicio", pCubiculo.PosicionHoraInicioDesc);
                cmd.Parameters.AddWithValue("@DescansoFin", pCubiculo.PosicionHoraFinalDesc);
                cmd.Parameters.AddWithValue("@Saved", 1);
                cmd.Parameters["@Saved"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.
                //con.Open();
                Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@Saved"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCubiculo.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar el cubiculo: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCubiculo.Accion = vResultado;
            if (vResultado == 0)
            {
                pCubiculo.Mensaje = "Se genero un error al insertar la información del cubiculo!";
            }
            else
            {
                pCubiculo.Mensaje = "Se ingreso el cubiculo correctamente!";
            }
            return pCubiculo;
        }

        public List<Cubiculo> GetCubiculos(int SucursalId)
        {
            List<Cubiculo> CubiculosList = new List<Cubiculo>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetCubiculo"); //Pasamos el procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CubiculosList = (from DataRow dr in dt.Rows

                                 select new Cubiculo()
                                  {
                                      SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                      PosicionId = Convert.ToString(dr["PosicionId"]),
                                      PosicionNombre = Convert.ToString(dr["PosicionNombre"]),
                                      TipoAtencionId = Convert.ToString(dr["TipoAtencionId"]),
                                      UsuarioId = Convert.ToString(dr["UsuarioId"]),
                                      ConfigItemDescripcion = Convert.ToString(dr["ConfigItemDescripcion"]),
                                      TipoAtencionId_COD = Convert.ToString(dr["TipoAtencionId_COD"]),
                                      PosicionHoraInicioDesc = Convert.ToString(dr["PosicionHoraInicioDesc"]),
                                      PosicionHoraFinalDesc = Convert.ToString(dr["PosicionHoraFinalDesc"]),
                                      PosicionHoraInicioDescMin = Convert.ToInt32(dr["PosicionHoraInicioDescMin"]),
                                      PosicionHoraFinalDescMin = Convert.ToInt32(dr["PosicionHoraFinalDescMin"]),
                                      ConfigId = Convert.ToString(dr["ConfigId"]),
                                      PosicionUsuario = Convert.ToString(dr["PosicionUsuario"]),
                                      Accion = 1,
                                      Mensaje = "Se cargaron correctamente los datos de los cubiculos"
                                  }).ToList();
                if (CubiculosList.Count == 0)
                {
                    Cubiculo ss = new Cubiculo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de cubiculos para las Sucursales!";
                    CubiculosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cubiculo oCubiculos = new Cubiculo();
                oCubiculos.Accion = 0;
                oCubiculos.Mensaje = ex.Message.ToString();
                CubiculosList.Add(oCubiculos);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CubiculosList;
        }

        public List<Cubiculo> GetCubiculosBySucursal(int SucursalId)
        {
            List<Cubiculo> CubiculosList = new List<Cubiculo>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cubiculos_GetBySucursal"); //Pasamos el procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CubiculosList = (from DataRow dr in dt.Rows
                                 select new Cubiculo()
                                 {
                                     SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                     PosicionId = Convert.ToString(dr["PosicionId"]),
                                     sucursal = Convert.ToString(dr["sucursal"]),
                                     posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                     posicionAbreviatura = Convert.ToString(dr["posicionAbreviatura"]),
                                     PosicionHoraInicioDesc = Convert.ToString(dr["PosicionHoraInicioDesc"]),
                                     PosicionHoraFinalDesc = Convert.ToString(dr["PosicionHoraFinalDesc"]),
                                     itemOrden = Convert.ToString(dr["itemOrden"]),
                                     TipoAtencionId = Convert.ToString(dr["TipoAtencionId"]),
                                     SucursalTipoAtencion = Convert.ToString(dr["SucursalTipoAtencion"]),
                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos de los cubiculos"
                                 }).ToList();
                if (CubiculosList.Count == 0)
                {
                    Cubiculo ss = new Cubiculo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de cubiculos para las Sucursales!";
                    CubiculosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cubiculo oCubiculos = new Cubiculo();
                oCubiculos.Accion = 0;
                oCubiculos.Mensaje = ex.Message.ToString();
                CubiculosList.Add(oCubiculos);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CubiculosList;
        }

        public List<Cubiculo> GetCubiculosBySucursalAtencion(int SucursalId)
        {
            List<Cubiculo> CubiculosList = new List<Cubiculo>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cubiculos_GetBySucursal_Atencion"); //Pasamos el procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CubiculosList = (from DataRow dr in dt.Rows
                                 select new Cubiculo()
                                 {
                                     SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                     PosicionId = Convert.ToString(dr["PosicionId"]),
                                     sucursal = Convert.ToString(dr["sucursal"]),
                                     posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                     posicionAbreviatura = Convert.ToString(dr["posicionAbreviatura"]),
                                     PosicionHoraInicioDesc = Convert.ToString(dr["PosicionHoraInicioDesc"]),
                                     PosicionHoraFinalDesc = Convert.ToString(dr["PosicionHoraFinalDesc"]),
                                     itemOrden = Convert.ToString(dr["itemOrden"]),
                                     TipoAtencionId = Convert.ToString(dr["TipoAtencionId"]),
                                     SucursalTipoAtencion = Convert.ToString(dr["SucursalTipoAtencion"]),
                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos de los cubiculos"
                                 }).ToList();
                if (CubiculosList.Count == 0)
                {
                    Cubiculo ss = new Cubiculo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de cubiculos para las Sucursales!";
                    CubiculosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cubiculo oCubiculos = new Cubiculo();
                oCubiculos.Accion = 0;
                oCubiculos.Mensaje = ex.Message.ToString();
                CubiculosList.Add(oCubiculos);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CubiculosList;
        }
      
        public List<Cubiculo> GetCubiculosByCita(int SucursalId)
        {
            List<Cubiculo> CubiculosList = new List<Cubiculo>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cubiculos_GetByCitas"); //Pasamos el procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CubiculosList = (from DataRow dr in dt.Rows
                                select new Cubiculo()
                                {
                                    SucursalId              = Convert.ToInt32(dr["SucursalId"]),
                                    PosicionId              = Convert.ToString(dr["PosicionId"]),
                                    posicionDescripcion     = Convert.ToString(dr["posicionDescripcion"]),
                                    Accion                  = 1,
                                    Mensaje                 = "Se cargaron correctamente los datos de los cubiculos"
                                }).ToList();
                if (CubiculosList.Count == 0)
                {
                    Cubiculo ss = new Cubiculo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de cubiculos para las Sucursales!";
                    CubiculosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cubiculo oCubiculos = new Cubiculo();
                oCubiculos.Accion = 0;
                oCubiculos.Mensaje = ex.Message.ToString();
                CubiculosList.Add(oCubiculos);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CubiculosList;
        }


       



        public List<Cubiculo> GetCubiculosBySucursalAndCita(int SucursalId)
        {
            List<Cubiculo> CubiculosList = new List<Cubiculo>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Cubiculos_GetBySucursalAndCitas"); //Pasamos el procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CubiculosList = (from DataRow dr in dt.Rows
                                 select new Cubiculo()
                                 {
                                     SucursalId = Convert.ToInt32(dr["SucursalId"]),
                                     PosicionId = Convert.ToString(dr["PosicionId"]),
                                     posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos de los cubiculos"
                                 }).ToList();
                if (CubiculosList.Count == 0)
                {
                    Cubiculo ss = new Cubiculo();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de cubiculos para las Sucursales!";
                    CubiculosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Cubiculo oCubiculos = new Cubiculo();
                oCubiculos.Accion = 0;
                oCubiculos.Mensaje = ex.Message.ToString();
                CubiculosList.Add(oCubiculos);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CubiculosList;
        }
        
        public Cubiculo GetCubiculo(int Id, string CubId)
        {
            Cubiculo vResultado = new Cubiculo(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetCubiculo"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", Id); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@PosicionId", CubId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.SucursalId = (int)consulta["SucursalId"];
                        vResultado.PosicionId = (string)consulta["PosicionId"];
                        vResultado.PosicionNombre = (string)consulta["PosicionNombre"];
                        vResultado.TipoAtencionId = (string)consulta["TipoAtencionId"];
                        vResultado.UsuarioId = (string)consulta["UsuarioId"];
                        vResultado.ConfigItemDescripcion = (string)consulta["ConfigItemDescripcion"];
                        vResultado.TipoAtencionId_COD = (string)consulta["TipoAtencionId_COD"];
                        vResultado.ConfigId = (string)consulta["ConfigId"];
                        vResultado.PosicionHoraInicioDesc = (string)consulta["PosicionHoraInicioDesc"];
                        vResultado.PosicionHoraFinalDesc = (string)consulta["PosicionHoraFinalDesc"];
                        vResultado.PosicionHoraInicioDescMin = (int)consulta["PosicionHoraInicioDescMin"];
                        vResultado.PosicionHoraFinalDescMin = (int)consulta["PosicionHoraFinalDescMin"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó el cubiculo correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos del cubiculo: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Cubiculo Del(int SucursalId, string CubId)
        {
            SqlCommand cmd = new SqlCommand();
            Cubiculo vResultado = new Cubiculo();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelCubiculo");
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);
                cmd.Parameters.AddWithValue("@PosicionId", CubId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito la sucursal!";
                    vResultado.SucursalId = SucursalId;
                    vResultado.PosicionId = CubId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.SucursalId = SucursalId;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public int CheckCubiculo(int Sucid, string Cubiculo)
        {
            int vResultado = 0; //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Cubiculo_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", Sucid); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@CubiculoId", Cubiculo); //Agregamos los parametros.

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
