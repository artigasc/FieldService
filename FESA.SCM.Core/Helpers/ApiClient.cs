using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FESA.SCM.Core.Models;
using Newtonsoft.Json;

namespace FESA.SCM.Core.Helpers
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _parameters;

        public ApiClient(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                MaxResponseContentBufferSize = 2147483647
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _parameters = new Dictionary<string, string>();
        }

        public void AddParameter(string key, object value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value.ToString();
            }
            else
            {
                _parameters.Add(key, value.ToString());
            }
        }

        public void AddObjectParameter(string key, object value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            AddParameter(key, serializedValue);
        }

        public async Task<ClientResponse> ExecuteGet<T>(string method) where T : class
        {
            var fullMethod = $"{_baseUrl}/{method}";
            if (_parameters.Any())
            {
                fullMethod += "?";
                fullMethod = _parameters.Aggregate(fullMethod,
                    (current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
                fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
            }
            var response = await _httpClient.GetAsync(fullMethod).ConfigureAwait(false);

            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = new ClientResponse
            {
                Content = JsonConvert.DeserializeObject<T>(responseJson),
                Status = response.StatusCode,
                StatusDescription = response.ReasonPhrase
            };

            return result;
        }

        public async Task<ClientResponse> ExecuteGet(string method)
        {
            var fullMethod = $"{_baseUrl}/{method}";
            if (_parameters.Any())
            {
                fullMethod += "?";
                fullMethod = _parameters.Aggregate(fullMethod,
                    (current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
                fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
            }
            var response = await _httpClient.GetAsync(fullMethod);

            var data =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            var result = new ClientResponse
            {
                Content = data.FirstOrDefault().Value as bool?,
                Status = response.StatusCode,
                StatusDescription = response.ReasonPhrase
            };

            return result;
        }
        public async Task<ClientResponse> ExecutePost(string method)
        {
            var para = JsonConvert.SerializeObject(_parameters);
            var content = new StringContent(para, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).ConfigureAwait(false);
            var result = new ClientResponse
            {
                Status = response.StatusCode,
                StatusDescription = response.ReasonPhrase,
                Content = response.IsSuccessStatusCode
            };

            return result;
        }

        public async Task<ClientResponse> ExecutePost<T>(string method) where T : class 
        {
            var para = JsonConvert.SerializeObject(_parameters);
            var content = new StringContent(para, Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).GetAwaiter().GetResult();

            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = new ClientResponse
            {
                Content = JsonConvert.DeserializeObject<T>(responseJson),
                Status = response.StatusCode,
                StatusDescription = response.ReasonPhrase
            };

            return result;
        }
        public void CleanParameters()
        {
            _parameters.Clear();
        }
    }
}