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
    public class PrestamoRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Prestamos Save(Prestamos pPrestamo)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SF_SP_Guarda_Prestamo");

                //PARAMETROS PRESTAMO
                cmd.Parameters.AddWithValue("@Codigo", -1);
                cmd.Parameters.AddWithValue("@Codigo_CLI", pPrestamo.ClienteId);
                cmd.Parameters.AddWithValue("@mto_Solicitado", pPrestamo.MontoSolicitado);
                cmd.Parameters.AddWithValue("@Plazo_Meses", pPrestamo.PlazoMeses);
                cmd.Parameters.AddWithValue("@Garantia", pPrestamo.Garantia);
                cmd.Parameters.AddWithValue("@Porc_Interes", pPrestamo.Interes);
                cmd.Parameters.AddWithValue("@Destino", pPrestamo.Destino);
                cmd.Parameters.AddWithValue("@Frec_Pago", pPrestamo.FrecPago);
                cmd.Parameters.AddWithValue("@Observaciones", pPrestamo.Observaciones);


                //DATOS GENERALES ESTUDIO SOCIOECONOMICO

                cmd.Parameters.AddWithValue("@Personas", pPrestamo.Personas);
                cmd.Parameters.AddWithValue("@NegocioPropio", pPrestamo.NegocioPropio);
                cmd.Parameters.AddWithValue("@Salario", pPrestamo.Salario);
                cmd.Parameters.AddWithValue("@Finca", pPrestamo.Finca);
                cmd.Parameters.AddWithValue("@Otros", pPrestamo.Otros);
                cmd.Parameters.AddWithValue("@Renta", pPrestamo.Renta);
                cmd.Parameters.AddWithValue("@ServicioPublico", pPrestamo.ServicioPublico);
                cmd.Parameters.AddWithValue("@Prestamo", pPrestamo.Prestamo);
                cmd.Parameters.AddWithValue("@Transporte", pPrestamo.Transporte);
                cmd.Parameters.AddWithValue("@Alimentacion", pPrestamo.Alimentacion);
                cmd.Parameters.AddWithValue("@Vestuario", pPrestamo.Vestuario);
                cmd.Parameters.AddWithValue("@Otros1", pPrestamo.Otros1);
                cmd.Parameters.AddWithValue("@Observaciones1", pPrestamo.Observaciones1);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                //vResultado = Convert.ToInt32(cmd.Parameters["@RazonId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pPrestamo.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar la razon: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pPrestamo.Accion = vResultado;
            if (vResultado == 0)
            {
                pPrestamo.Mensaje = "Se genero un error al insertar la información del préstamo!";
            }
            else
            {
                pPrestamo.Mensaje = "Se ingreso el préstamo correctamente!";
            }
            return pPrestamo;
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

        public List<Prestamos> GetAllByTipo(int ClienteId)
        {
            List<Prestamos> PrestamosList = new List<Prestamos>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("dbo.SF_GetPrestamosClienteId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@ClienteId", ClienteId); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                PrestamosList = (from DataRow dr in dt.Rows

                               select new Prestamos()
                               {
                                   PrestamoId = Convert.ToInt32(dr["PRES_Codigo"]),
                                   FecSolicitud = Convert.ToString(dr["PRES_Fecha_Solicitud"]),
                                   MontoSolicitado = Convert.ToString(dr["PRES_mto_Solicitado"]),
                                   MontoAprobado = Convert.ToString(dr["PRES_mto_Aprobado"]),
                                   PlazoMeses = Convert.ToString(dr["PRES_Plazo_Meses"]),
                                   Interes = Convert.ToString(dr["PRES_Porc_Interes"]),
                                   Saldo = Convert.ToString(dr["PRES_Saldo"]),
                                   Estado = Convert.ToInt32(dr["PRES_Estado"]),
                                   Garantia = Convert.ToString(dr["GAR_Descripcion"]),
                                   ClienteId = Convert.ToInt32(dr["PRES_Codigo_CLI"]),
                                   NomPrestamo = Convert.ToString(dr["Nombre"]),
                                   Accion = 1,
                                   Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                               }).ToList();
            }
            catch (Exception ex)
            {
                Prestamos oRazon = new Prestamos();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                PrestamosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PrestamosList;
        }

        //VER PRESTAMO EN BOTON DE ACCION
        public List<Prestamos> GetDatosPrestamos(int id)
        {
            List<Prestamos> PrestamosList = new List<Prestamos>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SF_SP_GetDatosPrestamos"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@CodPrestamo", id); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                PrestamosList = (from DataRow dr in dt.Rows

                                 select new Prestamos()
                                 {
                                     //DATOS PRESTAMO
                                     Codigo = Convert.ToString(dr["PRES_Codigo"]),
                                     MontoSolicitado = Convert.ToString(dr["PRES_mto_Solicitado"]),
                                     PlazoMeses = Convert.ToString(dr["PRES_Plazo_Meses"]),
                                     Garantia = Convert.ToString(dr["GAR_Codigo"]),
                                     Interes = Convert.ToString(dr["PRES_Porc_Interes"]),
                                     Destino = Convert.ToString(dr["DES_Codigo"]),
                                     FrecPago = Convert.ToString(dr["PRES_Frec_Pago"]),
                                     Observaciones = Convert.ToString(dr["PRES_Observaciones"]),

                                     //DATOS DEL ESTUDIO SOCIOECONOMICO
                                     Personas = Convert.ToString(dr["ESE_NumeroDependientes"]),
                                     NegocioPropio = Convert.ToString(dr["ESE_NegocioPropio"]),
                                     Salario = Convert.ToString(dr["ESE_Salario"]),
                                     Finca = Convert.ToString(dr["ESE_ActividadAgropecuaria"]),
                                     Otros = Convert.ToString(dr["ESE_Otros_Ingresos"]),
                                     Renta = Convert.ToString(dr["ESE_Renta"]),
                                     ServicioPublico = Convert.ToString(dr["ESE_ServiciosPublicos"]),
                                     Prestamo = Convert.ToString(dr["ESE_Prestamos"]),
                                     Transporte = Convert.ToString(dr["ESE_Transporte"]),
                                     Alimentacion = Convert.ToString(dr["ESE_Alimentacion"]),
                                     Vestuario = Convert.ToString(dr["ESE_Vestuario"]),
                                     Otros1 = Convert.ToString(dr["ESE_Otros_Egresos"]),
                                     Observaciones1 = Convert.ToString(dr["ESE_Observaciones"]),

                                     Accion = 1,
                                     Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                 }).ToList();
            }
            catch (Exception ex)
            {
                Prestamos oRazon = new Prestamos();
                oRazon.Accion = 0;
                oRazon.Mensaje = ex.Message.ToString();
                PrestamosList.Add(oRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PrestamosList;
        }
    }
}