namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    public interface IUIExtensionService
    {
        [NotNull]
        [LinqTunnel]
        IEnumerable<IDynamicUICommand> CreateImageVisualizationCommands(Guid applicationId, [NotNull] Func<Boolean> canExecute);

        [NotNull]
        [LinqTunnel]
        IEnumerable<IDynamicUICommand> CreateCreateApplicationCommands(Guid groupId, Int32 column, Int32 row, Func<Boolean> canExecute);

        [NotNull]
        [LinqTunnel]
        IEnumerable<IDynamicUICommand> GetConfigurablePluginsUICommands(Func<Boolean> canExecute);
    }
}