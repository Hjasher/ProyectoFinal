using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
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
        public EstadoOrden Estado{ get; set; }
        public DateTime Fecha { get; set; }


        public Orden() { }

        public bool Validar()
        {
            // Validar que la orden tenga al menos un componente
            if (Componentes == null || Componentes.Count == 0)
            {
                return false;
            }

            // Validar que el total sea mayor que 0
            if (Total <= 0)
            {
                return false;
            }

            // Validar que haya un método de pago
            if (MetodoPago == null)
            {
                return false;
            }

            // Validar el método de pago
            if (!MetodoPago.Validar())
            {
                return false;
            }

            return true; // si todo es correcto retornamos true
        }
        public double CalcularTotal()
        {
            return Componentes.Sum(c => c.Precio);
        }

    }
}