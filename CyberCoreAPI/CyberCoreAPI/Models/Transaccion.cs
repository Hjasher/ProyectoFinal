using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyberCoreAPI.Models
{
    public class Transaccion
    {
        
        public int ID { get; set; } 
        public int OrdenID { get; set; } 
        public MetodoPago MetodoPago { get; set; } 
        public DateTime FechaTransaccion { get; set; } 
        public float Monto { get; set; } 
        public string Estado { get; set; } // Estado de la transacción (Pendiente, Completada, Fallida)
        public string CodigoAutorizacion { get; set; } 
        public string MensajeError { get; set; } 

        
        public Transaccion()
        {
            FechaTransaccion = DateTime.Now; 
            Estado = "Pendiente"; // Estado inicial
        }


        /*// Método para procesar la transacción con PayPal (Metodo aún en prueba)
        public bool Procesar()
        {
            // Validar que el método de pago sea válido
            if (MetodoPago == null || !MetodoPago.Validar())
            {
                Estado = "Fallida";
                MensajeError = "Método de pago no válido.";
                return false;
            }

            // Validar que el monto sea mayor que 0
            if (Monto <= 0)
            {
                Estado = "Fallida";
                MensajeError = "El monto de la transacción debe ser mayor que 0.";
                return false;
            }

            try
            {
                // Configurar las credenciales de PayPal
                var config = new Dictionary<string, string>
            {
                { "mode", "sandbox" }, // Usar el entorno de pruebas (sandbox)
                { "clientId", "TU_CLIENT_ID" }, // Reemplaza con tu Client ID
                { "clientSecret", "TU_CLIENT_SECRET" } // Reemplaza con tu Client Secret
            };

                var accessToken = new OAuthTokenCredential(config).GetAccessToken();
                var apiContext = new APIContext(accessToken) { Config = config };

                // Crear un pago con PayPal
                var pago = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        amount = new Amount
                        {
                            currency = "USD", // Moneda
                            total = Monto.ToString("F2") // Monto total
                        },
                        description = $"Pago de la orden {OrdenID}" // Descripción del pago
                    }
                },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = "https://tutienda.com/success", // URL de retorno después del pago
                        cancel_url = "https://tutienda.com/cancel" // URL de cancelación
                    }
                };

                // Crear el pago en PayPal
                var pagoCreado = pago.Create(apiContext);

                // Verificar si el pago fue aprobado
                if (pagoCreado.state.ToLower() == "approved")
                {
                    Estado = "Completada";
                    CodigoAutorizacion = pagoCreado.id; // ID del pago en PayPal
                    return true;
                }
                else
                {
                    Estado = "Fallida";
                    MensajeError = "El pago no fue aprobado.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Estado = "Fallida";
                MensajeError = ex.Message;
                return false;
            }
        }*/

        /*// genera un codigo de autorizacion simulado 
        private string GenerarCodigoAutorizacion()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Código de 8 caracteres
        }*/

        // detalles de la transacción 
        public string ObtenerDetalles()
        {
            return $"Transacción ID: {ID}\n" +
                   $"Orden ID: {OrdenID}\n" +
                   $"Método de Pago: {MetodoPago.TipoPago}\n" +
                   $"Fecha: {FechaTransaccion}\n" +
                   $"Monto: {Monto:C}\n" +
                   $"Estado: {Estado}\n" +
                   $"Código de Autorización: {CodigoAutorizacion}\n" +
                   $"Mensaje de Error: {MensajeError}";
        }
    }
}