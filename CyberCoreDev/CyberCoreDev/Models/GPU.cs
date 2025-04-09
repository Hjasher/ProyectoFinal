using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class GPU : Componentes
    {
        [Display(Order = 8)]
        public int Vram { get; set; }

        [Display(Order = 9)]
        public string TipoMemoria { get; set; }

        [Display(Order = 10)]
        public double VelocidadReloj { get; set; }
    }
}