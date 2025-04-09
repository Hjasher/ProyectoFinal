namespace CyberCoreAPI.Migrations
{
    using CyberCoreAPI.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CyberCoreAPI.Models.DBContextProject>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CyberCoreAPI.Models.DBContextProject context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            if (!context.Usuario.Any(u => u.esAdmin))
            {
                context.Usuario.Add(new Usuario
                {
                    Nombre = "Hjasher",
                    Correo = "hjasher204@gmail.com",
                    Contraseña = "1234",
                    esAdmin = true
                });
                context.SaveChanges();

                Console.WriteLine("Usuario admin inicial creado: hjasher204@gmail.com");
            }
        }
    }
}
