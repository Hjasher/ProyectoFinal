using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class PayPal : MetodoPago
	{
		public override string TipoPago => "PayPal";
		public string Email { get; set; }

        public PayPal() { }
        public override bool Validar()
        {
            if (string.IsNullOrEmpty(Email)) return false; // El email está vacío


            // Expresión para validar el formato del correo electrónico
            // verificamos que el correo tenga un @
            // tambien que contenga como minimo un punto despues del @
            // por ultimo verificamos que no haya espacios en blanco
            string regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(Email, regex))
            {
                return false;
            }


            /*
            // Verificar que el correo no esté en una lista de correos bloqueados este metodo sería utilizado en casos de tener una lista con correos bloqueados 
             
            string[] correosBloqueados = { "fraude@example.com", "spam@test.com" };
            if (correosBloqueados.Contains(Email))
            {
                return false;
            }
            */

            return true; // retornamos true si las validaciones pasan las pruebas 
        }

    }
}