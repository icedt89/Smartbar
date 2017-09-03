namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public interface IApplicationButtonProvider
    {
        Boolean CanCreateApplicationButton([CanBeNull] Application application);

        [NotNull]
        ApplicationButton CreateApplicationButton([CanBeNull] Application application);
    }
}