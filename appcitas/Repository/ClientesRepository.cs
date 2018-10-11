using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using appcitas.Models;
using appcitas.Context;
using System.Configuration;
using BAC.Crypto;

namespace appcitas.Repository
{
    public class ClientesRepository : CAD
    {
        private CryptoAes aes = new CryptoAes("8@c9@n@m@");
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        //INGRESAR NUEVO CLIENTE
        public Clientes Save(Clientes pClientes)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SF_SP_Guarda_Cliente");
                cmd.Parameters.AddWithValue("@Codigo", pClientes.ClienteId);
                cmd.Parameters["@Codigo"].Direction = ParameterDirection.InputOutput; //Se indica que el TipoId sera un parametro de Entrada/Salida.
            
                //DATOS GENERALES DEL CLIENTE
                cmd.Parameters.AddWithValue("@Identidad", pClientes.Identidad);
                cmd.Parameters.AddWithValue("@RTN", pClientes.RTN);
                cmd.Parameters.AddWithValue("@PrimerNombre", pClientes.PrimerNombre);
                cmd.Parameters.AddWithValue("@SegundoNombre", pClientes.SegundoNombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", pClientes.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", pClientes.SegundoApellido);
                cmd.Parameters.AddWithValue("@Genero", pClientes.Genero);
                cmd.Parameters.AddWithValue("@Fecha_Nacimiento", pClientes.Edad);
                cmd.Parameters.AddWithValue("@Estado", pClientes.EstadoCli);
                cmd.Parameters.AddWithValue("@Estado_Civil", pClientes.EstadoCivil);
                cmd.Parameters.AddWithValue("@Direccion", pClientes.Direccion);
                cmd.Parameters.AddWithValue("@TelCasa", pClientes.TelCasa);
                cmd.Parameters.AddWithValue("@Celular", pClientes.Cel);
                cmd.Parameters.AddWithValue("@Correo", pClientes.Correo);

                //PRIMERA REFERENCIA DEL CLIENTE
                cmd.Parameters.AddWithValue("@RefId_1", pClientes.RefId1);
                cmd.Parameters.AddWithValue("@refNombre_1", pClientes.RefNom1);
                cmd.Parameters.AddWithValue("@RefTel_1", pClientes.RefTel1);
                cmd.Parameters.AddWithValue("@RefCel_1", pClientes.RefCel1);

                //SEGUNDA REFERENCIA DEL CLIENTE
                cmd.Parameters.AddWithValue("RefId_2", pClientes.RefId2);
                cmd.Parameters.AddWithValue("refNombre_2", pClientes.RefNom2);
                cmd.Parameters.AddWithValue("RefTel_2", pClientes.RefTel2);
                cmd.Parameters.AddWithValue("RefCel_2", pClientes.RefCel2);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@Codigo"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pClientes.Mensaje = Ex.Message;
                throw new Exception("Ocurrio un error al guardar el cliente " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pClientes.Accion = vResultado;
            if (vResultado == 0)
            {
                pClientes.Mensaje = "Se genero un error al insertar la información del cliente!";
            }
            else
            {
                pClientes.Mensaje = "Se ingreso el cliente correctamente!";
            }
            return pClientes;
        }


