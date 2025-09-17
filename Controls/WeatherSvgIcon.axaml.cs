using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Controls.Shapes;

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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnIconChanged()
        {
            var t = (IconType ?? string.Empty).ToLowerInvariant();
            if (_path == null) return;

            // Very small set of stylized path data for sun/cloud/moon
            if (t.Contains("sun") || t.Contains("clear"))
            {
                _path.Fill = new SolidColorBrush(Colors.Gold);
                _path.Data = Geometry.Parse("M32,12 A20,20 0 1,1 31.99,12 Z");
            }
            else if (t.Contains("cloud") || t.Contains("overcast") || t.Contains("rain") || t.Contains("patchy"))
            {
                _path.Fill = new SolidColorBrush(Colors.LightGray);
                _path.Data = Geometry.Parse("M20,30 a10,10 0 0,1 10,-10 h6 a8,8 0 0,1 0,16 H20 z");
            }
            else if (t.Contains("moon") || t.Contains("night"))
            {
                _path.Fill = new SolidColorBrush(Colors.LightGoldenrodYellow);
                _path.Data = Geometry.Parse("M28,14 a12,12 0 1,0 0.1,0 Z");
            }
            else
            {
                _path.Fill = new SolidColorBrush(Colors.LightGray);
                _path.Data = Geometry.Parse("M20,30 a10,10 0 0,1 10,-10 h6 a8,8 0 0,1 0,16 H20 z");
            }
        }
    }
}
