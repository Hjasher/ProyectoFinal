using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Carrito
	{
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public List<Componentes> Componentes { get; set; }
        public double Total { get; set; }
    }
}