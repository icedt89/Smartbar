namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using JetBrains.Annotations;

    public sealed class SelectableProcessPriorityClass
    {
        private readonly Func<String> displayTextFactory;

        public SelectableProcessPriorityClass(ProcessPriorityClass processPriorityClass,
            [NotNull] Func<String> displayTextFactory)
        {
            if (displayTextFactory == null)
            {
                throw new ArgumentNullException(nameof(displayTextFactory));
            }

            this.displayTextFactory = displayTextFactory;
            this.ProcessPriorityClass = processPriorityClass;
        }

        public ProcessPriorityClass ProcessPriorityClass { get; private set; }

        public String DisplayText
        {
            get
            {
                return this.displayTextFactory();
            }
        }

        public static IEnumerable<SelectableProcessPriorityClass> CreateSelectableProcessPriorityClasses()
        {
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.RealTime, () => Localization.EditProcessApplication.PriorityClassRealtime);
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.High, () => Localization.EditProcessApplication.PriorityClassHigh);
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.AboveNormal, () => Localization.EditProcessApplication.PriorityClassAboveNormal);
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.Normal, () => Localization.EditProcessApplication.PriorityClassNormal);
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.BelowNormal, () => Localization.EditProcessApplication.PriorityClassBelowNormal);
            yield return new SelectableProcessPriorityClass(ProcessPriorityClass.Idle, () => Localization.EditProcessApplication.PriorityClassIdle);
        } 
    }
}
