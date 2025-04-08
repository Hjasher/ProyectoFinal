using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class Usuario
	{
		public int ID { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public bool esAdmin { get; set; }

        public Usuario() { }

    }
}