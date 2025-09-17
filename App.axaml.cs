using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Heather
{
    public class App : Application
    {
        public IServiceProvider Services { get; private set; } = default!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            services.AddSingleton<Services.WeatherApiClient>();
            services.AddSingleton<ViewModels.MainWindowViewModel>();
            Services = services.BuildServiceProvider();

            // Prefer a dark theme (Apple Weather-like) for the initial UX.
            // In Avalonia Fluent theme the theme variant can be set via Styles; for simplicity we set a resource.
            Styles.Add(new Avalonia.Styling.Style());

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mw = new MainWindow
                {
                    DataContext = Services.GetRequiredService<ViewModels.MainWindowViewModel>()
                };
                desktop.MainWindow = mw;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
