using System.Windows.Threading;

namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using JetBrains.Annotations;

    [Export(typeof(IWindowService))]
    internal sealed class WindowService : IWindowService
    {
        [NotNull]
        private readonly ICollection<OpenedWindowOptions> openedWindows;

        [NotNull]
        private readonly Stack<OpenedWindowOptions> openedModalWindows; 

        public WindowService()
        {
            this.openedWindows = new List<OpenedWindowOptions>();    
            this.openedModalWindows = new Stack<OpenedWindowOptions>();
        }

        public Task<MessageBoxResult> ShowWindowAsync<TWindow>(Object viewModel)
            where TWindow : Window
        {
            return this.ShowWindowCore<TWindow>(viewModel, null);
        }

        public Task ShowWindowAsync<TWindow>(Object viewModel, Action<MessageBoxResult> closedCallback)
            where TWindow : Window
        {
            if (closedCallback == null)
            {
                throw new ArgumentNullException(nameof(closedCallback));
            }

            return this.ShowWindowCore<TWindow>(viewModel, closedCallback);
        }

        private Task<MessageBoxResult> ShowWindowCore<TWindow>([NotNull] Object viewModel, Action<MessageBoxResult> closedCallback)
            where TWindow : Window
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var isSingleInstanceWindowType = IsSingleInstanceWindow<TWindow>();
            if (isSingleInstanceWindowType && this.openedWindows.Any(windowOptions => windowOptions.Window is TWindow))
            {
                return Task.FromResult(MessageBoxResult.None);
            }

			return Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var windowOptions = new OpenedWindowOptions(viewModel, WindowService.CreateWindow<TWindow>(viewModel))
                {
                    IsSingleInstance = isSingleInstanceWindowType,
                    IsModal = closedCallback == null,
                };

                windowOptions.Window.Closed += (sender, args) =>
                {
                    this.openedWindows.Remove(windowOptions);

                    if (!windowOptions.IsModal)
                    {
                        closedCallback(windowOptions.Result);
                    }

                    this.GetCurrentModalWindow()?.Activate();
                };
                this.openedWindows.Add(windowOptions);

                if (windowOptions.IsModal)
                {
                    this.openedModalWindows.Push(windowOptions);

                    windowOptions.Window.ShowDialog();

                    this.openedModalWindows.Pop();
                }
                else
                {
                    windowOptions.Window.Show();

                    windowOptions.Window.Activate();

                    return MessageBoxResult.None;
                }

                return windowOptions.Result;
            }).Task;
        }

        public void CloseWindow(Object viewModel, MessageBoxResult result)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var windowOptions = this.openedWindows.Single(options => options.ViewModel == viewModel);
            windowOptions.Result = result;

            windowOptions.Window.Close();
        }

        private static TWindow CreateWindow<TWindow>(params Object[] arguments)
            where TWindow : Window
        {
            return (TWindow)Activator.CreateInstance(typeof(TWindow), arguments);
        }

        private static Boolean IsSingleInstanceWindow<TWindow>()
        {
            return typeof (TWindow).GetInterface(typeof (ISingleInstanceWindow).Name) != null;
        }

        private Window GetCurrentModalWindow()
        {
            return this.openedModalWindows.Any()
                ? this.openedModalWindows.Peek().Window
                : Application.Current.MainWindow;
        }

        private sealed class OpenedWindowOptions
        {
            public OpenedWindowOptions([NotNull] Object ViewModel, [NotNull] Window Window)
            {
                if (ViewModel == null)
                {
                    throw new ArgumentNullException(nameof(ViewModel));
                }

                if (Window == null)
                {
                    throw new ArgumentNullException(nameof(Window));
                }

                this.ViewModel = ViewModel;
                this.Window = Window;
            }

            [NotNull]
            public Object ViewModel { get; private set; }

            [NotNull]
            public Window Window { get; private set; }

            public Boolean IsModal { get; set; }

            public Boolean IsSingleInstance { get; set; }

            public MessageBoxResult Result { get; set; }
        }
    }
}