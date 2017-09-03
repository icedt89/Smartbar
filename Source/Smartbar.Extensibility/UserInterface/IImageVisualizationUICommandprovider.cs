namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using JetBrains.Annotations;

    public interface IImageVisualizationUICommandProvider
    {
        [NotNull]
        IDynamicUICommand CreateUICommand(Guid applicationId, [NotNull] Func<Boolean> canExecute);
    }
}