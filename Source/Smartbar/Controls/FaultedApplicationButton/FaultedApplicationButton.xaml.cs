namespace JanHafner.Smartbar.Controls.FaultedApplicationButton
{
    using System;
    using JetBrains.Annotations;

    partial class FaultedApplicationButton
    {
        internal FaultedApplicationButton()
        {
            this.InitializeComponent();
        }

        public FaultedApplicationButton([NotNull] FaultedApplicationViewModel viewModel)
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
