using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Procesador
	{
        public string Socket { get; set; }
        public int Nucleos { get; set; }
        public int Hilos { get; set; }
        public double Frecuencia { get; set; }

    }
}