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
    public class ProcesadorsController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todos los procesadores disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de procesadores.
        /// Si no se encuentran procesadores, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de procesadores.</response>
        /// <response code="404">No encontrado. No se encontraron procesadores en la base de datos.</response>

        
        public IHttpActionResult GetProcesadores()
        {
            var procesadores = db.Componentes.OfType<Procesador>().ToList();

            if (procesadores == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron procesadores en la base de datos");
            }
            return Ok(procesadores);
        }

        /// <summary>
        /// Obtiene un procesador específico por su ID.
        /// </summary>
        /// <param name="id">El ID del procesador que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene el procesador con el ID especificado.
        /// Si no se encuentra el procesador, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve el procesador solicitado.</response>
        /// <response code="404">No encontrado. No se encontró un procesador con el ID especificado.</response>
        [HttpGet]
        [ResponseType(typeof(Procesador))]
        public IHttpActionResult GetProcesador(int id)
        {
            var procesador = db.Componentes.OfType<Procesador>().FirstOrDefault(i => i.ID == id);
            if (procesador == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el procesador con el id: {id}");
            }

            return Ok(procesador);
        }

        /// <summary>
        /// Actualiza un procesador existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID del procesador que se desea actualizar.</param>
        /// <param name="procesador">Los nuevos datos del procesador.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra el procesador, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. El procesador se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró un procesador con el ID especificado.</response>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutProcesador(int id, Procesador procesador)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            var procesadorExistente = db.Componentes.OfType<Procesador>().FirstOrDefault(p => p.ID == id);
            if (procesadorExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el procesador con el id: {id}");
            }

            procesadorExistente.Nombre = procesador.Nombre;
            procesadorExistente.Marca = procesador.Marca;
            procesadorExistente.Precio = procesador.Precio;
            procesadorExistente.Descripcion = procesador.Descripcion;
            procesadorExistente.Stock = procesador.Stock;
            procesadorExistente.Socket = procesador.Socket;
            procesadorExistente.Nucleos = procesador.Nucleos;
            procesadorExistente.Hilos = procesador.Hilos;
            procesadorExistente.Frecuencia = procesador.Frecuencia;

            

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcesadorExists(id))
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
                Mensaje = "El procesador se actualizó correctamente",
                Datos_Nuevos = procesadorExistente 

            });
        }

        /// <summary>
        /// Crea un nuevo procesador en la base de datos.
        /// </summary>
        /// <param name="procesador">Los datos del procesador que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos del procesador creado.
        /// </returns>
        /// <response code="201">Creado. El procesador se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(Procesador))]
        [HttpPost]
        public IHttpActionResult PostProcesador(Procesador procesador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(procesador);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = procesador.ID }, procesador);
        }

        /// <summary>
        /// Elimina un procesador específico por su ID.
        /// </summary>
        /// <param name="id">El ID del procesador que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra el procesador, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos del procesador eliminado.
        /// </returns>
        /// <response code="200">OK. El procesador fue eliminado correctamente.</response>
        /// <response code="404">No encontrado. No se encontró un procesador con el ID especificado.</response>
        [ResponseType(typeof(Procesador))]
        [HttpDelete]
        public IHttpActionResult DeleteProcesador(int id)
        {
            var procesador = db.Componentes.OfType<Procesador>().FirstOrDefault(p => p.ID == id);
            if (procesador == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró el procesador con el id: {id}");
            }

            db.Componentes.Remove(procesador);
            db.SaveChanges();

            return Content(HttpStatusCode.OK, new
            {
                Mensaje = $"El procesador fue eliminado correctamente",
                Procesador_Eliminado = procesador
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

        private bool ProcesadorExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}