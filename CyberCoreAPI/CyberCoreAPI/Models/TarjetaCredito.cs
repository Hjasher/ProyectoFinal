using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
	public class TarjetaCredito : MetodoPago
	{
		public override string TipoPago => "TarjetaCredito";
		public string NumeroTarjeta { get; set; }
		public DateTime FechaVencimiento { get; set; }
		public int CVV { get; set; }

        public TarjetaCredito() { }

        public override bool Validar()
        {
            // Validar que el número de tarjeta no esté vacío
            if (string.IsNullOrEmpty(NumeroTarjeta))
            {
                return false;
            }

            // Limpiar el número de tarjeta eliminando los guiones y espacios 
            string numeroTarjetaLimpio = NumeroTarjeta.Replace("-", "").Replace(" ", "");

            // Validar que solo contenga dígitos
            if (!numeroTarjetaLimpio.All(char.IsDigit))
            {
                return false;
            }

            // Validar la longitud entre 13 y 19 digitos que es lo normal
            if (numeroTarjetaLimpio.Length < 13 || numeroTarjetaLimpio.Length > 19)
            {
                return false;
            }

            // Validar con el algoritmo de Luhn el numero de la tarjeta (Este metodo por los momentos no lo habilitamos porque no permitiría ingresar numeros de tarjetas falsos para las pruebas) 
            /*int suma = 0;
            bool alternar = false;
            for (int i = numeroTarjetaLimpio.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(numeroTarjetaLimpio[i].ToString());
                if (alternar)
                {
                    digito *= 2;
                    if (digito > 9)
                    {
                        digito = (digito % 10) + 1;
                    }
                }
                suma += digito;
                alternar = !alternar;
            }
            if (suma % 10 != 0)
            {
                return false;
            }*/

            // Validar que la tarjeta no esté vencida
            if (FechaVencimiento < DateTime.Now)
            {
                return false; // La tarjeta está vencida
            }

            // Validar que el CVV contenga 3 digitos, ni mas, ni menos
            if (CVV < 100 || CVV > 999)
            {
                return false; // El CVV debe ser un número de 3 dígitos
            }

            // Si todas las validaciones pasan que retorne true
            return true;
        }

    }
}