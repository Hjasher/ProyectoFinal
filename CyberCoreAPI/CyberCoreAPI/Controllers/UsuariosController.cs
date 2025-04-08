using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CyberCoreAPI.Models;

namespace CyberCoreAPI.Controllers
{
    public class UsuariosController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // GET: api/Usuarios
        /// <summary>
        /// Obtiene todos los usuarios (Solo Admin)
        /// </summary>
        /// <returns>Lista completa de usuarios</returns>
        /// <response code="200">OK - Lista de usuarios</response>
        /// <response code="401">Unauthorized - No autenticado</response>
        /// <response code="403">Forbidden - No tiene rol Admin</response>
        /// <response code="404">Not Found - No hay usuarios registrados</response>

        [Route("api/usuario")]
        public IHttpActionResult GetUsuario()
        {
            var usuarios = db.Usuario.ToList();
            if (usuarios == null || !usuarios.Any())
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron usuarios en la base de datos");
            }
            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        /// <summary>
        /// Obtiene un usuario específico por ID
        /// </summary>
        /// <param name="id">ID del usuario (requiere ser el propio usuario o Admin)</param>
        /// <returns>Datos del usuario (sin contraseña)</returns>
        /// <response code="200">OK - Usuario encontrado</response>
        /// <response code="401">Unauthorized - No autenticado</response>
        /// <response code="403">Forbidden - No es el usuario dueño ni Admin</response>
        /// <response code="404">Not Found - Usuario no existe</response>
        [ResponseType(typeof(Usuario))]
        [HttpGet]
        [Route("api/usuario/{id}")]
        public IHttpActionResult GetUsuario(int id)
        {
            var usuario = db.Usuario
                .Select(u => new
                {
                    u.ID,
                    u.Nombre,
                    u.Correo,
                    u.esAdmin
                })
                .FirstOrDefault(u => u.ID == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Verificar si el usuario actual es admin o el propio usuario
            var usuarioActualId = int.Parse(User.Identity.Name);
            if (usuarioActualId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/usuario/{id}")]
        public IHttpActionResult PutUsuario(/*int id, */Usuario usuario)
        {
            int id = usuario.ID;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*if (id != usuario.ID)
            {
                return BadRequest();
            }*/

            //db.Entry(usuario).State = EntityState.Modified;

            var usuarioExistente = db.Usuario.Find(id);
            if (usuarioExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró al usuario con el id: {id}");
            }

            // obtenemos el id del usuario actual a traves de las claims del token JWT
            var usuarioActualId = int.Parse(User.Identity.Name);

            // Verificamos si el usuario actual es el mismo que el usuario a modificar o si es un administrador
            if (usuarioActualId != id && !User.IsInRole("Admin"))
            {
                return Unauthorized(); // Si no es el usuario actual o un admin no tiene permisos para modificar 
            }

            
            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Correo = usuario.Correo;

            // Verificamos si se proporcionó una nueva contraseña
            if (!string.IsNullOrEmpty(usuario.Contraseña))
            {
                usuarioExistente.Contraseña = BCrypt.Net.BCrypt.HashPassword(usuario.Contraseña);
            }

            db.SaveChanges(); 

            return Ok(usuarioExistente);

        }

        // POST: api/Usuarios
        /// <summary>
        /// Crea un nuevo usuario (No requiere autenticación)
        /// </summary>
        /// <param name="usuario">Datos del nuevo usuario</param>
        /// <returns>Usuario creado (sin contraseña)</returns>
        /// <response code="201">Created - Usuario registrado</response>
        /// <response code="400">Bad Request - Datos inválidos</response>
        /// <response code="409">Conflict - El correo ya está registrado</response>
        [ResponseType(typeof(Usuario))]
        [HttpPost]
        [Route("api/usuario")]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Con esto verificamos si el correo ya está registrado
            if (db.Usuario.Any(u => u.Correo == usuario.Correo))
            {
                return Conflict(); // Devuelve error 409 (Conflicto) si el correo ya existe
            }

            // Por defecto asignamos el rol de no administrador
            usuario.esAdmin = false;


            db.Usuario.Add(usuario);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuario.ID }, usuario);
        }

        // DELETE: api/Usuarios/5
        /// <summary>
        /// Elimina un usuario (Solo Admin)
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Confirmación de eliminación</returns>
        /// <response code="200">OK - Usuario eliminado</response>
        /// <response code="401">Unauthorized - No autenticado</response>
        /// <response code="403">Forbidden - No tiene rol Admin</response>
        /// <response code="404">Not Found - Usuario no existe</response>
        [ResponseType(typeof(Usuario))]
        [HttpDelete]
        [Route("api/usuario/{id}")]
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el usuario con el id: {id}");
            }

            db.Usuario.Remove(usuario);
            db.SaveChanges();

            return Content(HttpStatusCode.OK, new
            {
                Mensaje = $"El usuario fue eliminado correctamente",
                Usuario_Eliminado = usuario
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuario.Count(e => e.ID == id) > 0;
        }
    }
}