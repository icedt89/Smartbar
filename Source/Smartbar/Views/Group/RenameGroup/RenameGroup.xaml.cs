namespace JanHafner.Smartbar.Views.Group.RenameGroup
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class RenameGroup : ISingleInstanceWindow
    {
        private RenameGroup()
        {
            this.InitializeComponent();
        }

        public RenameGroup([NotNull] RenameGroupViewModel viewModel)
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
