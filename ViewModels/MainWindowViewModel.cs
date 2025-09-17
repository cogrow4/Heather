using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Heather.Models;
using Heather.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

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
            
            // Initialize with default search
            _ = SearchAsync();

            // default days selector: 3 (user requested 1-3 day selector)
            SelectedDays = 3;
            DaysOptions = new[] { 1, 2, 3 };
        }

        public string UnitToggleText => UseFahrenheit ? "°F" : "°C";

        public IRelayCommand ToggleUnitsCommand { get; }

        private void ToggleUnits()
        {
            UseFahrenheit = !UseFahrenheit;

            // Recreate current and forecast viewmodels to reflect changed units
            if (CurrentModelBacking != null)
            {
                Current = new CurrentViewModel(CurrentModelBacking, UseFahrenheit);
            }

            var recreated = ForecastDays.Select(f => f.BackingDay != null ? new ForecastDayViewModel(f.BackingDay, UseFahrenheit) : f).ToList();
            ForecastDays.Clear();
            foreach (var r in recreated)
                ForecastDays.Add(r);

            OnPropertyChanged(nameof(Current));
            OnPropertyChanged(nameof(UseFahrenheit));
            OnPropertyChanged(nameof(UnitToggleText));
        }

        public int[] DaysOptions { get; }

        private int _selectedDays;
        public int SelectedDays
        {
            get => _selectedDays;
            set
            {
                if (_selectedDays == value) return;
                _selectedDays = value;
                OnPropertyChanged(nameof(SelectedDays));
                // trigger a fresh search with the new day selection
                _ = SearchAsync();
            }
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

        private bool _isSearching = false;

        // subtle highlight opacity for transitions when data updates
        private double _highlightOpacity = 0.0;
        public double HighlightOpacity
        {
            get => _highlightOpacity;
            set
            {
                _highlightOpacity = value;
                OnPropertyChanged(nameof(HighlightOpacity));
            }
        }

        private async Task SearchAsync()
        {
            // Prevent concurrent searches
            if (_isSearching) return;
            _isSearching = true;

            // Add subtle highlight animation
            HighlightOpacity = 0.05;
            await Task.Delay(100);
            HighlightOpacity = 0.0;

            IsLoading = true;
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(ErrorMessage));
            OnPropertyChanged(nameof(HasError));
            
            try
            {
                var resp = await _client.GetForecastAsync(Query ?? string.Empty, SelectedDays);
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
                        {
                            var vm = new ForecastDayViewModel(d, UseFahrenheit);
                            ForecastDays.Add(vm);
                        }
                    }
                    OnPropertyChanged(nameof(LocationName));
                    OnPropertyChanged(nameof(LocalTime));
                    OnPropertyChanged(nameof(Current));
                    OnPropertyChanged(nameof(HasError));
                    
                    // Trigger subtle success animation
                    HighlightOpacity = 0.02;
                    await Task.Delay(200);
                    HighlightOpacity = 0.0;
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
                _isSearching = false;
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
        public string IconUrl { get; set; } = string.Empty;

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
            IconUrl = NormalizeIconUrl(current.Condition?.Icon);

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

        public string IsDayText => IsDay ? "Day" : "Night";

        private string NormalizeIconUrl(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            if (raw.StartsWith("//")) return "https:" + raw;
            if (raw.StartsWith("http")) return raw;
            return raw;
        }
    }

    public class ForecastDayViewModel
    {
        public Models.Forecastday? BackingDay { get; }
        public string DateReadable { get; }
        public string Condition { get; }
        public double MaxTempC { get; }
        public double MinTempC { get; }
        private bool UseF { get; }
        public string MaxTempFormatted => UseF ? $"{MaxTempC * 9 / 5 + 32:F0}°F" : $"{MaxTempC:F0}°C";
        public string MinTempFormatted => UseF ? $"{MinTempC * 9 / 5 + 32:F0}°F" : $"{MinTempC:F0}°C";
        public int ChanceOfPrecip { get; }
        public string Sunrise { get; }
        public string Sunset { get; }
        public string MoonPhase { get; }
        public string MoonIllumination { get; }
        public string Moonrise { get; }
        public string Moonset { get; }
        public string IconUrl { get; }

        public ForecastDayViewModel(Models.Forecastday day, bool useF)
        {
            BackingDay = day;
            DateReadable = day.Date ?? string.Empty;
            Condition = day.Day?.Condition?.Text ?? string.Empty;
            MaxTempC = day.Day?.MaxtempC ?? 0;
            MinTempC = day.Day?.MintempC ?? 0;
            UseF = useF;
            ChanceOfPrecip = day.Day?.DailyChanceOfPrecip ?? 0;
            Sunrise = day.Astro?.Sunrise ?? string.Empty;
            Sunset = day.Astro?.Sunset ?? string.Empty;
            MoonPhase = day.Astro?.MoonPhase ?? string.Empty;
            MoonIllumination = day.Astro?.MoonIllumination?.ToString() ?? string.Empty;
            Moonrise = day.Astro?.Moonrise ?? string.Empty;
            Moonset = day.Astro?.Moonset ?? string.Empty;
            IconUrl = NormalizeIconUrl(day.Day?.Condition?.Icon);
        }

        private string NormalizeIconUrl(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            if (raw.StartsWith("//")) return "https:" + raw;
            if (raw.StartsWith("http")) return raw;
            return raw;
        }
    }
}
