using System;

namespace JanHafner.Smartbar.Common.UserInterface
{
    using System.Windows;
    using System.Windows.Threading;
    using JetBrains.Annotations;

    internal sealed class RunOnDispatcherProgress<T> : Progress<T>
    {
        public RunOnDispatcherProgress([NotNull] Action<T> action)
            : base(action)
        {
        }

        protected override void OnReport([NotNull] T value)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                base.OnReport(value);
            }, DispatcherPriority.Background);
        }
    }
}
