namespace JanHafner.Smartbar.Views.EditSmartbarSettings
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class EditSmartbarSettings : ISingleInstanceWindow
    {
        private EditSmartbarSettings()
        {
            this.InitializeComponent();
        }

        public EditSmartbarSettings([NotNull] EditSmartbarSettingsViewModel viewModel)
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
