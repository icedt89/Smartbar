namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Common;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    [Export(typeof(ISmartbarService))]
    internal sealed class SmartbarService : ISmartbarService
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public SmartbarService([NotNull] ISmartbarDbContext smartbarDbContext,
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

        public IEnumerable<Group> GetGroups()
        {
            return this.smartbarDbContext.Groups.OrderBy(group => group.Position).ToList();
        }

        public Group GetGroup(Guid groupId)
        {
            return this.smartbarDbContext.Groups.Single(group => group.Id == groupId);
        }

        public IEnumerable<T> GetApplications<T>(IEnumerable<Guid> applicationIds)
            where T : Application
        {
            return this.smartbarDbContext.Groups.SelectMany(g => g.Applications).Join(applicationIds, id => id.Id, a => a, (id, a) => id).Cast<T>().ToList();
        }

        public T GetApplication<T>(Guid applicationId)
            where T : Application
        {
            return this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<T>().Single(application => application.Id == applicationId);
        }
            
        public IEnumerable<PositionInformation> GetPositionInformation(Guid groupId, Int32 startColumnIndex, Int32 startRowIndex)
        {
            var group = this.smartbarDbContext.Groups.Single(g => g.Id == groupId);

            for (var j = startRowIndex; j < this.smartbarSettings.Rows; j++)
            {
                for (var i = startColumnIndex; i < this.smartbarSettings.Columns; i++)
                {
                    var assignedApplication = group.Applications.SingleOrDefault(application => application.Column == i && application.Row == j);
                    yield return new PositionInformation(groupId, i, j, assignedApplication?.Id);
                }

                startColumnIndex = 0;
            }
        }

        public IEnumerable<PositionInformation> GetPositionInformation(Int32 startColumnIndex, Int32 startRowIndex)
        {
            return this.smartbarDbContext.Groups.Select(g => g.Id).SelectMany(groupId => this.GetPositionInformation(groupId, startColumnIndex, startRowIndex));
        }

        public IEnumerable<PositionInformation> GetOutOfRangeApplicationPositions(Guid groupId, Int32 lowerBoundColumnIndex, Int32 lowerBoundRowIndex)
        {
            var group = this.smartbarDbContext.Groups.Single(g => g.Id == groupId);

            return @group.Applications.Where(application => application.Row > lowerBoundRowIndex || application.Column > lowerBoundColumnIndex).Select(application => new PositionInformation(groupId, application.Column, application.Row, application.Id));
        }

        public IEnumerable<PositionInformation> GetOutOfRangeApplicationPositions(Int32 lowerBoundColumnIndex, Int32 lowerBoundRowIndex)
        {
            return this.smartbarDbContext.Groups.Select(g => g.Id).SelectMany(groupId => this.GetOutOfRangeApplicationPositions(groupId, lowerBoundColumnIndex, lowerBoundRowIndex));
        }
    }
}
