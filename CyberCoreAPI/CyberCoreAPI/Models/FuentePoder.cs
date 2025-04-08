using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class FuentePoder : Componentes 
	{
        public int Potencia { get; set; }
        public string Certificacion { get; set; }


        public FuentePoder() { }


    }
}