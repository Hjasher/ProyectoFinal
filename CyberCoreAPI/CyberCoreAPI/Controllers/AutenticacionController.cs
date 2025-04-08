using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using BCrypt.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CyberCoreAPI.Models;
using System.Web.Helpers;
using System;
using System.Configuration;
using System.Web.Http.Description;

namespace CyberCoreAPI.Controllers
{ 
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly DBContextProject _context;

        public AuthController()
        {
            _context = new DBContextProject(); 
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// POST /api/auth/login
        /// {
        ///     "correo": "usuario@example.com",
        ///     "contraseña": "MiClaveSegura123"
        /// }
        /// 
        /// El token JWT devuelto debe incluirse en el header 'Authorization' de solicitudes subsiguientes:
        /// Authorization: Bearer [token]
        /// </remarks>
        /// <param name="loginDTO">Credenciales de acceso</param>
        /// <returns>Token JWT para autenticación</returns>
        /// <response code="200">OK - Devuelve el token JWT</response>
        /// <response code="400">Bad Request - Datos inválidos</response>
        /// <response code="401">Unauthorized - Credenciales incorrectas</response>
        [HttpPost]
        [Route("login")]
        [ResponseType(typeof(TokenResponse))]
        public IHttpActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = _context.Usuario
                .FirstOrDefault(u => u.Correo == loginDTO.Correo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Contraseña, usuario.Contraseña))
            {
                return Unauthorized();
            }

            var token = GenerarToken(usuario.ID, usuario.esAdmin);

            return Ok(new TokenResponse { Token = token });
        }

        /// <summary>
        /// Genera un token JWT para el usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <param name="esAdmin">Indica si el usuario tiene rol de administrador</param>
        /// <returns>Token JWT firmado</returns>
        private string GenerarToken(int usuarioId, bool esAdmin)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString())
        };

            if (esAdmin)
            {
                claims = claims.Concat(new[]
                {
                new Claim(ClaimTypes.Role, "Admin")
            }).ToArray();
            }

            var secretKey = ConfigurationManager.AppSettings["JwtSecretKey"];
            var issuer = ConfigurationManager.AppSettings["JwtIssuer"];
            var audience = ConfigurationManager.AppSettings["JwtAudience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    
    }
}
