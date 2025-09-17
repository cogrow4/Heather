using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heather.Models;
using Heather.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Heather.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly WeatherApiClient _client;

        public MainWindowViewModel(WeatherApiClient client)
        {
            _client = client;
            ForecastDays = new ObservableCollection<ForecastDayViewModel>();
            SearchCommand = new AsyncRelayCommand(SearchAsync);
            UseFahrenheit = false;
            ToggleUnitsCommand = new RelayCommand(ToggleUnits);
        }

        public IRelayCommand ToggleUnitsCommand { get; }

        private void ToggleUnits()
        {
            UseFahrenheit = !UseFahrenheit;
            // Update formatted values
            Current = new CurrentViewModel(CurrentModelBacking ?? new Models.Current(), UseFahrenheit);
            var items = new ObservableCollection<ForecastDayViewModel>();
            foreach (var d in ForecastDays)
            {
                // Recreate from underlying model is ideal, but we store simple viewmodels; in real app store models too.
            }
            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(UseFahrenheit));
        }

    public string Query { get; set; } = "New York";
    public bool UseFahrenheit { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string LocalTime { get; set; } = string.Empty;

    public CurrentViewModel Current { get; set; } = new CurrentViewModel();
    private Models.Current? CurrentModelBacking { get; set; }

        public ObservableCollection<ForecastDayViewModel> ForecastDays { get; }

        public IAsyncRelayCommand SearchCommand { get; }
    public bool IsLoading { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        private async Task SearchAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(ErrorMessage));
            try
            {
                var resp = await _client.GetForecastAsync(Query ?? string.Empty, 3);
                if (resp?.Location != null && resp.Current != null && resp.Forecast?.Forecastday != null)
                {
                    LocationName = $"{resp.Location.Name}, {resp.Location.Country}";
                    LocalTime = resp.Location.Localtime ?? string.Empty;
                    CurrentModelBacking = resp.Current;
                    Current = new CurrentViewModel(resp.Current, UseFahrenheit);
                    ForecastDays.Clear();
                    foreach (var d in resp.Forecast.Forecastday)
                    {
                        if (d != null)
                            ForecastDays.Add(new ForecastDayViewModel(d, UseFahrenheit));
                    }
                    OnPropertyChanged(nameof(LocationName));
                    OnPropertyChanged(nameof(LocalTime));
                    OnPropertyChanged(nameof(Current));
                    OnPropertyChanged(nameof(HasError));
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasError));
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }

    public class CurrentViewModel : ObservableObject
    {
        public string ConditionText { get; set; } = "";
        public double TempC { get; set; }
        public double TempF => TempC * 9 / 5 + 32;
        public string TemperatureFormatted { get; set; } = string.Empty;
        public string FeelsLikeFormatted { get; set; } = string.Empty;
        public string WindText { get; set; } = "";
        public int Humidity { get; set; }
        public int Cloud { get; set; }
        public string GustText { get; set; } = string.Empty;
        public string DewpointText { get; set; } = string.Empty;
        public string PrecipText { get; set; } = string.Empty;
        public bool IsDay { get; set; }

        public CurrentViewModel() { }

        public CurrentViewModel(Models.Current current, bool useF)
        {
            ConditionText = current.Condition?.Text ?? string.Empty;
            TempC = current.TempC;
            Humidity = current.Humidity;
            Cloud = current.Cloud;
            WindText = $"{current.WindKph:F0} kph {current.WindDir}";
            GustText = $"Gusts {current.GustKph:F0} kph";
            DewpointText = $"Dew {current.DewpointC:F1}°C";
            PrecipText = $"{current.PrecipMm:F1} mm";
            if (useF)
            {
                TemperatureFormatted = $"{TempF:F0}°F";
                FeelsLikeFormatted = $"{current.FeelsLikeF:F0}°F";
                DewpointText = $"Dew {current.DewpointC * 9 / 5 + 32:F1}°F";
                WindText = $"{current.WindMph:F0} mph {current.WindDir}";
                GustText = $"Gusts {current.GustKph * 0.621371:F0} mph";
            }
            else
            {
                TemperatureFormatted = $"{TempC:F0}°C";
                FeelsLikeFormatted = $"{current.FeelsLikeC:F0}°C";
                DewpointText = $"Dew {current.DewpointC:F1}°C";
                WindText = $"{current.WindKph:F0} kph {current.WindDir}";
                GustText = $"Gusts {current.GustKph:F0} kph";
            }
            IsDay = current.IsDay == 1;
        }
    }

    public class ForecastDayViewModel
    {
        public string DateReadable { get; }
        public string Condition { get; }
        public double MaxTempC { get; }
        public double MinTempC { get; }
        public string MaxTempFormatted => $"{MaxTempC:F0}°C";
        public string MinTempFormatted => $"{MinTempC:F0}°C";
        public int ChanceOfPrecip { get; }
        public string Sunrise { get; }
        public string Sunset { get; }
        public string MoonPhase { get; }
        public string MoonIllumination { get; }

        public ForecastDayViewModel(Models.Forecastday day, bool useF)
        {
            DateReadable = day.Date ?? string.Empty;
            Condition = day.Day?.Condition?.Text ?? string.Empty;
            MaxTempC = day.Day?.MaxtempC ?? 0;
            MinTempC = day.Day?.MintempC ?? 0;
            ChanceOfPrecip = day.Day?.DailyChanceOfPrecip ?? 0;
            Sunrise = day.Astro?.Sunrise ?? string.Empty;
            Sunset = day.Astro?.Sunset ?? string.Empty;
            MoonPhase = day.Astro?.MoonPhase ?? string.Empty;
            MoonIllumination = day.Astro?.MoonIllumination ?? string.Empty;
        }
    }
}
