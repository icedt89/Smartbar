namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class SelectNativeResource : ISingleInstanceWindow
    {
        private SelectNativeResource()
        {
            this.InitializeComponent();
        }

        [UsedImplicitly]
        public SelectNativeResource([NotNull] SelectNativeResourceViewModel viewModel)
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
