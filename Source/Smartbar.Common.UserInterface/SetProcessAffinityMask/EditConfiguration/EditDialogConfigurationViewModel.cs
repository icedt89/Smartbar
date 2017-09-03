namespace JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask.EditConfiguration
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

        private Boolean showOnlyAvailableProcessors;

        public EditPluginConfigurationViewModel([NotNull] ProcessAffinityMaskDialogConfiguration pluginConfiguration,
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

            this.showOnlyAvailableProcessors = pluginConfiguration.ShowOnlyAvailableProcessors;
        }

        public Boolean ShowOnlyAvailableProcessors
        {
            get
            {
                return this.showOnlyAvailableProcessors;
            }
            set
            {
                this.SetProperty(ref this.showOnlyAvailableProcessors, value);
            }
        }

        [NotNull]
        public ICommand CloseCommand
        {
            get { return new CommonOKCommand<EditPluginConfigurationViewModel>(this, this.windowService); }
        }
    }
}