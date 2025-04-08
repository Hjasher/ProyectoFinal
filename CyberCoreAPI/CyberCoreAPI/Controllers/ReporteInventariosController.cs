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
    public class ReporteInventariosController : ApiController
    {
        private DBContextProject db = new DBContextProject();

        // GET: api/ReporteInventarios
        /// <summary>
        /// Genera un reporte completo del inventario actual.
        /// </summary>
        /// <remarks>
        /// Este endpoint:
        /// 1. Recupera todos los componentes del inventario
        /// 2. Calcula el stock total automáticamente
        /// 3. Incluye la fecha de generación del reporte
        /// </remarks>
        /// <returns>Reporte de inventario detallado</returns>
        /// <response code="200">OK - Devuelve el reporte generado</response>
        /// <response code="500">Internal Server Error - Error al procesar el reporte</response>

        [HttpGet]
        [ResponseType(typeof(ReporteInventario))]
        public IHttpActionResult GetReporteInventario()
        {
            try
            {
                var listaDeComponentes = db.Componentes.ToList();

                var reporte = new ReporteInventario
                {
                    Fecha = DateTime.Now,
                    Componentes = listaDeComponentes
                };
                reporte.CalcularTotalStock();

                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/ReporteInventarios/5
        /*[ResponseType(typeof(ReporteInventario))]
        public IHttpActionResult GetReporteInventario(int id)
        {
            ReporteInventario reporteInventario = db.ReporteInventario.Find(id);
            if (reporteInventario == null)
            {
                return NotFound();
            }

            return Ok(reporteInventario);
        }

        // PUT: api/ReporteInventarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReporteInventario(int id, ReporteInventario reporteInventario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reporteInventario.ID)
            {
                return BadRequest();
            }

            db.Entry(reporteInventario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteInventarioExists(id))
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

        // POST: api/ReporteInventarios
        [ResponseType(typeof(ReporteInventario))]
        public IHttpActionResult PostReporteInventario(ReporteInventario reporteInventario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ReporteInventario.Add(reporteInventario);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reporteInventario.ID }, reporteInventario);
        }

        // DELETE: api/ReporteInventarios/5
        [ResponseType(typeof(ReporteInventario))]
        public IHttpActionResult DeleteReporteInventario(int id)
        {
            ReporteInventario reporteInventario = db.ReporteInventario.Find(id);
            if (reporteInventario == null)
            {
                return NotFound();
            }

            db.ReporteInventario.Remove(reporteInventario);
            db.SaveChanges();

            return Ok(reporteInventario);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReporteInventarioExists(int id)
        {
            return db.ReporteInventario.Count(e => e.ID == id) > 0;
        }
    }
}