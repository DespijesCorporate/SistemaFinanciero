using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace appcitas.Dtos
{
    public class ConfigItemDto
    {
        [Key]
        public string ConfigItemID { get; set; }
        public string ConfigID { get; set; }
        public string ConfigItemDescripcion { get; set; }
        public string ConfigItemAbreviatura { get; set; }
    }
}