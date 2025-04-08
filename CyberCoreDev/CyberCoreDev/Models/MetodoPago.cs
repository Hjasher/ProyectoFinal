using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace CyberCoreDev.Models
{
	public class MetodoPago
	{
        public enum TipoPagoValido
        {
            PayPal,
            TarjetaCredito,
            TarjetaDebito
        }
        [Key]
        [Required(ErrorMessage = "El ID es obligatorio")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Los detalles son obligatorios")]
        [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string Detalles { get; set; }

        [Required(ErrorMessage = "El tipo de pago es obligatorio")]
        public TipoPagoValido TipoPago { get; set; }

        // Validación para PayPal
        [EmailAddress(ErrorMessage = "Email inválido")]
        [RequiredIf(nameof(TipoPago), TipoPagoValido.PayPal,
            ErrorMessage = "El email es obligatorio para PayPal")]
        public string Email { get; set; }

        // Validación para tarjetas (combinada)
        [CreditCard(ErrorMessage = "Número de tarjeta inválido")]
        [RequiredIf(nameof(TipoPago),
            new object[] { TipoPagoValido.TarjetaCredito, TipoPagoValido.TarjetaDebito },
            ErrorMessage = "El número de tarjeta es obligatorio para tarjetas")]
        public string NumeroTarjeta { get; set; }

        [RequiredIf(nameof(TipoPago),
            new object[] { TipoPagoValido.TarjetaCredito, TipoPagoValido.TarjetaDebito },
            ErrorMessage = "La fecha de vencimiento es obligatoria para tarjetas")]
        [FutureDate(ErrorMessage = "La fecha debe ser futura")]
        public DateTime FechaVencimiento { get; set; }

        [Range(100, 999, ErrorMessage = "CVV debe ser de 3 dígitos")]
        [RequiredIf(nameof(TipoPago),
            new object[] { TipoPagoValido.TarjetaCredito, TipoPagoValido.TarjetaDebito },
            ErrorMessage = "El CVV es obligatorio para tarjetas")]
        public int CVV { get; set; }


    }
}