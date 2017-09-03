namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using JetBrains.Annotations;

    public interface IShowPluginConfigurationUICommandProvider
    {
        [NotNull]
        IDynamicUICommand CreateUICommand([NotNull] Func<Boolean> canExecute);
    }
}