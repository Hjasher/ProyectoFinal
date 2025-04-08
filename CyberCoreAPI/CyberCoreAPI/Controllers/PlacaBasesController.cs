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
    public class PlacaBasesController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todas las placas base disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de placas base.
        /// Si no se encuentran placas base, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de placas base.</response>
        /// <response code="404">No encontrado. No se encontraron placas base en la base de datos.</response>

        
        public IHttpActionResult GetPlacasBases()
        {
            var pb = db.Componentes.OfType<PlacaBase>().ToList();

            if (pb == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron placas bases en la base de datos");
            }
            return Ok(pb);
        }

        /// <summary>
        /// Obtiene una placa base específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la placa base que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene la placa base con el ID especificado.
        /// Si no se encuentra la placa base, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la placa base solicitada.</response>
        /// <response code="404">No encontrado. No se encontró una placa base con el ID especificado.</response>
        [HttpGet]
        [ResponseType(typeof(PlacaBase))]
        public IHttpActionResult GetPlacaBase(int id)
        {
            var pb = db.Componentes.OfType<PlacaBase>().FirstOrDefault(i => i.ID == id);
            if (pb == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la placa base con el id: {id}");
            }

            return Ok(pb);
        }

        /// <summary>
        /// Actualiza una placa base existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la placa base que se desea actualizar.</param>
        /// <param name="pb">Los nuevos datos de la placa base.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra la placa base, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. La placa base se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró una placa base con el ID especificado.</response>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutPlacaBase(int id, PlacaBase pb)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var pbExistente = db.Componentes.OfType<PlacaBase>().FirstOrDefault(p => p.ID == id);
            if (pbExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la placa base con el id: {id}");
            }

            pbExistente.Nombre = pb.Nombre;
            pbExistente.Marca = pb.Marca;
            pbExistente.Precio = pb.Precio;
            pbExistente.Descripcion = pb.Descripcion;
            pbExistente.Stock = pb.Stock;
            pbExistente.Socket = pb.Socket;
            pbExistente.Formato = pb.Formato;
            pbExistente.SlotsRAM = pb.SlotsRAM;
            pbExistente.SlotsPCIe = pb.SlotsPCIe;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlacaBaseExists(id))
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
                Mensaje = "La placa base se actualizó correctamente",
                Datos_Nuevos = pbExistente

            });
        }

        /// <summary>
        /// Crea una nueva placa base en la base de datos.
        /// </summary>
        /// <param name="placaBase">Los datos de la placa base que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos de la placa base creada.
        /// </returns>
        /// <response code="201">Creado. La placa base se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(PlacaBase))]
        [HttpPost]
        public IHttpActionResult PostPlacaBase(PlacaBase placaBase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(placaBase);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = placaBase.ID }, placaBase);
        }

        /// <summary>
        /// Elimina una placa base específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la placa base que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra la placa base, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos de la placa base eliminada.
        /// </returns>
        /// <response code="200">OK. La placa base fue eliminada correctamente.</response>
        /// <response code="404">No encontrado. No se encontró una placa base con el ID especificado.</response>
        [ResponseType(typeof(PlacaBase))]
        [HttpDelete]
        public IHttpActionResult DeletePlacaBase(int id)
        {
            var placaBase = db.Componentes.OfType<PlacaBase>().FirstOrDefault(p => p.ID == id);
            if (placaBase == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la placa base con el id: {id}");
            }

            db.Componentes.Remove(placaBase);
            db.SaveChanges();

            return Ok(placaBase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlacaBaseExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}