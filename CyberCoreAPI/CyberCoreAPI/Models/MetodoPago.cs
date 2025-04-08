using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public abstract class MetodoPago
	{
		
		public int ID { get; set; }
		public string Detalles { get; set; }
        public abstract string TipoPago { get; }

		public MetodoPago() { }

		public abstract bool Validar();

    }
}