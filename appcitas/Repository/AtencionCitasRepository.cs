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
using appcitas.Services;

namespace appcitas.Repository
{
    public class AtencionCitasRepository : CAD
    {
		private CryptoAes aes = new CryptoAes("8@c9@n@m@");
        public List<Citas> GetCitasBySurcursal(int SucursalId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_CitaWalkinCola"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {

                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Convert.ToString(dr["CitaCuenta"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public Citas GetCitasProgramadasCola(string Posicionid, int Sucursalid)
        {
            Citas vResultado = new Citas(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_CitaProgramadaCola"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@PosicionId", Posicionid); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@SucursalId", Sucursalid); //Agregamos los parametros.
                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        vResultado.CitaCuenta = Convert.ToString(consulta["CitaCuenta"]);
                        vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        vResultado.CitaHora = (string)consulta["CitaHora"];
                        vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        vResultado.posicionDescripcion = (string)consulta["posicionDescripcion"];
                        vResultado.segmento = (string)consulta["segmento"];
                        vResultado.sucursal = (string)consulta["sucursal"];
                        vResultado.tramite = (string)consulta["tramite"];
                        vResultado.PosicionId = (string)consulta["PosicionId"];
                        vResultado.CitaFlagClienteIniciaAtencion = Convert.ToString(consulta["CitaFlagClienteIniciaAtencion"]);
                        vResultado.CitaFlagClienteIniciaEspera = Convert.ToString(consulta["CitaFlagClienteIniciaEspera"]);
                        vResultado.CitaEstadoDescripcion = Convert.ToString(consulta["CitaEstadoDescripcion"]);
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                    //else
                    //{
                    //    vResultado.Accion = 0;
                    //    vResultado.Mensaje = "No se encontraron citas programadas!";
                    //}
                }
                else
                {
                    vResultado.Accion = 0;
                    vResultado.Mensaje = "No se encontraron citas programadas!";
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

        public Atencion GetCitasPendientes(string Posicionid, int Sucursalid)
        {
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_CitaPendiente"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@PosicionId", Posicionid); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@SucursalId", Sucursalid); //Agregamos los parametros.
                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        //vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        //vResultado.CitaCuenta = Convert.ToString(consulta["CitaCuenta"]);
                        //vResultado.CitaTarjeta = Convert.ToString(consulta["CitaTarjeta"]);
                        //vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        //vResultado.CitaHora = (string)consulta["CitaHora"];
                        //vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        //vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        //vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        //vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        //vResultado.TramiteId = (int)consulta["TramiteId"];
                        //vResultado.CitaEstado = Convert.ToString(consulta["CitaEstado"]);
                        //vResultado.SucursalId = Convert.ToString(consulta["SucursalId"]);
                        //vResultado.SucursalIdReferido = Convert.ToString(consulta["SucursalIdReferido"]);
                        //vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        //vResultado.CitaTarjeta = Convert.ToString(consulta["CitaTarjeta"]);
                        //vResultado.CitaSegmentoId = Convert.ToString(consulta["CitaSegmentoId"]);
                        //vResultado.CitaTelefonoCasa = Convert.ToString(consulta["CitaTelefonoCasa"]);
                        //vResultado.CitaTelefonoCelular = Convert.ToString(consulta["CitaTelefonoCelular"]);
                        //vResultado.CitaTelefonoOficina = Convert.ToString(consulta["CitaTelefonoOficina"]);
                        //vResultado.CitaTicket = Convert.ToString(consulta["CitaTicket"]);
                        //vResultado.segmento = (string)consulta["segmento"];
                        //vResultado.sucursal = (string)consulta["sucursal"];
                        //vResultado.tramite = (string)consulta["tramite"];
                        //vResultado.duracion = (int)consulta["tramiteDuracion"];
                        //vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        //vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        //vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        //vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        //vResultado.Familia = (string)consulta["Familia"];
                        //vResultado.Producto = (string)consulta["Producto"];
                        //vResultado.MarcaTarjeta = (string)consulta["MarcaTarjeta"];
                        //vResultado.EmisorCuenta = (string)consulta["EmisorCuenta"];
                        vResultado.MinutosAtencionCitaPendiente = (int)consulta["MinutosAtencionCitaPendiente"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                    //else
                    //{
                    //    vResultado.Accion = 0;
                    //    vResultado.Mensaje = "No se encontraron citas programadas!";
                    //}
                }
                else
                {
                    vResultado.Accion = 0;
                    vResultado.Mensaje = "No se encontraron citas programadas!";
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

        public Atencion GetCitasByID(int CitaID)
        {
            
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            
                cmd = CrearComando("SGRC_SP_Cita_ById"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@CitaId", CitaID); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.
                try
                {
                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        vResultado.CitaCuenta = Decryt(Convert.ToString(consulta["CitaCuenta"]));
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        vResultado.CitaHora = (string)consulta["CitaHora"];
                        vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        vResultado.TramiteId = (int)consulta["TramiteId"];
                        vResultado.CitaEstado = Convert.ToString(consulta["CitaEstado"]);
                        vResultado.SucursalId = Convert.ToString(consulta["SucursalId"]);
                        vResultado.SucursalIdReferido = Convert.ToString(consulta["SucursalIdReferido"]);
                        vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaSegmentoId = Convert.ToString(consulta["CitaSegmentoId"]);
                        vResultado.CitaTelefonoCasa = Convert.ToString(consulta["CitaTelefonoCasa"]);
                        vResultado.CitaTelefonoCelular = Convert.ToString(consulta["CitaTelefonoCelular"]);
                        vResultado.CitaTelefonoOficina = Convert.ToString(consulta["CitaTelefonoOficina"]);
                        vResultado.CitaTicket = Convert.ToString(consulta["CitaTicket"]);
                        vResultado.segmento = (string)consulta["segmento"];
                        vResultado.sucursal = (string)consulta["sucursal"];
                        vResultado.tramite = (string)consulta["tramite"];
                        vResultado.duracion = (int)consulta["tramiteDuracion"];
                        vResultado.TiempoMuerto = (int)consulta["TiempoMuerto"];
                        vResultado.AlertaPrevia = (int)consulta["AlertaPrevia"];
                        vResultado.ToleranciaFinalizacion = (int)consulta["ToleranciaFinalizacion"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        vResultado.Familia = (string)consulta["Familia"];
                        vResultado.Producto = (string)consulta["Producto"];
                        vResultado.MarcaTarjeta = (string)consulta["MarcaTarjeta"];
                        vResultado.EmisorCuenta = (string)consulta["EmisorCuenta"];
                        vResultado.SucHorarioCorrido = (bool)consulta["SucHorarioCorrido"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la cita: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }


        public Atencion GetCitasByIDFinCita(int CitaID)
        {

            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();

            cmd = CrearComando("SGRC_SP_Cita_ByIdFinCita"); //Pasamos el nombre del procedimiento almacenado.
            cmd.Parameters.AddWithValue("@CitaId", CitaID); //Agregamos los parametros.

            AbrirConexion(); //Se abre la conexion a la BD.
            SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.
            try
            {
                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        vResultado.CitaCuenta = Decryt(Convert.ToString(consulta["CitaCuenta"]));
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        vResultado.CitaHora = (string)consulta["CitaHora"];
                        vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        vResultado.TramiteId = (int)consulta["TramiteId"];
                        vResultado.CitaEstado = Convert.ToString(consulta["CitaEstado"]);
                        vResultado.SucursalId = Convert.ToString(consulta["SucursalId"]);
                        vResultado.SucursalIdReferido = Convert.ToString(consulta["SucursalIdReferido"]);
                        vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaSegmentoId = Convert.ToString(consulta["CitaSegmentoId"]);
                        vResultado.CitaTelefonoCasa = Convert.ToString(consulta["CitaTelefonoCasa"]);
                        vResultado.CitaTelefonoCelular = Convert.ToString(consulta["CitaTelefonoCelular"]);
                        vResultado.CitaTelefonoOficina = Convert.ToString(consulta["CitaTelefonoOficina"]);
                        vResultado.CitaTicket = Convert.ToString(consulta["CitaTicket"]);
                        vResultado.segmento = (string)consulta["segmento"];
                        vResultado.sucursal = (string)consulta["sucursal"];
                        vResultado.tramite = (string)consulta["tramite"];
                        vResultado.duracion = (int)consulta["tramiteDuracion"];
                        vResultado.TiempoMuerto = (int)consulta["TiempoMuerto"];
                        vResultado.AlertaPrevia = (int)consulta["AlertaPrevia"];
                        vResultado.ToleranciaFinalizacion = (int)consulta["ToleranciaFinalizacion"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        vResultado.Familia = (string)consulta["Familia"];
                        vResultado.Producto = (string)consulta["Producto"];
                        vResultado.MarcaTarjeta = (string)consulta["MarcaTarjeta"];
                        vResultado.EmisorCuenta = (string)consulta["EmisorCuenta"];
                        vResultado.SucHorarioCorrido = (bool)consulta["SucHorarioCorrido"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la cita: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Atencion GetCitasByIdAndPosicion(int CitaID, string PosicionId)
        {
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_ByIdAndPosicion"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@CitaId", CitaID); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@Posicion", PosicionId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        vResultado.CitaCuenta = Decryt(Convert.ToString(consulta["CitaCuenta"]));
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        vResultado.CitaHora = (string)consulta["CitaHora"];
                        vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        vResultado.TramiteId = (int)consulta["TramiteId"];
                        vResultado.CitaEstado = Convert.ToString(consulta["CitaEstado"]);
                        vResultado.SucursalId = Convert.ToString(consulta["SucursalId"]);
                        vResultado.SucursalIdReferido = Convert.ToString(consulta["SucursalIdReferido"]);
                        vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaSegmentoId = Convert.ToString(consulta["CitaSegmentoId"]);
                        vResultado.CitaTelefonoCasa = Convert.ToString(consulta["CitaTelefonoCasa"]);
                        vResultado.CitaTelefonoCelular = Convert.ToString(consulta["CitaTelefonoCelular"]);
                        vResultado.CitaTelefonoOficina = Convert.ToString(consulta["CitaTelefonoOficina"]);
                        vResultado.CitaTicket = Convert.ToString(consulta["CitaTicket"]);
                        vResultado.segmento = (string)consulta["segmento"];
                        vResultado.sucursal = (string)consulta["sucursal"];
                        vResultado.tramite = (string)consulta["tramite"];
                        vResultado.duracion = (int)consulta["tramiteDuracion"];
                        vResultado.TiempoMuerto = (int)consulta["TiempoMuerto"];
                        vResultado.AlertaPrevia = (int)consulta["AlertaPrevia"];
                        vResultado.ToleranciaFinalizacion = (int)consulta["ToleranciaFinalizacion"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        vResultado.Familia = (string)consulta["Familia"];
                        vResultado.Producto = (string)consulta["Producto"];
                        vResultado.MarcaTarjeta = (string)consulta["MarcaTarjeta"];
                        vResultado.EmisorCuenta = (string)consulta["EmisorCuenta"];
                        vResultado.SucHorarioCorrido = (bool)consulta["SucHorarioCorrido"];
                        vResultado.MinutosProximaCitaProgramada = (int)consulta["MinutosProximaCitaProgramada"];
                        vResultado.FlagProximaCitaProgramada = (bool)consulta["FlagProximaCitaProgramada"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la cita: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas Clientellegocita(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_Clientellego");
                cmd.Parameters.AddWithValue("@CitaId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se registró que cliente llegó!";
                    vResultado.CitaId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas Clienteentro(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_Clienteentro");
                cmd.Parameters.AddWithValue("@CitaId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se registró que cliente entró!";
                    vResultado.CitaId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas Clientesalio(int Id)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_Clientesalio");
                cmd.Parameters.AddWithValue("@CitaId", Id);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se registró que cliente salió!";
                    vResultado.CitaId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Atencion Save(Atencion pCita)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_CitaWK_Save");
                cmd.Parameters.AddWithValue("@CitaId", pCita.CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput; //Se indica que el IdAlmacen sera un parametro de Entrada/Salida.

                cmd.Parameters.AddWithValue("@CitaIdentificacion", pCita.CitaIdentificacion);
                cmd.Parameters.AddWithValue("@CitaNombre", pCita.CitaNombre);
                cmd.Parameters.AddWithValue("@CitaCorreoElectronico", pCita.CitaCorreoElectronico);
                cmd.Parameters.AddWithValue("@CitaCuenta", Encrypt(pCita.CitaCuenta));
                cmd.Parameters.AddWithValue("@CitaTarjeta", Encrypt(pCita.CitaTarjeta));
                cmd.Parameters.AddWithValue("@CitaTelefonoCelular", pCita.CitaTelefonoCelular);
                cmd.Parameters.AddWithValue("@CitaTelefonoCasa", pCita.CitaTelefonoCasa);
                cmd.Parameters.AddWithValue("@CitaTelefonoOficina", pCita.CitaTelefonoOficina);
                cmd.Parameters.AddWithValue("@TramiteId", pCita.TramiteId);
                cmd.Parameters.AddWithValue("@SucursalId", pCita.SucursalId);
                cmd.Parameters.AddWithValue("@SucursalIdReferido", pCita.SucursalIdReferido);
                cmd.Parameters.AddWithValue("@CitaSegmentoId", pCita.CitaSegmentoId);
                cmd.Parameters.AddWithValue("@CitaFecha", pCita.CitaFecha);
                cmd.Parameters.AddWithValue("@CitaHora", pCita.CitaHora);
                cmd.Parameters.AddWithValue("@CitaEstado", pCita.CitaEstado);
                cmd.Parameters.AddWithValue("@PrioridadId", pCita.PrioridadId);
                cmd.Parameters.AddWithValue("@EmisorId", pCita.EmisorId);
                cmd.Parameters.AddWithValue("@usuarioCreacion", HttpContext.Current.Session["usuario"]);

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCita.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar cita.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCita.Accion = vResultado;
            if (vResultado == 0)
            {
                pCita.Mensaje = "Hubo un inconveniente al intentar guardar la información!";
            }
            else
            {
                if(vResultado == -1)
                {
                    pCita.Mensaje = "Ya existe una cita en cola para el clente " + pCita.CitaNombre;
                }
                else
                {
                    pCita.Mensaje = "Cita ingresada correctamente!";
                }                
            }
            return pCita;
        }

        public Atencion GetCitaTicket(int CitaID)
        {
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_CitaTicket_ById"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@CitaId", CitaID); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.CitaTicket = (string)consulta["CitaTicket"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

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

        public Citas Clienteabandona(int Id, string Motivo)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Cita_Clienteabandona");
                cmd.Parameters.AddWithValue("@CitaId", Id);
                cmd.Parameters.AddWithValue("@CitaObservacion", Motivo);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se registró que cliente abandona sucursal!";
                    vResultado.CitaId = Id;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = Id;
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }


        public List<Citas> ConteoUsuarioFecha()
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_ConteoUsuarioFecha"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@Usuario", HttpContext.Current.Session["usuario"]); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 ContadorWalkin = Convert.ToInt32(dr["ContadorWalkin"]),
                                 ContadorProg = Convert.ToInt32(dr["ContadorProg"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }



        public List<Citas> Campocitarazon(int CitaId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Cita_CampoRazon"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 
                                 CampoRazon = Convert.ToInt32(dr["citaRazon"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }



        public Citas IniciaCitaFecha(int CitaId, string Cubiculo)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_IniciaCita");
                cmd.Parameters.AddWithValue("@CitaId", CitaId);
                cmd.Parameters.AddWithValue("@PosicionId", Cubiculo);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se Inicio la cita correctamente!";
                    vResultado.CitaId = CitaId;
                }
                else {
                    //vResultado.Accion = 0;
                    //vResultado.Mensaje = "La cita seleccionada ya está siendo atendida, favor seleccione otra cita!";
                    //vResultado.CitaId = CitaId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = CitaId;
                throw new Exception("No se pudo Iniciar la cita por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas FinalizarCita(int CitaId, string Observacion, string Resolucion)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_FinalizarCita");
                cmd.Parameters.AddWithValue("@CitaId", CitaId);
                cmd.Parameters.AddWithValue("@CitaObservacion", Observacion);
                cmd.Parameters.AddWithValue("@Resolucion", Resolucion);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se finalizo la cita correctamente!";
                    vResultado.CitaId = CitaId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = CitaId;
                throw new Exception("No se pudo finalizar la cita por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<Citas> GetCitasProgramadasLlamadas(int SucursalId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_CitasProgramadasLlamadas"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {

                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Convert.ToString(dr["CitaCuenta"]),
                                 CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetCitasWalkinLlamadas(int SucursalId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_CitasWalkinLlamadas"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {

                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Convert.ToString(dr["CitaCuenta"]),
                                 CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }


        public List<Citas> GetSucursalName(int SucursalId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {

                //"CrearComando" esta definido en la libreria AccesoDatos.dll
                SqlCommand cmd = CrearComando("SGRC_SP_Sucursal_Get_Name"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", SucursalId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //"GetDS" esta definido en la libreria AccesoDatos.dll
                //ds = GetDS(cmd, "SGRC_SP_GetCita"); //Se envia el nombre del procedimiento almacenado.
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();

                //Bind EmpModel generic list using LINQ 
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 sucursal = Convert.ToString(dr["SucursalNombre"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public Atencion GetPromedioTiempoEspera(int SucursalId)
        {
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetPromedioTiempoEspera"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.DiferenciaMinutos = Convert.ToInt32(consulta["DiferenciaMinutos"]);
                        vResultado.NumeroClientes = Convert.ToInt32(consulta["NumeroClientes"]);
                        vResultado.PromedioTiempoEspera = Convert.ToInt32(consulta["PromedioTiempoEspera"]);
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

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

        public List<Citas> GetRazonesByCita(int CitaId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Razon_GetByCita_Atencion"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 TipoId = Convert.ToInt32(dr["TipoId"]),
                                 tipoRazonAbreviatura = Convert.ToString(dr["tipoRazonAbreviatura"]),
                                 RazonId = Convert.ToInt32(dr["RazonId"]),
                                 RazonDescripcion = Convert.ToString(dr["RazonDescripcion"]),
                                 RazonAbreviatura = Convert.ToString(dr["RazonAbreviatura"]),
                                 TipoEtiquetaListadoExtra = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                 RazonGroup = Convert.ToString(dr["RazonGroup"]),
                                 RazonGrupo = Convert.ToString(dr["RazonGrupo"]),
                                 RazonStatus = Convert.ToString(dr["RazonStatus"]),
                                 listadoExtraDescripcion = Convert.ToString(dr["listadoDescripcionExtraCONFIG"]),
                                 DatoExtraId = Convert.ToString(dr["DatoExtraId"]),
                                 TipoOrigenExtraId = Convert.ToString(dr["TipoOrigenExtraId"]),
                                 CodigoListadoOrigenExtraId = Convert.ToString(dr["CodigoListadoOrigenExtraId"]),
                                 listadoExtra = Convert.ToString(dr["listadoExtra"]),
                                 RazonOrigen = Convert.ToString(dr["RazonOrigen"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la razón."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Atencion> GetCitaTarjetas(int CitaId)
        {
            List<Atencion> CitasList = new List<Atencion>();
            try
            {
                SqlCommand cmd = CrearComando("SRGC_SP_GetCitaTarjetas"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                //cmd.Parameters.AddWithValue("@Cuenta", Cuenta); //Agregamos los parametros. 
                //cmd.Parameters.AddWithValue("@Tarjeta", Tarjeta); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Atencion()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 Familia = Convert.ToString(dr["Familia"]),
                                 Producto = Convert.ToString(dr["Producto"]),
                                 MarcaTarjeta = Convert.ToString(dr["MarcaTarjeta"]),
                                 Resolucion = Convert.ToString(dr["Resolucion"]),
                                 CitaObservacion = Convert.ToString(dr["Comentario"]),
                                 CuentaOriginal = Decryt(Convert.ToString(dr["CuentaOriginal"])),
                                 TarjetaOriginal = Decryt(Convert.ToString(dr["TarjetaOriginal"])),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de las tarjetas."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Atencion ss = new Atencion();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron tarjetas para esta cita!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Atencion oCita = new Atencion();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public List<Citas> GetCitasDia(int SucursalId, string PosicionId)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Citas_Programadas_Dia"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@SucursalId", SucursalId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@PosicionId", PosicionId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 CitaIdentificacion = Convert.ToString(dr["CitaIdentificacion"]),
                                 CitaFecha = Convert.ToString(dr["CitaFecha"]),
                                 CitaNombre = Convert.ToString(dr["CitaNombre"]),
                                 CitaCorreoElectronico = Convert.ToString(dr["CitaCorreoElectronico"]),
                                 CitaTelefonoCelular = Convert.ToString(dr["CitaTelefonoCelular"]),
                                 CitaHora = Convert.ToString(dr["CitaHora"]),
                                 CitaHoraInicioCompleta = Convert.ToString(dr["CitaHoraInicioCompleta"]),
                                 CitaHoraFinCompleta = Convert.ToString(dr["CitaHoraFinCompleta"]),
                                 tramite = Convert.ToString(dr["tramite"]),
                                 PosicionId = Convert.ToString(dr["PosicionId"]),
                                 posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                                 CitaCuenta = Convert.ToString(dr["CitaCuenta"]),
                                 CitaTicket = Convert.ToString(dr["CitaTicket"]),
                                 segmento = Convert.ToString(dr["segmento"]),
                                 sucursal = Convert.ToString(dr["sucursal"]),
                                 CitaEstado = Convert.ToString(dr["CitaEstado"]),
                                 CitaEstadoDescripcion = Convert.ToString(dr["CitaEstadoDescripcion"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la cita."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron citas para el dia de hoy!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public CitaHerramienta SaveHerramienta(CitaHerramienta pHerramienta)
        {
            SqlCommand cmd = new SqlCommand();
            CitaHerramienta obj = new CitaHerramienta();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_Herramienta_Save");
                cmd.Parameters.AddWithValue("@CitaId", pHerramienta.CitaId);
                cmd.Parameters.AddWithValue("@HerramientaId", pHerramienta.HerramientaId);
                cmd.Parameters.AddWithValue("@HerramientaObservacion", pHerramienta.HerramientaObservacion);
                vResultado = Ejecuta_Accion(ref cmd);
                obj = pHerramienta;
                obj.Accion = vResultado;
                if(vResultado == 1)
                {
                    obj.Mensaje = "La información se agrego correctamente!";
                }
                else
                {
                    obj.Mensaje = "No se guardo la herramienta!!";
                }

                return obj;

            }
            catch (Exception ex)
            {
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                return obj;
            }
        }

        public List<CitaHerramienta> GetHerramientasByCita(int CitaId)
        {
            List<CitaHerramienta> List = new List<CitaHerramienta>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_GetHerramienta_ByCitaId"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                List = (from DataRow dr in dt.Rows
                        select new CitaHerramienta()
                             {
                                 HerramientaId = Convert.ToString(dr["HerramientaId"]),
                                 CitaId = Convert.ToInt32(dr["CitaId"]),
                                 HerramientaDescripcion = Convert.ToString(dr["HerramientaDescripcion"]),
                                 HerramientaObservacion = Convert.ToString(dr["HerramientaObservacion"]),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la razón."
                             }).ToList();
                if (List.Count == 0)
                {
                    CitaHerramienta ss = new CitaHerramienta();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    List.Add(ss);
                }
            }
            catch (Exception ex)
            {
                CitaHerramienta oCita = new CitaHerramienta();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                List.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return List;
        }

        public CitaHerramienta deleteHerramienta(int citaId, string herramientaId)
        {
            SqlCommand cmd = new SqlCommand();
            CitaHerramienta vResultado = new CitaHerramienta();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Herramienta_Delete");
                cmd.Parameters.AddWithValue("@citaId", citaId);
                cmd.Parameters.AddWithValue("@herramientaId", herramientaId);

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito la herramienta!";
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }


        public List<Atencion> GetReporteCentroAtencion()
        {
            List<Atencion> List = new List<Atencion>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_GetReporteCentroAtencion"); //Pasamos el procedimiento almacenado. 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                List = (from DataRow dr in dt.Rows
                        select new Atencion()
                        {
                            SucursalNombre = Convert.ToString(dr["SucursalNombre"]),
                            DiferenciaMinutos = Convert.ToInt32(dr["DiferenciaMinutos"]),
                            NumeroClientes = Convert.ToInt32(dr["NumeroClientes"]),
                            PromedioTiempoEspera = Convert.ToInt32(dr["PromedioTiempoEspera"]),
                            Accion = 1,
                            Mensaje = "Se cargó correctamente la información de la razón."
                        }).ToList();
                if (List.Count == 0)
                {
                    Atencion ss = new Atencion();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    List.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Atencion oCita = new Atencion();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                List.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return List;
        }

        public List<Atencion> GetCitasLlamadoTicket(int Id)
        {
            List<Atencion> List = new List<Atencion>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_Citas_GetCitasLlamadoTicket"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@sucursalid", Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                List = (from DataRow dr in dt.Rows
                        select new Atencion()
                        {
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            PosicionId = Convert.ToString(dr["PosicionId"]),
                            posicionDescripcion = Convert.ToString(dr["posicionDescripcion"]),
                            CitaTicket = Convert.ToString(dr["CitaTicket"]),
                            CitaTipo = Convert.ToInt32(dr["CitaTipo"]),
                            Accion = 1,
                            Mensaje = "Se cargó correctamente la información de la razón."
                        }).ToList();
                if (List.Count == 0)
                {
                    Atencion ss = new Atencion();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    List.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Atencion oCita = new Atencion();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                List.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return List;
        }

        public Atencion GetCitaByIdentificacion(string Id)
        {
            Atencion vResultado = new Atencion(); //Se crea una variable que contendra los datos del almacen.
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = CrearComando("SGRC_SP_GetCita_ByIdentificacion"); //Pasamos el nombre del procedimiento almacenado.
                cmd.Parameters.AddWithValue("@Id", Id); //Agregamos los parametros.

                AbrirConexion(); //Se abre la conexion a la BD.
                SqlDataReader consulta = Ejecuta_Consulta(cmd); //Enviamos el comando con los paramentros agregados.

                if (consulta.Read())
                {
                    if (consulta.HasRows)
                    {
                        //Obtenemos el valor de cada campo
                        vResultado.CitaId = (int)consulta["CitaId"];
                        vResultado.CitaCorreoElectronico = (string)consulta["CitaCorreoElectronico"];
                        vResultado.CitaCuenta = Decryt(Convert.ToString(consulta["CitaCuenta"]));
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaFecha = (string)consulta["CitaFecha"];
                        vResultado.CitaHora = (string)consulta["CitaHora"];
                        vResultado.CitaHoraFinCompleta = (string)consulta["CitaHoraFinCompleta"];
                        vResultado.CitaHoraInicioCompleta = (string)consulta["CitaHoraInicioCompleta"];
                        vResultado.CitaIdentificacion = (string)consulta["CitaIdentificacion"];
                        vResultado.CitaNombre = (string)consulta["CitaNombre"];
                        vResultado.CitaTelefonoCelular = (string)consulta["CitaTelefonoCelular"];
                        vResultado.TramiteId = (int)consulta["TramiteId"];
                        vResultado.CitaEstado = Convert.ToString(consulta["CitaEstado"]);
                        vResultado.SucursalId = Convert.ToString(consulta["SucursalId"]);
                        vResultado.SucursalIdReferido = Convert.ToString(consulta["SucursalIdReferido"]);
                        vResultado.PrioridadId = (int)consulta["PrioridadId"];
                        vResultado.CitaTarjeta = Decryt(Convert.ToString(consulta["CitaTarjeta"]));
                        vResultado.CitaSegmentoId = Convert.ToString(consulta["CitaSegmentoId"]);
                        vResultado.CitaTelefonoCasa = Convert.ToString(consulta["CitaTelefonoCasa"]);
                        vResultado.CitaTelefonoCelular = Convert.ToString(consulta["CitaTelefonoCelular"]);
                        vResultado.CitaTelefonoOficina = Convert.ToString(consulta["CitaTelefonoOficina"]);
                        vResultado.CitaTicket = Convert.ToString(consulta["CitaTicket"]);
                        vResultado.segmento = (string)consulta["segmento"];
                        vResultado.sucursal = (string)consulta["sucursal"];
                        vResultado.tramite = (string)consulta["tramite"];
                        vResultado.duracion = (int)consulta["tramiteDuracion"];
                        vResultado.TiempoMuerto = (int)consulta["TiempoMuerto"];
                        vResultado.AlertaPrevia = (int)consulta["AlertaPrevia"];
                        vResultado.ToleranciaFinalizacion = (int)consulta["ToleranciaFinalizacion"];
                        vResultado.SucursalNombre = (string)consulta["SucursalNombre"];
                        vResultado.SucursalUbicacion = (string)consulta["SucursalUbicacion"];
                        vResultado.PrioridadNombre = (string)consulta["PrioridadNombre"];
                        vResultado.CitaHora = Convert.ToString(consulta["CitaHora"]);
                        vResultado.Familia = (string)consulta["Familia"];
                        vResultado.Producto = (string)consulta["Producto"];
                        vResultado.MarcaTarjeta = (string)consulta["MarcaTarjeta"];
                        vResultado.EmisorCuenta = (string)consulta["EmisorCuenta"];
                        vResultado.Accion = 1;
                        vResultado.Mensaje = "Se cargó la cita correctamente!";

                        //Si los campos admiten valores nulos convertir explicitamente
                        //ej: vResultado.Nombre = Convert.ToString(consulta["Nombre"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = Ex.Message.ToString();
                throw new Exception("Error al cargar los datos de la cita: " + Ex.Message, Ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas SaveTarjetas(int CitaId, string Cuenta, string Tarjeta, string Resolucion, string Comentario, int Emisor, string CuentaOriginal, string TarjetaOriginal)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            int x = 0;
            try
            {
                cmd = CrearComando("SRGC_SP_TarjetasSave");
                cmd.Parameters.AddWithValue("@CitaId", CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters.AddWithValue("@CitaCuenta", Encrypt(Cuenta));
                cmd.Parameters.AddWithValue("@CitaTarjeta", Encrypt(Tarjeta));
                cmd.Parameters.AddWithValue("@Resolucion", Resolucion);
                cmd.Parameters.AddWithValue("@Comentario", Comentario);
                cmd.Parameters.AddWithValue("@EmisorId", Emisor);
                cmd.Parameters.AddWithValue("@CuentaOriginal", Encrypt(CuentaOriginal));
                cmd.Parameters.AddWithValue("@TarjetaOriginal", Encrypt(TarjetaOriginal));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);
                x = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se agrego la información correctamente!";
                    vResultado.CitaId = CitaId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = CitaId;
                throw new Exception("No se pudo agregar por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public List<Citas> GetTarjetasRazonesByCita(int CitaId, string Cuenta, string Tarjeta)
        {
            List<Citas> CitasList = new List<Citas>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_TarjetasRazon_GetByCita"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(Cuenta)); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta)); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                CitasList = (from DataRow dr in dt.Rows
                             select new Citas()
                             {
                                 TipoId = Convert.ToInt32(dr["TipoId"]),
                                 tipoRazonAbreviatura = Convert.ToString(dr["tipoRazonAbreviatura"]),
                                 RazonId = Convert.ToInt32(dr["RazonId"]),
                                 RazonDescripcion = Convert.ToString(dr["RazonDescripcion"]),
                                 RazonAbreviatura = Convert.ToString(dr["RazonAbreviatura"]),
                                 TipoEtiquetaListadoExtra = Convert.ToString(dr["TipoEtiquetaListadoExtra"]),
                                 RazonGroup = Convert.ToString(dr["RazonGroup"]),
                                 RazonGrupo = Convert.ToString(dr["RazonGrupo"]),
                                 RazonStatus = Convert.ToString(dr["RazonStatus"]),
                                 listadoExtraDescripcion = Convert.ToString(dr["listadoDescripcionExtraCONFIG"]),
                                 DatoExtraId = Convert.ToString(dr["DatoExtraId"]),
                                 TipoOrigenExtraId = Convert.ToString(dr["TipoOrigenExtraId"]),
                                 CodigoListadoOrigenExtraId = Convert.ToString(dr["CodigoListadoOrigenExtraId"]),
                                 listadoExtra = Convert.ToString(dr["listadoExtra"]),
                                 RazonOrigen = Convert.ToString(dr["RazonOrigen"]),
                                 CitaId = Convert.ToInt32(dr["CitaID"]),
                                 CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                                 CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                                 Accion = 1,
                                 Mensaje = "Se cargó correctamente la información de la razón."
                             }).ToList();
                if (CitasList.Count == 0)
                {
                    Citas ss = new Citas();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    CitasList.Add(ss);
                }
            }
            catch (Exception ex)
            {
                Citas oCita = new Citas();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                CitasList.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return CitasList;
        }

        public Citas AsignarTarjetasRazon(Citas pCita)
        {
            SqlCommand cmd = new SqlCommand();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_TarjetasRazon_Save");
                cmd.Parameters.AddWithValue("@CitaId", pCita.CitaId);
                cmd.Parameters["@CitaId"].Direction = ParameterDirection.InputOutput;

                cmd.Parameters.AddWithValue("@TipoRazonId", pCita.TipoRazonId);
                cmd.Parameters.AddWithValue("@RazonId", pCita.RazonId);
                cmd.Parameters.AddWithValue("@TipoOrigenExtraId", pCita.TipoOrigenExtraId);
                cmd.Parameters.AddWithValue("@CodigoListadoOrigenExtraId", pCita.CodigoListadoOrigenExtraId);
                cmd.Parameters.AddWithValue("@DatoExtraId", pCita.DatoExtraId);
                cmd.Parameters.AddWithValue("@RazonOrigen", pCita.RazonOrigen);
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(pCita.CitaCuenta));
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(pCita.CitaTarjeta));
                cmd.Parameters.AddWithValue("@CuentaOriginal", Encrypt(pCita.CuentaOriginal));
                cmd.Parameters.AddWithValue("@TarjetaOriginal", Encrypt(pCita.TarjetaOriginal));

                //con.Open();
                vResultado = Ejecuta_Accion(ref cmd);
                vResultado = Convert.ToInt32(cmd.Parameters["@CitaId"].Value);
                pCita.Accion = vResultado;
                //con.Close();
            }
            catch (Exception Ex)
            {
                pCita.Accion = 0;
                pCita.Mensaje = Ex.Message;
                throw new Exception("Hubo un inconveniente al intentar guardar razón.");
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            pCita.Accion = vResultado;
            if (vResultado == 0)
            {
                pCita.Mensaje = "No se pudo ingresar porque ya existe un registro ó genero una excepción/error!";
            }
            else
            {
                pCita.Mensaje = "Razón guardada correctamente!";
            }
            return pCita;
        }

        public Citas deleteTarjetaRazon(int citaId, int tipoRazonId, int razonId, string datoExtraId, string Cuenta, string Tarjeta)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_TarjetasRazon_Delete");
                cmd.Parameters.AddWithValue("@citaId", citaId);
                cmd.Parameters.AddWithValue("@tipoRazonId", tipoRazonId);
                cmd.Parameters.AddWithValue("@razonId", razonId);
                cmd.Parameters.AddWithValue("@datoExtraId", datoExtraId);
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(Cuenta));
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito la razón!";
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = ex.Message.ToString();
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas deleteTarjeta(int citaId, string Cuenta, string Tarjeta)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_Tarjetas_Delete");
                cmd.Parameters.AddWithValue("@citaId", citaId);
                cmd.Parameters.AddWithValue("@Cuenta",  Encrypt(Cuenta));
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito la razón!";
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 0;
                vResultado.Mensaje = ex.Message.ToString();
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public CitaHerramienta SaveTarjetaHerramienta(CitaHerramienta pHerramienta)
        {
            SqlCommand cmd = new SqlCommand();
            CitaHerramienta obj = new CitaHerramienta();
            int vResultado = -1;
            try
            {
                AbrirConexion();
                //connection();
                cmd = CrearComando("SGRC_SP_TarjetasHerramienta_Save");
                cmd.Parameters.AddWithValue("@CitaId", pHerramienta.CitaId);
                cmd.Parameters.AddWithValue("@HerramientaId", pHerramienta.HerramientaId);
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(pHerramienta.CitaCuenta));
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(pHerramienta.CitaTarjeta));
                cmd.Parameters.AddWithValue("@HerramientaObservacion", pHerramienta.HerramientaObservacion);
                vResultado = Ejecuta_Accion(ref cmd);
                obj = pHerramienta;
                obj.Accion = vResultado;
                if (vResultado == 1)
                {
                    obj.Mensaje = "La información se agrego correctamente!";
                }
                else
                {
                    obj.Mensaje = "No se guardo la herramienta!!";
                }

                return obj;

            }
            catch (Exception ex)
            {
                obj.Accion = 0;
                obj.Mensaje = ex.Message.ToString();
                return obj;
            }
        }

        public List<CitaHerramienta> GetTarjetaHerramientasByCita(int CitaId, string Cuenta, string Tarjeta)
        {
            List<CitaHerramienta> List = new List<CitaHerramienta>();
            try
            {
                SqlCommand cmd = CrearComando("SGRC_SP_GeTarjetastHerramienta_ByCitaId"); //Pasamos el procedimiento almacenado. 
                cmd.Parameters.AddWithValue("@CitaId", CitaId); //Agregamos los parametros. 
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(Cuenta)); //Agregamos los parametros.
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta)); //Agregamos los parametros.
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                AbrirConexion();
                da.Fill(dt);
                CerrarConexion();
                List = (from DataRow dr in dt.Rows
                        select new CitaHerramienta()
                        {
                            HerramientaId = Convert.ToString(dr["HerramientaId"]),
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            HerramientaDescripcion = Convert.ToString(dr["HerramientaDescripcion"]),
                            HerramientaObservacion = Convert.ToString(dr["HerramientaObservacion"]),
                            CitaCuenta = Decryt(Convert.ToString(dr["CitaCuenta"])),
                            CitaTarjeta = Decryt(Convert.ToString(dr["CitaTarjeta"])),
                            Accion = 1,
                            Mensaje = "Se cargó correctamente la información de la razón."
                        }).ToList();
                if (List.Count == 0)
                {
                    CitaHerramienta ss = new CitaHerramienta();
                    ss.Accion = 0;
                    ss.Mensaje = "No se encontraron registros!";
                    List.Add(ss);
                }
            }
            catch (Exception ex)
            {
                CitaHerramienta oCita = new CitaHerramienta();
                oCita.Accion = 0;
                oCita.Mensaje = ex.Message.ToString();
                List.Add(oCita);
                throw new Exception("Error Obteniendo todos los registros " + ex.Message, ex);
            }
            return List;
        }

        public CitaHerramienta deleteTarjetaHerramienta(int citaId, string herramientaId, string Cuenta, string Tarjeta)
        {
            SqlCommand cmd = new SqlCommand();
            CitaHerramienta vResultado = new CitaHerramienta();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_TarjetasHerramienta_Delete");
                cmd.Parameters.AddWithValue("@citaId", citaId);
                cmd.Parameters.AddWithValue("@herramientaId", herramientaId);
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(Cuenta));
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se eliminó con exito la herramienta!";
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                throw new Exception("No se pudo eliminar el registro por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas CambiarCuenta(int CitaId, string Segmento, int Emisor, string Cuenta)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_CambiarCuentaCita");
                cmd.Parameters.AddWithValue("@CitaId", CitaId);
                cmd.Parameters.AddWithValue("@CitaSegmentoId", Segmento);
                cmd.Parameters.AddWithValue("@Emisor", Emisor);
                cmd.Parameters.AddWithValue("@Cuenta", Encrypt(Cuenta));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se cambio la cita correctamente!";
                    vResultado.CitaId = CitaId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = CitaId;
                throw new Exception("No se pudo cambiar la cita por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }

        public Citas CambiarTarjeta(int CitaId, string Segmento, int EmisorTar, string Tarjeta)
        {
            SqlCommand cmd = new SqlCommand();
            Citas vResultado = new Citas();
            int vControl = -1;
            try
            {
                cmd = CrearComando("SGRC_SP_CambiarTarjetaCita");
                cmd.Parameters.AddWithValue("@CitaId", CitaId);
                cmd.Parameters.AddWithValue("@CitaSegmentoId", Segmento);
                cmd.Parameters.AddWithValue("@Emisor", EmisorTar);
                cmd.Parameters.AddWithValue("@Tarjeta", Encrypt(Tarjeta));

                AbrirConexion();
                vControl = Ejecuta_Accion(ref cmd);

                if (vControl > 0)
                {
                    vResultado.Accion = 1;
                    vResultado.Mensaje = "Se cambio la cita correctamente!";
                    vResultado.CitaId = CitaId;
                }
            }
            catch (Exception ex)
            {
                vResultado.Accion = 1;
                vResultado.Mensaje = ex.Message.ToString();
                vResultado.CitaId = CitaId;
                throw new Exception("No se pudo cambiar la cita por el siguiente error: " + ex.Message, ex);
            }
            finally
            {
                cmd.Dispose();
                CerrarConexion();
            }
            return vResultado;
        }
		private string Decryt(string data)
        {
            string dataDecrypted = string.Empty;
            try
            {
                if (data != null)
                {
                    if (data.Trim().Length != 0)
                        dataDecrypted = aes.Decrypt(data);
                }
            }
            catch
            {
                dataDecrypted = "Error en Decrypt";
            }
            return dataDecrypted;
        }
        private string Encrypt(string data)
        {
            string dataEncrypted = string.Empty;
            try
            {
                if (data != null)
                {
                    if (data.Trim().Length != 0)
                        dataEncrypted = aes.Encrypt(data);
                }
            }
            catch
            {
                dataEncrypted = "Error en Encrypt";
            }
            return dataEncrypted;
        }
    }
}
