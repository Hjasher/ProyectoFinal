using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Almacenamiento
	{
        public int Capacidad { get; set; }
        public string TipoAlmacenamiento { get; set; }
        public double VelocidadLectura { get; set; }
        public int VelocidadEscritura { get; set; }

    }
}