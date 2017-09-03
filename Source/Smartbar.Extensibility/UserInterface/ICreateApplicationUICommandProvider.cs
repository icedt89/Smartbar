namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using JetBrains.Annotations;

    public interface ICreateApplicationUICommandProvider
    {
        [NotNull]
        IDynamicUICommand CreateUICommand(Guid groupId, Int32 column, Int32 row, [NotNull] Func<Boolean> canExecute);
    }
}