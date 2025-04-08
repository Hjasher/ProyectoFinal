using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class ReporteVentas
	{
        public int ID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public double TotalVentas { get; set; }
        public List<Orden> Ordenes { get; set; }
    }
}