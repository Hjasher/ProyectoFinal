using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Procesador : Componentes
    {
        [Display(Order = 8)]
        public string Socket { get; set; }

        [Display(Order = 9)]
        public int Nucleos { get; set; }

        [Display(Order = 10)]
        public int Hilos { get; set; }

        [Display(Order = 11)]
        public double Frecuencia { get; set; }

    }
}