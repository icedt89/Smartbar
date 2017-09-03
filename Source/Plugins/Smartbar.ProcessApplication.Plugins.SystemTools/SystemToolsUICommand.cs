namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools
{
    using System;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    internal sealed class SystemToolsUICommand : DynamicUICommand
    {
        public SystemToolsUICommand([NotNull] Func<String> displayTextFactory)
            : base(displayTextFactory, () => { }, () => true)
        {
        }
    }
}