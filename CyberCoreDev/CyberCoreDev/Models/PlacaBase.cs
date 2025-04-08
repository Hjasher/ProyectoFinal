using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class PlacaBase
	{
        public string Socket { get; set; }
        public string Formato { get; set; }
        public int SlotsRAM { get; set; }
        public int SlotsPCIe { get; set; }

    }
}