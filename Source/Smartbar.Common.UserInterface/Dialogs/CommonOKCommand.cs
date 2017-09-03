namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Services;

    public sealed class CommonOKCommand<T> : CommonCloseCommand<T>
    {
        public CommonOKCommand(T viewModel, IWindowService windowService)
            : base(viewModel, MessageBoxResult.OK, windowService)
        {
        }

        public CommonOKCommand(T viewModel, Action<T> execute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.OK, execute, windowService)
        {
        }

        public CommonOKCommand(T viewModel, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.OK, canExecute, windowService)
        {
        }

        public CommonOKCommand(T viewModel, Action<T> execute, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.OK, execute, canExecute, windowService)
        {
        }
    }
}
