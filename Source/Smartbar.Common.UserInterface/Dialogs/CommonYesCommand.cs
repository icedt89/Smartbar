namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Services;

    public sealed class CommonYesCommand<T> : CommonCloseCommand<T>
    {
        public CommonYesCommand(T viewModel, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Yes, windowService)
        {
        }

        public CommonYesCommand(T viewModel, Action<T> execute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Yes, execute, windowService)
        {
        }

        public CommonYesCommand(T viewModel, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Yes, canExecute, windowService)
        {
        }

        public CommonYesCommand(T viewModel, Action<T> execute, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(viewModel, MessageBoxResult.Yes, execute, canExecute, windowService)
        {
        }
    }
}
