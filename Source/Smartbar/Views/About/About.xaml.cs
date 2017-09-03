namespace JanHafner.Smartbar.Views.About
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class About : ISingleInstanceWindow
    {
        private About()
        {
            this.InitializeComponent();
        }

        public About([NotNull] AboutViewModel viewModel)
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
