using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Heather.Controls
{
    public partial class WeatherIcon : UserControl
    {
        private Canvas? _sun;
        private Canvas? _cloud;
        private Canvas? _moon;

        public static readonly StyledProperty<string?> IconTypeProperty = AvaloniaProperty.Register<WeatherIcon, string?>(nameof(IconType));

        public string? IconType
        {
            get => GetValue(IconTypeProperty);
            set => SetValue(IconTypeProperty, value);
        }

        public WeatherIcon()
        {
            InitializeComponent();
            _sun = this.FindControl<Canvas>("SunGroup");
            _cloud = this.FindControl<Canvas>("CloudGroup");
            _moon = this.FindControl<Canvas>("MoonGroup");

            IconTypeProperty.Changed.AddClassHandler<WeatherIcon>((x, e) => x.OnIconChanged());
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnIconChanged()
        {
            var t = (IconType ?? string.Empty).ToLowerInvariant();
            if (t.Contains("sun") || t.Contains("clear"))
            {
                SetVisible(_sun, true);
                SetVisible(_cloud, false);
                SetVisible(_moon, false);
            }
            else if (t.Contains("cloud") || t.Contains("overcast"))
            {
                SetVisible(_sun, false);
                SetVisible(_cloud, true);
                SetVisible(_moon, false);
            }
            else if (t.Contains("moon") || t.Contains("night"))
            {
                SetVisible(_sun, false);
                SetVisible(_cloud, false);
                SetVisible(_moon, true);
            }
            else
            {
                // default to cloud
                SetVisible(_sun, false);
                SetVisible(_cloud, true);
                SetVisible(_moon, false);
            }
        }

        private void SetVisible(Canvas? c, bool visible)
        {
            if (c == null) return;
            c.IsVisible = visible;
        }
    }
}
