using CyberCoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;

namespace CyberCoreAPI.Controllers
{
    [RoutePrefix("api/componentes")]
    public class ComponentesController : ApiController
    {

        private DBContextProject db = new DBContextProject();

        /// <summary>
        /// Filtra los componentes de computadora por un rango de precios especificado.
        /// </summary>
        /// <param name="precioMin">El precio mínimo del rango. Solo se incluirán componentes con un precio igual o superior a este valor.</param>
        /// <param name="precioMax">El precio máximo del rango. Solo se incluirán componentes con un precio igual o inferior a este valor.</param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes cuyo precio está dentro del rango especificado.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo (por ejemplo, Procesador, RAM, etc.).
        /// Si no se encuentran componentes en el rango de precios, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET: api/Componentes/filtrar-por-precio?precioMin=100&amp;precioMax=500
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de componentes filtrados.</response>
        /// <response code="404">No encontrado. No se encontraron componentes en el rango de precios especificado.</response>
        [HttpGet]
        [Route("filtrar-por-precio")]
        public IHttpActionResult FiltrarPorPrecio(double precioMin, double precioMax)
        {
            var componentes = db.Componentes
                .Where(c => c.Precio >= precioMin && c.Precio <= precioMax)
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name // esto nos dice de que tipo es el componente ya sea Procesador, RAm, etc...
                    
                })
                .ToList();

            if (componentes == null || !componentes.Any())
            {
                return Content(HttpStatusCode.NotFound, $"No se encontraron componentes entre el rango de precio de: {precioMin} - {precioMax} ");
            }

