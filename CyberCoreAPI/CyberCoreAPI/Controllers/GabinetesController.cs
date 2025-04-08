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
    public class GabinetesController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todos los gabinetes disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de gabinetes.
        /// Si no se encuentran gabinetes, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de gabinetes.</response>
        /// <response code="404">No encontrado. No se encontraron gabinetes en la base de datos.</response>

        
        public IHttpActionResult GetGabinete()
        {
            var gabinete = db.Componentes.OfType<Gabinete>().ToList();

            if (gabinete == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron gabinetes en la base de datos");
            }
            return Ok(gabinete);
        }

        /// <summary>
        /// Obtiene un gabinete específico por su ID.
        /// </summary>
        /// <param name="id">El ID del gabinete que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene el gabinete con el ID especificado.
        /// Si no se encuentra el gabinete, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve el gabinete solicitado.</response>
        /// <response code="404">No encontrado. No se encontró un gabinete con el ID especificado.</response>
        [HttpGet]
        [ResponseType(typeof(Gabinete))]
        public IHttpActionResult GetGabinete(int id)
        {
            var gabinete = db.Componentes.OfType<Gabinete>().FirstOrDefault(i => i.ID == id);
            if (gabinete == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el gabinete con el id: {id}");
            }

            return Ok(gabinete);
        }

        /// <summary>
        /// Actualiza un gabinete existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID del gabinete que se desea actualizar.</param>
        /// <param name="gabinete">Los nuevos datos del gabinete.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra el gabinete, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. El gabinete se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró un gabinete con el ID especificado.</response>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutGabinete(int id, Gabinete gabinete)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var gabineteExistente = db.Componentes.OfType<Gabinete>().FirstOrDefault(p => p.ID == id);
            if (gabineteExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró al gabinete con el id: {id}");
            }

            gabineteExistente.Nombre = gabinete.Nombre;
            gabineteExistente.Marca = gabinete.Marca;
            gabineteExistente.Precio = gabinete.Precio;
            gabineteExistente.Descripcion = gabinete.Descripcion;
            gabineteExistente.Stock = gabinete.Stock;
            gabineteExistente.Formato = gabinete.Formato;
            gabineteExistente.VentanaAcrilico = gabinete.VentanaAcrilico;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GabineteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content(HttpStatusCode.OK, new
            {
                Mensaje = "El gabinete se actualizó correctamente",
                Datos_Nuevos = gabineteExistente

            });
        }


        /// <summary>
        /// Crea un nuevo gabinete en la base de datos.
        /// </summary>
        /// <param name="gabinete">Los datos del gabinete que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos del gabinete creado.
        /// </returns>
        /// <response code="201">Creado. El gabinete se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(Gabinete))]
        [HttpPost]
        public IHttpActionResult PostGabinete(Gabinete gabinete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(gabinete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gabinete.ID }, gabinete);
        }

        /// <summary>
        /// Elimina un gabinete específico por su ID.
        /// </summary>
        /// <param name="id">El ID del gabinete que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra el gabinete, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos del gabinete eliminado.
        /// </returns>
        /// <response code="200">OK. El gabinete fue eliminado correctamente.</response>
        /// <response code="404">No encontrado. No se encontró un gabinete con el ID especificado.</response>
        [ResponseType(typeof(Gabinete))]
        [HttpDelete]
        public IHttpActionResult DeleteGabinete(int id)
        {
            var gabinete = db.Componentes.OfType<Gabinete>().FirstOrDefault(p => p.ID == id);
            if (gabinete == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró al gabinete con el id: {id}");
            }

            db.Componentes.Remove(gabinete);
            db.SaveChanges();

            return Ok(gabinete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GabineteExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}