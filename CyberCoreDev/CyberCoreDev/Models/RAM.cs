using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class RAM : Componentes
    {
        [Display(Order = 8)]
        public int Capacidad { get; set; }

        [Display(Order = 9)]
        public string TipoMemoria { get; set; }

        [Display(Order = 10)]
        public int Frecuencia { get; set; }

    }
}