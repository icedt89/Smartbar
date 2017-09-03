namespace JanHafner.Smartbar.Views.Group.CreateGroup
{
    using System;
    using JetBrains.Annotations;

    partial class CreateGroup
    {
        private CreateGroup()
        {
            this.InitializeComponent();
        }

        public CreateGroup([NotNull] CreateGroupViewModel viewModel)
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
