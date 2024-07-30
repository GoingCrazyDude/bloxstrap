using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;

namespace Bloxstrap.UI.ViewModels.Bootstrapper {
    public class TerminalDialogViewModel : BootstrapperDialogViewModel {
        public string Username { get; init; }
        public TerminalDialogViewModel(IBootstrapperDialog dialog, string username) : base(dialog) {
            Username = username;
        }
    }
}
