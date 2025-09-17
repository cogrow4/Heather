using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace Heather
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs before AppMain.
        public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
