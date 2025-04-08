using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class DBContextProject : DbContext
	{
		public DBContextProject() : base("MyDbConnectionString") 
        {
            Database.Log = Console.WriteLine;
        }

		public DbSet<Componentes> Componentes { get; set; }
        public DbSet<Procesador> Procesadores { get; set; }
        public DbSet<GPU> GPUs { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<Almacenamiento> Almacenamientos { get; set; }
        public DbSet<PlacaBase> PlacaBases { get; set; }
        public DbSet<FuentePoder> FuentesPoder { get; set; }
        public DbSet<Gabinete> Gabinetes { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<MetodoPago> MetodoPagos { get; set; }
        //public DbSet<TarjetaCredito> TarjetasCredito { get; set; }
        //public DbSet<TarjetaDebito> TarjetasDebito { get; set; }
        //public DbSet<PayPal> PayPals { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<ReporteInventario> ReporteInventario { get; set; }
        public DbSet<ReporteVentas> ReporteVentas { get; set; }
        


        
    }
}