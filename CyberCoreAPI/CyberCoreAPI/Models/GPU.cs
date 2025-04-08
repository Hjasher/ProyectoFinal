using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class GPU : Componentes
	{
		public int Vram { get; set; }
        public string TipoMemoria{ get; set; }
        public double VelocidadReloj { get; set; }

        public GPU() { }
        



    }
}