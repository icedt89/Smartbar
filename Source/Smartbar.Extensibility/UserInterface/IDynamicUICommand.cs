namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using JetBrains.Annotations;

    public interface IDynamicUICommand : ICommand
    {
        [NotNull]
        String DisplayText { get; }

        [NotNull]
        IEnumerable<IDynamicUICommand> ChildMenuItems { get; }
    }
}