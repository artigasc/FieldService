using FESA.SCM.WebSite.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Helpers {

	public class ApiClient {

		private readonly HttpClient _httpClient;
		private readonly string _baseUrl;
		private readonly Dictionary<string, string> _parameters;

		public ApiClient(string baseUrl) {
			if (string.IsNullOrEmpty(baseUrl))
				throw new ArgumentNullException(nameof(baseUrl));
			_baseUrl = baseUrl;
			_httpClient = new HttpClient {
				MaxResponseContentBufferSize = 2147483647
			};
			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_parameters = new Dictionary<string, string>();
		}

		internal Task ExecuteGet<T>(object twitter) {
			throw new NotImplementedException();
		}

		internal Task ExecuteGet<T>(object place, string id) {
			throw new NotImplementedException();
		}

		//internal Task ExecutePost(object addProduct)
		//{
		//    throw new NotImplementedException();
		//}

		public void AddParameter(string key, object value) {
			if (_parameters.ContainsKey(key)) {
				_parameters[key] = value.ToString();
			} else {
				_parameters.Add(key, value.ToString());
			}
		}

		public void AddObjectParameter(string key, object value) {
			var serializedValue = JsonConvert.SerializeObject(value);
			AddParameter(key, serializedValue);
		}



		public async Task<ClientResponse> ExecuteGet<T>(string method) where T : class {
			var fullMethod = $"{_baseUrl}/{method}";
			if (_parameters.Any()) {
				fullMethod += "?";
				fullMethod = _parameters.Aggregate(fullMethod,
					(current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
				fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
			}
			var response = await _httpClient.GetAsync(fullMethod).ConfigureAwait(false);

			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			var result = new ClientResponse {
				Content = JsonConvert.DeserializeObject<T>(responseJson),
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};
			return result;
		}

		public async Task<ClientResponse> ExecuteGetToString(string method) {
			var fullMethod = $"{_baseUrl}/{method}";
			if (_parameters.Any()) {
				fullMethod += "?";
				fullMethod = _parameters.Aggregate(fullMethod,
					(current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
				fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
			}
			var response = await _httpClient.GetAsync(fullMethod).ConfigureAwait(false);

			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			var result = new ClientResponse {
				Content = responseJson,
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};
			return result;
		}


		public async Task<ClientResponse> ExecuteGet<T>(string method, string id) where T : class {
			var fullMethod = $"{_baseUrl}/{method}/{id}";
			//if (_parameters.Any()) {
			//    fullMethod += "?";
			//    fullMethod = _parameters.Aggregate(fullMethod,
			//        (current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
			//    fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
			//}
			var response = await _httpClient.GetAsync(fullMethod).ConfigureAwait(false);

			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			var result = new ClientResponse {
				Content = JsonConvert.DeserializeObject<T>(responseJson),
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};

			return result;
		}


		public async Task<ClientResponse> ExecuteGet(string method) {
			var fullMethod = $"{_baseUrl}/{method}";
			if (_parameters.Any()) {
				fullMethod += "?";
				fullMethod = _parameters.Aggregate(fullMethod,
					(current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
				fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
			}
			var response = await _httpClient.GetAsync(fullMethod).ConfigureAwait(false);

			var data =
				JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

			var result = new ClientResponse {
				Content = data.FirstOrDefault().Value as bool?,
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};
			return result;
		}

		
		public async Task<ClientResponse> ExecutePut(string method, string nameParameter) {
			//var para = JsonConvert.SerializeObject(_parameters, Formatting.Indented);
			var para = _parameters[nameParameter].ToString();
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = await _httpClient.PutAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).ConfigureAwait(false);
			var result = new ClientResponse {
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase,
				Content = response.IsSuccessStatusCode
			};

			return result;
		}

		public async Task<ClientResponse> ExecuteDelete(string method, string id) {
			var fullMethod = $"{_baseUrl}/{method}/{id}";
			//if (_parameters.Any()) {
			//    fullMethod += "?";
			//    fullMethod = _parameters.Aggregate(fullMethod,
			//        (current, parameter) => current + $"{parameter.Key}={parameter.Value}&");
			//    fullMethod = fullMethod.Substring(0, fullMethod.Length - 1);
			//}
			var response = await _httpClient.DeleteAsync(fullMethod).ConfigureAwait(false);

			//var data =
			//    JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
			//var result = new ClientResponse {
			//    Content = data.FirstOrDefault().Value as bool?,
			//    Status = response.StatusCode,
			//    StatusDescription = response.ReasonPhrase
			//};

			var result = new ClientResponse {
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase,
				Content = response.IsSuccessStatusCode
			};

			return result;
		}

		public async Task<ClientResponse> ExecutePost(string method) {
			var para = JsonConvert.SerializeObject(_parameters, Formatting.Indented);
			//var para = _parameters["user"].ToString();
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).ConfigureAwait(false);
			var result = new ClientResponse {
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase,
				Content = response.IsSuccessStatusCode
			};

			return result;
		}

		public async Task<ClientResponse> ExecutePost(string method, string nameParameter) {
			//var para = JsonConvert.SerializeObject(_parameters, Formatting.Indented);
			var para = _parameters[nameParameter].ToString();
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).ConfigureAwait(false);
			var result = new ClientResponse {
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase,
				Content = response.IsSuccessStatusCode
			};

			return result;
		}


		public async Task<ClientResponse> ExecutePost<T>(string method) where T : class {
			var para = JsonConvert.SerializeObject(_parameters, Formatting.Indented);
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).GetAwaiter().GetResult();

			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			var result = new ClientResponse {
				Content = JsonConvert.DeserializeObject<T>(responseJson),
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};

			return result;
		}

		public async Task<ClientResponse> ExecutePost<T>(string method, object _object) where T : class {
			//var para = JsonConvert.SerializeObject(_object);
			var para = JsonConvert.SerializeObject(_object, Formatting.Indented);
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).GetAwaiter().GetResult();
			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = new ClientResponse {
				Content = JsonConvert.DeserializeObject<T>(responseJson),
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};
			return result;
		}

		public async Task<ClientResponse> ExecutePost(string method, object _object) {
			//var para = JsonConvert.SerializeObject(_object);
			var para = JsonConvert.SerializeObject(_object, Formatting.Indented);
			var content = new StringContent(para, Encoding.UTF8, "application/json");
			var response = _httpClient.PostAsync(new Uri($"{_baseUrl}/{method}", UriKind.Absolute), content).GetAwaiter().GetResult();
			var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = new ClientResponse {
				Content = JsonConvert.DeserializeObject(responseJson),
				Status = response.StatusCode,
				StatusDescription = response.ReasonPhrase
			};
			return result;
		}

		public void CleanParameters() {
			_parameters.Clear();
		}

	}

}
