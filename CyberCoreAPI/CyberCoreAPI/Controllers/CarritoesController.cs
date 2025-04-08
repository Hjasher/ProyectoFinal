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
    public class CarritoesController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // GET: api/Carritoes
        /// <summary>
        /// Obtiene todos los carritos existentes.
        /// </summary>
        /// <returns>Lista de carritos</returns>
        /// <response code="200">OK - Devuelve la lista de carritos</response>
        /// <response code="404">Not Found - No se encontraron carritos</response>


        public IHttpActionResult GetCarritos()
        {
            var carritos = db.Carritos.ToList();
            if (carritos == null || !carritos.Any())
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron carritos en la base de datos");
            }
            return Ok(carritos);
        }

        // GET: api/Carritoes/5
        /// <summary>
        /// Obtiene un carrito específico por su ID.
        /// </summary>
        /// <param name="id">ID del carrito (entero positivo)</param>
        /// <returns>El carrito solicitado</returns>
        /// <response code="200">OK - Devuelve el carrito solicitado</response>
        /// <response code="404">Not Found - No se encontró el carrito con el ID especificado</response>

        [ResponseType(typeof(Carrito))]
        
        public IHttpActionResult GetCarrito(int id)
        {
            Carrito carrito = db.Carritos.Find(id);
            if (carrito == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el carrito con el id: {id}");
            }

            return Ok(carrito);
        }

        // PUT: api/Carritoes/5
        /// <summary>
        /// Actualiza un carrito existente.
        /// </summary>
        /// <param name="id">ID del carrito a actualizar</param>
        /// <param name="carrito">Datos actualizados del carrito</param>
        /// <returns>Respuesta sin contenido</returns>
        /// <response code="204">No Content - Carrito actualizado correctamente</response>
        /// <response code="400">Bad Request - Datos del carrito inválidos</response>
        /// <response code="404">Not Found - Carrito no encontrado</response>
        /// <response code="409">Conflict - Error de concurrencia (datos modificados por otro usuario)</response>
        
        [ResponseType(typeof(void))]

        public IHttpActionResult PutCarrito(int id, Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*if (id != carrito.ID)
            {
                return BadRequest();
            }*/

            var CarritoOriginal = db.Carritos.Find(id);
            if (CarritoOriginal == null)
            {
                return NotFound();
            }

            carrito.ID = CarritoOriginal.ID;

            carrito.Total = carrito.CalcularTotal();

            db.Entry(carrito).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarritoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Carritoes
        /// <summary>
        /// Crea un nuevo carrito.
        /// </summary>
        /// <param name="carrito">Datos del nuevo carrito</param>
        /// <returns>El carrito recién creado</returns>
        /// <response code="201">Created - Devuelve el carrito creado</response>
        /// <response code="400">Bad Request - Datos del carrito inválidos</response>
        
        [ResponseType(typeof(Carrito))]

        public IHttpActionResult PostCarrito(Carrito carrito)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carritos.Add(carrito);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = carrito.ID }, carrito);
        }

        // DELETE: api/Carritoes/5
        /// <summary>
        /// Elimina un carrito existente.
        /// </summary>
        /// <param name="id">ID del carrito a eliminar</param>
        /// <returns>El carrito eliminado</returns>
        /// <response code="200">OK - Devuelve el carrito eliminado</response>
        /// <response code="404">Not Found - Carrito no encontrado</response>
       
        [ResponseType(typeof(Carrito))]

        public IHttpActionResult DeleteCarrito(int id)
        {
            Carrito carrito = db.Carritos.Find(id);
            if (carrito == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró al carrito con el id: {id}");
            }

            db.Carritos.Remove(carrito);
            db.SaveChanges();

            return Ok(carrito);
        }

        /// <summary>
        /// Elimina un componente específico de un carrito.
        /// </summary>
        /// <param name="carritoId">ID del carrito</param>
        /// <param name="componenteId">ID del componente a eliminar</param>
        /// <returns>Respuesta sin contenido</returns>
        /// <response code="204">No Content - Componente eliminado correctamente</response>
        /// <response code="404">Not Found - Carrito o componente no encontrado</response>
        
        [HttpDelete]
        [Route("api/carritos/{carritoId}/componentes/{componenteId}")]

        public IHttpActionResult DeleteComponenteFromCarrito(int carritoId, int componenteId)
        {
            var carrito = db.Carritos.Include(c => c.Componentes).FirstOrDefault(c => c.ID == carritoId );
            if (carrito == null)
            {
                return NotFound();
            }

            var componente = carrito.Componentes.FirstOrDefault(c => c.ID == componenteId);
            if (componente == null)
            {
                return NotFound();
            }

            carrito.Componentes.Remove(componente);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarritoExists(int id)
        {
            return db.Carritos.Count(e => e.ID == id) > 0;
        }
    }
}