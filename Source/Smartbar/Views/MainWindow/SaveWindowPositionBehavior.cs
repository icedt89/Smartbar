namespace JanHafner.Smartbar.Views.MainWindow
{
    using System;
    using System.Windows;
    using System.Windows.Interactivity;

    internal sealed class SaveWindowPositionBehavior : Behavior<MainWindow>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Closed += this.OnClosing;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Closing -= this.OnClosing;
        }

        private void OnClosing(Object sender, EventArgs eventArgs)
        {
            var mainWindowViewModel = (MainWindowViewModel)this.AssociatedObject.DataContext;

            mainWindowViewModel.InitialPosition = new Point(this.AssociatedObject.Left, this.AssociatedObject.Top);
        }
    }
}
