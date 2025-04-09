using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class FuentePoder : Componentes
    {
        [Display(Order = 8)]
        public int Potencia { get; set; }
        [Display(Order = 9)]
        public string Certificacion { get; set; }
    }
}