            return Ok(componentes);
        }

        /// <summary>
        /// Filtra los componentes de computadora por una marca específica.
        /// </summary>
        /// <param name="marca">La marca de los componentes que se desean filtrar. Solo se incluirán componentes que coincidan con esta marca.</param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes que pertenecen a la marca especificada.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo (por ejemplo, Procesador, RAM, etc.).
        /// Si no se encuentran componentes de la marca especificada, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET: api/Componentes/filtrar-por-marca?marca=Intel
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de componentes filtrados por la marca especificada.</response>
        /// <response code="404">No encontrado. No se encontraron componentes de la marca especificada.</response>
        [HttpGet]
        [Route("filtrar-por-marca")]
        public IHttpActionResult FiltrarPorMarca(string marca)
        {
            var componentes = db.Componentes
                .Where(c => c.Marca == marca)
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name
                })
                .ToList();

            if (componentes == null || !componentes.Any())
            {
                return Content(HttpStatusCode.NotFound, $"No se encontraron componentes con la marca: {marca} ");
            }

            return Ok(componentes);
        }

        /// <summary>
        /// Filtra los componentes de computadora por un stock mínimo especificado.
        /// </summary>
        /// <param name="stockMin">El stock mínimo requerido. Solo se incluirán componentes con un stock igual o superior a este valor.</param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes cuyo stock es igual o superior al valor especificado.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo (por ejemplo, Procesador, RAM, etc.).
        /// Si no se encuentran componentes con el stock mínimo especificado, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET: api/Componentes/filtrar-por-stock?stockMin=10
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de componentes filtrados por el stock mínimo especificado.</response>
        /// <response code="404">No encontrado. No se encontraron componentes con el stock mínimo especificado.</response>
        [HttpGet]
        [Route("filtrar-por-stock")]
        public IHttpActionResult FiltrarPorStock(int stockMin)
        {
            var componentes = db.Componentes
                .Where(c => c.Stock >= stockMin)
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name
                })
                .ToList();

            if (componentes == null || !componentes.Any())
            {
                return Content(HttpStatusCode.NotFound, $"No se encontraron componentes con la cantidad en stock seleccionada: {stockMin} ");
            }

            return Ok(componentes);
        }


        /// <summary>
        /// Busca componentes de computadora cuyo nombre contenga una cadena específica.
        /// </summary>
        /// <param name="nombre">La cadena de texto que se buscará dentro de los nombres de los componentes. No distingue entre mayúsculas y minúsculas.</param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes cuyos nombres contienen la cadena especificada.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo (por ejemplo, Procesador, RAM, etc.).
        /// Si no se encuentran componentes que coincidan con la cadena especificada, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// </returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET: api/Componentes/buscar-por-nombre?nombre=Procesador
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de componentes que coinciden con la cadena especificada.</response>
        /// <response code="404">No encontrado. No se encontraron componentes que coincidan con la cadena especificada.</response>
        [HttpGet]
        [Route("buscar-por-nombre")]
        public IHttpActionResult BuscarPorNombre(string nombre)
        {
            var componentes = db.Componentes
                .Where(c => c.Nombre.Contains(nombre))
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name
                })
                .ToList();

            if (componentes == null || !componentes.Any())
            {
                return Content(HttpStatusCode.NotFound, $"No se encontraron componentes con el nombre: {nombre} ");
            }

            return Ok(componentes);
        }


        /// <summary>
        /// Ordena los componentes de computadora por precio, en orden ascendente o descendente.
        /// </summary>
        /// <param name="orden">
        /// El criterio de ordenación. Puede ser "asc" para orden ascendente o "desc" para orden descendente.
        /// Si no se proporciona un valor, por defecto se ordenará en orden ascendente.
        /// </param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes ordenados por precio.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo (por ejemplo, Procesador, RAM, etc.).
        /// Si el parámetro 'orden' no es válido, se devuelve un código de estado 400 (Solicitud incorrecta) con un mensaje descriptivo.
        /// </returns>
        /// <remarks>
        /// Ejemplos de solicitud:
        /// - Orden ascendente: GET: api/Componentes/ordenar-por-precio?orden=asc
        /// - Orden descendente: GET: api/Componentes/ordenar-por-precio?orden=desc
        /// - Por defecto (ascendente): GET: api/Componentes/ordenar-por-precio
        /// </remarks>
        /// <response code="200">OK. Devuelve la lista de componentes ordenados por precio.</response>
        /// <response code="400">Solicitud incorrecta. El parámetro 'orden' no es válido.</response>
        [HttpGet]
        [Route("ordenar-por-precio")]
        public IHttpActionResult OrdenarPorPrecio(string orden = "asc")
        {
            IQueryable<Componentes> query = db.Componentes;

            if (orden.ToLower() == "asc")
            {
                query = query.OrderBy(c => c.Precio);
            }
            else if (orden.ToLower() == "desc")
            {
                query = query.OrderByDescending(c => c.Precio);
            }
            else
            {
                return BadRequest("El parámetro 'orden' debe ser 'asc' o 'desc'.");
            }

            var componentes = query
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name
                })
                .ToList();

            return Ok(componentes);
        }

        /// <summary>
        /// Filtra los componentes de computadora por un tipo específico, utilizando nombres comunes para la búsqueda.
        /// </summary>
        /// <param name="tipo">
        /// El tipo de componente que se desea filtrar. Puede ser uno de los siguientes valores:
        /// - "Procesador"
        /// - "Tarjeta gráfica", "Tarjeta grafica", "GPU"
        /// - "Memoria RAM", "RAM"
        /// - "Disco duro", "Disco duro HDD", "SSD"
        /// - "Placa base", "Placa madre"
        /// - "Fuente de poder", "Fuente"
        /// - "Caja", "Gabinete"
        /// </param>
        /// <returns>
        /// Un resultado HTTP que contiene una lista de componentes que coinciden con el tipo especificado.
        /// Cada componente incluye su ID, nombre, marca, precio, stock y tipo.
        /// Si no se encuentran componentes del tipo especificado, se devuelve un código de estado 404 (No encontrado) con un mensaje descriptivo.
        /// Si el tipo proporcionado no es válido, se devuelve un código de estado 400 (Solicitud incorrecta) con un mensaje que lista los tipos válidos.
        /// </returns>
        /// <response code="200">OK. Devuelve la lista de componentes que coinciden con el tipo especificado.</response>
        /// <response code="400">Solicitud incorrecta. El tipo proporcionado no es válido. Se incluye un mensaje con los tipos válidos.</response>
        /// <response code="404">No encontrado. No se encontraron componentes del tipo especificado.</response>
        [HttpGet]
        [Route("filtrar-por-tipo")]
        public IHttpActionResult FiltrarPorTipo(string tipo)
        {
            // Diccionario para mapear nombres comunes a nombres de clases
            var mapeoTipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Procesador", "Procesador" },
                { "Tarjeta gráfica", "GPU" },
                { "Tarjeta grafica", "GPU" }, // Alternativa sin tilde
                { "GPU", "GPU" }, // Alternativa en inglés
                { "Memoria RAM", "RAM" },
                { "RAM", "RAM" }, // Alternativa corta
                { "Disco duro", "Almacenamiento" },
                { "Disco duro HDD", "Almacenamiento" }, // Alternativa específica
                { "SSD", "Almacenamiento" }, // Alternativa para SSD
                { "Placa base", "PlacaBase" },
                { "Placa madre", "PlacaBase" }, // Alternativa común
                { "Fuente de poder", "FuentePoder" },
                { "Fuente", "FuentePoder" }, // Alternativa corta
                { "Caja", "Gabinete" },
                { "Gabinete", "Gabinete" } // Alternativa en español
            };

            // Obtener el nombre de la clase correspondiente al tipo
            if (!mapeoTipos.TryGetValue(tipo, out var nombreClase))
            {
                // Si el tipo no es válido, devolver un error con los tipos válidos
                var tiposValidos = mapeoTipos.Keys.OrderBy(k => k).ToList();
                return Content(HttpStatusCode.BadRequest, $"El tipo '{tipo}' no es válido. Los tipos válidos son: {string.Join(", ", tiposValidos)}");
            }

            // Filtrar los componentes por el nombre de la clase
            var componentes = db.Componentes
                .Where(c => c.GetType().Name == nombreClase)
                .ToList()
                .Select(c => new
                {
                    c.ID,
                    c.Nombre,
                    c.Marca,
                    c.Precio,
                    c.Stock,
                    c.Categoria,
                    Tipo = c.GetType().Name
                })
                .ToList();

            if (componentes == null || !componentes.Any())
            {
                return Content(HttpStatusCode.NotFound, $"No se encontraron componentes de tipo: {tipo}");
            }

            return Ok(componentes);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        








    }
}
