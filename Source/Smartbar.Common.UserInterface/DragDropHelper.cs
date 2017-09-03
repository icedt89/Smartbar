namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public static class DragDropHelper
    {
        /// <summary>
        /// Left Control-Key is pressed.
        /// </summary>
        public static Boolean IsCopyAction
        {
            get { return Application.Current.Dispatcher.Invoke(() => Keyboard.IsKeyDown(Key.LeftCtrl)); }
        }

        /// <summary>
        /// Left Shift-Key is pressed.
        /// </summary>
        public static Boolean IsExchangeAction
        {
            get { return Application.Current.Dispatcher.Invoke(() => Keyboard.IsKeyDown(Key.LeftShift)); }
        }

        /// <summary>
        /// Left Control-Key is pressed.
        /// </summary>
        public static Boolean IsMultiSelectAction
        {
            get { return Application.Current.Dispatcher.Invoke(() => Keyboard.IsKeyDown(Key.LeftCtrl)); }
        }
    }
}
