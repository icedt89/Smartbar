namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    public abstract class CommonCloseCommand<T> : DelegateCommand<T>
    {
        protected CommonCloseCommand(T viewModel, MessageBoxResult result, IWindowService windowService)
            : this(viewModel, result, parameter => { }, parameter => true, windowService)
        {
        }

        protected CommonCloseCommand(T viewModel, MessageBoxResult result, Action<T> execute, IWindowService windowService)
            : this(viewModel, result, execute, parameter => true, windowService)
        {
        }

        protected CommonCloseCommand(T viewModel, MessageBoxResult result, Func<T, Boolean> canExecute, IWindowService windowService)
            : this(viewModel, result, parameter => { }, canExecute, windowService)
        {
        }

        protected CommonCloseCommand(T viewModel, MessageBoxResult result, Action<T> execute, Func<T, Boolean> canExecute, IWindowService windowService)
            : base(parameter =>
            {
                try
                {
                    execute(viewModel);
                }
                catch (AbortCloseException)
                {
                    return;
                }

                windowService.CloseWindow(viewModel, result);
            }, _ => canExecute(viewModel))
        {
        }
    }
}
