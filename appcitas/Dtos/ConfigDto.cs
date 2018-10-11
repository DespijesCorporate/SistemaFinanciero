using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ConfigDto
    {
        [Key]
        public string ConfigID { set; get; }
        public string ConfigDesc { set; get; }
    }
}