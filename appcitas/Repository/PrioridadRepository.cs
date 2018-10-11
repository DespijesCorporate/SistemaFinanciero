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
    public class PrioridadRepository : CAD
    {

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();
            con = new SqlConnection(constr);

        }

        public Prioridades Save(Prioridades pPrioridad)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Prioridad_Save");
                cmd.Parameters.AddWithValue("@PrioridadId", pPrioridad.PrioridadId);
                cmd.Parameters["@PrioridadId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@PrioridadNombre", pPrioridad.PrioridadNombre);
                cmd.Parameters.AddWithValue("@PrioridadNivel", pPrioridad.PrioridadNivel);
                cmd.Parameters.AddWithValue("@PrioridadCodigo", pPrioridad.PrioridadCodigo);
                cmd.Parameters.AddWithValue("@Identificador", pPrioridad.Identificador);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@PrioridadId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pPrioridad.Mensaje = Ex.Message;
                throw new Exception("Ocurrio el un error al guardar la Prioridad: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pPrioridad.Accion = vResultado;
            if (vResultado == 0)
            {
                pPrioridad.Mensaje = "Se genero un error al insertar la información de la Prioridad!";
            }
            else
            {
                pPrioridad.Mensaje = "Se ingreso la Prioridad correctamente!";
            }
            return pPrioridad;
        }

        public List<Prioridades> GetPrioridades()
        {
            List<Prioridades> PrioridadesList = new List<Prioridades>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetPrioridad"); //Pasamos el procedimiento almacenado.  
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetPrioridad"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                PrioridadesList = (from DataRow dr in dt.Rows

                                  select new Prioridades()
                                  {
                                      PrioridadId = Convert.ToInt32(dr["PrioridadId"]),
                                      PrioridadNombre = Convert.ToString(dr["PrioridadNombre"]),
                                      PrioridadCodigo = Convert.ToString(dr["PrioridadCodigo"]),
                                      PrioridadNivel = Convert.ToInt32(dr["PrioridadNivel"]),
                                      Identificador = Convert.ToString(dr["Identificador"]),
                                      Accion = 1,
                                      Mensaje = "Se cargaron correctamente los datos de las Prioridades"
                                  }).ToList();
                if (PrioridadesList.Count == 0)
                {
                    Prioridades ss = new Prioridades();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de las prioridades!";
                    PrioridadesList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Prioridades oPrioridad = new Prioridades();
                oPrioridad.Accion = 0;
                oPrioridad.Mensaje = ex.Message.ToString();
                PrioridadesList.Add(oPrioridad);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return PrioridadesList;
        }

        public Prioridades GetPrioridad(int Id)
        {
            Prioridades vResultado = new Prioridades(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetPrioridad"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.PrioridadCodigo = (string)consulta["PrioridadCodigo"];
                        vResultado.PrioridadNivel = (int)consulta["PrioridadNivel"];
                        vResultado.Identificador = (string)consulta["Identificador"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la Prioridad correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de prioridad: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Prioridades CheckPrioridad(string nombre, string codigo)
        {
            Prioridades vResultado = new Prioridades(); //Se crea una variable que contendra los datos del trámite.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Prioridad_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@PrioridadNombre", nombre); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@PrioridadCodigo", codigo); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.cantidadRegistros = (int)consulta["cantidadRegistros"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó correctamente la Prioridad!";

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

        public Prioridades Del(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Prioridades vResultado = new Prioridades();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_DelPrioridad");
                cmd.Parameters.AddWithValue("@PrioridadId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se elimino con exito la Prioridad!";
                    vResultado.PrioridadId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.PrioridadId = Id;
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