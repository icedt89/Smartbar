namespace JanHafner.Smartbar.Views.PluginManager
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class PluginManager : ISingleInstanceWindow
    {
        private PluginManager()
        {
            this.InitializeComponent();
        }

        public PluginManager([NotNull] PluginManagerViewModel viewModel)
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
