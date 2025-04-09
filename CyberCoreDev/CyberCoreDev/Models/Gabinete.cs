using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Gabinete : Componentes
    {
        [Display(Order = 8)]
        public string Formato { get; set; }

        [Display(Order = 9)]
        public bool VentanaAcrilico { get; set; }

    }
}