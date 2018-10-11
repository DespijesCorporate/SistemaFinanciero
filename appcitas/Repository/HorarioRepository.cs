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

    public class HorarioRepository : CAD

    {

        private SqlConnection con;

        //To Handle connection related activities

        private void connection()

        {

            string constr = ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ToString();

            con = new SqlConnection(constr);



        }



        public Horarios Save(Horarios pHorario)

        {

            SqlCommand cmd = new SqlCommand();

            int vResultado = -1;

            try

            {

                AbrirConexion();

                //connection();

                cmd = CrearComando("SGRC_SP_Horario_Save");

                cmd.Parameters.AddWithValue("@SucursalId", pHorario.SucursalId);

                cmd.Parameters.AddWithValue("@SucHorarioId", pHorario.SucHorarioId);

                cmd.Parameters["@SucHorarioId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.



                cmd.Parameters.AddWithValue("@SucHorarioIndLaboral", pHorario.SucHorarioIndLaboral);
                
                cmd.Parameters.AddWithValue("@SucHorarioCorrido", pHorario.SucHorarioCorrido);

                cmd.Parameters.AddWithValue("@SucHorarioHoraInicio", pHorario.SucHorarioHoraInicio);

                cmd.Parameters.AddWithValue("@SucHorarioHoraFinal", pHorario.SucHorarioHoraFinal);



                //con.Open();

                vResultado = Ejecuta_Accion(ref cmd);

                pHorario.Accion = vResultado;

                //vResultado = Convert.ToInt32(cmd.Parameters["@Id"].Value);

                //con.Close();

            }

            catch (Exception Ex)

            {

                pHorario.Accion = 0;

                pHorario.Mensaje = Ex.Message;

                throw new Exception("Ocurrio el un error al guardar el Horario: " + Ex.Message, Ex);

            }

            finally

            {

                cmd.Dispose();

                CerrarConexion();

            }

            pHorario.Accion = vResultado;

            if (vResultado == 0)

            {

                pHorario.Mensaje = "Se genero un error al insertar la información del horario!";

            }

            else

            {

                pHorario.Mensaje = "Se ingreso el Horario correctamente!";

            }

            return pHorario;

        }



        public List<Horarios> GetHorarios()

        {

            List<Horarios> HorariosList = new List<Horarios>();

            try

            {



                //"CrearComando" esta definido en la libreria AccesoDatos.dll

                SqlCommand cmd = CrearComando("SGRC_SP_GetHorario"); //Pasamos el procedimiento almacenado.  

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                //"GetDS" esta definido en la libreria AccesoDatos.dll

                //ds = GetDS(cmd, "SGRC_SP_GetHorario"); //Se envia el nombre del procedimiento almacenado.

                AbrirConexion();

                da.Fill(dt);

                CerrarConexion();



                //Bind EmpModel generic list using LINQ 

                HorariosList = (from DataRow dr in dt.Rows



                                   select new Horarios()

                                   {

                                       SucursalId = Convert.ToInt32(dr["SucursalId"]),

                                       SucHorarioId = Convert.ToString(dr["SucHorarioId"]),

                                       SucHorarioHoraInicio = Convert.ToString(dr["SucHorarioHoraInicio"]),

                                       SucHorarioHoraFinal = Convert.ToString(dr["SucHorarioHoraFinal"]),

                                       SucHorarioCorrido = Convert.ToBoolean(dr["SucHorarioCorrido"]),
                                       
                                       SucHorarioIndLaboral = Convert.ToBoolean(dr["SucHorarioIndLaboral"]),

                                       Orden = Convert.ToInt32(dr["Orden"]),

                                       Accion = 1,

                                       Mensaje = "Se cargaron correctamente los datos de las Horarios"

                                   }).ToList();

            }

            catch (Exception ex)

            {

                Horarios oHorario = new Horarios();

                oHorario.Accion = 0;

                oHorario.Mensaje = ex.Message.ToString();

                HorariosList.Add(oHorario);

                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);

            }

            return HorariosList;

        }



        public Horarios GetHorario(string SucId, string HorId)

        {

            Horarios vResultado = new Horarios(); //Se crea una variable que contendra los datos del almacen.

            SqlCommand cmd = new SqlCommand();

            try

            {

                cmd = CrearComando("SGRC_SP_GetHorario"); //Pasamos el nombre del procedimiento almacenado.

                cmd.Parameters.AddWithValue("@SucursalId", SucId); //Agregamos los parametros.

                cmd.Parameters.AddWithValue("@SucHorarioId", HorId); //Agregamos los parametros.



                AbrirConexion(); //Se abre la conexion a la BD.

                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.



                if (consulta.Read())

                {

                    if (consulta.HasRows)

                    {

                        //Obtenemos el valor de cada campo

                        vResultado.SucursalId = (int)consulta["SucursalId"];

                        vResultado.SucHorarioId = (string)consulta["SucHorarioId"];

                        vResultado.SucHorarioIndLaboral = (bool)consulta["SucHorarioIndLaboral"];

                        vResultado.SucHorarioHoraInicio = (string)consulta["SucHorarioHoraInicio"];

                        vResultado.SucHorarioHoraFinal = (string)consulta["SucHorarioHoraFinal"];
                        vResultado.Orden = (int)consulta["Orden"];

                        vResultado.Accion = 1;

                        vResultado.Mensaje = "Se cargó la Horario correctamente!";



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



        public Horarios Del(int SucursalId, string HorarioId)

        {

            SqlCommand cmd = new SqlCommand();

            Horarios vResultado = new Horarios();

            int vControl = -1;

            try

            {

                cmd = CrearComando("SGRC_SP_DelHorario");

                cmd.Parameters.AddWithValue("@SucHorarioId", HorarioId);
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId);



                AbrirConexion();

                vControl = Ejecuta_Accion(ref cmd);



                if (vControl > 0)

                {

                    vResultado.Accion = 1;

                    vResultado.Mensaje = "Se elimino con exito la Horario!";

                    vResultado.SucHorarioId = HorarioId;

                }

            }

            catch (Exception ex)

            {

                vResultado.Accion = 0;

                vResultado.Mensaje = ex.Message.ToString();

                vResultado.SucHorarioId = HorarioId;

                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);

            }

            finally

            {

                cmd.Dispose();

                CerrarConexion();

            }

            return vResultado;

        }



        public List<Horarios> GetHorariosBySucurcal(int id)
        {
            List<Horarios> HorariosList = new List<Horarios>();
            try
            {
                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_GetHorarioBySucursalId"); //Pasamos el procedimiento almacenado.  
                cmd.Parameters.AddWithValue("@SucursalId", id); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetHorario"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                //Bind EmpModel generic list using LINQ 
                HorariosList = (from DataRow dr in dt.Rows
                                select new Horarios()
                                {
                                    SucursalId              = Convert.ToInt32(dr["SucursalId"]),
                                    SucHorarioId            = Convert.ToString(dr["SucHorarioId"]),
                                    SucHorarioHoraInicio    = Convert.ToString(dr["SucHorarioHoraInicio"]),
                                    SucHorarioHoraFinal     = Convert.ToString(dr["SucHorarioHoraFinal"]),
                                    SucHorarioIndLaboral    = Convert.ToBoolean(dr["SucHorarioIndLaboral"]),
                                    SucHorarioCorrido       = Convert.ToBoolean(dr["SucHorarioCorrido"]),
                                    Orden                   = Convert.ToInt32(dr["Orden"]),
                                    Accion                  = 1,
                                    Mensaje                 = "Se cargaron correctamente los datos de las Horarios"
                                }).ToList();
                if (HorariosList.Count == 0)
                {
                    Horarios ss = new Horarios();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros de los horarios!";
                    HorariosList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Horarios oHorario = new Horarios();
                oHorario.Accion = 0;
                oHorario.Mensaje = ex.Message.ToString();
                HorariosList.Add(oHorario);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return HorariosList;
        }

        public int CheckHorario(int Sucid, string Horario)
        {
            int vResultado = 0; //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Horario_Check"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", Sucid); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@Horario", Horario); //Agregamos los parametros.

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
                throw new Exception("Error al verificar la existencia de los horarios: " + Ex.Message, Ex);
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