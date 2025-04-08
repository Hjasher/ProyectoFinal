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
    public class ReporteVentasController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // GET: api/ReporteVentas
        /// <summary>
        /// Genera un reporte de ventas dentro de un rango de fechas específico.
        /// </summary>
        /// <remarks>
        /// Criterios importantes:
        /// - Las fechas deben estar en formato (YYYY-MM-DD)
        /// - El rango máximo permitido es de 1 año
        /// - Fechas futuras no son permitidas
        /// </remarks>
        /// <param name="fechaInicio">Fecha de inicio del reporte </param>
        /// <param name="fechaFinal">Fecha de fin del reporte </param>
        /// <returns>Reporte detallado de ventas</returns>
        /// <response code="200">OK - Devuelve el reporte generado</response>
        /// <response code="400">Bad Request - Parámetros inválidos</response>
        /// <response code="404">Not Found - No se encontraron ventas en el rango</response>
        /// <response code="500">Internal Server Error - Error al procesar el reporte</response>


        [Route("api/reportes/ventas")]
        public IHttpActionResult GenerarReporteVentas(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                // Validación de parámetros
                if (fechaInicio > fechaFinal)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor a la fecha final");
                }

                if ((fechaFinal - fechaInicio).TotalDays > 365)
                {
                    return BadRequest("El rango máximo permitido es de 1 año");
                }

                if (fechaFinal > DateTime.Now)
                {
                    return BadRequest("No se permiten fechas futuras");
                }

                var ordenes = db.Ordenes
                               .Where(o => o.Fecha >= fechaInicio && o.Fecha <= fechaFinal)
                               .ToList();

                if (!ordenes.Any())
                {
                    return Content(HttpStatusCode.NotFound,
                        $"No se encontraron órdenes entre {fechaInicio:yyyy-MM-dd} y {fechaFinal:yyyy-MM-dd}");
                }

                double totalVentas = ordenes.Sum(o => o.Total);

                var reporte = new ReporteVentas
                {
                    FechaInicio = fechaInicio,
                    FechaFinal = fechaFinal,
                    TotalVentas = totalVentas,
                    Ordenes = ordenes,
                    
                };

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/ReporteVentas/5
        /*[ResponseType(typeof(ReporteVentas))]
        public IHttpActionResult GetReporteVentas(int id)
        {
            ReporteVentas reporteVentas = db.ReporteVentas.Find(id);
            if (reporteVentas == null)
            {
                return NotFound();
            }

            return Ok(reporteVentas);
        }

        // PUT: api/ReporteVentas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReporteVentas(int id, ReporteVentas reporteVentas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reporteVentas.ID)
            {
                return BadRequest();
            }

            db.Entry(reporteVentas).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteVentasExists(id))
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

        // POST: api/ReporteVentas
        [ResponseType(typeof(ReporteVentas))]
        public IHttpActionResult PostReporteVentas(ReporteVentas reporteVentas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReporteVentas.Add(reporteVentas);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reporteVentas.ID }, reporteVentas);
        }

        // DELETE: api/ReporteVentas/5
        [ResponseType(typeof(ReporteVentas))]
        public IHttpActionResult DeleteReporteVentas(int id)
        {
            ReporteVentas reporteVentas = db.ReporteVentas.Find(id);
            if (reporteVentas == null)
            {
                return NotFound();
            }

            db.ReporteVentas.Remove(reporteVentas);
            db.SaveChanges();

            return Ok(reporteVentas);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReporteVentasExists(int id)
        {
            return db.ReporteVentas.Count(e => e.ID == id) > 0;
        }
    }
}