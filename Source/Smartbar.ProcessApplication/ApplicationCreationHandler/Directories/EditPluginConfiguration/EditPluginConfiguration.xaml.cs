namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories.EditPluginConfiguration
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class EditPluginConfiguration : ISingleInstanceWindow
    {
        private EditPluginConfiguration()
        {
            this.InitializeComponent();
        }

        public EditPluginConfiguration([NotNull] EditPluginConfigurationViewModel viewModel)
            : this()
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            this.DataContext = viewModel;
        }
    }
}
