using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace appcitas.Models
{
    public class Clientes
    {
        [Key]
        public string Identidad { get; set; }
        public int ClienteId { get; set; }
        public int cantidadRegistros { get; set; }
        public string RTN { get; set; }
        public string Nombre { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Genero { get; set; }
        public string Edad { get; set; }
        public string EstadoCli { get; set; }
        public string Direccion { get; set; }
        public string EstadoCivil { get; set; }
        public string CliAgrego { get; set; }
        public string TelCasa { get; set; }
        public string Cel { get; set; }
        public string Correo { get; set; }

        //REFERENCIAS DEL CLIENTE
        public string RefNom1 { get; set; }
        public string RefId1 { get; set; }
        public string RefTel1 { get; set; }
        public string RefCel1 { get; set; }
        public string RefNom2 { get; set; }
        public string RefId2 { get; set; }
        public string RefTel2 { get; set; }
        public string RefCel2 { get; set; }


        public int Accion { get; set; }
        public string Mensaje { get; set; }
    }
}