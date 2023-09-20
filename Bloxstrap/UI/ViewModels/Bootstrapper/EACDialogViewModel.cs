using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bloxstrap.UI.ViewModels.Bootstrapper
{
    public class EACDialogViewModel : BootstrapperDialogViewModel
    {
        public ImageSource EACBackgroundLocation { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Resources/BootstrapperStyles/EACDialog/EACBackground.jpg"));
        public BitmapSource EACLogo { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Resources/BootstrapperStyles/EACDialog/EACLogo.gif"));
        public Brush Background { get; set; } = new SolidColorBrush(Color.FromArgb(75,0,0,0));

        public EACDialogViewModel(IBootstrapperDialog dialog) : base(dialog)
        {
        }
    }
}