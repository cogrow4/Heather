using Heather.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Heather.Services
{
    public class WeatherApiClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public WeatherApiClient(HttpClient http)
        {
            _http = http;
            // Require an API key from environment for security. Don't commit keys to source.
            _apiKey = Environment.GetEnvironmentVariable("WEATHERAPI_KEY") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("WEATHERAPI_KEY environment variable is not set. Set it to your WeatherAPI key before running the app.");
        }

        public async Task<WeatherResponse?> GetForecastAsync(string query, int days = 3)
        {
            if (string.IsNullOrWhiteSpace(_apiKey) || _apiKey == "YOUR_API_KEY_HERE")
                throw new InvalidOperationException("Weather API key not set. Set WEATHERAPI_KEY environment variable.");

            var url = $"https://api.weatherapi.com/v1/forecast.json?key={_apiKey}&q={Uri.EscapeDataString(query)}&days={days}&aqi=no&alerts=no";
            var res = await _http.GetAsync(url);
            res.EnsureSuccessStatusCode();
            var s = await res.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };
            var obj = JsonSerializer.Deserialize<WeatherResponse>(s, options);
            return obj;
        }
    }
}
