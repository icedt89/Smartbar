namespace JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask
{
    using System;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    partial class SetProcessAffinityMask : ISingleInstanceWindow
    {
        private SetProcessAffinityMask()
        {
            this.InitializeComponent();
        }

        [UsedImplicitly]
        public SetProcessAffinityMask([NotNull] SetProcessAffinityMaskViewModel viewModel)
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
