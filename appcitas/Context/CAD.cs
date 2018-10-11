using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos;
using System.Data.SqlClient;
using System.Configuration; //Libreria que ayuda a obtener el stringconexion del web.config

namespace appcitas.Context
{
    public class CAD : AccesoSQL
    {
        public CAD()
        {
            cConex = System.Configuration.ConfigurationManager.ConnectionStrings["appcitas.Properties.Settings.Setting"].ConnectionString;
            //cConex = "Data Source=.;Initial Catalog=ContratosTarjetasCredito;trusted_Connection=Yes;"; //Creamos la cadena de conexion.
            conexion = new SqlConnection(cConex); //Cargamos la cadena de conexion.
        }
    }
}