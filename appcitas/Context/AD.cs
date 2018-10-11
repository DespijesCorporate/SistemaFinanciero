using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WSValidarUsuario;

namespace appcitas.Context
{
    public class AD
    {
        public Boolean CheckUserAD(string user, string pass)
        {
            WSValidarUsuario.AuthenticationService.AuthenticationService ws = new WSValidarUsuario.AuthenticationService.AuthenticationService();
            ws.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            Boolean ValidaUsuario = ws.ValidarUsuario(user, pass, "BAC_NT");
            return ValidaUsuario;
        }
    }
}