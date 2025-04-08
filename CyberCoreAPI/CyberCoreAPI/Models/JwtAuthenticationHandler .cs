using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CyberCoreAPI.Models
{
    public class JwtAuthenticationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 1. Validar header Authorization
            if (request.Headers.Authorization == null ||
                !request.Headers.Authorization.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Falta token Bearer");
            }

            // 2. Validar token JWT
            try
            {
                var token = request.Headers.Authorization.Parameter;
                var principal = ValidateJwtToken(token);

                // Establecer el principal en el hilo y contexto HTTP
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Token expirado");
            }
            catch (SecurityTokenValidationException)
            {
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Token inválido");
            }
            catch (Exception ex)
            {
                return request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    $"Error interno: {ex.Message}");
            }

            return await base.SendAsync(request, cancellationToken);
        }

        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "CyberCoreAPI",
                ValidAudience = "CyberCoreAPI",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }
    }
}