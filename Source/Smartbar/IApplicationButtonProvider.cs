namespace JanHafner.Smartbar
{
    using System;
    using Controls;
    using Core;

    public interface IApplicationButtonProvider
    {
        Boolean CanCreateApplicationButton(Application application);

        ApplicationButton CreateApplicationButton(Application application, Guid groupId);
    }
}
