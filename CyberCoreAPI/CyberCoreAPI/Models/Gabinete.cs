using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class Gabinete : Componentes
	{
        public string Formato { get; set; }
        public bool VentanaAcrilico { get; set; }
        

        public Gabinete() { }

    }
}