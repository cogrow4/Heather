# Heather — Polished Weather App (Avalonia/.NET)

This repository contains a polished desktop weather application for Linux using Avalonia UI and .NET 8.

Features
- Current conditions (temp, feels-like, wind, humidity, cloud)
- Forecast (3–7 day) with sunrise/sunset and moon data
- Uses WeatherAPI (https://www.weatherapi.com/) forecast.json endpoint

Quick start (Linux)

1. Install .NET 8 SDK.
2. Set your WeatherAPI key in the environment (recommended). Use the exact env var name `WEATHERAPI_KEY`.

Example (bash):

```bash
export WEATHERAPI_KEY=000000000000000000000000000000
```

Important: do not commit your API key to version control. Prefer environment variables, `dotnet user-secrets`, or another secure store for production.

3. Build and run:

```bash
dotnet restore
dotnet build
dotnet run --project Heather.csproj
```

Notes
- The app currently expects an environment variable `WEATHERAPI_KEY`. You can change the code in `Services/WeatherApiClient.cs` to read from configuration instead.
- This is an initial scaffold with polished layout and should be extended with icons, caching, error handling, unit tests, and accessibility improvements.
# Heather
A weather application designed for Linux and Hyperland
