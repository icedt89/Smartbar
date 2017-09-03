namespace JanHafner.Smartbar.Views.ModuleExplorer
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class ModuleExplorer : ISingleInstanceWindow
    {
        private ModuleExplorer()
        {
            this.InitializeComponent();
        }

        public ModuleExplorer([NotNull] ModuleExplorerViewModel viewModel)
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
