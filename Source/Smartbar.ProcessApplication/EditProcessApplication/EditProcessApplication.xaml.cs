namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class EditProcessApplication : ISingleInstanceWindow
    {
        private EditProcessApplication()
        {
            this.InitializeComponent();
        }

        public EditProcessApplication([NotNull] EditProcessApplicationViewModel viewModel)
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
