using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Controls.Shapes;
using System;
using Avalonia.Animation;
using Avalonia.Styling;

namespace Heather.Controls
{
    public partial class WeatherSvgIcon : UserControl
    {
        public static readonly StyledProperty<string?> IconTypeProperty = AvaloniaProperty.Register<WeatherSvgIcon, string?>(nameof(IconType));

        public string? IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

    private Avalonia.Controls.Shapes.Path? _path;

        public WeatherSvgIcon()
        {
            InitializeComponent();
            _path = this.FindControl<Avalonia.Controls.Shapes.Path>("IconPath");
            IconTypeProperty.Changed.AddClassHandler<WeatherSvgIcon>((x, e) => x.OnIconChanged());
            
            // Add subtle rotation animation for sun icons
            var rotateAnimation = new Animation
            {
                Duration = TimeSpan.FromSeconds(20),
                IterationCount = IterationCount.Infinite,
                Children =
                {
                    new KeyFrame
                    {
                        Cue = new Cue(0d),
                        Setters = { new Setter(RotateTransform.AngleProperty, 0d) }
                    },
                    new KeyFrame
                    {
                        Cue = new Cue(1d),
                        Setters = { new Setter(RotateTransform.AngleProperty, 360d) }
                    }
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnIconChanged()
        {
            var t = (IconType ?? string.Empty).ToLowerInvariant();
            if (_path == null) return;

            // Enhanced stylized path data with better visual appeal
            if (t.Contains("sun") || t.Contains("clear"))
            {
                // Gradient brush for sun
                var sunGradient = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Color.FromRgb(255, 215, 0), 0),
                        new GradientStop(Color.FromRgb(255, 165, 0), 1)
                    }
                };
                _path.Fill = sunGradient;
                _path.Data = Geometry.Parse("M32,8 A24,24 0 1,1 31.99,8 Z M32,0 L32,6 M32,58 L32,64 M0,32 L6,32 M58,32 L64,32 M9.86,9.86 L14.14,14.14 M49.86,49.86 L54.14,54.14 M9.86,54.14 L14.14,49.86 M49.86,14.14 L54.14,9.86");
            }
            else if (t.Contains("cloud") || t.Contains("overcast") || t.Contains("rain") || t.Contains("patchy"))
            {
                // Gradient brush for clouds
                var cloudGradient = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Color.FromRgb(240, 240, 240), 0),
                        new GradientStop(Color.FromRgb(200, 200, 200), 1)
                    }
                };
                _path.Fill = cloudGradient;
                _path.Data = Geometry.Parse("M16,40 C8,40 2,34 2,26 C2,18 8,12 16,12 C18,8 22,6 26,6 C32,6 38,10 40,16 C44,16 48,20 48,24 C52,24 56,28 56,32 C56,36 52,40 48,40 Z");
                
                // Add rain drops for rainy conditions
                if (t.Contains("rain"))
                {
                    var rainData = "M20,45 L18,50 M28,47 L26,52 M36,45 L34,50 M44,47 L42,52";
                    _path.Data = Geometry.Parse(_path.Data.ToString() + " " + rainData);
                }
            }
            else if (t.Contains("moon") || t.Contains("night"))
            {
                // Gradient brush for moon
                var moonGradient = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Color.FromRgb(255, 255, 224), 0),
                        new GradientStop(Color.FromRgb(255, 248, 220), 1)
                    }
                };
                _path.Fill = moonGradient;
                _path.Data = Geometry.Parse("M32,8 A24,24 0 1,0 32,56 A20,20 0 1,1 32,12 Z");
            }
            else
            {
                // Default to enhanced cloud
                var defaultGradient = new LinearGradientBrush
                {
                    StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                    EndPoint = new RelativePoint(0, 1, RelativeUnit.Relative),
                    GradientStops = new GradientStops
                    {
                        new GradientStop(Color.FromRgb(220, 220, 220), 0),
                        new GradientStop(Color.FromRgb(180, 180, 180), 1)
                    }
                };
                _path.Fill = defaultGradient;
                _path.Data = Geometry.Parse("M16,40 C8,40 2,34 2,26 C2,18 8,12 16,12 C18,8 22,6 26,6 C32,6 38,10 40,16 C44,16 48,20 48,24 C52,24 56,28 56,32 C56,36 52,40 48,40 Z");
            }
        }
    }
}
