using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos;
using System.Data.SqlClient;

namespace appcitas.Context
{
    public class SecurityBAC : AccesoSQLSecurity
    {
        public SecurityBAC()
        {
            cConex = System.Configuration.ConfigurationManager.ConnectionStrings["security"].ConnectionString;
            //cConex = "Data Source=.;Initial Catalog=ContratosTarjetasCredito;trusted_Connection=Yes;"; //Creamos la cadena de conexion.
            conexion = new SqlConnection(cConex); //Cargamos la cadena de conexion.
        }
    }
}