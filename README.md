# Heather — Polished Weather App (Avalonia/.NET)
# Heather — Polished Weather App (Avalonia / .NET)

Heather is a lightweight, polished desktop weather application built with Avalonia and .NET 8. It targets Linux (and other desktop platforms supported by Avalonia) and provides current conditions and short-term forecasts with a focus on clean visuals and a macOS "Tahoe"-like dark style.

Key features
- Current conditions: temperature, feels-like, wind speed/direction, gusts, humidity, cloud cover, dew point, precipitation
- Short forecast: 1–3 day forecast selectable (hourly details available in the API)
- Astronomical data: sunrise, sunset, moonrise, moonset, moon phase, moon illumination
- AQI snippets: the WeatherAPI `air_quality` block is preserved and available to show
- Unit toggle: Celsius by default, Fahrenheit supported and converted client-side
- Accessibility: keyboard navigation and visible focus styles
- Local-first: API key is provided via environment variable (no keys are committed)

Screenshot
> (Add screenshots here after you run the app locally — see "Try it" below.)

Quick start (Linux)

1. Install .NET 8 SDK (see https://dotnet.microsoft.com for instructions).
2. Clone this repo and cd into it:

```bash
git clone https://github.com/cogrow4/Heather.git
cd Heather
```

3. Set your WeatherAPI key (recommended via environment variable):

```bash
# bash / zsh
export WEATHERAPI_KEY=005dbfba0d8e4bfda6524825251709
```

Important: do not commit your API key. Use environment variables, `dotnet user-secrets` or another secure store for production.

4. Restore, build and run:

```bash
dotnet restore
dotnet build
dotnet run --project Heather.csproj
```

Usage notes
- Search box: type a city name, postal code, or latitude,longitude and hit Search.
- Days selector: choose 1–3 days for the forecast. The default is 3 days.
- Unit toggle: click the °C / °F button to switch temperature units locally.
- If the app shows an error banner, check that `WEATHERAPI_KEY` is set and that you have network access.

Developer notes
- Project: `Heather.csproj` targets .NET 8 and uses Avalonia 11 packages.
- API client: `Services/WeatherApiClient.cs` calls `https://api.weatherapi.com/v1/forecast.json` and deserializes into `Models/WeatherResponse.cs`.
- ViewModels: `ViewModels/MainWindowViewModel.cs` contains the UI logic, day selection, unit conversion and subtle UI pulse when data updates.
- Controls: `Controls/WeatherSvgIcon.axaml` is a small SVG-based icon control used for crisp weather icons.

Model compatibility checklist
- The WeatherAPI JSON returns `location`, `current`, and `forecast.forecastday[]`.
- `current` fields we use: `temp_c`, `feelslike_c`, `wind_kph`, `wind_mph`, `gust_kph`, `dewpoint_c`, `is_day`, `condition` (text/icon), and `air_quality`.
- `astro.moon_illumination` is numeric — the model `Astro.MoonIllumination` maps to `int?` and the ViewModel converts it to string for display.

Troubleshooting
- NU1903 advisory for `System.Text.Json` was present earlier. The project upgrades the package reference; if you still see advisory warnings, run `dotnet restore` and make sure the SDK has network access to pull the newer package.
- If the app fails to start with an exception citing `WEATHERAPI_KEY`, ensure the env var is exported correctly in the same shell where you run `dotnet run`.
- XAML parse errors usually indicate malformed XAML. If you edit XAML files, keep backups and run `dotnet build` frequently.

Roadmap / next polish items
- Replace the slim built-in SVGs with richer Apple-like SVG icons and micro-animations.
- Add hourly drill-down UI for each forecast day.
- Persist user preferences (units, last-search, selected days).
- Caching / offline mode for last-known conditions and expiration.

Contributing
- PRs welcome. Please keep API keys out of commits and use feature branches.

License
- The project includes the repository LICENSE file. Verify licensing before re-use.

Contact
- Open issues or PRs on https://github.com/cogrow4/Heather

---

Built with Avalonia and ❤️ — enjoy Heather on your desktop.
