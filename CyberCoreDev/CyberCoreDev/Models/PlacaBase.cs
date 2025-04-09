using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class PlacaBase : Componentes
    {
        [Display(Order = 8)]
        public string Socket { get; set; }

        [Display(Order = 9)]
        public string Formato { get; set; }

        [Display(Order = 10)]
        public int SlotsRAM { get; set; }

        [Display(Order = 11)]
        public int SlotsPCIe { get; set; }

    }
}