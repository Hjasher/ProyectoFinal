using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Almacenamiento : Componentes
	{
        [Display(Order = 8)]
        public int Capacidad { get; set; }

        [Display(Order = 9)]
        public string TipoAlmacenamiento { get; set; }

        [Display(Order = 10)]
        public double VelocidadLectura { get; set; }

        [Display(Order = 11)]
        public int VelocidadEscritura { get; set; }

    }
}