namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using JetBrains.Annotations;

    public sealed class SelectableProcessWindowStyle
    {
        private readonly Func<String> displayTextFactory;

        public SelectableProcessWindowStyle(ProcessWindowStyle processWindowStyle,
            [NotNull] Func<String> displayTextFactory)
        {
            if (displayTextFactory == null)
            {
                throw new ArgumentNullException(nameof(displayTextFactory));
            }

            this.displayTextFactory = displayTextFactory;
            this.ProcessWindowStyle = processWindowStyle;
        }

        public ProcessWindowStyle ProcessWindowStyle { get; private set; }

        public String DisplayText
        {
            get
            {
                return this.displayTextFactory();
            }
        }

        public static IEnumerable<SelectableProcessWindowStyle> CreateSelectableProcessWindowStyles()
        {
            yield return new SelectableProcessWindowStyle(ProcessWindowStyle.Normal, () => Localization.EditProcessApplication.WindowStyleNormal);
            yield return new SelectableProcessWindowStyle(ProcessWindowStyle.Maximized, () => Localization.EditProcessApplication.WindowStyleMaximized);
            yield return new SelectableProcessWindowStyle(ProcessWindowStyle.Minimized, () => Localization.EditProcessApplication.WindowStyleMinimized);
            yield return new SelectableProcessWindowStyle(ProcessWindowStyle.Hidden, () => Localization.EditProcessApplication.WindowStyleHidden);
        } 
    }
}
