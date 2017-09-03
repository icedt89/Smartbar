namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public sealed class ScrollSelectionIntoView : DependencyObject
    {
        #region Enabled

        public static readonly DependencyProperty Enabled =
          DependencyProperty.RegisterAttached("Enabled", typeof(Boolean), typeof(ScrollSelectionIntoView), new PropertyMetadata(default(Boolean), ToggleScrollSelection));

        public static void SetEnabled(ListBox element, Boolean value)
        {
            element.SetValue(Enabled, value);
        }
        public static Boolean GetEnabled(ListBox element)
        {
            return (Boolean)element.GetValue(Enabled);
        }

        #endregion

        private static void ToggleScrollSelection(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var listBox = (ListBox) dependencyObject;
            if ((Boolean) args.NewValue)
            {
                listBox.SelectionChanged += ListBoxOnSelectionChanged;              
            }
            else
            {
                listBox.SelectionChanged -= ListBoxOnSelectionChanged;
            }
        }

        private static void ListBoxOnSelectionChanged(Object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var listBox = (ListBox)sender;
            if (selectionChangedEventArgs.AddedItems.Count > 0)
            {
                listBox.ScrollIntoView(selectionChangedEventArgs.AddedItems[0]);
                selectionChangedEventArgs.Handled = true;
            }
        }
    }
}
