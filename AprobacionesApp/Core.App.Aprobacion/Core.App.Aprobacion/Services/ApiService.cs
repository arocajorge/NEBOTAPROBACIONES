namespace Core.App.Aprobacion.Services
{
    using Core.App.Aprobacion.Helpers;
    using Core.App.Aprobacion.Models;
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    public class ApiService
    {
        public async Task<Response> CheckConnection(string urlServidor = "")
        {
            string[] cadena = urlServidor.Split(':');
            string IPCompleta = string.Empty;
            IPCompleta = urlServidor;
            urlServidor = string.Empty;
            urlServidor = cadena[0]+":";
            urlServidor += cadena[1];

            if (string.IsNullOrEmpty(urlServidor))
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Configuración incorrecta"
                };
            }
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Verifique su conexión a internet"
                };
            }

            var isReachable = await CrossConnectivity.Current.IsRemoteReachable(urlServidor);
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se puede conectar al servidor"
                };
            }

            var response_cs = await GetObject<bool>(IPCompleta, Settings.RutaCarpeta, "ValidarConexion", "");
            if (!response_cs.IsSuccess)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No se puede validar la conexión con la ruta de aplicaciones de el servidor"
                };
            }

            return new Response
            {
                IsSuccess = true,
                Message = "Ok"
            };

        }

        public async Task<Response> GetList<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string parameters)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}/{1}", servicePrefix, controller) + (string.IsNullOrEmpty(parameters) ? "" : ("?" + parameters));
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> GetObject<T>(
           string urlBase,
           string servicePrefix,
           string controller,
           string parameters)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}/{1}", servicePrefix, controller) +  (string.IsNullOrEmpty(parameters) ? "" : ("?" + parameters));
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                var list = JsonConvert.DeserializeObject<T>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<Response> Post<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}/{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var error = JsonConvert.DeserializeObject<Response>(result);
                    error.IsSuccess = false;
                    return error;
                }

                var newRecord = JsonConvert.DeserializeObject<T>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Envío correcto",
                    Result = newRecord
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
