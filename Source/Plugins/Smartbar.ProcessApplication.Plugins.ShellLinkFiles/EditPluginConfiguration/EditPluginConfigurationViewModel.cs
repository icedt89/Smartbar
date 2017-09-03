namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles.EditPluginConfiguration
{
    using System;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Mvvm;

    internal sealed class EditPluginConfigurationViewModel : BindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        private Boolean tryDeleteSourceShellLink;

        public EditPluginConfigurationViewModel([NotNull] ShellLinkDragDropHandlerPluginConfiguration pluginConfiguration,
            [NotNull] IWindowService windowService)
        {
            if (pluginConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pluginConfiguration));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;

            this.tryDeleteSourceShellLink = pluginConfiguration.TryDeleteSourceShellLink;
        }

        public Boolean TryDeleteSourceShellLink
        {
            get
            {
                return this.tryDeleteSourceShellLink;
            }
            set
            {
                this.SetProperty(ref this.tryDeleteSourceShellLink, value);
            }
        }

        [NotNull]
        public ICommand CloseCommand
        {
            get { return new CommonOKCommand<EditPluginConfigurationViewModel>(this, this.windowService); }
        }

        [NotNull]
        public String PluginName
        {
            get { return ShellLinkDragDropHandler.PluginName; }
        }
    }
}