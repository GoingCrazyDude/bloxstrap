using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bloxstrap.UI.ViewModels.Bootstrapper
{
    public class EasyAntiCheatDialogViewModel : BootstrapperDialogViewModel
    {
        public ImageSource EasyAntiCheatBackgroundLocation { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Resources/BootstrapperStyles/EasyAntiCheatDialog/EasyAntiCheatBackground.png"));
        public ImageSource EasyAntiCheatLogoLocation { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Resources/BootstrapperStyles/EasyAntiCheatDialog/EasyAntiCheatLogo.png"));
        public Thickness DialogBorder { get; set; } = new Thickness(0);
        public System.Windows.Media.Brush Background { get; set; } = System.Windows.Media.Brushes.Transparent;
        public System.Windows.Media.Brush Foreground { get; set; } = System.Windows.Media.Brushes.Transparent;
        public System.Windows.Media.Brush IconColor { get; set; } = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));

        public EasyAntiCheatDialogViewModel(IBootstrapperDialog dialog) : base(dialog)
        {
        }
    }
}