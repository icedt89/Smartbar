namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Services;

    public sealed class CommonNoCommand<T> : CommonCloseCommand<T>
    {
        public CommonNoCommand(T viewModel, IWindowService windowService)
            : base(viewModel, MessageBoxResult.No, windowService)
        {
        }

        public CommonNoCommand(T viewModel, Action<T> execute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.No, execute, windowService)
        {
        }

        public CommonNoCommand(T viewModel, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.No, canExecute, windowService)
        {
        }

        public CommonNoCommand(T viewModel, Action<T> execute, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.No, execute, canExecute, windowService)
        {
        }
    }
}
