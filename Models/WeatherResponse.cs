using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Heather.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("location")]
        public Location? Location { get; set; }

        [JsonPropertyName("current")]
        public Current? Current { get; set; }

        [JsonPropertyName("forecast")]
        public Forecast? Forecast { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("name")] public string? Name { get; set; }
        [JsonPropertyName("country")] public string? Country { get; set; }
        [JsonPropertyName("localtime")] public string? Localtime { get; set; }
    }

    public class Current
    {
        [JsonPropertyName("temp_c")] public double TempC { get; set; }
        [JsonPropertyName("condition")] public Condition? Condition { get; set; }
        [JsonPropertyName("wind_kph")] public double WindKph { get; set; }
        [JsonPropertyName("wind_mph")] public double WindMph { get; set; }
        [JsonPropertyName("gust_kph")] public double GustKph { get; set; }
        [JsonPropertyName("wind_dir")] public string? WindDir { get; set; }
        [JsonPropertyName("humidity")] public int Humidity { get; set; }
        [JsonPropertyName("cloud")] public int Cloud { get; set; }
        [JsonPropertyName("feelslike_c")] public double FeelsLikeC { get; set; }
        [JsonPropertyName("feelslike_f")] public double FeelsLikeF { get; set; }
        [JsonPropertyName("precip_mm")] public double PrecipMm { get; set; }
        [JsonPropertyName("vis_km")] public double VisKm { get; set; }
        [JsonPropertyName("dewpoint_c")] public double DewpointC { get; set; }
        [JsonPropertyName("is_day")] public int IsDay { get; set; }
    }

    public class Condition
    {
        [JsonPropertyName("text")] public string? Text { get; set; }
        [JsonPropertyName("icon")] public string? Icon { get; set; }
    }

    public class Forecast
    {
        [JsonPropertyName("forecastday")]
        public List<Forecastday>? Forecastday { get; set; }
    }

    public class Forecastday
    {
        [JsonPropertyName("date")] public string? Date { get; set; }
        [JsonPropertyName("day")] public Day? Day { get; set; }
        [JsonPropertyName("astro")] public Astro? Astro { get; set; }
    }

    public class Day
    {
        [JsonPropertyName("maxtemp_c")] public double? MaxtempC { get; set; }
        [JsonPropertyName("mintemp_c")] public double? MintempC { get; set; }
        [JsonPropertyName("maxwind_kph")] public double? MaxwindKph { get; set; }
        [JsonPropertyName("totalprecip_mm")] public double? TotalPrecipMm { get; set; }
        [JsonPropertyName("avgvis_km")] public double? AvgVisKm { get; set; }
        [JsonPropertyName("daily_chance_of_rain")] public int? DailyChanceOfRain { get; set; }
        [JsonPropertyName("daily_chance_of_snow")] public int? DailyChanceOfSnow { get; set; }
        [JsonPropertyName("condition")] public Condition? Condition { get; set; }

        // convenience
        public int DailyChanceOfPrecip => DailyChanceOfRain ?? DailyChanceOfSnow ?? 0;
    }

    public class Astro
    {
        [JsonPropertyName("sunrise")] public string? Sunrise { get; set; }
        [JsonPropertyName("sunset")] public string? Sunset { get; set; }
        [JsonPropertyName("moonrise")] public string? Moonrise { get; set; }
        [JsonPropertyName("moonset")] public string? Moonset { get; set; }
        [JsonPropertyName("moon_phase")] public string? MoonPhase { get; set; }
        [JsonPropertyName("moon_illumination")] public int? MoonIllumination { get; set; }
    }
}
