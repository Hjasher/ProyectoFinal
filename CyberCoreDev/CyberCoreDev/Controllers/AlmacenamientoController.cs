using CyberCoreDev.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CyberCoreDev.Controllers
{
    public class AlmacenamientoController : ApiController
    {
        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44310/api/Almacenamientoes";
            var respuestaJson = await GetAsync(apiUrl);
            List<Almacenamiento> listaAlmacenamiento = JsonConvert.DeserializeObject<List<Almacenamiento>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listaAlmacenamiento, loadOptions));
        }

        public static async Task<string> GetAsync(string uri)
        {
            try
            {
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }

            }
            catch (Exception e)
            {
                var m = e.Message;
                return null;
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(FormDataCollection form)
        {
            var values = form.Get("values");

            if (values == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Values cannot be null");
            }

            var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");

            var url = "https://localhost:44310/api/Almacenamientoes";
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient(handler))
            {
                var response = await client.PostAsync(url, httpContent);

                var result = response.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }


        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(FormDataCollection form)
        {
            var key = Convert.ToInt32(form.Get("key"));

            var apiUrlDelAlmacenamiento= "https://localhost:44310/api/Almacenamientoes/" + key;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient(handler))
            {
                var respuestaAlmacenamiento = await client.DeleteAsync(apiUrlDelAlmacenamiento);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPut]
        public async Task<HttpResponseMessage> Put()
        {
            try
            {
                // Leer el contenido de la solicitud como string
                var requestContent = await Request.Content.ReadAsStringAsync();

                // Parsear el contenido JSON
                var requestData = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestContent);

                // Obtener key y values
                var key = Convert.ToInt32(requestData["key"]);
                var values = requestData["values"];

                // 1. Obtener el objeto actual desde la API
                var apiUrlGet = $"https://localhost:44310/api/Almacenamientoes/{key}";
                var respuesta = await GetAsync(apiUrlGet);

                // 2. Deserializar como Almacenamiento
                var almacenamiento = JsonConvert.DeserializeObject<Almacenamiento>(respuesta);

                // 3. Actualizar propiedades
                if (!string.IsNullOrEmpty(values))
                {
                    JsonConvert.PopulateObject(values, almacenamiento);
                }

                // 4. Configurar el handler para SSL
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                // 5. Enviar actualización
                var jsonString = JsonConvert.SerializeObject(almacenamiento);
                var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                using (var client = new HttpClient(handler))
                {
                    var response = await client.PutAsync(apiUrlGet, httpContent);
                    response.EnsureSuccessStatusCode();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en PUT: {ex}");
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
