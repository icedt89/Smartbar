namespace JanHafner.Smartbar.Controls.EmptyApplicationButton
{
    using System;
    using JetBrains.Annotations;

    partial class EmptyApplicationButton
    {
        internal EmptyApplicationButton()
        {
            this.InitializeComponent();
        }

        public EmptyApplicationButton([NotNull] EmptyApplicationViewModel viewModel)
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
