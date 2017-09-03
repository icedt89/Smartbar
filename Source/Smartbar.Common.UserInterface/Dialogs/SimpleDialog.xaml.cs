namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using JetBrains.Annotations;

    partial class SimpleDialog
    {
        private SimpleDialog()
        {
            this.InitializeComponent();
        }

        [UsedImplicitly]
        public SimpleDialog([NotNull] SimpleDialogViewModel viewModel)
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