        public List<Solicitudes> GetSolicitudesByIdentificacion(string CLI_Codigo)
        {
            List<Solicitudes> CitasList = new List<Solicitudes>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_Get_Prestamos_Solicitados"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaBusqueda", CLI_Codigo); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Solicitudes()
                             {
                                 PRES_Codigo = Convert.ToInt32(dr["PRES_Codigo"]),
                                 CLI_Codigo = Convert.ToInt32(dr["CLI_Codigo"]),
                                 CLI_Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                 CLI_Nombre = Convert.ToString(dr["CLI_Nombre"]),
                                 GAR_Descripcion = Convert.ToString(dr["GAR_Descripcion"]),
                                 PRES_Fecha_Solicitud = Convert.ToString(dr["PRES_Fecha_Solicitud"]),
                                 PRES_mto_Solicitado = Convert.ToInt32(dr["PRES_mto_Solicitado"]),
                                 PRES_Plazo_Meses = Convert.ToInt32(dr["PRES_Plazo_Meses"]),
                                 PRES_Estado = Convert.ToInt32(dr["PRES_Estado"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Solicitudes ss = new Solicitudes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Solicitudes oCita = new Solicitudes();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

public List<Clientes> GetClientes()
        {
            List<Clientes> ClientesList = new List<Clientes>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("dbo.SF_Clientes_Get"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetSucursal"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                ClientesList = (from DataRow dr in dt.Rows

                                  select new Clientes()
                                  {
                                      ClienteId = Convert.ToInt32(dr["CLI_Codigo"]),
                                      Identidad = Convert.ToString(dr["CLI_Identidad"]),
                                      RTN = Convert.ToString(dr["CLI_RTN"]),
                                      Nombre = Convert.ToString(dr["Nombre"]),
                                      Genero = Convert.ToString(dr["CLI_Genero"]),
                                      Edad = Convert.ToString(dr["Edad"]),
                                      EstadoCli = Convert.ToString(dr["EstadoCli"]),
                                      Direccion = Convert.ToString(dr["CLI_Direccion"]),
                                      TelCasa = Convert.ToString(dr["CLI_Tel_Casa"]),
                                      Cel = Convert.ToString(dr["CLI_Cel"]),
                                      Correo = Convert.ToString(dr["CLI_Correo"]),
                                      Accion = 1,
                                      Mensaje = "Se cargaron correctamente los datos del tipo de razon"
                                  }).ToList();
                if (ClientesList.Count == 0)
                {
                    Clientes ss = new Clientes();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los Tipos!";
                    ClientesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Clientes oTipoRazon = new Clientes();
                oTipoRazon.Accion = 0;
                oTipoRazon.Mensaje = ex.Message.ToString();
                ClientesList.Add(oTipoRazon);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return ClientesList;
        }


        public MensajesResultado AprobarRechazarSolicitudByCodigo(int Accion, int Id, int Monto)
        {
            SqlCommand cmd = new SqlCommand();
            MensajesResultado vResultado = new MensajesResultado();
            int a = Accion;
            int vControl = -1;
            try
            {
                cmd = CrearComando("[SF_SP_Aprobacion_Prestamo]");
                cmd.Parameters.AddWithValue("@Accion", Accion);
                cmd.Parameters.AddWithValue("@Codigo", Id);
                cmd.Parameters.AddWithValue("@Monto", Monto);
                //cmd.Parameters.AddWithValue("@Observacion", Observacion);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    
                    if (a == 1) {
                    vResultado.Estado = 1;
                    vResultado.Mensaje = "La solicitud fue aprobada satisfactoriamente!";  
                        }
                    else 
                        {
                    vResultado.Estado = 2;
                    vResultado.Mensaje = "La solicitud fue rechazada satisfactoriamente!";
                    }
                }
            }
            catch (Exception ex)
            {
                vResultado.Estado = 1;
                vResultado.Mensaje = ex.Message.ToString();
                if (a == 1) {
                throw new Exception("La solicitud no pudo ser aprobada por el siguiente error: " + ex.Message, ex);
                 }
                    else 
                        {
                            throw new Exception("La solicitud no pudo ser rechazada por el siguiente error: " + ex.Message, ex);
                        }
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

public Clientes CheckClientes(string rtn, string identidad)
        {
            Clientes vResultado = new Clientes(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("dbo.SF_Clientes_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Identidad", identidad); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@RTN", rtn); //Agregamos los parametros.

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

        public List<TransaccionesPrestamos> GetTransaccionesPrestamos(int codigo)
        {
            List<TransaccionesPrestamos> CitasList = new List<TransaccionesPrestamos>();
            try
            {
                SqlCommand cmd = CrearComando("SF_SP_Get_Trans_Prestamos"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@Codigo", codigo); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new TransaccionesPrestamos()
                             {
                                 Fecha = Convert.ToString(dr["Fecha"]),
                                 TRP_Otorgado = Convert.ToDecimal(dr["TRP_Otorgado"]),
                                 TRP_Capital = Convert.ToDecimal(dr["TRP_Capital"]),
                                 TRP_Interes = Convert.ToDecimal(dr["TRP_Interes"]),
                                 TRP_Mora = Convert.ToDecimal(dr["TRP_Mora"]),
                                 TRP_Agrego = Convert.ToString(dr["TRP_Agrego"]),
                                 Saldo = Convert.ToDecimal(dr["Saldo"]),
                                 TRP_Codigo_Prestamo = Convert.ToInt32(dr["TRP_Codigo_Prestamo"]),
                                 //PRES_Estado = Convert.ToInt32(dr["PRES_Estado"]),
                                 Accion = 1,
                                 Mensaje = "Las transacciones fueron cargadas exitosamente."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    TransaccionesPrestamos ss = new TransaccionesPrestamos();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                TransaccionesPrestamos oCita = new TransaccionesPrestamos();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }
    }
}
