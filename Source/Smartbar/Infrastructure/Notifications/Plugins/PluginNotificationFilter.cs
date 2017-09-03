namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JetBrains.Annotations;
    using NuGet;

    [Export(typeof(IPluginNotificationFilter))]
    internal sealed class PluginNotificationFilter : IPluginNotificationFilter
    {
        [NotNull]
        private readonly ICollection<IPackage> alreadyNotifiedUpdatabblePackages;

        public PluginNotificationFilter()
        {
            this.alreadyNotifiedUpdatabblePackages = new List<IPackage>();
        } 

        public IEnumerable<IPackage> Filter(IEnumerable<IPackage> updatablePackages)
        {
            if (updatablePackages == null)
            {
                throw new ArgumentNullException(nameof(updatablePackages));
            }

            foreach (var updatablePackage in updatablePackages.Where(updatablePackage => this.alreadyNotifiedUpdatabblePackages.All(package => package.Id != updatablePackage.Id)))
            {
                this.alreadyNotifiedUpdatabblePackages.Add(updatablePackage);

                yield return updatablePackage;
            }
        }
    }
}
