namespace JanHafner.Smartbar.Infrastructure.Commanding.Plugins
{
    using NuGet;
    using Prism.Events;

    internal sealed class PluginInstalled : PubSubEvent<IPackage>
    {
    }
}
