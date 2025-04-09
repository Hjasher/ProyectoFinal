using CyberCoreDev.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace CyberCoreDev.Controllers
{
    public class UsuarioController : ApiController
    {

        [HttpGet]
        public async Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
        {
            var apiUrl = "https://localhost:44310/api/usuario";
            var respuestaJson = await GetAsync(apiUrl);
            //System.Diagnostics.Debug.WriteLine(respuestaJson); imprimir info
            List<Usuario> listaUser = JsonConvert.DeserializeObject<List<Usuario>>(respuestaJson);
            return Request.CreateResponse(DataSourceLoader.Load(listaUser, loadOptions));
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
            var usuario = JsonConvert.DeserializeObject<Usuario>(values);

            // Validar el modelo
            var context = new ValidationContext(usuario, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(usuario, context, results, true);

            if (!isValid)
            {
                var errors = results.Select(r => r.ErrorMessage).ToList();
                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }

            // Verificar si el correo ya existe en la base de datos
            var apiUrl = "https://localhost:44310/api/usuario?correo=" + usuario.Correo;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var usuarioExistente = JsonConvert.DeserializeObject<Usuario>(result);
                    if (usuarioExistente != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "El correo ya está registrado.");
                    }
                }
            }

            var httpContent = new StringContent(values, System.Text.Encoding.UTF8, "application/json");
            var url = "https://localhost:44310/api/usuario";
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

            var apiUrlDelUser = "https://localhost:44310/api/usuario/" + key;
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient(handler))
            {
                var respuestaUser = await client.DeleteAsync(apiUrlDelUser);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [HttpPut]
        public async Task<HttpResponseMessage> Put(FormDataCollection form)
        {
            //Parámetros del form
            var key = Convert.ToInt32(form.Get("key")); //llave que estoy modificando
            var values = form.Get("values"); //Los valores que yo modifiqué en formato JSON

            var apiUrlGetUser = "https://localhost:44310/api/usuario/" + key;
            var respuestaUser = await GetAsync(apiUrlGetUser = "https://localhost:44310/api/usuario/" + key);
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(respuestaUser);

            JsonConvert.PopulateObject(values, usuario);

            string jsonString = JsonConvert.SerializeObject(usuario);
            var httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            using (var client = new HttpClient(handler))
            {
                var url = "https://localhost:44310/api/usuario/" + key;
                var response = await client.PutAsync(url, httpContent);

                var result = response.Content.ReadAsStringAsync().Result;
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
