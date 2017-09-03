namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public interface ISmartbarService
    {
        [NotNull]
        IEnumerable<Group> GetGroups();

        [NotNull]
        Group GetGroup(Guid groupId);

        [NotNull]
        T GetApplication<T>(Guid applicationId)
            where T : Application;

        [NotNull]
        IEnumerable<T> GetApplications<T>(IEnumerable<Guid> applicationIds)
            where T : Application;

        [NotNull]
        IEnumerable<PositionInformation> GetPositionInformation(Guid groupId, Int32 startColumnIndex, Int32 startRowIndex);

        [NotNull]
        IEnumerable<PositionInformation> GetPositionInformation(Int32 startColumnIndex, Int32 startRowIndex);

        [NotNull]
        IEnumerable<PositionInformation> GetOutOfRangeApplicationPositions(Guid groupId, Int32 lowerBoundColumnIndex, Int32 lowerBoundRowIndex);

        [NotNull]
        IEnumerable<PositionInformation> GetOutOfRangeApplicationPositions(Int32 lowerBoundColumnIndex, Int32 lowerBoundRowIndex);
    }
}