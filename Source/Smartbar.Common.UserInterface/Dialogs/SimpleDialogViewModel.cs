namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    public sealed class SimpleDialogViewModel
    {
        [NotNull]
        private readonly IWindowService windowService;

        public SimpleDialogViewModel([NotNull] String caption, [NotNull] String text, [NotNull] IWindowService windowService, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            if (String.IsNullOrWhiteSpace(caption))
            {
                throw new ArgumentNullException(nameof(caption));
            }

            if (String.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;
            this.Caption = caption;
            this.Text = text;
            this.Image = messageBoxImage;

            this.ConfigureCommands(messageBoxButton);
        }

        private void ConfigureCommands(MessageBoxButton messageBoxButton)
        {
            switch (messageBoxButton)
            {
                default:
                case MessageBoxButton.OK:
                    this.ShowOKCommand = true;
                    break;
                case MessageBoxButton.OKCancel:
                    this.ShowOKCommand = true;
                    this.ShowCancelCommand = true;
                    break;
                case MessageBoxButton.YesNo:
                    this.ShowYesCommand = true;
                    this.ShowNoCommand = true;
                    break;

                case MessageBoxButton.YesNoCancel:
                    this.ShowYesCommand = true;
                    this.ShowNoCommand = true;
                    this.ShowCancelCommand = true;
                    break;
            }
        }

        public MessageBoxImage Image { get; private set; }

        [NotNull]
        public String Caption { get; private set; }

        [NotNull]
        public String Text { get; private set; }

        [NotNull]
        public ICommand CancelCommand
        {
            get { return new CommonCancelCommand<SimpleDialogViewModel>(this, this.windowService); }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<SimpleDialogViewModel>(this, this.windowService);
            }
        }

        [NotNull]
        public ICommand YesCommand
        {
            get
            {
                return new CommonYesCommand<SimpleDialogViewModel>(this, this.windowService);
            }
        }

        [NotNull]
        public ICommand NoCommand
        {
            get
            {
                return new CommonNoCommand<SimpleDialogViewModel>(this, this.windowService);
            }
        }

        public Boolean ShowNoCommand { get; private set; }

        public Boolean ShowYesCommand { get; private set; }

        public Boolean ShowOKCommand { get; private set; }

        public Boolean ShowCancelCommand { get; private set; }
    }
}