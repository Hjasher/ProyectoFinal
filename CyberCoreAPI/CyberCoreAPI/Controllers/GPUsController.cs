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
    public class GPUsController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Obtiene una lista de todas las GPU disponibles en la base de datos.
        /// </summary>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de GPU.
        /// Si no se encuentran GPU, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de GPU.</response>
        /// <response code="404">No encontrado. No se encontraron GPU en la base de datos.</response>

        
        public IHttpActionResult GetGPUs()
        {
            var gpus = db.Componentes.OfType<GPU>().ToList();

            if (gpus == null)
            {
                return Content(HttpStatusCode.NotFound, "No se encontraron GPU´s en la base de datos");
            }
            return Ok(gpus);
        }

        /// <summary>
        /// Obtiene una GPU específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la GPU que se desea obtener.</param>
        /// <returns>
        /// Un resultado HTTP que contiene la GPU con el ID especificado.
        /// Si no se encuentra la GPU, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <response code="200">OK. Devuelve la GPU solicitada.</response>
        /// <response code="404">No encontrado. No se encontró una GPU con el ID especificado.</response>
        [HttpGet]
        [ResponseType(typeof(GPU))]
        public IHttpActionResult GetGPU(int id)
        {
            var gpu = db.Componentes.OfType<GPU>().FirstOrDefault(i => i.ID == id);
            if (gpu == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la GPU con el id: {id}");
            }

            return Ok(gpu);
        }

        /// <summary>
        /// Actualiza una GPU existente en la base de datos.
        /// </summary>
        /// <param name="id">El ID de la GPU que se desea actualizar.</param>
        /// <param name="gpu">Los nuevos datos de la GPU.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la actualización fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si no se encuentra la GPU, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la actualización es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos actualizados.
        /// </returns>
        /// <response code="200">OK. La GPU se actualizó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        /// <response code="404">No encontrado. No se encontró una GPU con el ID especificado.</response>
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutGPU(int id, GPU gpu)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var gpuExistente = db.Componentes.OfType<GPU>().FirstOrDefault(p => p.ID == id);
            if (gpuExistente == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la GPU con el id: {id}");
            }

            gpuExistente.Nombre = gpu.Nombre;
            gpuExistente.Marca = gpu.Marca;
            gpuExistente.Precio = gpu.Precio;
            gpuExistente.Descripcion = gpu.Descripcion;
            gpuExistente.Stock = gpu.Stock;
            gpuExistente.Vram = gpu.Vram;
            gpuExistente.TipoMemoria = gpu.TipoMemoria;
            gpuExistente.VelocidadReloj = gpu.VelocidadReloj;
           

            

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GPUExists(id))
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
                Mensaje = "La GPU se actualizó correctamente",
                Datos_Nuevos = gpuExistente

            });
        }

        /// <summary>
        /// Crea una nueva GPU en la base de datos.
        /// </summary>
        /// <param name="gPU">Los datos de la GPU que se desea crear.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la creación fue exitosa.
        /// Si el modelo no es válido, se devuelve un código de estado 400 (Solicitud incorrecta).
        /// Si la creación es exitosa, se devuelve un código de estado 201 (Creado) con la ubicación del nuevo recurso y los datos de la GPU creada.
        /// </returns>
        /// <response code="201">Creado. La GPU se creó correctamente.</response>
        /// <response code="400">Solicitud incorrecta. El modelo no es válido.</response>
        [ResponseType(typeof(GPU))]
        [HttpPost]
        public IHttpActionResult PostGPU(GPU gPU)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Componentes.Add(gPU);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gPU.ID }, gPU);
        }

        /// <summary>
        /// Elimina una GPU específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la GPU que se desea eliminar.</param>
        /// <returns>
        /// Un resultado HTTP que indica si la eliminación fue exitosa.
        /// Si no se encuentra la GPU, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si la eliminación es exitosa, se devuelve un código de estado 200 (OK) con un mensaje de éxito y los datos de la GPU eliminada.
        /// </returns>
        /// <response code="200">OK. La GPU fue eliminada correctamente.</response>
        /// <response code="404">No encontrado. No se encontró una GPU con el ID especificado.</response>
        [ResponseType(typeof(GPU))]
        [HttpDelete]
        public IHttpActionResult DeleteGPU(int id)
        {
            var gpu = db.Componentes.OfType<GPU>().FirstOrDefault(p => p.ID == id);
            if (gpu == null)
            {
                return Content(HttpStatusCode.NotFound, $"No se encontró la GPU con el id: {id}");
            }

            db.Componentes.Remove(gpu);
            db.SaveChanges();

            return Ok(gpu);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GPUExists(int id)
        {
            return db.Componentes.Count(e => e.ID == id) > 0;
        }
    }
}