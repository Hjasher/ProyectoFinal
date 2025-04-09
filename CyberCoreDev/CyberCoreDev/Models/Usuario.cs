using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreDev.Models
{
	public class Usuario
	{
        public int ID { get; set; }
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un Correo Valido (ejemplo@gmail.com)")]
        public string Correo { get; set; }

        [Required, MinLength(8)]
        public string Contraseña { get; set; }
        
        public bool esAdmin { get; set; }
    }
}