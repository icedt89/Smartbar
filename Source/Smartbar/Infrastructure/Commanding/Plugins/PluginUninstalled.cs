namespace JanHafner.Smartbar.Infrastructure.Commanding.Plugins
{
    using NuGet;
    using Prism.Events;

    internal sealed class PluginUninstalled : PubSubEvent<IPackage>
    {
    }
}
