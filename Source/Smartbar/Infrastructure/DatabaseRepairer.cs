namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using JanHafner.Smartbar.Model;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;

    [Export(typeof(DatabaseRepairer))]
    internal sealed class DatabaseRepairer
    {
        private readonly ISmartbarDbContext smartbarDbContext;

        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public DatabaseRepairer([NotNull] ISmartbarDbContext smartbarDbContext,
            [NotNull] ISmartbarSettings smartbarSettings)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.smartbarDbContext = smartbarDbContext;
            this.smartbarSettings = smartbarSettings;
        }

        public async Task RepairAsync()
        {
            this.DeleteUnresolvedPluginConfigurations();
            this.DeleteUnresolvedApplications();

            if (this.smartbarDbContext.Groups.Any())
            {
                foreach (var group in this.smartbarDbContext.Groups)
                {
                    this.DeleteApplicationsOnDoublePositions(group);
                    this.DeleteApplicationsOutOfRange(group);
                }

                this.CorrectGroupSelection();
            }
            else
            {
                this.CreateDefaultGroup();
            }

            await this.smartbarDbContext.SaveChangesAsync();
        }

        private void CorrectGroupSelection()
        {
            // Select first found group if no group is selected
            if (!this.smartbarDbContext.Groups.Any(group => group.IsSelected))
            {
                this.smartbarDbContext.Groups.First().Select();
            } // If there are more than one group selected, unselect all but the first
            else if (this.smartbarDbContext.Groups.Count(group => group.IsSelected) > 1)
            {
                this.smartbarDbContext.Groups.Where(group => group.IsSelected).Skip(1).ForEach(group => group.Unselect());
            }
        }

        private void CreateDefaultGroup()
        {
            // Create default group if no group exists at all
            var defaultGroup = new Group(this.smartbarSettings.DefaultDummyGroupName);
            defaultGroup.Select();

            this.smartbarDbContext.Groups.Add(defaultGroup);
        }

        private void DeleteApplicationsOnDoublePositions(Group group)
        {
            // Remove applications on double positions inside a group
            foreach (var applicationOnPosition in group.Applications.GroupBy(application => new { application.Row, application.Column }).Where(_ => _.Count() > 1))
            {
                foreach (var notFirst in applicationOnPosition.Skip(1).ToList())
                {
                    group.Applications.Remove(notFirst);
                }
            }
        }

        private void DeleteApplicationsOutOfRange(Group group)
        {
            // Remove applications which are out of bounds of Smartbar
            foreach (var applicationOverTheTop in group.Applications.Where(application => application.Row >= this.smartbarSettings.Rows || application.Column >= this.smartbarSettings.Columns).ToList())
            {
                group.Applications.Remove(applicationOverTheTop);
            }
        }

        private void DeleteUnresolvedPluginConfigurations()
        {
            // Remove unresolved plugin configurations
            foreach (var unresolvedPluginConfiguration in this.smartbarDbContext.PluginConfigurations.OfType<UnresolvedPluginConfiguation>().ToList())
            {
                this.smartbarDbContext.PluginConfigurations.Remove(unresolvedPluginConfiguration);
            }
        }

        private void DeleteUnresolvedApplications()
        {
            // Remove unresolved applications
            foreach (var unresolvedApplication in this.smartbarDbContext.Groups.SelectMany(group => group.Applications, (group, application) => new
            {
                Group = group,
                Application = application
            }).Where(_ => _.Application is UnresolvedApplication).ToList())
            {
                unresolvedApplication.Group.Applications.Remove(unresolvedApplication.Application);
            }
        }
    }
}
