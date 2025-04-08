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
using static CyberCoreAPI.Models.Orden;

namespace CyberCoreAPI.Controllers
{
    public class OrdensController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // GET: api/Ordens
        /// <summary>
        /// Obtiene todas las órdenes existentes.
        /// </summary>
        /// <returns>Lista de órdenes</returns>
        /// <response code="200">OK - Devuelve la lista de órdenes</response>
        /// <response code="404">Not Found - No se encontraron órdenes</response>


        
        public IHttpActionResult GetOrdenes()
        {
            var ordenes = db.Ordenes.ToList();
            if (ordenes == null || !ordenes.Any())
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron ordenes en la base de datos");
            }
            return Ok(ordenes);
        }

        // GET: api/Ordens/5
        /// <summary>
        /// Obtiene una orden específica por su ID.
        /// </summary>
        /// <param name="id">ID de la orden (entero positivo)</param>
        /// <returns>La orden solicitada</returns>
        /// <response code="200">OK - Devuelve la orden solicitada</response>
        /// <response code="404">Not Found - Orden no encontrada</response>

        [HttpGet]
        [ResponseType(typeof(Orden))]
        public IHttpActionResult GetOrden(int id)
        {
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la orden con el id: {id}");
            }

            return Ok(orden);
        }

        /// <summary>
        /// Obtiene el historial de órdenes de un usuario específico.
        /// </summary>
        /// <param name="usuarioId">ID del usuario (entero positivo)</param>
        /// <returns>Lista de órdenes del usuario</returns>
        /// <response code="200">OK - Devuelve el historial de órdenes</response>
        /// <response code="404">Not Found - No se encontraron órdenes para el usuario</response>
        
        [HttpGet]
        [Route("api/usuarios/{usuarioId}/ordenes")]
        public IHttpActionResult GetHistorialOrdenes(int usuarioId)
        {
            var ordenes = db.Ordenes.Where(o => o.UsuarioID == usuarioId).ToList();
            if (ordenes == null || ordenes.Count == 0)
            {
                return NotFound();
            }

            return Ok(ordenes);
        }

        /// <summary>
        /// Actualiza una orden existente.
        /// </summary>
        /// <param name="id">ID de la orden a actualizar</param>
        /// <param name="orden">Datos actualizados de la orden</param>
        /// <returns>Respuesta sin contenido</returns>
        /// <response code="204">No Content - Orden actualizada correctamente</response>
        /// <response code="400">Bad Request - Datos inválidos</response>
        /// <response code="404">Not Found - Orden no encontrada</response>
        /// <response code="409">Conflict - Error de concurrencia</response>
        // PUT: api/Ordens/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrden(int id, Orden orden)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*if (id != orden.ID)
            {
                return BadRequest();
            }*/

            var OrdenOriginal = db.Ordenes.Find(id);
            if (OrdenOriginal == null)
            {
                return NotFound();
            }

            orden.ID = OrdenOriginal.ID;
            orden.Total = orden.CalcularTotal();

            db.Entry(orden).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdenExists(id))
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

        /// <summary>
        /// Actualiza el estado de una orden existente.
        /// </summary>
        /// <param name="id">ID de la orden</param>
        /// <param name="nuevoEstado">Nuevo estado de la orden</param>
        /// <returns>Respuesta sin contenido</returns>
        /// <response code="204">No Content - Estado actualizado correctamente</response>
        /// <response code="404">Not Found - Orden no encontrada</response>
        [HttpPut]
        [Route("api/ordenes/{id}/estado")]
        public IHttpActionResult UpdateEstadoOrden(int id, EstadoOrden nuevoEstado)
        {
            var orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return NotFound();
            }

            orden.Estado = nuevoEstado;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Ordens
        /// <summary>
        /// Crea una nueva orden.
        /// </summary>
        /// <param name="orden">Datos de la nueva orden</param>
        /// <returns>La orden recién creada</returns>
        /// <response code="201">Created - Devuelve la orden creada</response>
        /// <response code="400">Bad Request - Datos inválidos</response>
        [HttpPost]
        [ResponseType(typeof(Orden))]
        public IHttpActionResult PostOrden(Orden orden)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ordenes.Add(orden);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = orden.ID }, orden);
        }

        // DELETE: api/Ordens/5
        /// <summary>
        /// Elimina una orden existente.
        /// </summary>
        /// <param name="id">ID de la orden a eliminar</param>
        /// <returns>La orden eliminada</returns>
        /// <response code="200">OK - Devuelve la orden eliminada</response>
        /// <response code="404">Not Found - Orden no encontrada</response>
        [HttpDelete]
        [ResponseType(typeof(Orden))]
        public IHttpActionResult DeleteOrden(int id)
        {
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la orden con el id: {id}");
            }

            db.Ordenes.Remove(orden);
            db.SaveChanges();

            return Ok(orden);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdenExists(int id)
        {
            return db.Ordenes.Count(e => e.ID == id) > 0;
        }
    }
}