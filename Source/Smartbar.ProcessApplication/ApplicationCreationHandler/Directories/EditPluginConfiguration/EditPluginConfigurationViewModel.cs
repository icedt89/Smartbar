namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories.EditPluginConfiguration
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

        private Boolean processDesktopIni;

        public EditPluginConfigurationViewModel([NotNull] DirectoryDragDropHandlerPluginConfiguration pluginConfiguration,
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

            this.processDesktopIni = pluginConfiguration.ProcessDesktopIni;
        }

        public Boolean ProcessDesktopIni
        {
            get
            {
                return this.processDesktopIni;
            }
            set
            {
                this.SetProperty(ref this.processDesktopIni, value);
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
            get { return DirectoryDragDropHandler.PluginName; }
        }
    }
}