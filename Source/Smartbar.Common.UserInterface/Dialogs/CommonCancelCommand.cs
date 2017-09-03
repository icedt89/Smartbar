namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Services;

    public sealed class CommonCancelCommand<T> : CommonCloseCommand<T>
    {
        public CommonCancelCommand(T viewModel, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Cancel, windowService)
        {
        }

        public CommonCancelCommand(T viewModel, Action<T> execute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Cancel, execute, windowService)
        {
        }

        public CommonCancelCommand(T viewModel, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Cancel, canExecute, windowService)
        {
        }

        public CommonCancelCommand(T viewModel, Action<T> execute, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Cancel, execute, canExecute, windowService)
        {
        }
    }
}
