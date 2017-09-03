namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using NuGet;
    using Prism.Events;

    public sealed class SmartbarUpdateAvailable : PubSubEvent<IPackage>
    {
    }
}