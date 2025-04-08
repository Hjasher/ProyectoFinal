using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class Categoria
	{
        public int ID { get; set; }
        public string nombre { get; set; }
        public string Descripcion { get; set; }

        public Categoria() { }

    }
}