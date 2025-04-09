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

        [Display(Order = 1)]
        [Key]
        public int ID { get; set; }

        [Display(Order = 2)]
        public string Nombre { get; set; }

        [Display(Order = 3)]
        public string Marca { get; set; }

        [Display(Order = 4)]
        public double Precio { get; set; }

        [Display(Order = 5)]
        public string Descripcion { get; set; }

        [Display(Order = 6)]
        public string Categoria { get; set; }

        [Display(Order = 7)]
        public int Stock { get; set; }

        


    }
}