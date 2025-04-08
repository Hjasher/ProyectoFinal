using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class Procesador : Componentes
	{
		public string Socket { get; set; }
        public int Nucleos { get; set; }
        public int Hilos { get; set; }
        public double Frecuencia { get; set; }


        public Procesador() { }



    }
}