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
    public class RAMsController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todas las RAM disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de RAM.
        /// Si no se encuentran RAM, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de RAM.</response>
        /// <response code="404">No encontrado. No se encontraron RAM en la base de datos.</response>

        
        public IHttpActionResult GetRAMs()
        {
            var rams = db.Componentes.OfType<RAM>().ToList();

            if (rams == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron RAM´s en la base de datos");
            }
            return Ok(rams);
        }

        /// <summary>
        /// Obtiene una RAM específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la RAM que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene la RAM con el ID especificado.
        /// Si no se encuentra la RAM, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la RAM solicitada.</response>
        /// <response code="404">No encontrado. No se encontró una RAM con el ID especificado.</response>
        [HttpGet]

        [ResponseType(typeof(RAM))]
        public IHttpActionResult GetRAM(int id)
        {
            var ram = db.Componentes.OfType<RAM>().FirstOrDefault(i => i.ID == id);
            if (ram == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la RAM con el id: {id}");
            }

            return Ok(ram);
        }

        /// <summary>
        /// Actualiza una RAM existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la RAM que se desea actualizar.</param>
        /// <param name="ram">Los nuevos datos de la RAM.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra la RAM, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. La RAM se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró una RAM con el ID especificado.</response>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutRAM(int id, RAM ram)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var ramExistente = db.Componentes.OfType<RAM>().FirstOrDefault(p => p.ID == id);
            if (ramExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la RAM con el id: {id}");
            }

            ramExistente.Nombre = ram.Nombre;
            ramExistente.Marca = ram.Marca;
            ramExistente.Precio = ram.Precio;
            ramExistente.Descripcion = ram.Descripcion;
            ramExistente.Stock = ram.Stock;
            ramExistente.Capacidad = ram.Capacidad;
            ramExistente.TipoMemoria = ram.TipoMemoria;
            ramExistente.Frecuencia = ram.Frecuencia;




            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RAMExists(id))
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
                Mensaje = "La RAM se actualizó correctamente",
                Datos_Nuevos = ramExistente

            });
        }

        /// <summary>
        /// Crea una nueva RAM en la base de datos.
        /// </summary>
        /// <param name="ram">Los datos de la RAM que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos de la RAM creada.
        /// </returns>
        /// <response code="201">Creado. La RAM se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(RAM))]
        [HttpPost]
        public IHttpActionResult PostGPU(RAM ram)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(ram);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ram.ID }, ram);
        }

        /// <summary>
        /// Elimina una RAM específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la RAM que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra la RAM, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos de la RAM eliminada.
        /// </returns>
        /// <response code="200">OK. La RAM fue eliminada correctamente.</response>
        /// <response code="404">No encontrado. No se encontró una RAM con el ID especificado.</response>
        [ResponseType(typeof(RAM))]
        [HttpDelete]
        public IHttpActionResult DeleteRAM(int id)
        {
            var rAM = db.Componentes.OfType<RAM>().FirstOrDefault(p => p.ID == id);
            if (rAM == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la RAM con el id: {id}");
            }

            db.Componentes.Remove(rAM);
            db.SaveChanges();

            return Ok(rAM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RAMExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}