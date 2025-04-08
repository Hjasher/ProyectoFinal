using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class ReporteInventario
	{
		public int ID { get; set; }
		public DateTime Fecha { get; set; }
		public List<Componentes> Componentes { get; set; }
		public int TotalStock { get; set; }

		public ReporteInventario() { }

        public void CalcularTotalStock()
        {
            TotalStock = Componentes.Sum(c => c.Stock);
        }
    }
}