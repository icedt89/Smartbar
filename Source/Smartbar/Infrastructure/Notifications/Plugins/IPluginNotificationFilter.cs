namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using NuGet;

    internal interface IPluginNotificationFilter
    {
        [NotNull]
        [LinqTunnel]
        IEnumerable<IPackage> Filter([NotNull] IEnumerable<IPackage> updatablePackages);
    }
}