using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class LoginDTO
	{
        [Required, EmailAddress]
        public string Correo { get; set; }

        [Required, MinLength(8)]
        public string Contraseña { get; set; }

        public LoginDTO() { }
    }
}