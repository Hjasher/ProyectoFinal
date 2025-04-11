using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
    public class Componentes
    {
        /*public enum CategoriaComponente
        {
            Procesador,
            MemoriaRAM,
            TarjetaGrafica,
            Almacenamiento,
            PlacaBase,
            FuenteDePoder,
            Gabinete,
        }*/


        [Key]
        public int ID { get; set; }


        public string Nombre { get; set; }


        public string Marca { get; set; }


        public double Precio { get; set; }


        public string Descripcion { get; set; }


        public string Categoria { get; set; }


        public int Stock { get; set; }

        


    }
}