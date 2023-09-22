﻿using System.ComponentModel;
using System.Windows.Forms;
using Bloxstrap.UI.Elements.Bootstrapper.Base;
using Bloxstrap.UI.ViewModels.Bootstrapper;

namespace Bloxstrap.UI.Elements.Bootstrapper
{
    /// <summary>
    /// Interaction logic for EACDialog.xaml
    /// </summary>

    public partial class EACDialog : IBootstrapperDialog
    {
        private readonly EACDialogViewModel _viewModel;

        public Bloxstrap.Bootstrapper? Bootstrapper { get; set; }

        private bool _isClosing;

        #region UI Elements
        public string Message
        {
            get => _viewModel.Message;
            set
            {
                _viewModel.Message = value;
                _viewModel.OnPropertyChanged(nameof(_viewModel.Message));
            }
        }

        public ProgressBarStyle ProgressStyle
        {
            get => _viewModel.ProgressIndeterminate ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            set
            {
                _viewModel.ProgressIndeterminate = (value == ProgressBarStyle.Marquee);
                _viewModel.OnPropertyChanged(nameof(_viewModel.ProgressIndeterminate));
            }
        }

        public int ProgressValue
        {
            get => _viewModel.ProgressValue;
            set
            {
                _viewModel.ProgressValue = value;
                _viewModel.OnPropertyChanged(nameof(_viewModel.ProgressValue));
            }
        }

        public bool CancelEnabled
        {
            get => _viewModel.CancelEnabled;
            set
            {
                _viewModel.CancelEnabled = value;

                _viewModel.OnPropertyChanged(nameof(_viewModel.CancelButtonVisibility));
                _viewModel.OnPropertyChanged(nameof(_viewModel.CancelEnabled));
            }
        }
        #endregion

        public EACDialog()
        {

            _viewModel = new EACDialogViewModel(this);
            DataContext = _viewModel;
            Title = App.Settings.Prop.BootstrapperTitle;
            Icon = App.Settings.Prop.BootstrapperIcon.GetIcon().GetImageSource();

            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_isClosing)
                Bootstrapper?.CancelInstall();
        }

        #region IBootstrapperDialog Methods
        public void ShowBootstrapper() => this.ShowDialog();

        public void CloseBootstrapper()
        {
            _isClosing = true;
            Dispatcher.BeginInvoke(this.Close);
        }

        public void ShowSuccess(string message, Action? callback) => BaseFunctions.ShowSuccess(message, callback);
        #endregion
    }
}