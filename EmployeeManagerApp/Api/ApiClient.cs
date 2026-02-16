using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace EmployeeManagerApp.Api
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(string baseUrl, string? token = null)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(20)
            };

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            _jsonOptions = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true };
            _jsonOptions.Converters.Add(new DateOnlyJsonConverter());
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url)
        {
            var response = await _httpClient.GetAsync(url);

            await EnsureSuccess(response);

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        { 
            var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);

            await EnsureSuccess(response);

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);

            await EnsureSuccess(response);

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        public async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            await EnsureSuccess(response);
        }

        private static async Task EnsureSuccess(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            { 
                return;
            }

            var error = await response.Content.ReadAsStringAsync();

            throw new ApiException((int)response.StatusCode, string.IsNullOrWhiteSpace(error) ? response.ReasonPhrase: error);
        }
    }
}
