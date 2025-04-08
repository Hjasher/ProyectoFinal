using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class RAM : Componentes
	{
		public int Capacidad { get; set; }
        public string TipoMemoria{ get; set; }
        public int Frecuencia { get; set; }

        public RAM() { }


    }
}