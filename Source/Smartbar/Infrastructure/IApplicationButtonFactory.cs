namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    internal interface IApplicationButtonFactory
    {
        [NotNull]
        ApplicationButton CreateApplicationButton([CanBeNull] Application application, Guid groupId, Int32 column, Int32 row);
    }
}