namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Windows;
    using JanHafner.Toolkit.Common.ExtensionMethods;

    public sealed class FrameworkElement
    {
        public static readonly DependencyProperty TryDisposeDataContextProperty = DependencyProperty.RegisterAttached(
            "TryDisposeDataContext", typeof (Boolean), typeof (FrameworkElement), new PropertyMetadata(default(Boolean), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var associatedObject = (System.Windows.FrameworkElement)dependencyObject;
            if ((Boolean)dependencyPropertyChangedEventArgs.NewValue)
            {
                associatedObject.Unloaded += FrameworkElement.OnUnloadAssociatedObject;
            }
            else
            {
                associatedObject.Unloaded -= FrameworkElement.OnUnloadAssociatedObject;
            }
        }

        private static void OnUnloadAssociatedObject(Object sender, RoutedEventArgs args)
        {
            var associatedObject = (System.Windows.FrameworkElement)sender;
            associatedObject.DataContext.ToDisposable().Dispose();
        }

        public static void SetTryDisposeDataContext(System.Windows.FrameworkElement element, Boolean value)
        {
            element.SetValue(TryDisposeDataContextProperty, value);
        }

        public static Boolean GetTryDisposeDataContext(System.Windows.FrameworkElement element)
        {
            return (Boolean) element.GetValue(TryDisposeDataContextProperty);
        }
    }
}
