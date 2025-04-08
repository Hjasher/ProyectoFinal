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
    public class AlmacenamientoesController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todas las unidades de almacenamiento disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de unidades de almacenamiento.
        /// Si no se encuentran unidades de almacenamiento, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de unidades de almacenamiento.</response>
        /// <response code="404">No encontrado. No se encontraron unidades de almacenamiento en la base de datos.</response>

        
        public IHttpActionResult GetAlmacenamientos()
        {
            var alms = db.Componentes.OfType<Almacenamiento>().ToList();

            if (alms == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron unidades de almacenamiento en la base de datos");
            }
            return Ok(alms);
        }

        /// <summary>
        /// Obtiene una unidad de almacenamiento específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la unidad de almacenamiento que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene la unidad de almacenamiento con el ID especificado.
        /// Si no se encuentra la unidad de almacenamiento, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la unidad de almacenamiento solicitada.</response>
        /// <response code="404">No encontrado. No se encontró una unidad de almacenamiento con el ID especificado.</response>
        [ResponseType(typeof(Almacenamiento))]

        public IHttpActionResult GetAlmacenamiento(int id)
        {
            var alms = db.Componentes.OfType<Almacenamiento>().FirstOrDefault(i => i.ID == id);
            if (alms == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la unidad de Almacenamiento con el id: {id}");
            }

            return Ok(alms);
        }

        /// <summary>
        /// Actualiza una unidad de almacenamiento existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la unidad de almacenamiento que se desea actualizar.</param>
        /// <param name="alm">Los nuevos datos de la unidad de almacenamiento.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra la unidad de almacenamiento, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. La unidad de almacenamiento se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró una unidad de almacenamiento con el ID especificado.</response>
        [ResponseType(typeof(void))]

        [HttpPut]
        public IHttpActionResult PutAlmacenamiento(int id, Almacenamiento alm)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var almExistente = db.Componentes.OfType<Almacenamiento>().FirstOrDefault(p => p.ID == id);
            if (almExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la unidad de almacenamiento con el id: {id}");
            }

            almExistente.Nombre = alm.Nombre;
            almExistente.Marca = alm.Marca;
            almExistente.Precio = alm.Precio;
            almExistente.Descripcion = alm.Descripcion;
            almExistente.Stock = alm.Stock;
            almExistente.Capacidad = alm.Capacidad;
            almExistente.TipoAlmacenamiento = alm.TipoAlmacenamiento;
            almExistente.VelocidadLectura = alm.VelocidadLectura;
            almExistente.VelocidadEscritura = alm.VelocidadEscritura;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlmacenamientoExists(id))
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
                Mensaje = "La unidad de almacenamiento se actualizó correctamente",
                Datos_Nuevos = almExistente

            });
        }

        /// <summary>
        /// Crea una nueva unidad de almacenamiento en la base de datos.
        /// </summary>
        /// <param name="almacenamiento">Los datos de la unidad de almacenamiento que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos de la unidad de almacenamiento creada.
        /// </returns>
        /// <response code="201">Creado. La unidad de almacenamiento se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(Almacenamiento))]

        [HttpPost]
        public IHttpActionResult PostAlmacenamiento(Almacenamiento almacenamiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(almacenamiento);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = almacenamiento.ID }, almacenamiento);
        }

        /// <summary>
        /// Elimina una unidad de almacenamiento específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la unidad de almacenamiento que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra la unidad de almacenamiento, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos de la unidad de almacenamiento eliminada.
        /// </returns>
        /// <response code="200">OK. La unidad de almacenamiento fue eliminada correctamente.</response>
        /// <response code="404">No encontrado. No se encontró una unidad de almacenamiento con el ID especificado.</response>
        [ResponseType(typeof(Almacenamiento))]

        [HttpDelete]
        public IHttpActionResult DeleteAlmacenamiento(int id)
        {
            var almacenamiento = db.Componentes.OfType<Almacenamiento>().FirstOrDefault(p => p.ID == id);
            if (almacenamiento == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la unidad de almacenamiento con el id: {id}");
            }

            db.Componentes.Remove(almacenamiento);
            db.SaveChanges();

            return Ok(almacenamiento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlmacenamientoExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}