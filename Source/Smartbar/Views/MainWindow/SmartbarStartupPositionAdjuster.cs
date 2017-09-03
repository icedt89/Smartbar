namespace JanHafner.Smartbar.Views.MainWindow
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;

    internal sealed class SmartbarStartupPositionAdjuster
    {
        public static Point GetAdjustedPosition(Point desiredPosition)
        {
            if(Screen.AllScreens.All(screen => !screen.WorkingArea.Contains((Int32)desiredPosition.X, (Int32)desiredPosition.Y)))
            {
                return new Point(0, 0);
            }

            return desiredPosition;
        }
    }
}
