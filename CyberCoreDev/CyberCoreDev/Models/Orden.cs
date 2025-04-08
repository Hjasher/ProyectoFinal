using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Orden
	{
        public enum EstadoOrden
        {
            Pendiente,
            Procesada,
            Enviada,
            Completada,
            Cancelada
        }
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }
        public List<Componentes> Componentes { get; set; }
        public int CarritoID { get; set; }
        public double Total { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public EstadoOrden Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}