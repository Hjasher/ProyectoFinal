using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class Almacenamiento : Componentes
	{
		public int Capacidad { get; set; }
        public string TipoAlmacenamiento{ get; set; }
        public double VelocidadLectura { get; set; }
        public int VelocidadEscritura { get; set; }

        public Almacenamiento() { }
       

    }
}