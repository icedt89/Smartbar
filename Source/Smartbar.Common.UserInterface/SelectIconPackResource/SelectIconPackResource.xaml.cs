namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class SelectIconPackResource : ISingleInstanceWindow
    {
        private SelectIconPackResource()
        {
            this.InitializeComponent();
        }

        [UsedImplicitly]
        public SelectIconPackResource([NotNull] SelectIconPackResourceViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            this.DataContext = viewModel;
            
            this.InitializeComponent();
        }
    }
}
