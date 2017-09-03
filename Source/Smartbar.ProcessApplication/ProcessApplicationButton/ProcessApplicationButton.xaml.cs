namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System;
    using JetBrains.Annotations;

    partial class ProcessApplicationButton
    {
        internal ProcessApplicationButton()
        {
            this.InitializeComponent();
        }

        public ProcessApplicationButton([NotNull] ProcessApplicationButtonViewModel viewModel)
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
