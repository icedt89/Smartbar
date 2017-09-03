namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System.Collections.Generic;
    using NuGet;
    using Prism.Events;

    public sealed class PluginUpdatesAvailable : PubSubEvent<IEnumerable<IPackage>>
    {
    }
}